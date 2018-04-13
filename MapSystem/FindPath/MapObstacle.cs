using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SceneMap;
using System.IO;
using Client;
using Common;
using UnityEngine.Profiling;
using Engine.Utility;
/// <summary>
/// 地图阻挡
/// </summary>
public class MapObstacle
{
    static MapObstacle()
    {
        Instance = new MapObstacle();
    }
    public static MapObstacle Instance { get; private set; }

    public static MapNav MyNav;

    class FastDictionary<TKey, TValue> //where TKey : object
    {
        LinkedList<KeyValuePair<TKey, TValue>> llDictionary
          = new LinkedList<KeyValuePair<TKey, TValue>>();

        // 遍历索引
        private int m_nIndex = 0;

        // 元素数量
        public int Count
        {
            get
            {
                return llDictionary.Count;
            }
        }
        /// <summary>
        /// 索引器，输入TKey可以找到对应的TValue
        /// </summary>
        /// <param name="tk">键值</param>
        /// <returns></returns>
        public TValue this[TKey tk]
        {
            get
            {
                foreach (KeyValuePair<TKey, TValue> kvp in llDictionary)
                {
                    if (tk.Equals(kvp.Key))
                    {
                        return kvp.Value;
                    }
                }
                return default(TValue);
            }

            set
            {
                if (ContainsKey(tk))
                {
                    Modify(tk, value);
                }
                else
                {
                    Add(tk, value);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public bool ContainsKey(TKey tk)
        {
            LinkedList<KeyValuePair<TKey, TValue>>.Enumerator iter = llDictionary.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Key.Equals(tk))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <param name="tk"></param>
        /// <param name="tv"></param>
        public void Add(TKey tk, TValue tv)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in llDictionary)
            {
                if (tk.Equals(kvp.Key))
                {
                    throw new Exception("新增失败：主键已经存在");
                }
            }
            llDictionary.AddLast(new KeyValuePair<TKey, TValue>(tk, tv));
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        /// <param name="tk"></param>
        public void Remove(TKey tk)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in llDictionary)
            {
                if (tk.Equals(kvp.Key))
                {
                    llDictionary.Remove(kvp);
                    return;
                }
            }
            //throw new Exception("删除失败：不存在这个主键");
        }
        /// <summary>
        /// 修改一组数据
        /// </summary>
        /// <param name="tk"></param>
        /// <param name="tv"></param>
        public void Modify(TKey tk, TValue tv)
        {
            LinkedListNode<KeyValuePair<TKey, TValue>> lln = llDictionary.First;
            while (lln != null)
            {
                if (tk.Equals(lln.Value.Key))
                {
                    llDictionary.AddBefore(lln, new KeyValuePair<TKey, TValue>(tk, tv));
                    llDictionary.Remove(lln);
                    return;
                }
                lln = lln.Next;
            }
            //throw new Exception("修改失败：不存在这个主键");
        }
        //-------------------------------------------------------------------------------------------------------
        public void Clear()
        {
            llDictionary.Clear();
        }

        // 支持遍历的方法
        //-------------------------------------------------------------------------------------------------------
        public void Begin()
        {
            m_nIndex = 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public bool MoveNext(out KeyValuePair<TKey, TValue> pair)
        {
            pair = default(KeyValuePair<TKey, TValue>);
            if (m_nIndex < llDictionary.Count)
            {
                pair = llDictionary.ElementAt(m_nIndex++);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 格子坐标的尺寸
    /// </summary>
    const int TileWidth = 1;
    const int TileHeight = 1;

    Dictionary<int, PathNode> totalDic = new Dictionary<int, PathNode>();
    Dictionary<int, PathNode> openDic = new Dictionary<int, PathNode>();
    Dictionary<int, PathNode> closeDic = new Dictionary<int, PathNode>();
    PriorityQueueB<PathNode> m_openHash = new PriorityQueueB<PathNode>(new NodeItemComp());
    // PathNode对象池
    ObjPool<PathNode> m_PathNodePool = new ObjPool<PathNode>();

    /// <summary>
    /// 阻挡格子
    /// </summary>
    private stSrvMapBlock[,] tiles = null;



    private void Clear()
    {
        Enable = false;
        tiles = null;
    }

    public bool Enable = false;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public bool Load(string obstacleFilePath)
    {
        Profiler.BeginSample("Deserialize map tile");
        Clear();

        try
        {
            byte[] data;
            Engine.Utility.FileUtils.Instance().GetBinaryFileBuff(obstacleFilePath, out data);
            if (data == null)
                return false;

            using (var asset = new MemoryStream(data))
            {
                if (asset == null)
                {
                    Debug.LogError(String.Format("无效的阻挡文件路径：{0}", obstacleFilePath));
                    return false;
                }

                Enable = true;

                var map = new SceneMap.MPS();
                //map.Deserialize(asset);


                map.Deserialize_MemoryStream(asset);


                tiles = map.tiles;
                data = null;
                // map.tiles = null;


                Width = (int)map.head.width;
                Height = (int)map.head.height;

            }
        }
        catch (System.Exception ex)
        {
            Engine.Utility.Log.Error("加载阻挡{0}失败！:{1}", obstacleFilePath, ex.ToString());
            return false;
        }
        Profiler.EndSample();

        return true;
    }
    public bool IsValidPt(int x, int y)
    {
        return ((0 < x && x < Width) && (0 < y && y < Height));
    }
    public bool IsValid(Vector2 pos)
    {
        return ((0 < pos.x && pos.x < Width) && (0 < pos.y && pos.y < Height));
    }

    /// <summary>
    /// 获取格子信息。先这样导出，后面再设置访问权限
    /// </summary>
    /// <returns></returns>
    public stSrvMapBlock[,] GetTiles()
    {
        return tiles;
    }

    /// <summary>
    /// 转换成格子坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool GetObstaclePosition(Vector2 pos, ref int x, ref int y)
    {
        if (IsValid(pos) == false)
        {
            x = 0;
            y = 0;
            return false;
        }

        x = (int)(pos.x);
        y = (int)(pos.y);

        return true;
    }

    /// <summary>
    /// 两个点是否在同一个格子中
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    public bool IsInSameGrid(Vector2 pos1, Vector2 pos2)
    {
        if (IsValid(pos1) == false || IsValid(pos2) == false)
        {
            return false;
        }

        return (int)(pos1.x) == (int)(pos2.x) && (int)(pos1.y) == (int)(pos2.y);
    }

    /// <summary>
    /// 判断阻挡
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <param name="flags">阻挡类型</param>
    /// <returns>true,无阻挡</returns>
    public bool CheckObstacle(Vector2 pos, byte flags)
    {
        if (IsValid(pos) == false)
        {
            return false;
        }

        var tile = tiles[(int)pos.x, (int)pos.y];
        //if (tile != null)
        //{
        return CanWalk(tile);
        //}

        //  Debug.Log(String.Format("未找到阻挡：{0}, {1}", pos.x, pos.y));
        return true;
    }
    public bool CanWalk(int x, int y)
    {
        return CanWalk(tiles[x, y]);
    }

    public bool Check(int x, int y, TileType tileType)
    {
        if (tiles == null)
        {
            return false;
        }
        stSrvMapBlock tile = tiles[x, y];
        //if (tile == null)
        //{
        //return false;
        //}

        return (tile.flags2 & (int)tileType) != 0;
    }

    private bool CanWalk(stSrvMapBlock tile)
    {
        return tile.flags2 != (byte)TileType.TileType_Walk_BLOCK;
    }

    #region A star 寻路算法
    class PathNode
    {
        public int index;
        public int parentIndex;
        public float g;
        public float h;
        public float f;

        public MapGrid grid;
        public Vector2 position;
        public bool in_open_list = false;
        public LinkedListNode<PathNode> list_node;
        public PathNode()
        {
        }

        public void Init(int _index, int _parentIndex, float _g, float _h)
        {
            in_open_list = false;
            list_node = new LinkedListNode<PathNode>(this);
            index = _index;
            parentIndex = _parentIndex;
            g = _g;
            h = _h;
            f = g + h;

            grid = new MapGrid() { x = index % MapObstacle.Instance.Width, z = index / MapObstacle.Instance.Width };
            position = MapGrid.GetMapPosV2(grid);
        }

        public void setG(float _g)
        {
            g = _g;
            f = g + h;
        }
    }

    struct NodeOffset
    {
        public int x;
        public int y;
        public int offset;
        public float distance;

        public NodeOffset(int _offset, float _distance)
        {
            x = 0;
            y = 0;
            offset = _offset;
            distance = _distance;
        }

        public NodeOffset(int _x, int _y, float _distance)
        {
            x = _x;
            y = _y;
            offset = 0;
            distance = _distance;
        }

        public override string ToString()
        {
            return string.Format("offset:" + offset + "," + distance);
        }
    }
    float GetDistance(PathNode p1, PathNode p2)
    {
        return Math.Abs(p1.grid.x - p2.grid.x) + Math.Abs(p1.grid.z - p2.grid.z);
    }
    /// <summary>
    ///  格子寻路。MapGird(x,z)，实际上是地图平面坐标(x,y)
    /// </summary>
    /// <param name="fromGridX"></param>
    /// <param name="fromGridZ"></param>
    /// <param name="toGridX"></param>
    /// <param name="toGridZ"></param>
    /// <param name="path">可行走路径</param>
    /// <returns>是否找到路径</returns>
    //public bool GetPath(int fromGridX, int fromGridZ, int toGridX, int toGridZ, out List<MapGrid> path)
    //{
    //    var mapWidth = this.Width;
    //    var mapHeight = this.Height;
    //    //var tiles = this.GetTiles();

    //    path = new List<MapGrid>();
    //    if (CanWalk(tiles[fromGridX, fromGridZ]) == false)
    //    {
    //        return false;
    //    }

    //    if (CanWalk(tiles[toGridX, toGridZ]) == false)
    //    {
    //        return false;
    //    }

    //    // 同一个点也导出路径
    //    if (fromGridX == toGridX && fromGridZ == toGridZ)
    //    {
    //        path.Add(new MapGrid() { x = fromGridX, z = fromGridZ });
    //        path.Add(new MapGrid() { x = toGridX, z = toGridZ });
    //        return true;
    //    }

    //    ClearPath();

    //    //Profiler.BeginSample("GetPath Prepare");
    //    PathNode pnTo = new PathNode();//m_PathNodePool.Alloc();
    //    pnTo.Init(toGridZ * mapWidth + toGridX, 0, 0, 0);
    //    PathNode pnFrom = new PathNode();//m_PathNodePool.Alloc();
    //    pnFrom.Init(fromGridZ * mapWidth + fromGridX, 0, 0, 0);

    //    openDic.Add(pnFrom.index, pnFrom);
    //    totalDic.Add(pnFrom.index, pnFrom);
    //    //Profiler.EndSample();

    //    while (openDic.Count > 0)
    //    {
    //        PathNode pn = null;
    //        Profiler.BeginSample("GetPath 0-1");

    //        foreach (var dic in openDic)
    //        {
    //            if (pn == null)
    //            {
    //                pn = dic.Value;
    //            }
    //            if (dic.Value.f < pn.f)
    //            {
    //                pn = dic.Value;
    //            }
    //        }
    //        Profiler.EndSample();

    //        if (pn == null)
    //        {
    //            break;
    //        }

    //        //Profiler.BeginSample("GetPath 0-2");
    //        closeDic.Add(pn.index, pn);
    //        openDic.Remove(pn.index);
    //        //Profiler.EndSample();
    //        if (pn.index == pnTo.index)
    //        {
    //            break;
    //        }

    //        List<NodeOffset> offsets = ClacRoundPoint(pn);
    //        if (offsets.Count <= 0)
    //        {
    //            openDic.Remove(pn.index);
    //            continue;
    //        }

    //        Profiler.BeginSample("GetPath 2");
    //        for (int i = 0; i < offsets.Count; ++i)
    //        {
    //            NodeOffset offset = offsets[i];

    //            int x = pn.grid.x + offset.x;
    //            if (x < 0 || x >= mapWidth) continue;
    //            int y = pn.grid.z + offset.y;
    //            if (y < 0 || y >= mapHeight) continue;

    //            int index = y * mapWidth + x;

    //            if (closeDic.ContainsKey(index)) continue;

    //            PathNode pntmp = null;
    //            float g = pn.g + offset.distance;

    //            //if (openDic.ContainsKey(index))
    //            if (openDic.TryGetValue(index, out pntmp))
    //            {
    //                pntmp = openDic[index];
    //                if (g < pntmp.g)
    //                {
    //                    pntmp.parentIndex = pn.index;
    //                    pntmp.setG(g);
    //                }
    //            }
    //            else
    //            {
    //                pntmp = new PathNode();//m_PathNodePool.Alloc();
    //                pntmp.Init(index, pn.index, 0, 0);

    //                openDic[index] = pntmp;
    //                totalDic[index] = pntmp;
    //                pntmp.h = Mathf.Abs(Vector2.Distance(pntmp.position, pnTo.position));
    //                pntmp.setG(g);
    //            }
    //        }
    //        Profiler.EndSample();
    //    }

    //    if (!closeDic.ContainsKey(pnTo.index))
    //    {
    //        return false;
    //    }

    //    Profiler.BeginSample("GetPath End");
    //    List<MapGrid> tmpPath = new List<MapGrid>();
    //    PathNode node = closeDic[pnTo.index];
    //    while (node != null)
    //    {
    //        tmpPath.Add(node.grid);

    //        if (node.parentIndex == 0)
    //        {
    //            break;
    //        }

    //        node = totalDic[node.parentIndex];
    //    }

    //    tmpPath.Reverse_NoHeapAlloc(); // 路径反转
    //    path = tmpPath;

    //    ClearPath();
    //    Profiler.EndSample();

    //    return true;
    //}
    LinkedList<PathNode> open_list = new LinkedList<PathNode>();
    void InsertToList(PathNode pn)
    {
       
        pn.in_open_list = true;
        LinkedListNode<PathNode> first = open_list.First;
        if (first == null)
        {
            open_list.AddFirst(pn.list_node);
            return;
        }

        LinkedListNode<PathNode> last = open_list.Last;
        if (last.Value.f <= pn.f)
        {
            open_list.AddLast(pn.list_node);
            return;
        }

        foreach (PathNode try_node in open_list)
        {
            if (try_node.f > pn.f)
            {
                open_list.AddBefore(try_node.list_node, pn.list_node);
                break;
            }

        }


    }

    void RemoveFromList(PathNode pn)
    {
     
        if (!pn.in_open_list) return;

        open_list.Remove(pn.list_node);

        pn.in_open_list = false;
    }
    public bool GetPathDistance(int fromGridX, int fromGridZ, int toGridX, int toGridZ, out List<MapGrid> path)
    {
        var mapWidth = this.Width;
        var mapHeight = this.Height;
        //var tiles = this.GetTiles();

        path = new List<MapGrid>();
        if (CanWalk(tiles[fromGridX, fromGridZ]) == false)
        {
            return false;
        }

        if (CanWalk(tiles[toGridX, toGridZ]) == false)
        {
            return false;
        }

        // 同一个点也导出路径
        if (fromGridX == toGridX && fromGridZ == toGridZ)
        {
            path.Add(new MapGrid() { x = fromGridX, z = fromGridZ });
            path.Add(new MapGrid() { x = toGridX, z = toGridZ });
            return true;
        }

        ClearPath();

        
        PathNode pnTo = new PathNode();//m_PathNodePool.Alloc();
        pnTo.Init(toGridZ * mapWidth + toGridX, 0, 0, 0);
        PathNode pnFrom = new PathNode();//m_PathNodePool.Alloc();
        pnFrom.Init(fromGridZ * mapWidth + fromGridX, 0, 0, 0);

        openDic.Add(pnFrom.index, pnFrom);
        InsertToList(pnFrom);
        totalDic.Add(pnFrom.index, pnFrom);
    

        while (openDic.Count > 0)
        {

            if (open_list.First == null)
                break;
            PathNode pn = open_list.First.Value;


            if (pn == null)
            {
                break;
            }

            closeDic.Add(pn.index, pn);
            openDic.Remove(pn.index);
            RemoveFromList(pn);
            if (pn.index == pnTo.index)
            {
                break;
            }

            List<NodeOffset> offsets = ClacRoundPoint(pn);
            if (offsets.Count <= 0)
            {
                openDic.Remove(pn.index);
                RemoveFromList(pn);
                continue;
            }

            for (int i = 0; i < offsets.Count; ++i)
            {
                NodeOffset offset = offsets[i];

                int x = pn.grid.x + offset.x;
                if (x < 0 || x >= mapWidth) continue;
                int y = pn.grid.z + offset.y;
                if (y < 0 || y >= mapHeight) continue;

                int index = y * mapWidth + x;

                if (closeDic.ContainsKey(index)) continue;

                PathNode pntmp = null;
                float g = pn.g + offset.distance;

                //if (openDic.ContainsKey(index))
                if (openDic.TryGetValue(index, out pntmp))
                {
                    pntmp = openDic[index];
                    if (g < pntmp.g)
                    {
                        pntmp.parentIndex = pn.index;
                        RemoveFromList(pntmp);
                        pntmp.setG(g);
                        InsertToList(pntmp);
                    }
                }
                else
                {
                    pntmp = new PathNode();//m_PathNodePool.Alloc();
                    pntmp.Init(index, pn.index, 0, 0);

                    openDic[index] = pntmp;
                    totalDic[index] = pntmp;
                    pntmp.h = GetDistance(pntmp, pnTo);//Math.Abs(Vector2.Distance(pntmp.position, pnTo.position));
                    pntmp.setG(g);
                    InsertToList(pntmp);
                }
            }
        }

        if (!closeDic.ContainsKey(pnTo.index))
        {
            return false;
        }

        List<MapGrid> tmpPath = new List<MapGrid>();
        PathNode node = closeDic[pnTo.index];
        while (node != null)
        {
            tmpPath.Add(node.grid);

            if (node.parentIndex == 0)
            {
                break;
            }

            node = totalDic[node.parentIndex];
        }

        tmpPath.Reverse_NoHeapAlloc(); // 路径反转
        path = tmpPath;

        ClearPath();

        return true;
    }
    class NodeItemComp : IComparer<PathNode>
    {
        public int Compare(PathNode x, PathNode y)
        {
            if (x.f < y.f)
            {
                return -1;
            }
            if (x.f > y.f)
            {
                return 1;
            }
            return 0;
        }
    }
    public class PriorityQueueB<T>
    {
        #region Variables Declaration
        public List<T> InnerList = new List<T>();
        protected IComparer<T> mComparer;
        #endregion


        #region Contructors
        public PriorityQueueB()
        {
            mComparer = Comparer<T>.Default;
        }

        public PriorityQueueB(IComparer<T> comparer)
        {
            mComparer = comparer;
        }

        public PriorityQueueB(IComparer<T> comparer, int capacity)
        {
            mComparer = comparer;
            InnerList.Capacity = capacity;
        }
        #endregion

        #region Methods
        protected void SwitchElements(int i, int j)
        {
            T h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return mComparer.Compare(InnerList[i], InnerList[j]);
        }

        /// <summary>
        /// Push an object onto the PQ
        /// </summary>
        /// <param name="O">The new object</param>
        /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
        public int Push(T item)
        {
            int p = InnerList.Count, p2;
            InnerList.Add(item); // E[p] = O
            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            return p;
        }

        /// <summary>
        /// Get the smallest object and remove it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Pop()
        {
            T result = InnerList[0];
            int p = 0, p1, p2, pn;
            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList.RemoveAt(InnerList.Count - 1);
            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);

            return result;
        }

        /// <summary>
        /// Notify the PQ that the object at position i has changed
        /// and the PQ needs to restore order.
        /// Since you dont have access to any indexes (except by using the
        /// explicit IList.this) you should not call this function without knowing exactly
        /// what you do.
        /// </summary>
        /// <param name="i">The index of the changed object.</param>
        public void Update(int i)
        {
            int p = i, pn;
            int p1, p2;
            do  // aufsteigen
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            if (p < i)
                return;
            do     // absteigen
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);
        }

        /// <summary>
        /// Get the smallest object without removing it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Peek()
        {
            if (InnerList.Count > 0)
                return InnerList[0];
            return default(T);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public void RemoveLocation(T item)
        {
            int index = -1;
            for (int i = 0; i < InnerList.Count; i++)
            {

                if (mComparer.Compare(InnerList[i], item) == 0)
                    index = i;
            }

            if (index != -1)
                InnerList.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return InnerList[index]; }
            set
            {
                InnerList[index] = value;
                Update(index);
            }
        }
        #endregion
    }
    //public bool GetPathHash(int fromGridX, int fromGridZ, int toGridX, int toGridZ, out List<MapGrid> path)
    //{
    //    var mapWidth = this.Width;
    //    var mapHeight = this.Height;
    //    //var tiles = this.GetTiles();

    //    path = new List<MapGrid>();
    //    if (CanWalk(tiles[fromGridX, fromGridZ]) == false)
    //    {
    //        return false;
    //    }

    //    if (CanWalk(tiles[toGridX, toGridZ]) == false)
    //    {
    //        return false;
    //    }

    //    // 同一个点也导出路径
    //    if (fromGridX == toGridX && fromGridZ == toGridZ)
    //    {
    //        path.Add(new MapGrid() { x = fromGridX, z = fromGridZ });
    //        path.Add(new MapGrid() { x = toGridX, z = toGridZ });
    //        return true;
    //    }

    //    ClearPath();


    //    //Profiler.BeginSample("GetPath Prepare");
    //    PathNode pnTo = new PathNode();//m_PathNodePool.Alloc();
    //    pnTo.Init(toGridZ * mapWidth + toGridX, 0, 0, 0);
    //    PathNode pnFrom = new PathNode();//m_PathNodePool.Alloc();
    //    pnFrom.Init(fromGridZ * mapWidth + fromGridX, 0, 0, 0);

    //    m_openHash.Push(pnFrom);

    //    totalDic.Add(pnFrom.index, pnFrom);
    //    //Profiler.EndSample();

    //    while (m_openHash.Count > 0)
    //    {
    //        // Log.Error("count =============" + m_openHash.Count);
    //        PathNode pn = m_openHash.Pop(); ;

    //        //foreach (var dic in pn)
    //        //{
    //        //    if (pn == null)
    //        //    {
    //        //        pn = dic;
    //        //    }
    //        //    if (dic.f < pn.f)
    //        //    {
    //        //        pn = dic;
    //        //    }
    //        //}

    //        if (pn == null)
    //        {
    //            break;
    //        }

    //        //Profiler.BeginSample("GetPath 0-2");
    //        closeDic.Add(pn.index, pn);
    //        // m_openHash.Remove(pn);
    //        //Profiler.EndSample();
    //        if (pn.index == pnTo.index)
    //        {
    //            break;
    //        }

    //        List<NodeOffset> offsets = ClacRoundPoint(pn);
    //        if (offsets.Count <= 0)
    //        {
    //            //  m_openHash.Remove(pn);
    //            continue;
    //        }

    //        //Profiler.BeginSample("GetPath 2");
    //        for (int i = 0; i < offsets.Count; ++i)
    //        {
    //            NodeOffset offset = offsets[i];

    //            int x = pn.grid.x + offset.x;
    //            if (x < 0 || x >= mapWidth) continue;
    //            int y = pn.grid.z + offset.y;
    //            if (y < 0 || y >= mapHeight) continue;

    //            int index = y * mapWidth + x;

    //            if (closeDic.ContainsKey(index)) continue;

    //            PathNode pntmp = null;
    //            float g = pn.g + offset.distance;

    //            bool bContain = false;
    //            foreach (var item in m_openHash.InnerList)
    //            {
    //                if (item.index == index)
    //                {
    //                    bContain = true;
    //                    pntmp = item;
    //                    if (g < pntmp.g)
    //                    {
    //                        pntmp.parentIndex = pn.index;
    //                        pntmp.setG(g);
    //                    }
    //                    break;
    //                }
    //            }
    //            //if (openDic.ContainsKey(index))
    //            if (!bContain)
    //            {
    //                pntmp = new PathNode();//m_PathNodePool.Alloc();
    //                pntmp.Init(index, pn.index, 0, 0);

    //                m_openHash.Push(pntmp);
    //                totalDic[index] = pntmp;
    //                pntmp.h = Mathf.Abs(Vector2.Distance(pntmp.position, pnTo.position));
    //                pntmp.setG(g);
    //            }
    //        }
    //        //Profiler.EndSample();
    //    }

    //    if (!closeDic.ContainsKey(pnTo.index))
    //    {
    //        return false;
    //    }

    //    //Profiler.BeginSample("GetPath End");
    //    List<MapGrid> tmpPath = new List<MapGrid>();
    //    PathNode node = closeDic[pnTo.index];
    //    while (node != null)
    //    {
    //        tmpPath.Add(node.grid);

    //        if (node.parentIndex == 0)
    //        {
    //            break;
    //        }

    //        node = totalDic[node.parentIndex];
    //    }

    //    tmpPath.Reverse_NoHeapAlloc(); // 路径反转
    //    path = tmpPath;

    //    ClearPath();
    //    //Profiler.EndSample();

    //    return true;
    //}
    private void ClearPath()
    {
        openDic.Clear();
        closeDic.Clear();
        open_list.Clear();
        Dictionary<int, PathNode>.Enumerator iter = totalDic.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value != null)
            {
                m_PathNodePool.Free(iter.Current.Value);
            }
        }

        //KeyValuePair<int, PathNode> pair;
        //totalDic.Begin();
        //while (totalDic.MoveNext(out pair))
        //{
        //    if (pair.Value != null)
        //    {
        //        m_PathNodePool.Free(pair.Value);
        //    }
        //}
        totalDic.Clear();
    }

    static float disCornor = Mathf.Sqrt(TileWidth * TileWidth + TileHeight * TileHeight);
    static NodeOffset[] offsets = new NodeOffset[]
		{
			new NodeOffset(-1,1, disCornor),	// 左下
			new NodeOffset(0,1, TileHeight),	// 下
			new NodeOffset(1,1, disCornor),	    // 右下
			new NodeOffset(-1,0, TileWidth),	// 左
			new NodeOffset(1,0, TileWidth),	    // 右
			new NodeOffset(-1,-1, disCornor),	// 左上
			new NodeOffset(0,-1, TileHeight),	// 上
			new NodeOffset(1,-1, disCornor),	// 右上
		};

    private List<NodeOffset> RoundNodes = new List<NodeOffset>();

    // 计算点周围可以选择的点
    private List<NodeOffset> ClacRoundPoint(PathNode pn)
    {
        var mapWidth = this.Width;
        var mapHeight = this.Height;
        var tiles = this.GetTiles();

        RoundNodes.Clear();

        for (int i = 0; i < offsets.Length; ++i)
        {
            NodeOffset offset = offsets[i];

            int x = pn.grid.x + offset.x;
            if (x < 0 || x >= mapWidth) continue;
            int y = pn.grid.z + offset.y;
            if (y < 0 || y >= mapHeight) continue;

            int index = y * mapWidth + x;
            if (closeDic.ContainsKey(index)) continue;

            if (!CanWalk(tiles[x, y])) continue;

            // 忽略拐角拌脚点 斜方向
            if (Mathf.Abs(offset.y) + Mathf.Abs(offset.x) > 1)
            {
                if (!CanWalk(tiles[x, y - offset.y]) || !CanWalk(tiles[x - offset.x, y]))
                {
                    continue;
                }
            }

            RoundNodes.Add(offset);
        }

        return RoundNodes;
    }


    #endregion
}
