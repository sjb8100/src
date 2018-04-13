using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Profiling;
using Engine.Utility;
/// <summary>
/// 自动寻路移动
/// </summary>
class PathMove
{
    private List<MapVector2> path = new List<MapVector2>(); // 最后一个点是终点

    public event Action<List<Vector2>> PathFind;

    static PathMove s_Inst = null;
    public static PathMove Instance()
    {
        if (null == s_Inst)
        {
            s_Inst = new PathMove();
        }

        return s_Inst;
    }

    /// <summary>
    /// 用格子寻路
    /// </summary>
    /// <returns></returns>
    private bool findPathFromGrid(Vector2 src, Vector2 dst, out List<Vector2> path)
    {
        path = new List<Vector2>();
        int fromGridX = 0, fromGridZ = 0, toGridX = 0, toGridZ = 0;
        if (!MapObstacle.Instance.GetObstaclePosition(src, ref fromGridX, ref fromGridZ) ||
            !MapObstacle.Instance.GetObstaclePosition(dst, ref toGridX, ref toGridZ))
        {
            return false;
        }

        //Profiler.BeginSample("GetPath");
        List<MapGrid> gridPath = null;
        //if (!MapObstacle.Instance.GetPath(fromGridX, fromGridZ, toGridX, toGridZ, out gridPath)) // 获取路径
        //{
        // /*   Profiler.EndSample();*/
        //    gridPath = null;
        //    return false;
        //}
        //Profiler.EndSample();


        Profiler.BeginSample("GetPathDistance");
        //if (!MapObstacle.Instance.GetPathDistance(25, 167, 297, 131, out gridPath)) // 获取路径
        //{
        //    Profiler.EndSample();
        //    gridPath = null;
        //    return false;
        //}
        if (!MapObstacle.Instance.GetPathDistance(fromGridX, fromGridZ, toGridX, toGridZ, out gridPath)) // 获取路径
        {
            Profiler.EndSample();
            gridPath = null;
            return false;
        }
        Profiler.EndSample();
        for (int i = 0; i < gridPath.Count; ++i)
        {
            path.Add(MapGrid.GetMapPosV2(gridPath[i]));
        }

        gridPath = null;
        return true;
    }

    /// <summary>
    /// 底层寻路，找到行走路径。
    /// </summary>
    /// <param name="dst"></param>
    /// <returns></returns>
    public bool findPath(Vector2 src, Vector2 dst, float range, out List<Vector2> lstPath)
    {
        range = Mathf.Max(0, range);
        List<Vector2> path = null;
        lstPath = new List<Vector2>();
        //UnityEngine.Profiler.BeginSample("findPathFromGrid");
        if (findPathFromGrid(src, dst, out path) == false)
        {
            //UnityEngine.Profiler.EndSample();
            path = null;
            Engine.Utility.Log.Error(string.Format("自动寻路未找到路径。from({0}, {1}) to({2}, {3})", src.x, src.y, dst.x, dst.y));
            return false; // 未找到路径则不移动
        }
        //UnityEngine.Profiler.EndSample();

        //UnityEngine.Profiler.BeginSample("findPathFromGrid End");
        // 起始点和路径起点在同一个格子中时
        if (path.Count > 0)
        {
            var pathBegin = path[0];
            if (MapObstacle.Instance.IsInSameGrid(pathBegin, src))
            {
                path[0] = src;
                //path.RemoveAt(0);
            }
        }
        // 目标点和路径终点在同一个格子中时
        if (path.Count >= 1)
        {
            var pathEnd = path[path.Count - 1];
            if (MapObstacle.Instance.IsInSameGrid(pathEnd, dst))
            {
                path[path.Count - 1] = dst;
            }
        }

    
        List<Vector2> newPath = OptimizePath(path);
        // 删除起始点
        if (path.Count > 0)
        {
            path.RemoveAt(0);
        }

        // 最后一个点，判断一下范围
        if (range > 0 && path.Count > 0)
        {
            var lastPos = newPath[path.Count - 1];
            var last2Pos = src;
            if (path.Count >= 2)
            {
                last2Pos = path[path.Count - 2];
            }
            newPath.RemoveAt(path.Count - 1); // 最后一个点肯定要删除重算。
            // 计算提前停下来的位置
            var distance = Vector2.Distance(last2Pos, lastPos);
            if (GameUtil.RoughlyGreatThan(distance, range))
            {
                var realLastPos = last2Pos + ((distance - range) / distance) * (lastPos - last2Pos);
                newPath.Add(realLastPos);
            }
        }

        // 坐标转换
        for (int i = 0; i < newPath.Count; ++i)
        {
            lstPath.Add(new Vector2(newPath[i].x, -newPath[i].y));
        }

        //向大地图中添加数据
        if (this.PathFind != null)
        {
            this.PathFind(lstPath);
        }

        //UnityEngine.Profiler.EndSample();
        path = null;
        return true;
    }
    //-------------------------------------------------------------------------------------------------------
    private List<Vector2> OptimizePath(List<Vector2> path)
    {
        return LinePathEx(path);
        // return Floyd(path);
    }
    #region floyd
    List<Vector2> Floyd(List<Vector2> path)
    {
        if (path == null)
        {
            return path;
        }


        int len = path.Count;
        //去掉同一条线上的点。
        if (len > 2)
        {
            Vector3 vector = ToVector3(path[len - 1] - path[len - 2]);
            Vector3 tempvector;
            for (int i = len - 3; i >= 0; i--)
            {
                tempvector = ToVector3(path[i + 1] - path[i]);
                if (Vector3.Cross(vector, tempvector).y == 0f)
                {
                    path.RemoveAt(i + 1);
                }
                else
                {
                    vector = tempvector;
                }
            }
        }
        //去掉无用拐点
        len = path.Count;
        for (int i = len - 1; i >= 0; i--)
        {
            for (int j = 0; j <= i - 1; j++)
            {
                if (CheckCrossNoteWalkable(ToVector3(path[i]), ToVector3(path[j])))
                {
                    for (int k = i - 1; k >= j; k--)
                    {
                        path.RemoveAt(k);
                    }
                    i = j;
                    //len = path.Count;
                    break;
                }
            }
        }
        return path;
    }

    //判断路径上是否有障碍物
    public bool CheckCrossNoteWalkable(Vector3 p1, Vector3 p2)
    {
        float Tilesize = MapGrid.Width;
        bool changexz = Mathf.Abs(p2.z - p1.z) > Mathf.Abs(p2.x - p1.x);
        if (changexz)
        {
            float temp = p1.x;
            p1.x = p1.z;
            p1.z = temp;
            temp = p2.x;
            p2.x = p2.z;
            p2.z = temp;
        }
        if (!Checkwalkable(changexz, p1.x, p1.z))
        {
            return false;
        }
        float stepX = p2.x > p1.x ? Tilesize : (p2.x < p1.x ? -Tilesize : 0);
        float stepY = p2.y > p1.y ? Tilesize : (p2.y < p1.y ? -Tilesize : 0);
        float deltay = Tilesize * ((p2.z - p1.z) / Mathf.Abs(p2.x - p1.x));
        float nowX = p1.x + stepX / 2;
        float nowY = p1.z - stepY / 2;
        float CheckY = nowY;

        while (nowX != p2.x)
        {
            if (!Checkwalkable(changexz, nowX, CheckY))
            {
                return false;
            }
            nowY += deltay;
            if (nowY >= CheckY + stepY)
            {
                CheckY += stepY;
                if (!Checkwalkable(changexz, nowX, CheckY))
                {
                    return false;
                }
            }
            nowX += stepX;
        }
        return true;
    }
    private bool Checkwalkable(bool changeXZ, float x, float z)
    {
        if (changeXZ)
        {
            return MapObstacle.Instance.CanWalk((int)z, (int)x);
        }
        else
        {
            return MapObstacle.Instance.CanWalk((int)x, (int)z);
        }
    }
    #endregion
    Vector3 ToVector3(Vector2 vec)
    {
        Vector3 v3 = new Vector3(vec.x, 0, vec.y);
        return v3;
    }
    //-------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 把AStar算出的倒叙路径变正序，并拉直
    /// </summary>
    /// <returns>The path ex.</returns>
    /// <param name="srcPath">Source path.</param>
    private List<Vector2> LinePathEx(List<Vector2> srcPath)
    {
        List<Vector2> dstPath = srcPath;

        int nStartIndex = 0;
        int nEndIndex = 0;
        int nSize = dstPath.Count;
        for (; nStartIndex < nSize - 2; ++nStartIndex)
        {
            for (nEndIndex = nSize - 1; nEndIndex > nStartIndex - 1; )
            {

                //UnityEngine.Profiler.BeginSample("LinePathClear");
                //    bool bRet = CheckCrossNoteWalkable(ToVector3( dstPath[nStartIndex]),ToVector3( dstPath[nEndIndex]));
                bool bRet = LinePointClear(dstPath[nStartIndex], dstPath[nEndIndex]);
                //UnityEngine.Profiler.EndSample();
                if (bRet)
                {
                    dstPath.RemoveRange(nStartIndex + 1, nEndIndex - nStartIndex - 1);
                    nSize = dstPath.Count;
                    break;
                }
                else
                {

                    nEndIndex--;
                }
                //UnityEngine.Profiler.EndSample();
            }
        }

        // 目前这种移动同步方式，不用再切分路径了 MapNav里面切分了。
        return dstPath;
    }
    //-------------------------------------------------------------------------------------------------------
    private const float ShortestMoveDst = 0.02f;

    private ObjPool<MapGrid> MapGridPool = new ObjPool<MapGrid>();
    private bool LinePointClear(Vector2 src, Vector2 dst)
    {
       // Engine.Utility.Log.Error("test ==========================================begin");
        Vector2 test1 = new Vector2(176.5f, 194.50f);
        Vector2 test2 = new Vector2(176.5f, 159.5f);
      //  bool ret = CheckLine(test1, test2);
       // Engine.Utility.Log.Error("test ==========================================end");
        return CheckLine(src, dst);
        //MapGrid dstPt = new MapGrid();//MapGridPool.Alloc();
        //MapGrid.GetMapGrid(dst, ref dstPt);

        //Vector2 dir = dst - src;
        //dir.Normalize();
        //dir *= ShortestMoveDst;

        //MapGrid curPt = new MapGrid();
        //curPt.x = 0;
        //curPt.z = 0;
        //float fDistance = Vector2.Distance(src, dst);
        //bool bDiff = false;
        //bool bSame = false;
        //for (; fDistance > ShortestMoveDst; src += dir)
        //{
        //    int x = 0, z = 0;
        //    MapGrid.GetMapGrid(src, ref x, ref z);
        //    bDiff = !IsSameGrid(curPt, x, z);
        //    fDistance = Vector2.Distance(src, dst);

        //    if (bDiff)
        //    {
        //        curPt.x = x;
        //        curPt.z = z;
        //    }
        //    else
        //    {
        //        continue;
        //    }

        //    if (!MapObstacle.Instance.CanWalk((int)curPt.x, (int)curPt.z))
        //    {
        //        return false;
        //    }

        //    bSame = (curPt == dstPt);
        //    if (bSame)
        //    {
        //        break;
        //    }
        //}

        //return true;
    }
    bool MoreF(float x1, float x2)
    {
        return x1 >= x2;
    }
    bool LessF(float x1, float x2)
    {
        return x1 < x2;
    }
    bool CheckLine(Vector2 pt1, Vector2 pt2)
    {
        float x2 = pt2.x;
        float y2 = pt2.y;
        float x1 = pt1.x;
        float y1 = pt1.y;
        Vector2 dir = pt2 - pt1;
        dir.Normalize();
        float nx = dir.x;
        float ny = dir.y;
        float test_len = 0;
        bool ret = CheckAround(x2, y2, pt1, pt2);
       // Log.Error("ret ==== " + ret);
        if (ret ==false)
        {
        //    Log.Error("tewst====");
            return false;
        }
        while (true)
        {
            float x = x1 + nx * test_len;
            float y = y1 + ny * test_len;
            if(x >= 109)
            {
                int a = 10;
            }
            if (pt1.x >= pt2.x)
            {
                if (!MoreF(x, x2)) break;

            }
            else
            {
                if (!LessF(x, x2)) break;

            }
            if (pt1.y >= pt2.y)
            {
                if (!MoreF(y, y2)) break;
            }
            else
            {
                if (!LessF(y, y2)) break;
            }

            if (!CheckAround(x, y, pt1, pt2))
            {
                return false;
            }


            test_len += 1;
        }
        return true;
    }
    bool IsSameGrid(MapGrid grid, int x, int z)
    {
        return grid.x == x && grid.z == z;
    }
    bool CheckPt(float x, float y)
    {
        if (!MapObstacle.Instance.IsValidPt((int)x,(int) y))
        {//如果是无效点 不在检查相交性 认为可以行走 以移除噪点
            return true;
        }
        return MapObstacle.Instance.CanWalk((int)x, (int)y);

    }
    bool CheckAround(float x, float y, Vector2 pt1, Vector2 pt2)
    {
        bool ret = false;
        int xx = (int)x;
        int yy = (int)y;
        int ix, iy;
        ix = xx - 1;
        iy = yy;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        ix = xx;
        iy = yy;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        ix = xx + 1;
        iy = yy;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }

        ix = xx - 1;
        iy = yy - 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        ix = xx;
        iy = yy - 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            if (ret)
                return false;
        }
        ix = xx + 1;
        iy = yy - 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }

        ix = xx - 1;
        iy = (int)y + 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        ix = (int)x;
        iy = yy + 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        ix = xx + 1;
        iy = yy + 1;
        ret = CheckPt(ix, iy);
        if (!ret)
        {
            ret = RSIntersection(ix, iy, ix + 1, iy + 1, pt1, pt2);
            if (ret)
                return false;
        }
        return true;
    }
    /*
 * 判断区间[x1, x2]和区间[x3, x4](x4可能小于x3)是否有交集，有返回1，无0
 */
    bool IntervalOverlap(float x1, float x2, float x3, float x4)
    {
        float t;
        if (x3 > x4)
        {
            t = x3;
            x3 = x4;
            x4 = t;
        }

        if (x3 > x2 || x4 < x1)
            return false;
        else
            return true;
    }

    /*
     * 判断矩形r和线段AB是否有交集，有返回1，无0
     */
    bool RSIntersection(float xmin, float ymin, float xmax, float ymax, Vector2 A, Vector2 B)
    {
        xmax -= 0.0001f;
        ymax -= 0.0001f;
        if (A.y == B.y)	// 线段平行于x轴
        {
            if (A.y <= ymax && A.y >= ymin)
            {
                return IntervalOverlap(xmin, xmax, A.x, B.x);
            }
            else
            {
                return false;
            }
        }
        // AB两点交换，让B点的y坐标最大
        Vector2 t;
        if (A.y > B.y)
        {
            t = A;
            A = B;
            B = t;
        }

        // 在线段AB上确定点C和D
        // 两点确定一条直线: (x-x1)/(x2-x1)=(y-y1)/(y2-y1)
        float k = (B.x - A.x) / (B.y - A.y);
        Vector2 C, D;
        if (A.y < ymin)
        {
            C.y = ymin;
            C.x = k * (C.y - A.y) + A.x;
        }
        else
            C = A;

        if (B.y > ymax)
        {
            D.y = ymax;
            D.x = k * (D.y - A.y) + A.x;
        }
        else
            D = B;

        if (D.y >= C.y)	// y维上有交集
        {
            return IntervalOverlap(xmin, xmax, D.x, C.x);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 严格检查两点之间是否通畅
    /// </summary>
    /// <returns><c>true</c>, if path clear was lined, <c>false</c> otherwise.</returns>
    /// <param name="src">Vec source.</param>
    /// <param name="dst">Vec dst.</param>
    private bool LinePathClear(Vector2 src, Vector2 dst)
    {
        MapGrid dstPt = MapGridPool.Alloc();
        MapGrid.GetMapGrid(dst, ref dstPt);

        Vector2 dir = dst - src;
        dir.Normalize();
        dir *= ShortestMoveDst;

        MapGrid curPt = MapGridPool.Alloc();
        curPt.x = 0;
        curPt.z = 0;
        float fDistance = Vector2.Distance(src, dst);
        bool bDiff = false;
        bool bSame = false;
        for (; fDistance > ShortestMoveDst; src += dir)
        {
            //UnityEngine.Profiler.BeginSample("LinePathClear 1");
            MapGrid tmp = MapGridPool.Alloc();
            MapGrid.GetMapGrid(src, ref tmp);
            //UnityEngine.Profiler.EndSample();

            //UnityEngine.Profiler.BeginSample("LinePathClear same");
            bDiff = (curPt != tmp);
            //UnityEngine.Profiler.EndSample();
            //UnityEngine.Profiler.BeginSample("LinePathClear distance");
            fDistance = Vector2.Distance(src, dst);////Engine.Utility.Geometry.GetDistance(ref src, ref dst, false);
            //UnityEngine.Profiler.EndSample();

            //UnityEngine.Profiler.BeginSample("LinePathClear 2");
            if (bDiff)
            {
                curPt = tmp;
            }
            else
            {
                MapGridPool.Free(tmp);
                //UnityEngine.Profiler.EndSample();
                continue;
            }
            MapGridPool.Free(tmp);
            //UnityEngine.Profiler.EndSample();

            if (!MapObstacle.Instance.CanWalk((int)curPt.x, (int)curPt.z))
            {
                MapGridPool.Free(dstPt);
                MapGridPool.Free(curPt);
                return false;
            }

            bSame = (curPt == dstPt);
            if (bSame)
            {
                break;
            }
        }

        MapGridPool.Free(dstPt);
        MapGridPool.Free(curPt);
        return true;
    }

    /// <summary>
    /// 给服务器发送行走信息，最长路段长度
    /// </summary>
    private const float MaxPathLen = 5.0f;
    /// <summary>
    /// 两点间要是超过最大路径距离，就切分路段
    /// </summary>
    /// <param name="grid1">Grid1.</param>
    /// <param name="grid2">Grid2.</param>
    /// <param name="path">Path.</param>
    private void SplitObstacleLineEx(MapGrid grid1, MapGrid grid2, out List<MapGrid> path)
    {
        path = new List<MapGrid>();
        if (grid1.x == grid2.x && grid1.z == grid2.z)
            return;

        path = new List<MapGrid>();
        path.Clear();

        Vector2 vecSrc = MapGrid.GetMapPos(grid1).ToVector2();
        Vector2 vecDst = MapGrid.GetMapPos(grid2).ToVector2();
        Vector2 dir = vecDst - vecSrc;
        float fLength = dir.magnitude;
        if (fLength < ShortestMoveDst)
            return;
        dir.Normalize();
        dir *= ShortestMoveDst;

        Vector2 curPos = vecSrc;
        MapGrid curPt = grid1;

        path.Add(grid1);
        for (; ; )
        {
            curPos += dir;
            if (new MapGrid(curPos) == curPt)
                continue;

            curPt = new MapGrid(curPos);

            // 已经是最后一个节点，退出
            if (curPt == grid2)
            {
                if (path[path.Count - 1] != grid2)
                    path.Add(grid2);
                break;
            }

            // 大于一段路的最大长度，加入变换点
            if (path.Count > 0)
            {
                if ((MapGrid.GetMapPos(curPt).ToVector2() - MapGrid.GetMapPos(path[path.Count - 1]).ToVector2()).magnitude >= MaxPathLen)
                {
                    path.Add(curPt);
                }
            }
        }

        return;
    }

}
