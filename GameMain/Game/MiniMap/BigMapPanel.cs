using UnityEngine;
using System.Collections.Generic;
using System;
using Client;
using System.Text;
using Engine.Utility;
using System.Collections;
using table;
using Engine;

partial class BigMapPanel : UIPanelBase
{
    public const string MINIMAP_TEXTURE_PATH = "ui/texture/pack/minimap/";
    UITexture mapTex;
    IPlayer mainPlayer;
    Transform playerIconTrans;
    Vector2 mapSize;
    /// <summary>
    /// 地图上显示的所有小图片列表
    /// </summary>
    List<GameObject> m_showIconList = new List<GameObject>();
    List<GameObject> m_pathIconList = new List<GameObject>();
    List<Vector2> PathPointList = new List<Vector2>();
    List<GameObject> m_duiyouList = new List<GameObject>();

    List<NPCInfo> showNpcList = new List<NPCInfo>();
    Dictionary<MapScrollItemType, bool> showDic = new Dictionary<MapScrollItemType, bool>() 
    { 
    {MapScrollItemType.NPC , false} ,
    { MapScrollItemType.Monster, false },
    { MapScrollItemType.Transmit , false} 
    };
    bool bShowSmallItem = false;
    uint m_mapid = 0;
    Engine.ITexture m_texture;
    Dictionary<MapScrollItemType, Transform> btnDic = new Dictionary<MapScrollItemType, Transform>();

    Dictionary<int, Texture> m_dicWorldMapTexture = new Dictionary<int, Texture>();
    Dictionary<int, Transform> m_dicWorldMapTrans = new Dictionary<int, Transform>();

    Transform m_transDuiyouIcon = null;


    /// <summary>
    /// 队员信息list
    /// </summary>
    List<TeamMemberInfo> m_listTeamMember = new List<TeamMemberInfo>();


    public int renderQueue = 3010;
    //贴图宽度
    float m_mapTextureWidth = 0;
    //贴图高度
    float m_mapTextureHeight = 0;
    MapDataManager mapDataManager
    {
        get
        {
            return DataManager.Manager<MapDataManager>();
        }
    }

    Vector3 originPos = new Vector3(-120, -19, 0);


    protected override void OnLoading()
    {
        base.OnLoading();
        RegisterEvents(true);
        mapTex = m_widget_BigMapContent.transform.Find("MapTexture").GetComponent<UITexture>();

        mainPlayer = ClientGlobal.Instance().MainPlayer;
        GameObject goIcon = MapIconPoolManager.Instance.GetIcon(IconType.playericon, mainPlayer, m_widget_BigMapContent.transform);
        playerIconTrans = goIcon.transform;
        UISprite spr = goIcon.GetComponent<UISprite>();
        if (spr != null)
        {
            spr.depth = renderQueue;
        }
        AddIconToMap(goIcon, Vector3.zero);
        GameObject tex = m_widget_BigMapContent.transform.Find("MapTexture").gameObject;
        UIEventListener.Get(tex).onClick = MapClick;
        btnDic.Add(MapScrollItemType.NPC, m_trans_npcitemContainer.transform);
        btnDic.Add(MapScrollItemType.Monster, m_trans_monsterContainer.transform);
        btnDic.Add(MapScrollItemType.Transmit, m_trans_transmitContainer);
        GameObject go = new GameObject("duiyou");
        go.transform.parent = m_widget_BigMapContent.transform;
        m_transDuiyouIcon = go.transform;
        m_transDuiyouIcon.transform.localPosition = Vector3.zero;
        m_transDuiyouIcon.transform.localScale = Vector3.one;
        m_transDuiyouIcon.transform.localRotation = Quaternion.identity;
        m_trans_IconContainer.gameObject.SetActive(false);

        if (m_ctor_bgContent != null)
        {
            GameObject cloneFTemp = m_label_ChildPrefab.gameObject;
            m_ctor_bgContent.Initialize<UIBtnTipsGrid>(cloneFTemp, OnGridDataUpdate, OnGridUIEvent);
        }
    }
    private void OnGridDataUpdate(UIGridBase data, int index)
    {

        if (data is UIBtnTipsGrid)
        {
            UIBtnTipsGrid tab = data as UIBtnTipsGrid;
            tab.SetGridData(mapData[index].dwID);
            tab.SetName(mapData[index].strName);
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIBtnTipsGrid)
                {
                    UIBtnTipsGrid tab = data as UIBtnTipsGrid;
                    GotoMap((int)tab.Data);
                }
                break;
        }
    }

    void RegisterEvents(bool bReg)
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
    void ReleaseTexture()
    {
        if (m_texture != null)
        {
            m_texture.Release();
            m_texture = null;
        }
    }
    void OnEvent(int eventID, object param)
    {

        if (eventID == (int)GameEventID.ENTITYSYSTEM_LEAVEMAP)
        {
            ReleaseTexture();
            m_mapid = 0;
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterEvents(false);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        UIEventListener.Get(m__wordBg.gameObject).onClick = OnWorldMapClick;
        IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
        if (mapsys == null)
        {
            Engine.Utility.Log.Error("Mapsystem is null");
            return;
        }

        if (m_mapid != mapsys.GetMapID())
        {
            if (m_texture == null)
            {
                string path = MINIMAP_TEXTURE_PATH + mapsys.GetMapTextureName() + ".unity3d";
                Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref path, ref m_texture, CreateTextureEvent, path, Engine.TaskPriority.TaskPriority_Immediate);

            }
            ClearShowIcon();


            mapSize = mapsys.GetMapSize();

            if (m_label_MapName != null)
            {
                m_label_MapName.text = mapsys.GetMapName();
            }
            m_mapid = mapsys.GetMapID();
            table.MapDataBase mdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(m_mapid);
            if (mdb != null)
            {
                ShowIconOnMap(mdb);
            }
        }

        int count = m__wordBg.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform subItem = m__wordBg.transform.GetChild(i);
            string[] nameArray = subItem.name.Split('_');
            if (nameArray.Length > 1)
            {
                int index = 0;
                if (int.TryParse(nameArray[1], out index))
                {
                    if (!m_dicWorldMapTexture.ContainsKey(index))
                    {
                        Transform texture = subItem.Find("Texture");
                        if (texture != null)
                        {
                            UITexture mapTex = texture.GetComponent<UITexture>();
                            if (mapTex != null)
                            {
                                m_dicWorldMapTexture.Add(index, mapTex.mainTexture);
                                m_dicWorldMapTrans.Add(index, subItem);
                            }
                        }

                    }
                }
            }
        }
        ShowCurrentMap(true);
        //InitNpcList();
        ShowNpcNameOnMap();
        ToggleCurMap("");
        AddSelfIcon();
        m_btn_npcitem.gameObject.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
        m_label_line_label.text = DataManager.Manager<MapDataManager>().CurLineNum + "线";
        NetService.Instance.Send(new GameCmd.stOpenSynTeamPosRelationUserCmd_C());
    }
    private void CreateTextureEvent(ITexture obj, object param = null)
    {
        IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
        if (mapsys == null)
        {
            Engine.Utility.Log.Error("Mapsystem is null");
            return;
        }
        if (m_texture != null)
        {
            mapTex.mainTexture = m_texture.GetTexture();

            mapTex.MakePixelPerfect();
        }
        else
        {
            string texpath = (string)param;
            Log.Error("texture is null texpath is " + texpath);
        }
        m_mapid = mapsys.GetMapID();
        table.MapDataBase mdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(m_mapid);
        if (mdb != null)
        {
            string pos = mdb.uioffset;
            string[] xy = pos.Split('_');
            float x = 0, y = 0;
            if (xy.Length == 2)
            {
                if (float.TryParse(xy[0], out x))
                {
                    if (float.TryParse(xy[1], out y))
                    {
                        originPos = new Vector3(x, y, 0);
                    }
                }
            }

        }
        m_mapTextureWidth = mdb.textureWidth;
        m_mapTextureHeight = mdb.textureWidth;
        mapTex.width = (int)m_mapTextureWidth;
        mapTex.height = (int)m_mapTextureHeight;
        mapTex.transform.localPosition = originPos;
    }
    void AddShowIcon(float x, float y, string iconName)
    {
        Vector3 iconPos = new Vector3(x, 0, -y);
        iconPos = GetIconPosByMapPos(iconPos);
        GameObject go = NGUITools.AddChild(mapTex.gameObject, m_sprite_iconprefab.gameObject);
        if (go != null)
        {
            go.SetActive(true);
            go.transform.localPosition = iconPos;

            UISprite spr = go.GetComponent<UISprite>();
            if (spr != null)
            {
                spr.spriteName = iconName;
            }
            AddShowIconList(go);
        }
    }
    void ShowIconOnMap(MapDataBase mdb)
    {
        if (mdb == null)
            return;
        List<NPCInfo> transList = DataManager.Manager<MapDataManager>().GetTransmitList();
        for (int i = 0; i < transList.Count; i++)
        {
            NPCInfo npc = transList[i];
            AddShowIcon(npc.pos.x, npc.pos.y, "tubiao_chuansongdian");
        }
        string mapInfo = mdb.mapinfo;
        if (string.IsNullOrEmpty(mapInfo))
        {
            return;
        }
        string[] infoArray = mapInfo.Split(',');
        for (int i = 0; i < infoArray.Length; i++)
        {
            string info = infoArray[i];
            string[] strArray = info.Split(';');
            if (strArray.Length != 3)
            {
                Log.Error("长度不符合规定");
                return;
            }
            Vector3 iconPos = Vector3.zero;
            float x, y;
            if (float.TryParse(strArray[0], out x))
            {
                if (float.TryParse(strArray[1], out y))
                {
                    AddShowIcon(x, y, strArray[2]);
                }
            }
        }


    }

    void ShowNpcNameOnMap()
    {
        List<NPCInfo> npcList = mapDataManager.GetNpcList();
        int count = mapTex.transform.childCount;
        for (int j = 0; j < count; j++)
        {

            string iconname = string.Format("npc{0:D2}", j);
            Transform npcTrans = mapTex.transform.Find(iconname);
            if (npcTrans != null)
            {
                npcTrans.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < npcList.Count; i++)
        {
            var npc = npcList[i];
            string iconname = string.Format("npc{0:D2}", i);
            Transform npcTrans = mapTex.transform.Find(iconname);
            // GameObject name = null;
            if (npcTrans == null)
            {
                Transform iconTrans = m_widget_BigMapContent.transform.Find("IconContainer/" + IconType.npcicon.ToString());
                GameObject npcIcon = GameObject.Instantiate(iconTrans.gameObject);
                npcTrans = npcIcon.transform;
            }
            npcTrans.gameObject.SetActive(true);
            npcTrans.gameObject.name = iconname;
            Vector3 iconPos = GetIconPosByMapPos(new Vector3(npc.pos.x, 0, -npc.pos.y));
            AddIconToMap(npcTrans.gameObject, iconPos);

        }
    }
    void AddShowIconList(GameObject go)
    {
        if (!m_showIconList.Contains(go))
        {
            m_showIconList.Add(go);
        }
    }
    void ClearShowIcon()
    {
        for (int i = 0; i < m_showIconList.Count; i++)
        {
            GameObject go = m_showIconList[i];
            if (go != null)
            {
                DestroyObject(go);
            }
        }
        m_showIconList.Clear();
    }
    protected override void OnStart()
    {
        base.OnStart();

    }


    void OnWorldMapClick(GameObject go)
    {
        Vector3 mousePos = UICamera.lastHit.point;
        Vector3 pos = m__wordBg.transform.worldToLocalMatrix.MultiplyPoint(mousePos);
        int count = m__wordBg.transform.childCount;
        List<string> nameList = new List<string>();
        for (int i = 0; i < count; i++)
        {
            Transform subItem = m__wordBg.transform.GetChild(i);
            Vector3 subPos = subItem.localPosition;
            Vector2 vec = new Vector2(pos.x, pos.y);
            Transform texture = subItem.Find("Texture");
            if (texture != null)
            {
                UITexture spr = texture.GetComponent<UITexture>();
                if (spr != null)
                {
                    Rect rec = new Rect(subPos.x - spr.width / 2, subPos.y - spr.height / 2, spr.width, spr.height);
                    if (rec.Contains(vec))
                    {
                        if (!nameList.Contains(subItem.name))
                        {
                            nameList.Add(subItem.name);
                        }
                    }
                }
            }

        }
        int mapID = 0;
        foreach (var name in nameList)
        {
            string[] nameArray = name.Split('_');
            if (nameArray.Length > 1)
            {
                int index = 0;
                if (int.TryParse(nameArray[1], out index))
                {
                    if (m_dicWorldMapTexture.ContainsKey(index))
                    {
                        Texture tex = m_dicWorldMapTexture[index];
                        if (tex != null)
                        {
                            Transform item = m__wordBg.transform.Find(name);
                            if (item != null)
                            {
                                Vector3 clickItemPos = item.worldToLocalMatrix.MultiplyPoint(mousePos);
                                Texture2D t2 = tex as Texture2D;
                                if (t2 != null)
                                {
                                    Vector2 texPos = new Vector2(clickItemPos.x + t2.width / 2, clickItemPos.y + t2.height / 2);
                                    Color c = t2.GetPixel((int)texPos.x, (int)texPos.y);
                                    if (c.a != 0)
                                    {
                                        ToggleCurMap(name);
                                        mapID = index;
                                        Log.LogGroup("ZDY", "click mapName is ========" + name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        GotoMap(mapID);
    }

    void ToggleCurMap(string name)
    {
        int count = m__wordBg.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform subItem = m__wordBg.transform.GetChild(i);
            Transform texture = subItem.Find("Texture");
            if (texture != null)
            {
                UITexture gaoliang = texture.GetComponent<UITexture>();
                if (gaoliang != null)
                {
                    if (subItem.name == name)
                    {
                        texture.gameObject.SetActive(true);
                    }
                    else
                    {
                        texture.gameObject.SetActive(false);
                    }
                }

            }
        }

    }

    void OnClickBtnMap(GameObject go)
    {

    }
    public void InitNpcList()
    {
        showNpcList.Clear();

    }


    void OnScrollItemClick(GameObject go)
    {

        MapNpcScrollItem item = go.GetComponent<MapNpcScrollItem>();

        if (item != null)
        {
            NPCInfo npcInfo = item.Info;
            if (npcInfo != null)
            {
                MapScrollItemType t = (MapScrollItemType)npcInfo.type;
                if (showDic.ContainsKey(t))
                {
                    bool bShow = showDic[t];
                    showDic[t] = !bShow;
                    MapScrollItemType bigType = MapScrollItemType.NPC | MapScrollItemType.Monster | MapScrollItemType.Transmit;
                    if ((t & bigType) != 0)
                    {
                        FilterList(t);
                    }
                }
                else
                {
                    ClearIcon();
                    Vector3 npcPos = new Vector3(npcInfo.pos.x, 0, -npcInfo.pos.y);
                    AutoFindPath(npcPos, (uint)npcInfo.npcID, true);
                }
            }
            else
            {
                Log.Error("npcinfo is null");
            }
        }
        else
        {
            Log.Error("item is null");
        }
    }

    void SetFilterBtnStates(MapScrollItemType activeBtnType)
    {
        UIToggle toggle = null;
        Transform togglets = null;
        bool visible = (activeBtnType == MapScrollItemType.NPC);
        if (null != m_btn_npcitem)
        {
            togglets = m_btn_npcitem.transform.Find("Toggle");
            if (null != togglets)
            {
                toggle = togglets.GetComponent<UIToggle>();
                if (null != toggle && (toggle.value != visible))
                {
                    toggle.Set(visible);
                }
            }
        }

        visible = (activeBtnType == MapScrollItemType.Monster);
        if (null != m_btn_monster)
        {
            togglets = m_btn_monster.transform.Find("Toggle");
            if (null != togglets)
            {
                toggle = togglets.GetComponent<UIToggle>();
                if (null != toggle && (toggle.value != visible))
                {
                    toggle.Set(visible);
                }
            }
        }

        visible = (activeBtnType == MapScrollItemType.Transmit);
        if (null != m_btn_transmit)
        {
            togglets = m_btn_transmit.transform.Find("Toggle");
            if (null != togglets)
            {
                toggle = togglets.GetComponent<UIToggle>();
                if (null != toggle && (toggle.value != visible))
                {
                    toggle.Set(visible);
                }
            }
        }

    }

    void FilterList(MapScrollItemType type)
    {
        SetFilterBtnStates(type);
        showNpcList.Clear();
        MapDataManager dm = DataManager.Manager<MapDataManager>();

        bShowSmallItem = showDic[type];
        if (type == MapScrollItemType.NPC)
        {
            showNpcList.AddRange(dm.GetNpcList());
        }
        else if (type == MapScrollItemType.Monster)
        {
            showNpcList.AddRange(dm.GetMonsterList());
        }
        else if (type == MapScrollItemType.Transmit)
        {
            showNpcList.AddRange(dm.GetTransmitList());
        }
        foreach (var dic in btnDic)
        {
            Transform btnTrans = dic.Value;
            if (dic.Key == type)
            {
                if (btnTrans != null)
                {
                    btnTrans.gameObject.SetActive(true);
                }

            }
            else
            {
                if (btnTrans != null)
                {
                    btnTrans.gameObject.SetActive(false);
                }
            }
        }
    }
    public void ClearScrollItem()
    {
        Release();
        if (m_trans_npcitemContainer != null)
        {
            Transform con = m_trans_npcitemContainer;
            if (con != null)
            {
                con.DestroyChildren();
            }
        }
        if (m_trans_monsterContainer != null)
        {
            Transform con = m_trans_monsterContainer;
            if (con != null)
            {
                con.DestroyChildren();
            }
        }
        if (m_trans_transmitContainer != null)
        {
            Transform con = m_trans_transmitContainer;
            if (con != null)
            {
                con.DestroyChildren();
            }
        }
    }
    void onClick_Npcitem_Btn(GameObject caster)
    {
        FilterList(MapScrollItemType.NPC);
        ShowList(m_trans_npcitemContainer);

    }
    void onClick_Monster_Btn(GameObject caster)
    {
        FilterList(MapScrollItemType.Monster);
        ShowList(m_trans_monsterContainer);
    }

    void onClick_Transmit_Btn(GameObject caster)
    {
        FilterList(MapScrollItemType.Transmit);
        ShowList(m_trans_transmitContainer);
    }
    void ReleaseScrollItem(Transform container)
    {
        MapNpcScrollItem[] npcitems = container.GetComponentsInChildren<MapNpcScrollItem>();
        foreach (var item in npcitems)
        {
            item.Release();
        }

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        ReleaseScrollItem(m_trans_npcitemContainer);
        ReleaseScrollItem(m_trans_monsterContainer);
        ReleaseScrollItem(m_trans_transmitContainer);
        ReleaseTeamInfoGrid();
    }

    void ShowList(Transform contain)
    {
        m_scrollview_npcscrollview.ResetPosition();
        Transform container = contain;
        if (container != null)
        {
            //if (container.childCount != 0)
            //    return;
            for (int j = showNpcList.Count; j < container.childCount; j++)
            {
                string itemName = string.Format("{0:D3}", j);
                Transform itemTrans = container.Find(itemName);
                if (itemTrans != null)
                {
                    itemTrans.gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < showNpcList.Count; i++)
            {
                NPCInfo info = showNpcList[i];
                MapNpcScrollItem itemInfo = null;
                GameObject bigItem = null;
                string itemName = string.Format("{0:D3}", i);

                if ((MapScrollItemType)info.type == MapScrollItemType.Small)
                {
                    Transform itemTrans = container.Find(itemName);
                    if (itemTrans != null)
                    {
                        bigItem = itemTrans.gameObject;
                    }
                    else
                    {
                        bigItem = NGUITools.AddChild(container.gameObject, m_sprite_btn_child.gameObject);
                        itemTrans = bigItem.transform;
                    }
                    bigItem.SetActive(true);
                    if (bigItem != null)
                    {
                        itemInfo = bigItem.GetComponent<MapNpcScrollItem>();
                        if (itemInfo == null)
                        {
                            itemInfo = bigItem.AddComponent<MapNpcScrollItem>();
                        }
                    }
                    bigItem.transform.localPosition = new Vector3(0, (i * -70) - 34, 0);
                }
                if (bigItem != null)
                {
                    bigItem.name = itemName;
                    UIEventListener.Get(bigItem.gameObject).onClick = OnScrollItemClick;
                }
                else
                {
                    Log.Error("bigItem is null");
                }
                if (itemInfo != null)
                {
                    itemInfo.InitInfo(info);
                }

            }
        }

    }


    void AddIconToMap(GameObject icon, Vector3 pos)
    {
        if (icon == null)
            return;
        icon.SetActive(true);
        icon.transform.parent = mapTex.transform;
        icon.transform.localPosition = pos;
        icon.transform.localRotation = Quaternion.identity;
        icon.transform.localScale = Vector3.one;
    }

    protected override void OnHide()
    {
        NetService.Instance.Send(new GameCmd.stCancelSynTeamPosRelationUserCmd_C());
        Release();
    }

    void onClick_Close_Btn(GameObject caster)
    {
        //ClearIcon();
        //OnHide();
        HideSelf();
        m_ctor_bgContent.SetVisible(false);
    }
    Vector2 GetUV(IEntity entity)
    {
        Vector3 pos = entity.GetPos();
        return GetUVByMapPos(pos);
    }
    Vector2 GetUVByMapPos(Vector3 pos)
    {
        float rotAngle = DataManager.Manager<MapDataManager>().camerRenderRotation;
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, -rotAngle, 0);
        //旋转坐标系
        pos = rot * pos;

        float fsize = (mapSize.x + mapSize.y) * (float)Math.Sin(rotAngle / 180.0f * Math.PI);
        //   pos.z /= (float)Math.Sin( 65.0f / 180.0f * Math.PI );
        pos.z += fsize * ((float)mapSize.y) / (mapSize.x + mapSize.y);


        float xPos = pos.x / fsize;
        float yPos = pos.z / fsize;

        Vector2 uv = new Vector2(xPos, yPos);
        return uv;
    }
    StringBuilder m_iconName = new StringBuilder();
    uint m_nFrameNum = 0;
    void Update()
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
        SetPlayerIconDir();


    }
    void SyncTeammatePos(GameCmd.TeamPosData teamData)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            m_iconName.Remove(0, m_iconName.Length);
            m_iconName.Append(IconType.duiyouicon.ToString());
            m_iconName.Append("_");
            m_iconName.Append(teamData.uid);
            TeamMemberInfo info = DataManager.Manager<TeamDataManager>().GetTeamMember(teamData.uid);
            string iconName = m_iconName.ToString();
            Transform iconTrans = mapTex.transform.Find(iconName);

            uint job = info.job;
            string textureName = "";
            table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                textureName = sdb.strprofessionIcon;
            }
            UITeammateMapInfoGrid teaminfo = null;
            if (iconTrans == null)
            {
                iconTrans = GameObject.Instantiate<Transform>(m_trans_UITeammateMapInfoGrid);

                m_duiyouList.Add(iconTrans.gameObject);
                iconTrans.name = iconName;
            }
            teaminfo = iconTrans.GetComponent<UITeammateMapInfoGrid>();
            if (teaminfo == null)
            {
                teaminfo = iconTrans.gameObject.AddComponent<UITeammateMapInfoGrid>();
            }
            if (teaminfo != null)
            {
                teaminfo.SetItemInfo(textureName, info.name);
            }

            Vector3 iconPos = GetIconPosByMapPos(new Vector3(teamData.x, 0, -teamData.y));

            AddIconToMap(iconTrans.gameObject, iconPos);
        }

    }
    public void DequeIcon()
    {
        if (mainPlayer != null)
        {
            Vector3 pos = mainPlayer.GetPos();
            Vector2 playerUV = GetUV(mainPlayer);
            float posX = playerUV.x * m_mapTextureWidth - m_mapTextureWidth / 2;
            float poxY = playerUV.y * m_mapTextureHeight - m_mapTextureHeight / 2;
            playerIconTrans.localPosition = new Vector3(posX, poxY, 0);

            if (PathPointList.Count > 0)
            {
                DataManager.Manager<MapDataManager>().bDoMoving = true;
                Vector2 pathPoint = PathPointList[0];
                Vector2 playerPoint = new Vector2(pos.x, pos.z);
                Vector2 pathdir = pathPoint - playerPoint;
                float delta = Vector2.Distance(pathPoint, playerPoint);
                if (delta <= 2)
                {
                    if (m_pathIconList.Count > 0)
                    {
                        GameObject icon = m_pathIconList[0];
                        m_pathIconList.RemoveAt(0);
                        MapIconPoolManager.Instance.ReturnIcon(icon);
                    }
                    PathPointList.RemoveAt(0);
                }

            }
            else
            {
                ClearIcon();
            }

        }

    }
    void SetPlayerIconDir()
    {
        if (MainPlayerHelper.GetMainPlayer() != null)
        {
            IPlayer player = MainPlayerHelper.GetMainPlayer();
            Vector3 myPos = player.GetPos();
            Vector3 dir = player.GetDir();
            Vector3 targetPos = myPos + dir;
            targetPos = GetIconPosByMapPos(targetPos);
            Vector3 iconDir = targetPos - playerIconTrans.localPosition;
            Vector3 cross = Vector3.Cross(iconDir, Vector3.up);
            float angle = Vector3.Angle(iconDir, Vector3.up);
            // Engine.Utility.Log.Error("angle is " + angle + "cross is "+cross);
            if (cross.z < 0)
            {
                angle = 360 - angle;
            }
            Quaternion qua = Quaternion.AngleAxis(-angle, Vector3.forward);
            if (playerIconTrans != null)
            {
                playerIconTrans.rotation = qua;
            }
        }
    }
    Vector3 GetIconPosByMapPos(Vector3 pos)
    {
        Vector2 playerUV = GetUVByMapPos(pos);
        float posX = playerUV.x * m_mapTextureWidth - m_mapTextureWidth / 2;
        float poxY = playerUV.y * m_mapTextureHeight - m_mapTextureHeight / 2;
        return new Vector3(posX, poxY, 0);
    }
    void AddSelfIcon()
    {
        if (mainPlayer == null)
        {
            return;
        }
        Transform iconTrans = m__wordBg.transform.Find("SelfIcon");
        uint job = (uint)mainPlayer.GetProp((int)PlayerProp.Job);
        string textureName = "";
        table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
        if (sdb != null)
        {
            textureName = sdb.strprofessionIcon;
        }
        UITeammateMapInfoGrid teaminfo = null;
        if (iconTrans == null)
        {
            iconTrans = GameObject.Instantiate<Transform>(m_trans_UITeammateMapInfoGrid);
        }
        teaminfo = iconTrans.GetComponent<UITeammateMapInfoGrid>();
        if (teaminfo == null)
        {
            teaminfo = iconTrans.gameObject.AddComponent<UITeammateMapInfoGrid>();
        }
        teaminfo.SetItemInfo(textureName, mainPlayer.GetName());
        if (iconTrans != null)
        {
            iconTrans.name = "SelfIcon";
            iconTrans.parent = m__wordBg.transform;
            if (m_dicWorldMapTrans.ContainsKey((int)m_mapid))
            {
                Transform temp = m_dicWorldMapTrans[(int)m_mapid];
                iconTrans.localPosition = temp.localPosition;
                iconTrans.gameObject.SetActive(true);
            }
            else
            {
                iconTrans.gameObject.SetActive(false);
            }
            iconTrans.localScale = Vector3.one;
            iconTrans.localRotation = Quaternion.identity;

        }

    }
    public void ClearIcon()
    {
        for (int i = 0; i < m_pathIconList.Count; i++)
        {
            var icon = m_pathIconList[i];
            MapIconPoolManager.Instance.ReturnIcon(icon);
        }
        m_pathIconList.Clear();
        PathPointList.Clear();
    }

    void MapClick(GameObject go)
    {
        Engine.Utility.Log.Error("MapClick");

        ClearIcon();


        Vector3 mousePos = UICamera.lastHit.point;
        Vector3 pos = mapTex.transform.worldToLocalMatrix.MultiplyPoint(mousePos);
        Log.Error("map click pos is " + pos);
        float u = (pos.x) / m_mapTextureWidth + 0.5f;
        //y是从中间开始的
        float v = (pos.y) / m_mapTextureHeight;
        float rotAngle = DataManager.Manager<MapDataManager>().camerRenderRotation;
        float fsize = (mapSize.x + mapSize.y) * (float)Math.Sin(rotAngle / 180.0f * Math.PI);
        // pos.y -= fsize * ( (float)mapHeight ) / ( mapWidth + mapHeight );

        //float fSin55 = (float)Math.Sin(55.0f / 180.0f * Math.PI);

        float xPos = u * fsize;
        float yPos = v * fsize;// / fSin55;
        //  float radius = (float)Math.Sin( 45.0f / 180.0f * Math.PI );
        Vector3 targetPos = new Vector3(xPos, 3, yPos);
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, rotAngle, 0);
        targetPos = rot * targetPos;
        //targetPos.z = -targetPos.z;

        AutoFindPath(targetPos);
    }
    void AutoFindPath(Vector3 targetPos, uint npcID = 0, bool isNpc = false)
    {
        IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
        IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
        if (mapSys == null)
        {
            return;
        }
        // 需要添加一个寻路的过程
        if (isNpc)
        {
            int mapID = DataManager.Manager<MapDataManager>().GetCurMapID();
            if (mapID > 0)
            {
                Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().VisiteNPC((uint)mapID, npcID, false);
                HideSelf();
            }
            else
            {
                Engine.Utility.Log.Error("mapid error ");
            }
        }
        else
        {
            if (ctrl != null)
            {
                Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                if (cs != null)
                {
                    Client.ICombatRobot robot = cs.GetCombatRobot();
                    if (robot != null && robot.Status != CombatRobotStatus.STOP)
                    {
                        robot.Stop();
                    }
                }
                m_vecTargetPos = new Vector2(targetPos.x, targetPos.z);
                mapSys.AddFindPathCallback(FindPathCallBack);
                ctrl.MoveToTarget(targetPos, null, true);
                mapSys.RemoveFindPathCallback(FindPathCallBack);
            }
        }

    }
    Vector2 m_vecTargetPos = Vector2.zero;
    void FindPathCallBack(List<Vector2> path)
    {
        List<Vector2> pathList = new List<Vector2>(path);
        Vector3 curPos = mainPlayer.GetPos();
        Vector2 playerPos = new Vector2(curPos.x, curPos.z);
        if (pathList.Count < 2)
        {
            /* pathList.Clear();*/
            pathList.Insert(0, playerPos);
            pathList.Insert(1, m_vecTargetPos);
        }
        else
        {
            pathList.Insert(0, playerPos);
        }
        InsertPathPoint(pathList);
        PathPointList.RemoveAt(0);
        for (int i = 0; i < PathPointList.Count; i++)
        {
            var vec = PathPointList[i];
            Vector3 mapos = new Vector3(vec.x, 0, vec.y);
            GameObject goIcon = MapIconPoolManager.Instance.GetIcon(IconType.pathpointicon, mainPlayer, m_widget_BigMapContent.transform);
            Vector3 iconPos = GetIconPosByMapPos(mapos);
            m_pathIconList.Add(goIcon);
            AddIconToMap(goIcon, iconPos);
        }

    }
    /// <summary>
    /// 添加差值路径点
    /// </summary>
    /// <param name="pathList">实际寻路点</param>
    void InsertPathPoint(List<Vector2> pathList)
    {
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            Vector2 thisPoint = pathList[i];
            Vector2 nextPoint = pathList[i + 1];
            Vector2 dir = (nextPoint - thisPoint).normalized;
            float distance = Vector2.Distance(nextPoint, thisPoint);
            PathPointList.Add(thisPoint);
            while (distance >= 10)
            {
                thisPoint = thisPoint + dir * 5f;
                PathPointList.Add(thisPoint);
                distance = Vector2.Distance(nextPoint, thisPoint);
            }
            PathPointList.Add(nextPoint);

        }
    }
    void onClick_Wordmap_Btn(GameObject caster)
    {
        ShowCurrentMap(true);
    }
    void onClick_Currentmap_Btn(GameObject caster)
    {
        ShowCurrentMap(false);
    }
    void ShowCurrentMap(bool bShow)
    {
        m_widget_BigMapContent.gameObject.SetActive(bShow);
        m_widget_WorldMapContent.gameObject.SetActive(!bShow);
    }
    void onClick_Btn_child_Btn(GameObject caster)
    {

    }
    void onClick_Changshengdian_Btn(GameObject caster)
    {
        ResetBtnBgPosition((MapBtnType)1);
    }

    void onClick_Jiuyoudigong_Btn(GameObject caster)
    {
        ResetBtnBgPosition((MapBtnType)2);
    }

    void onClick_Wanjieku_Btn(GameObject caster)
    {
        ResetBtnBgPosition((MapBtnType)3);
    }

    void onClick_Wuwanghai_Btn(GameObject caster)
    {
        ResetBtnBgPosition((MapBtnType)4);
    }
    enum MapBtnType
    {
        ChangShengDian = 1,
        JiuYouDiGong = 2,
        WanJieKu = 3,
        WuWangHai = 4,
    }
    List<uint> mapID = new List<uint>();
    List<MapDataBase> mapData = new List<MapDataBase>();
    bool isShowingGird = false;
    void ResetBtnBgPosition(MapBtnType type)
    {
        m_ctor_bgContent.SetVisible(true);
        m_btn_ColliderMask.gameObject.SetActive(true);
        TransferChildTypeDataBase tab = GameTableManager.Instance.GetTableItem<TransferChildTypeDataBase>((uint)type);
        if (tab == null)
        {
            Engine.Utility.Log.Error("传送表格找不到对应的表格数据ID{0}", (uint)type);
            return;
        }
        mapID.Clear();
        mapData.Clear();
        string[] ids = tab.MapIDList.Split('_');
        uint id = 0;
        for (int i = 0; i < ids.Length; i++)
        {
            if (uint.TryParse(ids[i], out id))
            {
                mapID.Add(id);
            }
        }

        if (mapID.Count == 1)
        {
            GotoMap((int)mapID[0]);
            return;
        }
        for (int j = 0; j < mapID.Count; j++)
        {
            MapDataBase map = GameTableManager.Instance.GetTableItem<MapDataBase>(mapID[j]);
            if (map != null)
            {
                mapData.Add(map);
            }
        }

        m_ctor_bgContent.CreateGrids(mapData.Count);
        isShowingGird = true;
        m_btn_ColliderMask.gameObject.SetActive(true);
        Vector3 vec = Vector3.zero;

        switch (type)
        {
            case MapBtnType.ChangShengDian:
                {
                    vec = new Vector3(330, -240, 0);
                }
                break;
            case MapBtnType.JiuYouDiGong:
                {
                    vec = new Vector3(155, -150, 0);
                }
                break;
            case MapBtnType.WanJieKu:
                {
                    vec = new Vector3(250, -20, 0);
                }
                break;
            case MapBtnType.WuWangHai:
                {
                    vec = new Vector3(100, 150, 0);
                }
                break;
        }
        AdjustBgPosition(vec, mapID.Count);
    }
    void AdjustBgPosition(Vector3 vec, int mapIDCount)
    {
        int height = 66 * mapIDCount;
        m_ctor_bgContent.CacheTransform.localPosition = vec;

        UISprite sp = m_ctor_bgContent.CacheTransform.Find("TipBg").GetComponent<UISprite>();
        sp.transform.localPosition = new Vector3(0, 30 - (30 * (mapIDCount - 1)), 0);
        sp.height = height;
    }
    void GotoMap(int mapID)
    {
        if (mapID != 0)
        {

            IMapSystem mapSystem = Client.ClientGlobal.Instance().GetMapSystem();
            if (mapSystem.GetMapID() == mapID)
            {
                //你就在这个场景
                TipsManager.Instance.ShowTipsById(514);
                return;
            }
            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs != null)
            {
                cs.GetCombatRobot().Stop();// 停止挂机
            }
            TransferDatabase tdb = GameTableManager.Instance.GetTableItem<TransferDatabase>((uint)mapID);
            if (tdb != null)
            {
                if (!MainPlayerHelper.IsHasEnoughMoney(tdb.costType, tdb.costValue, true))
                {
                    return;
                }
            }
            //DataManager.Manager<SliderDataManager>().SetSliderDelagate(() =>
            //{

            if (!KHttpDown.Instance().SceneFileExists((uint)mapID))
            {
                //打开下载界面
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                HideSelf();
                return;
            }

            IMapSystem ms = ClientGlobal.Instance().GetMapSystem();
            if (ms != null)
            {
                ms.RequestEnterMap((uint)mapID, 1);
            }
            //}, null, 3f);

            HideSelf();
        }
        m_ctor_bgContent.SetVisible(false);
        m_btn_ColliderMask.gameObject.SetActive(false);


    }

    void onClick_ColliderMask_Btn(GameObject caster)
    {
        if (isShowingGird)
        {
            m_ctor_bgContent.SetVisible(false);
            m_btn_ColliderMask.gameObject.SetActive(false);
        }
        else
        {
            m_btn_ColliderMask.gameObject.SetActive(false);
        }

    }
    public void SyncTeammatePos(GameCmd.stSynTeamPosRelationUserCmd_S cmd)
    {
        foreach (GameCmd.TeamPosData item in cmd.data)
        {
            SyncTeammatePos(item);
        }
        for (int i = cmd.data.Count; i < m_duiyouList.Count; i++)
        {
            GameObject go = m_duiyouList[i];
            if (go != null)
            {
                go.SetActive(false);
            }
        }
    }
    void ReleaseTeamInfoGrid()
    {
        foreach (var info in m_duiyouList)
        {
            UITeammateMapInfoGrid grid = info.GetComponent<UITeammateMapInfoGrid>();
            if (grid != null)
            {
                grid.Release();
            }
            Destroy(info);
        }
        m_duiyouList.Clear();
    }
}
