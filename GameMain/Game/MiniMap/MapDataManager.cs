using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Client;
using System.Xml;
using Vector2 = UnityEngine.Vector2;
using Vectro3 = UnityEngine.Vector3;
using UnityEngine;

class ChangeLineInfo
{
    public uint lineNum;//分线号
    public uint playerNum;//分线人数

}
public enum MapDispatchEnum
{
    RefreshLineNum = 0,
    RefreshLineInfo,
}
class MapDataManager : BaseModuleData, IManager
{
    //小地图宽
    public int m_nBgWidth = 148;
    //小地图高
    public int m_nBgHeight = 148;

    /// <summary>
    /// 被渲染的地图
    /// </summary>
    public Texture MapTexture
    {
        get;
        set;
    }
    /// <summary>
    /// 地图实际宽高
    /// </summary>
    public Vector2 MapSize
    {
        get;
        set;
    }
    Vector2 textureSize = Vector2.zero;
    /// <summary>
    /// 地图对应的贴图宽高
    /// </summary>
    public Vector2 MapTextureSize
    {
        get;
        set;
    }
    Dictionary<uint, ChangeLineInfo> m_dicLine = new Dictionary<uint, ChangeLineInfo>();

    uint m_uCurLineNum = 0;
    //是否为默认值
    public bool IsDefalultLine
    {
        get
        {
            return m_uCurLineNum == 0;
        }
    }
    /// <summary>
    /// 当前所在分线
    /// </summary>
    public uint CurLineNum
    {
        get
        {
            return m_uCurLineNum;
        }
        set
        {

            m_uCurLineNum = value;
            DispatchValueUpdateEvent(new ValueUpdateEventArgs()
            {
                key = MapDispatchEnum.RefreshLineNum.ToString(),
            });
        }
    }


    //渲染地图时的角度
    public float camerRenderRotation = 45;

    public bool bDoMoving = false;

    List<NPCInfo> allNpcList = new List<NPCInfo>();
    void InitNpcDic()
    {
        allNpcList.Clear();

        IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms != null)
        {
            ms.GetAllNpcInfo(out allNpcList);
        }

    }
    public List<NPCInfo> GetNpcList()
    {
        List<NPCInfo> list = new List<NPCInfo>();
        for (int i = 0; i < allNpcList.Count; i++)
        {
            var npc = allNpcList[i];
            NpcDataBase db = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npc.npcID);
            if (db != null)
            {
                if (db.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_FUNCTION)
                {
                    //if (db.dwMonsterType != 0)
                    {
                        list.Add(npc);
                    }

                }
            }
        }

        return list;
    }
    bool HasContain(List<NPCInfo> npcList, NPCInfo info)
    {
        bool bContain = false;
        for (int i = 0; i < allNpcList.Count; i++)
        {
            var npc = allNpcList[i];
            if (npc.pos.x == info.pos.x && npc.pos.y == info.pos.y)
            {
                bContain = true;
            }
        }
        return bContain;
    }
    public List<NPCInfo> GetMonsterList()
    {
        List<NPCInfo> list = new List<NPCInfo>();

        for (int i = 0; i < allNpcList.Count; i++)
        {
            var npc = allNpcList[i];
            NpcDataBase db = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npc.npcID);
            if (db != null)
            {
                if (db.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_NONE)
                {
                    if (db.dwMonsterType != 0)
                    {
                        list.Add(npc);
                    }

                }
            }
        }

        return list;
    }
    public List<NPCInfo> GetTransmitList()
    {
        List<NPCInfo> list = new List<NPCInfo>();

        for (int i = 0; i < allNpcList.Count; i++)
        {
            var npc = allNpcList[i];
            NpcDataBase db = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npc.npcID);
            if (db != null)
            {
                if (db.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRANSFER)
                {

                    list.Add(npc);


                }
            }
        }
        return list;
    }
    public int GetCurMapID()
    {
        IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms != null)
        {
            uint mapID = ms.GetMapID();
            return (int)mapID;
        }
        return -1;
    }
    //string GetMapXmlName()
    //{
    //    IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
    //    if ( ms != null )
    //    {
    //        uint mapID = ms.GetMapID();
    //        table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>( mapID );
    //        if ( mapDB == null )
    //        {
    //            Engine.Utility.Log.Error( "MapSystem:找不到地图配置数据{0}" , mapID );
    //            return null;
    //        }

    //        return mapDB.miniMapInfo;
    //    }
    //    return null;
    //}
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTERMAP)
        {

            InitNpcDic();
            BigMapPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.BigMapPanel) as BigMapPanel;
            if (panel != null)
            {
                panel.ClearScrollItem();
            }
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_LEAVEMAP)
        {
            //  DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MiniMapPanel, needDestroy: true);
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
        {
            if (bDoMoving)
            {
                Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
                if (stopEntity.uid == MainPlayerHelper.GetPlayerUID())
                {
                    BigMapPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.BigMapPanel) as BigMapPanel;
                    if (panel != null)
                    {
                        panel.ClearIcon();
                    }
                    bDoMoving = false;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);
                }
            }
        }
    }
    public Vector2 GetTextUVByTopView()
    {
        Vector2 uv = GetUVByTopView(MainPlayerHelper.GetMainPlayer());
        if (MapTexture == null)
        {
            //  Engine.Utility.Log.Error( "获取贴图失败" );
            return Vector2.zero;
        }
        int width = MapTexture.width;
        int height = MapTexture.height;
        float Xdelta = 0;
        float Ydelta = 0;
        float u = uv.x - Xdelta;
        float v = uv.y - Ydelta;
        Vector2 mapUv = new Vector2(u, v);
        return mapUv;
    }
    public Vector2 GetUVByTopView(IEntity entity)
    {
        if (entity == null)
        {
            return Vector2.zero; ;
        }
        Vector3 pos = entity.GetPos();

        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, -camerRenderRotation, 0);
        //旋转坐标系
        pos = rot * pos;
        float fsize = (MapSize.x + MapSize.y) * (float)Math.Sin(camerRenderRotation / 180.0f * Math.PI);
        pos.z += fsize * ((float)MapSize.y) / (MapSize.x + MapSize.y);
        // h/(h+w)

        float xPos = pos.x / fsize;
        float yPos = pos.z / fsize;

        Vector2 uv = new Vector2(xPos, yPos);
        //Vector2 uv = new Vector2( testPos.x , testPos.z );
        return uv;

    }
    void GetPosOnUV()
    {

    }
    public void OnReceiveChangeLineInfo(GameCmd.stResponLineInfoMapScreenUserCmd_S info)
    {
        m_dicLine.Clear();
        for (int i = 0; i < info.line.Count; i++)
        {
            ChangeLineInfo clinfo = new ChangeLineInfo();
            clinfo.lineNum = info.line[i];
            clinfo.playerNum = info.player_cnt[i];
            if (m_dicLine.ContainsKey(info.line[i]))
            {
                m_dicLine[info.line[i]] = clinfo;
            }
            else
            {
                m_dicLine.Add(info.line[i], clinfo);
            }
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs()
        {
            key = MapDispatchEnum.RefreshLineInfo.ToString(),
        });

    }
    int LineSort(ChangeLineInfo left ,ChangeLineInfo right) 
    {
        return (int)left.lineNum - (int)right.lineNum;
    }
    public List<ChangeLineInfo> GetLineInfoList()
    {
        List<ChangeLineInfo> list = m_dicLine.Values.ToList<ChangeLineInfo>();
        list.Sort(LineSort);
        return list;
    }
    #region Imanager
    public void ClearData()
    {
        RegisterEvents(false);
    }
    public void Initialize()
    {
        RegisterEvents(true);
    }
    void RegisterEvents(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTERMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTERMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
        }
    }
    public void Reset(bool depthClearData = false)
    {
        RegisterEvents(true);
        if (depthClearData)
        {
            m_uCurLineNum = 0;
        }
    }

    public void Process(float deltaTime)
    {
        BigMapPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.BigMapPanel) as BigMapPanel;
        if (panel != null)
        {
            panel.DequeIcon();
        }
    }

    public void SyncTeammatePos(GameCmd.stSynTeamPosRelationUserCmd_S cmd)
    {
        BigMapPanel panel = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.BigMapPanel) as BigMapPanel;
        if (panel != null)
        {
            panel.SyncTeammatePos(cmd);
        }
    }
    #endregion
}


