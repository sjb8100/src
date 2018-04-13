/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Main
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MainPanel_MiniMap
 * 版本号：  V1.0.0.0
 * 创建时间：7/10/2017 2:11:42 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector2 = UnityEngine.Vector2;
using UnityEngine;
using Client;
using GameCmd;
using System.Collections;
using Engine;
using UnityEngine.Profiling;
public enum IconType
{
    playericon = 1,//主角
    mastericon,//怪物
    npcicon,//npc
    duiyouicon,//队友
    otherplayericon,//其他玩家
    peticon,//宠物
    pathpointicon,//寻路点
    robot,//机器人
}

partial class MainPanel
{
    #region define
    MapDataManager MapData
    {
        get
        {
            return DataManager.Manager<MapDataManager>();
        }
    }
    #endregion

    #region property
    Dictionary<string, List<long>> entityTable = new Dictionary<string, List<long>>();
    Dictionary<long, GameObject> iconDic = new Dictionary<long, GameObject>();
    Transform quadTrans;
    Material mapMat;
    Texture mapTex;
    //IPlayer mainPlayer;

    Vector2 mapSize;
    float uScale = 0;
    float vScale = 0;
    public float test = 0;

    Transform playerIcon;
    bool bHideMap = false;
    uint m_mapid = 0;
    Engine.ITexture m_texture;
    Engine.ITexture m_maskTextrue;
    //
    public int renderQueue = 3010;
    bool bTopView = true;//顶视图

    QuadMesh m_QuadMesh;//修改小地图mesh的脚本

    Transform m_pointParentTrans;
    Transform m_transMainPlayerIcon;
    //小地图背景的宽高
    int m_nBackGroundWidth = 0;
    int m_nBackGroundHeight = 0;

    #endregion
    #region InitMiniMap
    private void InitMiniMap()
    {
        //   quadTrans = m_widget_Root.transform.Find( "Quad" );
        quadTrans = m_trans_minimapObj;
        if (quadTrans != null)
        {
            //Collider col = quadTrans.GetComponent<MeshCollider>();
            //if ( col != null ) col.enabled = false;
            //quadTrans.gameObject.AddComponent<QuadMesh>();
        }
        m_QuadMesh = quadTrans.GetComponent<QuadMesh>();
        if (m_QuadMesh == null)
        {
            m_QuadMesh = quadTrans.gameObject.AddComponent<QuadMesh>();
        }

        playerIcon = m_widget_Root.transform.Find("IconContainer/playericon");
        MeshRenderer mr = quadTrans.GetComponent<MeshRenderer>();
        mapMat = mr.material;


        mapMat.shader = Resources.Load("Shaders/Custom/Mask") as Shader;
        //        UIParticleWidget pw = quadTrans.GetComponent<UIParticleWidget>();
        //         if(pw != null)
        //         {
        //             pw.SetParticleDirty();
        //         }
        //mainPlayer = ClientGlobal.Instance().MainPlayer;

        entityTable.Add(GetStrByIconType(IconType.playericon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.npcicon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.mastericon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.otherplayericon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.duiyouicon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.peticon), new List<long>());
        entityTable.Add(GetStrByIconType(IconType.robot), new List<long>());

        //if (m_slider_Zoomslider != null)
        //{
        //    //UISlider[] sliders = obj.GetComponentsInChildren<UISlider>();
        //    //m_slider_Zoomslider.onChange.Add(OnClick_Zoomslider);

        //    EventDelegate.Add(m_slider_Zoomslider.onChange, OnClick_Zoomslider);
        //}
        m_pointParentTrans = m_trans_minimapObj;
        
        string maskPath = BigMapPanel.MINIMAP_TEXTURE_PATH + "minimapmask.unity3d";
        if (m_maskTextrue != null)
        {
            m_maskTextrue.Release();
            m_maskTextrue = null;
        }
        Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref maskPath, ref m_maskTextrue, CreateTextureEvent, null, Engine.TaskPriority.TaskPriority_Immediate);
        RegisterMiniMapEvents(true);
    }
    string GetStrByIconType(IconType type)
    {
        return MapIconPoolManager.Instance.GetStringByType(type);
    }
    void HideMiniMap()
    {
        MapData.ValueUpdateEvent -= MapData_ValueUpdateEvent;
    }
    private void ShowMiniMap()
    {
        MapData.ValueUpdateEvent += MapData_ValueUpdateEvent;
        IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
        if (m_mapid != mapsys.GetMapID())
        {
            m_mapid = mapsys.GetMapID();
            string formatString = BigMapPanel.MINIMAP_TEXTURE_PATH + "{0}.unity3d";
            if (m_texture == null)
            {
                string path = string.Format(formatString, Client.ClientGlobal.Instance().GetMapSystem().GetMapTextureName());
                Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref path, ref m_texture, CreateTextureEvent, null, Engine.TaskPriority.TaskPriority_Immediate);
            }
            //if (m_texture != null)
            //{
            //    mapMat.mainTexture = m_texture.GetTexture();
            //    mapTex = mapMat.mainTexture;
            //    if (mapTex == null)
            //        return;
            //    MapData.MapTexture = mapTex;
            //    table.MapDataBase mdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(m_mapid);
            //    if (mdb != null)
            //    {
            //        int texWidth = (int)mdb.textureWidth;
            //        mapMat.SetTexture("_Mask", m_maskTextrue.GetTexture());
            //        MapData.MapTextureSize = new Vector2(texWidth, texWidth);//new Vector2(  MapData.MapTextureSize.x , (float) MapData.MapTextureSize.y );
            //        Vector3 scale = new Vector3(texWidth, texWidth, 1);
            //        if (bTopView)
            //        {
            //            scale = new Vector3(1, 1, 1);
            //        }
            //        quadTrans.localScale = scale;
            //    }

            //    mapSize = Client.ClientGlobal.Instance().GetMapSystem().GetMapSize();
            //    MapData.MapSize = mapSize;
            //}
            uint lineNum = MapData.CurLineNum;
            bool bIsCopy = false;
            List<table.CopyDataBase> copyList = GameTableManager.Instance.GetTableList<table.CopyDataBase>();
            foreach(var copy in copyList)
            {
                if(copy.mapId == m_mapid)
                {
                    bIsCopy = true;
                    break;
                }
            }
         
            if (m_btn_btn_changeLine != null)
            {
                if(bIsCopy)
                {
                    m_btn_btn_changeLine.gameObject.SetActive(false);
                    lineNum = 1;
                }
                else
                {
                    m_btn_btn_changeLine.gameObject.SetActive(true);
                }
            }
            if (m_label_MapName != null)
            {
                m_label_MapName.text = string.Format("{0}({1}{2})", mapsys.GetMapName(), lineNum, CommonData.GetLocalString("线"));
            }

            Transform bgTrans = m_widget_Root.transform.Find("BackGround");
            if (bgTrans != null)
            {
                UISprite bgSpr = bgTrans.GetComponent<UISprite>();
                if (bgSpr != null)
                {
                    m_nBackGroundWidth = bgSpr.width;
                    m_nBackGroundHeight = bgSpr.height;
                }
            }
            AddMainPlayerIcon();
        }
    }
    private void CreateTextureEvent(ITexture obj, object param = null)
    {
        if (m_texture != null && m_maskTextrue != null && null != mapMat && null != quadTrans)
        {
            mapMat.mainTexture = m_texture.GetTexture();
            mapTex = mapMat.mainTexture;
            if (mapTex == null)
                return;
            MapData.MapTexture = mapTex;
            table.MapDataBase mdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(m_mapid);
            if (mdb != null)
            {
                int texWidth = (int)mdb.textureWidth;
                mapMat.SetTexture("_Mask", m_maskTextrue.GetTexture());
                MapData.MapTextureSize = new Vector2(texWidth, texWidth);//new Vector2(  MapData.MapTextureSize.x , (float) MapData.MapTextureSize.y );
                Vector3 scale = new Vector3(texWidth, texWidth, 1);
                if (bTopView)
                {
                    scale = new Vector3(1, 1, 1);
                }
                quadTrans.localScale = scale;
            }

            mapSize = Client.ClientGlobal.Instance().GetMapSystem().GetMapSize();
            MapData.MapSize = mapSize;
        }
    }
    void RegisterMiniMapEvents(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
        }
    }
    #endregion

    #region OP
    void MapData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e.key == MapDispatchEnum.RefreshLineNum.ToString())
        {
            IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
            if (mapsys != null)
            {
                if (m_label_MapName != null)
                {
                    m_label_MapName.text = string.Format("{0}({1}{2})", mapsys.GetMapName(), MapData.CurLineNum, CommonData.GetLocalString("线"));
                }
            }
        }
    }

    void SetPlayerIconFoward()
    {
        Profiler.BeginSample("SetPlayerIconFoward");
        if (MainPlayerHelper.GetMainPlayer() != null)
        {
            IPlayer player = MainPlayerHelper.GetMainPlayer();
            Vector3 myPos = player.GetPos();
            Vector3 dir = player.GetDir();
            Vector3 targetPos = myPos + dir;
            Vector2 uv = GetUVByPos(targetPos);
            Vector2 mapUV = GetUV(MainPlayerHelper.GetMainPlayer());
            Vector2 deltaUV = (uv - mapUV);

            float iconPosx = deltaUV.x * MapData.MapTextureSize.x;
            float iconPosy = deltaUV.y * (float)MapData.MapTextureSize.y;

            Vector3 forwardPos = new Vector3(iconPosx + m_nBackGroundWidth / 2, iconPosy + m_nBackGroundHeight / 2, 0);

            Vector3 iconDir = forwardPos - m_transMainPlayerIcon.localPosition;
            Vector3 cross = Vector3.Cross(iconDir, Vector3.up);
            float angle = Vector3.Angle(iconDir, Vector3.up);
            // Engine.Utility.Log.Error("angle is " + angle + "cross is "+cross);
            if (cross.z < 0)
            {
                angle = 360 - angle;
            }
            Quaternion qua = Quaternion.AngleAxis(-angle, Vector3.forward);
            if (m_transMainPlayerIcon != null)
            {
                m_transMainPlayerIcon.rotation = qua;
            }
        }
        Profiler.EndSample();
    }
    string GetAreaTypeByPos()
    {
        MapAreaType type = ClientGlobal.Instance().GetMapSystem().GetAreaTypeByPos(MainPlayerHelper.GetMainPlayer().GetPos());
        if (type == MapAreaType.Battle)
        {
            return CommonData.GetLocalString("对战区");
        }
        else if (type == MapAreaType.Block)
        {
            return CommonData.GetLocalString("阻挡区");
        }
        else if (type == MapAreaType.Boss)
        {
            return CommonData.GetLocalString("首领区");
        }
        else if (type == MapAreaType.Normal)
        {
            return CommonData.GetLocalString("普通区");
        }
        else if (type == MapAreaType.PK)
        {
            return CommonData.GetLocalString("竞技区");
        }
        else if (type == MapAreaType.Safe)
        {
            return CommonData.GetLocalString("安全区");
        }
        else if (type == MapAreaType.Fish)
        {
            return CommonData.GetLocalString("钓鱼区");
        }
        else
        {
            return "";
        }
    }
    StringBuilder m_posStr = new StringBuilder(15);
    int m_nFrameNum = 0;
    int lastPlayerPosX = 0;
    int lastPlayerPosY = 0;
    void MiniMapUpdate()
    {
        m_nFrameNum++;
        if (m_nFrameNum > 100000)
        {
            m_nFrameNum = 0;
        }
        if (m_nFrameNum % 5 != 0)
        {
            return;
        }
        Profiler.BeginSample("minimapupdate");
        if (MainPlayerHelper.GetMainPlayer() != null)
        {
            Vector3 playerPos = MainPlayerHelper.GetMainPlayer().GetPos();
            int x = (int)playerPos.x;
            int y = (int)playerPos.z;
            if (x != lastPlayerPosX || y != lastPlayerPosY)
            {
                Profiler.BeginSample(" fomat posstr");
                lastPlayerPosY = y;
                lastPlayerPosX = x;
                m_posStr.Remove(0, m_posStr.Length);
                m_posStr.Append("(");
                m_posStr.Append(x);
                m_posStr.Append(",");
                m_posStr.Append(-y);
                m_posStr.Append(")");
                // string posstr = string.Format("({0},{1})", (int)playerPos.x, (int)-playerPos.z);

                m_label_PlayerPos.text = m_posStr.ToString();
                Profiler.EndSample();
            }


            Profiler.BeginSample("GetAreaTypeByPos");
            // string strAreadesc = ClientGlobal.Instance().GetMapSystem().GetAreaTypeByPos(MainPlayerHelper.GetMainPlayer().GetPos()).GetEnumDescription();

            m_label_area.text = GetAreaTypeByPos();
            Profiler.EndSample();
            if (mapTex == null)
                return;

            CreateIcon();
            SetPlayerIconFoward();
            Profiler.BeginSample("renderQueue");
            if (m_sprite_minimapbg.drawCall != null)
            {
                int que = m_sprite_minimapbg.drawCall.sortingOrder;
                MeshRenderer mr = quadTrans.GetComponent<MeshRenderer>();
                mr.sortingOrder = que + 1;
            }
            Profiler.EndSample();

        }

        Profiler.EndSample();
    }
    //void onClick_Btn_exit_Btn(GameObject caster)
    //{
    //    TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, "即将离开副本，是否继续？", () =>
    //    {
    //        ComBatCopyDataManager copyData = DataManager.Manager<ComBatCopyDataManager>();
    //        if (copyData != null)
    //        {
    //            stExitCopyUserCmd_CS cmd = new stExitCopyUserCmd_CS();
    //            cmd.copy_base_id = copyData.EnterCopyID;
    //            NetService.Instance.Send(cmd);
    //        }
    //    });
    //}

    Vector2 GetTexUV()
    {
        Vector2 uv = GetUV(MainPlayerHelper.GetMainPlayer());
        if (mapTex == null)
        {
            Engine.Utility.Log.Error("获取贴图失败");
            return Vector2.zero;
        }
        int width = (int)MapData.MapTextureSize.x;
        int height = (int)MapData.MapTextureSize.y;
        float Xdelta = 0;// (float)( bgWidth / 2 * 1.0f / width * 1.0f );
        float Ydelta = 0;// (float)( bgHeight / 2 * 1.0f / height * 1.0f );
        float u = uv.x - Xdelta;
        float v = uv.y - Ydelta;
        Vector2 mapUv = new Vector2(u, v);
        return mapUv;
    }
    public float testCame = 45;
    public float mathRot = 45;
    public float icontest = 45;
    Vector2 GetUV(IEntity entity)
    {

        Vector3 pos = entity.GetPos();
        return GetUVByPos(pos);

    }
    Vector2 GetUVByPos(Vector3 pos)
    {
        float rotAngle = DataManager.Manager<MapDataManager>().camerRenderRotation;
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(test, -testCame, 0);
        //旋转坐标系
        pos = rot * pos;
        float fsize = (mapSize.x + mapSize.y) * (float)Math.Sin(mathRot / 180.0f * Math.PI);
        pos.z += fsize * ((float)mapSize.y) / (mapSize.x + mapSize.y);


        float xPos = pos.x / fsize - 0.5f;
        float yPos = pos.z / fsize - 0.5f;

        Vector2 uv = new Vector2(xPos, yPos);
        //Vector2 uv = new Vector2( testPos.x , testPos.z );
        return uv;
    }
    void onClick_Shrink_Btn(GameObject caster)
    {
        bHideMap = true;
        ToggleObjs();

    }
    void onClick_Expansion_Btn(GameObject caster)
    {
        bHideMap = false;
        ToggleObjs();

    }

    private void ToggleObjs()
    {
        m_btn_BackGround.gameObject.SetActive(!bHideMap);
        quadTrans.gameObject.SetActive(!bHideMap);
        //m_btn_expansion.gameObject.SetActive(bHideMap);
        Transform t = m_widget_Root.transform.Find("IconContainer");
        if (t != null)
        {
            t.gameObject.SetActive(!bHideMap);
        }
        m_label_area.enabled = !bHideMap;
    }
    void onClick_Btn_changeLine_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChangeLinePanel);
        stRequestLineInfoMapScreenUserCmd_C cmd = new stRequestLineInfoMapScreenUserCmd_C();
        NetService.Instance.Send(cmd);

    }

    void onClick_Viptransmit_Btn(GameObject caster)
    {
        if (DataManager.Manager<Mall_HuangLingManager>().NobleID <= 1)
        {
            TipsManager.Instance.ShowTipsById(515);
            ItemManager.DoJump(112);
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcDeliverPanel);
    }

    void onClick_ChinaBible_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChinaBiblePanel);
    }

    void ReleaseTexture()
    {
        if (m_texture != null)
        {
            m_texture.Release();
            m_texture = null;
        }
        m_mapid = 0;
    }
    #endregion

    #region Icon
    void AddMainPlayerIcon()
    {
        if (m_transMainPlayerIcon != null)
        {
            return;
        }
        if (MainPlayerHelper.GetMainPlayer() != null)
        {
            GameObject go = MapIconPoolManager.Instance.GetIcon(IconType.playericon, MainPlayerHelper.GetMainPlayer(), m_widget_Root.transform);
            if (go != null)
            {
                go.SetActive(true);
                m_transMainPlayerIcon = go.transform;

                m_transMainPlayerIcon.parent = m_pointParentTrans;
                m_transMainPlayerIcon.localScale = Vector3.one;
                m_transMainPlayerIcon.localRotation = Quaternion.identity;
                //new Vector3( iconPosx +bgWidth/2 , iconPosy+bgHeight/2 , 0 );
                m_transMainPlayerIcon.localPosition = new Vector3(m_nBackGroundWidth / 2, m_nBackGroundHeight / 2, 0);
            }
        }
    }
    /// <summary>
    /// 移除半径外的icon
    /// </summary>
    /// <param name="npcList"></param>
    void RemoveIcon(IList npcList, IconType type)
    {
        Profiler.BeginSample("RemoveIcon");
        List<long> oldNpcList;
        if (entityTable.TryGetValue(GetStrByIconType(type), out oldNpcList))
        {
            Profiler.BeginSample("remove");
            for (int i = 0; i < oldNpcList.Count; i++)
            {
                long entityUID = oldNpcList[i];
                bool bContain = false;
                Profiler.BeginSample("remove 1");
                for (int m = 0; m < npcList.Count; m++)
                {
                    var npc = npcList[m] as IEntity;
                    if (npc.GetUID() == entityUID)
                    {
                        bContain = true;
                        break;
                    }
                }
                Profiler.EndSample();
                if (!bContain)
                {
                    GameObject go;
                    if (iconDic.TryGetValue(entityUID, out go))
                    {
                        MapIconPoolManager.Instance.ReturnIcon(go);
                        iconDic.Remove(entityUID);
                    }
                }

            }
            Profiler.EndSample();
        }
        Profiler.EndSample();
    }
    void AddIcon(IList npcList, IconType type)
    {
        for (int j = 0; j < npcList.Count; j++)
        {
            IEntity entity = npcList[j] as IEntity;
            if (entity != null)
            {

                if (entity is INPC)
                {
                    INPC npc = entity as INPC;

                    if (npc != null)
                    {
                        if(npc.IsTrap())
                        {//陷阱不显示
                            continue;
                        }
                        if (npc.IsMonster())
                        {
                            type = IconType.mastericon;
                        }
                        else if (npc.IsPet())
                        {
                            type = IconType.peticon;
                        }
                        else
                        {

                            type = IconType.npcicon;
                        }
                    }
                }

                if (entity is IRobot)
                {
                    type = IconType.robot;
                }

                if (entity is IPlayer)
                {
                    IPlayer player = entity as IPlayer;
                    if (player != null)
                    {
                        type = GetIconTypeByPlayer(player);
                    }
                }

                GameObject iconGo;
                if (!iconDic.TryGetValue(entity.GetUID(), out iconGo))
                {
                    iconGo = MapIconPoolManager.Instance.GetIcon(type, entity, m_widget_Root.transform);
                    iconDic.Add(entity.GetUID(), iconGo);
                    AddIconToMap(iconGo, entity);
                }
                else
                {

                    iconGo.name = MapIconPoolManager.Instance.GetIconName(type, entity);
                    UISprite spr = iconGo.GetComponent<UISprite>();
                    if(spr != null)
                    {
                        SetIconSprite(type, spr);
                    }
                    AddIconToMap(iconGo, entity);
                }
            }

        }


        List<long> lstUID = entityTable[GetStrByIconType(type)];
        if (lstUID != null)
        {
            lstUID.Clear();
            for (int i = 0; i < npcList.Count; ++i)
            {
                IEntity entity = npcList[i] as IEntity;
                if (entity == null)
                {
                    continue;
                }
                lstUID.Add(entity.GetUID());
            }
        }


    }
    void SetIconSprite(IconType type,UISprite spr)
    {
        string sprName = "";
        if(type == IconType.playericon)
        {
            sprName = "yuandian-hese";
        }
        else if(type == IconType.mastericon)
        {
            sprName = "yuandian-hongse";
        }
        else if(type == IconType.duiyouicon)
        {
            sprName = "yuandian-qianlanse";
        }
        else if (type == IconType.npcicon)
        {
            sprName = "yuandian-huangse";
        }
        else if (type == IconType.otherplayericon)
        {
            sprName = "yuandian-lanse";
        }
        else if (type == IconType.peticon)
        {
            sprName = "yuandian-qianlanse";
        }
        else if (type == IconType.pathpointicon)
        {
            sprName = "yuandian-lujingdian";
        }
        else if (type == IconType.robot)
        {
            sprName = "yuandian-lanse";
        }
        if(spr != null&&!string.IsNullOrEmpty(sprName))
        {
            spr.spriteName = sprName;
        }
    }
    void CreateNpcIcon(List<INPC> npcList)
    {
        //先移除不在半径范围内的
        RemoveIcon(npcList, IconType.npcicon);
        RemoveIcon(npcList, IconType.mastericon);
        RemoveIcon(npcList, IconType.peticon);
        AddIcon(npcList, IconType.npcicon);

    }

    void CreateOtherPlayerIcon(List<IPlayer> npcList)
    {
        //先移除不在半径范围内的
        RemoveIcon(npcList, IconType.otherplayericon);
        RemoveIcon(npcList, IconType.duiyouicon);
        AddIcon(npcList, IconType.otherplayericon);


    }
    void CreateRobotPlayerIcon(List<IRobot> npcList)
    {
        //先移除不在半径范围内的
        RemoveIcon(npcList, IconType.robot);
        AddIcon(npcList, IconType.robot);


    }
    IconType GetIconTypeByPlayer(IEntity en)
    {
        if (DataManager.Manager<TeamDataManager>().IsMember(en.GetID()))
        {
            return IconType.duiyouicon;
        }
        else
        {
            return IconType.otherplayericon;
        }
    }
    List<IPlayer> m_otherPlayerList = new List<IPlayer>();
    List<IPlayer> m_duiyouList = new List<IPlayer>();
    List<IPlayer> m_playerList = new List<IPlayer>();
    List<INPC> m_npcList = new List<INPC>();
    List<IRobot> m_robotList = new List<IRobot>();
    void CreateIcon()
    {

        if (mapTex == null)
        {
            return;
        }

        Profiler.BeginSample("CreateIcon");
        float radius = (float)(MapData.m_nBgWidth / 2 * mapSize.x / MapData.MapTextureSize.x);
        float screen = (uint)GameCmd.GameCmdConst.SCREEN_GRID_WIDTH * 3 / 2;//使用9屏半径
        radius = radius>screen?screen:radius;
        Profiler.BeginSample("getlist");
        EntitySystem.EntityHelper.GetPlayerListByCricle(radius, ref m_playerList);
        EntitySystem.EntityHelper.GetNpcListByCricle(radius, ref m_npcList);
        EntitySystem.EntityHelper.GetRobotListByCricle(radius, ref m_robotList);
        Profiler.EndSample();

        Profiler.BeginSample("CreateSubIcon");

        CreateNpcIcon(m_npcList);
        CreateOtherPlayerIcon(m_playerList);
        CreateRobotPlayerIcon(m_robotList);
        Profiler.EndSample();

        m_playerList.Clear();
        m_npcList.Clear();
        m_robotList.Clear();

        Profiler.EndSample();
    }
    void CreateDuiyouIcon(IList npcList)
    {
        RemoveIcon(npcList, IconType.duiyouicon);
        AddIcon(npcList, IconType.duiyouicon);

    }
    void AddIconToMap(GameObject icon, IEntity entity)
    {
        Profiler.BeginSample("AddIconToMap");
        Vector2 entityUV = GetUV(entity);
        Vector2 mapUV = GetUV(MainPlayerHelper.GetMainPlayer());
        Vector2 deltaUV = (entityUV - mapUV);

        float iconPosx = deltaUV.x * MapData.MapTextureSize.x;
        float iconPosy = deltaUV.y * (float)MapData.MapTextureSize.y;


        icon.transform.parent = m_pointParentTrans;
        icon.transform.localScale = Vector3.one;
        icon.transform.localRotation = Quaternion.identity;
        icon.transform.localPosition = new Vector3(iconPosx + m_nBackGroundWidth / 2, iconPosy + m_nBackGroundHeight / 2, 0);
        icon.SetActive(true);
        Profiler.EndSample();
    }
    #endregion

    #region UIEvent

    void OnClick_Zoomslider()
    {
        //if (m_slider_Zoomslider != null)
        //{
        //    CameraFollow.Instance.SetCameraOffset(m_slider_Zoomslider.value);
        //}
    }

    void onClick_Done_Btn(GameObject caster)
    {

    }
    void onClick_BackGround_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BigMapPanel);
        //PanelManager.Me.ShowPanel( PanelID.BigMapPanel );
    }
    #endregion


}