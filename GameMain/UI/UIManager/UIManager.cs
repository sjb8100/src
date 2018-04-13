using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using System.Reflection;
using Engine;

public partial class UIManager : BaseModuleData,IManager,IGlobalEvent
{

    #region Property
    public const string CLASS_NAME = "UIManager";
    //UI层级名称
    public const string UI_LAYER_NAME = "UI";
    //动态图集信息字典
    //<spritename，atlasname>
    private Dictionary<string, int> mdicDynamicAtlas;
    private Dictionary<string, int> mdicDynamicIcons;
    //UI管理器根节点
    private int m_intUILayer = 0;
    private static UIManager m_instance = null;
    public static UIManager Instance
    {
        get
        {
            return m_instance;
        }
    }
    #endregion

    #region Camera
    private Camera m_uicamera;
    private Camera UICam
    {
        get
        {
            if (null == m_uicamera)
            {
                Transform camerats = UIRootHelper.Instance.UIRoot.Find("Camera");
                if (null != camerats)
                {
                    m_uicamera = camerats.GetComponent<Camera>();
                }
            }
            return m_uicamera;
        }
    }
    /// <summary>
    /// 设置UI摄像机状态
    /// </summary>
    /// <param name="enable"></param>
    public void SetCameraState(bool enable)
    {
        Camera camera = UICam;
        if (null != camera)
        {
            camera.enabled = enable;
        }
    }
    #endregion

    #region Panel

    /// <summary>
    /// 是否ui对象相对uicamera是否可见
    /// </summary>
    /// <param name="dependPanel"></param>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public static bool IsUIObjVisible(PanelID dependPanel,string relativePath)
    {
        GameObject targetObj = FindGameObjByPanel(dependPanel,relativePath);
        return IsUIObjVisible(dependPanel, targetObj);
    }


    /// <summary>
    /// 默认为UICamera
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsObjVisibleByCamera(GameObject obj)
    {
        Camera camera = UICamera.currentCamera;
        if (null != obj && obj.layer == LayerMask.NameToLayer(UI_LAYER_NAME))
        {
            return IsObjVisibleByCamera(obj, camera);
        }
        return false;
    }

    /// <summary>
    /// 获取游戏对象父节点
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parents"></param>
    public static void GetParentTransform(GameObject obj,ref List<Transform> parents)
    {
        if (null == obj || null == obj.transform.parent)
        {
            return;
        }
        if (!parents.Contains(obj.transform.parent))
        {
            parents.Add(obj.transform.parent);
        }
        GetParentTransform(obj.transform.parent.gameObject, ref parents);
    }

    /// <summary>
    /// 对象是否相对camera可见
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static bool IsObjVisibleByCamera(GameObject obj,Camera camera)
    {
        bool visible = false;
        if (null != camera && null != obj && obj.activeSelf)
        {
            List<Transform> parents = new List<Transform>();
            GetParentTransform(obj, ref parents);
            Bounds bounds = new Bounds();
            Vector3[] boundsVector = new Vector3[4];
            UIPanel panel = null;
            bool checkBoundsInCamera = true;
            bool nguiCamera = false;
            if (camera == UICamera.currentCamera)
            {
                nguiCamera = true;
                Bounds tempBounds = NGUIMath.CalculateRelativeWidgetBounds(obj.transform, obj.transform);
                Matrix4x4 local2WorldMatrix = obj.transform.localToWorldMatrix;
                bounds.center = local2WorldMatrix.MultiplyPoint3x4(tempBounds.center);
                bounds.min = local2WorldMatrix.MultiplyPoint3x4(tempBounds.min);
                bounds.max = local2WorldMatrix.MultiplyPoint3x4(tempBounds.max);
                
                //判断是否相对面板可见
                if (parents.Count > 0)
                {
                    boundsVector[0] = new Vector3(tempBounds.min.x, tempBounds.min.y, 1);
                    boundsVector[1] = new Vector3(tempBounds.min.x, tempBounds.max.y, 1);
                    boundsVector[2] = new Vector3(tempBounds.max.x, tempBounds.max.y, 1);
                    boundsVector[3] = new Vector3(tempBounds.max.x, tempBounds.min.y, 1);
                    for (int i = 0; i < 4; i++)
                    {
                        boundsVector[i] = local2WorldMatrix.MultiplyPoint3x4(boundsVector[i]);
                    }
                }
            }
            else
            {
                BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
                if (null != boxCollider)
                {
                    bounds = boxCollider.bounds;
                }else
                {
                    checkBoundsInCamera = false;
                }
            }
            if (checkBoundsInCamera)
            {
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

                bool isBoundsInCamera = GeometryUtility.TestPlanesAABB(planes, bounds);
                bool isParentVisible = true;
                if (isBoundsInCamera)
                {
                    for (int i = 0; i < parents.Count; i++)
                    {
                        if (null == parents[i])
                        {
                            continue;
                        }

                        if (!parents[i].gameObject.activeSelf)
                        {
                            isParentVisible = false;
                            break;
                        }
                        if (nguiCamera)
                        {
                            panel = parents[i].GetComponent<UIPanel>();
                            if (null == panel)
                            {
                                continue;
                            }
                            if ((panel.clipping == UIDrawCall.Clipping.None || panel.clipping == UIDrawCall.Clipping.ConstrainButDontClip))
                            {
                                continue;
                            }
                            if (!panel.IsVisible(boundsVector[0], boundsVector[1], boundsVector[2], boundsVector[3]))
                            {
                                isParentVisible = false;
                                break;
                            }
                        }
                    }
                }
                visible = isBoundsInCamera && isParentVisible;
            }
        }
        return visible;
    }

    /// <summary>
    /// 是否游戏对象相对面板可见
    /// </summary>
    /// <param name="dependPanel"></param>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public static bool IsUIObjVisible(PanelID dependPanel,GameObject obj)
    {
        if (null == obj || !obj.gameObject.activeSelf)
        {
            return false;
        }

        bool dependShow = DataManager.Manager<UIPanelManager>().IsShowPanel(dependPanel);
        UIPanelBase panelBase = DataManager.Manager<UIPanelManager>().GetPanel(dependPanel);
        if (null == panelBase || panelBase.Panel)
        {
            return false;
        }
        UIPanel panel = panelBase.Panel;
        UIWidget widget = obj.GetComponent<UIWidget>();
        bool isVisible = false;
        Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(obj.transform, obj.transform);
        Matrix4x4 local2WorldMatrix = obj.transform.localToWorldMatrix;
        Vector3[] boundsVector = new Vector3[4];
        boundsVector[0] = new Vector3(bounds.min.x, bounds.min.y, 1);
        boundsVector[1] = new Vector3(bounds.min.x, bounds.max.y, 1);
        boundsVector[2] = new Vector3(bounds.max.x, bounds.max.y, 1);
        boundsVector[3] = new Vector3(bounds.max.x, bounds.min.y, 1);
        for (int i = 0; i < 4; i++)
        {
            boundsVector[i] = local2WorldMatrix.MultiplyPoint3x4(boundsVector[i]);
        }
        isVisible = dependShow && panel.IsVisible(boundsVector[0], boundsVector[1], boundsVector[2], boundsVector[3]);
        //if (null == widget)
        //{
        //    Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(obj.transform, obj.transform);
        //    Matrix4x4 local2WorldMatrix = obj.transform.localToWorldMatrix;
        //    Vector3[] boundsVector = new Vector3[4];
        //    boundsVector[0] = new Vector3(bounds.min.x, bounds.min.y, 1);
        //    boundsVector[1] = new Vector3(bounds.min.x, bounds.max.y, 1);
        //    boundsVector[2] = new Vector3(bounds.max.x, bounds.max.y, 1);
        //    boundsVector[3] = new Vector3(bounds.max.x, bounds.min.y, 1);
        //    for (int i = 0; i < 4; i++)
        //    {
        //        boundsVector[i] = local2WorldMatrix.MultiplyPoint3x4(boundsVector[i]);
        //    }
        //    isVisible = dependShow && panel.IsVisible(boundsVector[0], boundsVector[1], boundsVector[2], boundsVector[3]);
        //}
        //else
        //{
        //    isVisible = dependShow && panel.IsVisible(widget);
        //}
        return isVisible;
    }

    /// <summary>
    /// 根据面板与对象相对路径获取目标对象
    /// </summary>
    /// <param name="panelId">对象依赖面板</param>
    /// <param name="relativePath">相对面板路径</param>
    /// <returns></returns>
    public static GameObject FindGameObjByPanel(PanelID panelId,string relativePath)
    {
        UIPanelBase panelbase = DataManager.Manager<UIPanelManager>().GetPanel(panelId);
        if (null == panelbase)
        {
            if (UIDefine.IsUIDebugLogEnable)
                Engine.Utility.Log.Error("UIManager->FindGameObjByPanel faield,unknow panelId:{0}"
                , panelId);
            return null;
        }

        Transform ts = panelbase.CacheTransform.Find(relativePath);
        if (null == ts)
        {
            if (UIDefine.IsUIDebugLogEnable)
                Engine.Utility.Log.Error("UIManager->FindGameObjByPanel faield,unknow path:{0}\npanelid:{1}"
                ,relativePath, panelId);
            return null;
        }
        return ts.gameObject;
    }
    #endregion

    #region IManager Method
    public void ClearData()
    {

    }
    public void Initialize()
    {
        m_instance = this;
        m_intUILayer = LayerMask.NameToLayer(UI_LAYER_NAME);
        LoadIconJsonFile();
        LoadAtlasJsonFile();
        RegisterGlobalEvent(true);
    }

    public void Reset(bool depthClearData = false)
    {
        
    }

    public void Process(float deltaTime)
    {

    }
    #endregion

    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYBTNVOICE, GlobalEventHandler);
            GameApp.ResgisterLowMemory(OnLowMemory);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYBTNVOICE, GlobalEventHandler);
            GameApp.UnRegisterLowMemoy(OnLowMemory);
        }
    }

    /// <summary>
    /// UI全局事件处理器
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.PLAYBTNVOICE:
                {
                    ButtonPlay.ButtonSountEffectType effectType = (ButtonPlay.ButtonSountEffectType)data;
                    PlayUIAudioEffect(effectType);
                }
                break;
        }
    }

    
    #endregion

    #region BtnSoundEffect（按钮音效）
    public const string CONST_BTNEFFECT_NAME = "BtnSoundEffectIds";
    private Dictionary<ButtonPlay.ButtonSountEffectType, uint> m_dicBtnSoundEffect = null;
    /// <summary>
    /// 根据音效类型获取音效ID
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public uint GetUIAudioIDByType(ButtonPlay.ButtonSountEffectType effectType)
    {
        if (null == m_dicBtnSoundEffect)
        {
            m_dicBtnSoundEffect = new Dictionary<ButtonPlay.ButtonSountEffectType, uint>();
        }

        if (m_dicBtnSoundEffect.ContainsKey(effectType))
        {
            return m_dicBtnSoundEffect[effectType];
        }
        uint audioId = GameTableManager.Instance.GetGlobalConfig<uint>(CONST_BTNEFFECT_NAME, effectType.ToString());
        m_dicBtnSoundEffect.Add(effectType, audioId);
        return audioId;
    }

    /// <summary>
    /// 播放UI音效
    /// </summary>
    /// <param name="effectType"></param>
    public void PlayUIAudioEffect(ButtonPlay.ButtonSountEffectType effectType)
    {
        PlayUIAudioEffect(GetUIAudioIDByType(effectType));
    }

    /// <summary>
    /// 播放UI音效
    /// </summary>
    /// <param name="audioID"></param>
    public void PlayUIAudioEffect(uint audioID)
    {
        if (audioID != 0)
        {
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null)
            {
                table.ResourceDataBase rd = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)audioID);
                if (rd != null)
                {
                    audio.PlayUIEffect(rd.strPath);
                }
            }
        }
    }
    #endregion

    private List<string> assetDependens = new List<string>();
    /// <summary>
    ///获取物品UI控制类(不包含金币类--GetUIMoneyItem)
    /// </summary>
    /// <param name="dwObjectId"></param>
    /// <returns></returns>
    public UIItem GetUICommonItem(uint dwObjectId, uint num, uint itemThisId = 0, Action<UIItemCommonGrid> callback = null,Action<UIItemCommonGrid,bool> pressCallback = null,bool showGetWay = true)
    {
        table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>( dwObjectId );
        if (db == null)
        {
            Engine.Utility.Log.Error(CLASS_NAME + "-> Not Found Item By ID:{0}", dwObjectId);
            return null;
        }
        return GetUIItem(db.itemIcon, num, dwObjectId, itemThisId,callback,pressCallback,showGetWay);
    }

    private UIItem GetUIItem(string spriteName, uint num, uint itemid = 0, uint itemThisId = 0, Action<UIItemCommonGrid> callback = null, Action<UIItemCommonGrid, bool> pressCallback = null, bool showGetWay = true)
    {
        UnityEngine.Object obj = UIManager.GetResGameObj(GridID.Uiitemcommongrid);
        if (obj == null)
        {
            Engine.Utility.Log.Error("Get GridID.Uiitemcommongrid failed");
            return null;
        }

        GameObject gridObj = GameObject.Instantiate(obj) as GameObject;
        UIItemCommonGrid grid = gridObj.GetComponent<UIItemCommonGrid>();
        if (null == grid)
            grid = gridObj.AddComponent<UIItemCommonGrid>();

        uint qulity = 0;
        table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemid);
        if (db != null)
        {
            qulity = db.quality;
        }
        grid.SetGridData(new ItemDefine.UIItemCommonData()
        {
            DwObjectId = itemid,
            IconName = spriteName,
            Num = num,
            ItemThisId = itemThisId,
            Qulity = qulity,
            ShowGetWay = showGetWay,
        });

        if (itemid != 0)//非零显示tips
        {
            bool blongPress = false;
            grid.RegisterUIEventDelegate((UIEventType eventType, object data, object param) =>
            {
                UIItemCommonGrid uigrid = data as UIItemCommonGrid;
                switch (eventType)
                {
                    case UIEventType.Click:
                        blongPress = false;

                        if (callback != null)
                        {
                            callback(uigrid);
                            return;
                        }

                        if (uigrid.Data.Num > 0)
                        {
                            if (uigrid.Data.ItemThisId > 0)
                            {
                                TipsManager.Instance.ShowItemTips(uigrid.Data.ItemThisId, uigrid.gameObject, false);
                            }
                            else
                            {
                                   //策划说货币也要弹出Tips
//                                 if (uigrid.Data.DwObjectId == MainPlayerHelper.GoldID || uigrid.Data.DwObjectId == MainPlayerHelper.MoneyTicketID)
//                                 {
//                                     return;
//                                 }
                                TipsManager.Instance.ShowItemTips(uigrid.Data.DwObjectId, uigrid.gameObject, false);
                            }
                        }
                        else
                        {
                            PanelID panelId = UIFrameManager.Instance.CurrShowPanelID;
                            uint itemID = uigrid.Data.DwObjectId;
                            if (DataManager.Manager<UIPanelManager>().IsShowPanel(panelId))
                            {
                                UIPanelBase panelBase = DataManager.Manager<UIPanelManager>().GetPanel(panelId);
                                UIPanelManager.LocalPanelInfo uidata = panelBase.PanelInfo;
                                //Client.UIPanelInfo uidata = panelBase.PanelShowInfo;
                                if (uidata != null && uidata.NeedBg)
                                {
                                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemID);
                                }
                            }
                            else if (panelId == PanelID.None)
                            {
                                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemID);
                            }
                        }
                        break;
                    case UIEventType.LongPress:
                        blongPress = true;
                        if (pressCallback != null)
                        {
                            pressCallback(uigrid, blongPress);
                            return;
                        }
                        if (grid.Data.ItemThisId >0)
                        {
                            TipsManager.Instance.ShowItemTips(grid.Data.ItemThisId, grid.gameObject, false);
                        }
                        else
                        {
                            TipsManager.Instance.ShowItemTips(uigrid.Data.DwObjectId, uigrid.gameObject, false);
                        }
                        break;
                    case UIEventType.Press:
                        bool press = (bool)param;
                        if (!press && blongPress)
                        {
                            blongPress = false;
                            if (pressCallback != null)
                            {
                                pressCallback(uigrid, blongPress);
                                return;
                            }
                            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ItemTipsPanel);
                        }
                        break;
                }
            });   
        }

        UIItem item = new UIItem(grid);
        return item;
    }

    /// <summary>
    /// 金币经验类icon显示
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public UIItem GetUIMoneyExpItem(ItemDefine.CommonItem type,uint num,bool showGetWay = true)
    {
        //货币的图集Icon跟枚举一致
        string keyStr = ((int)type).ToString();
        UIItem uiitem = GetUIItem(keyStr,num);
        if (!showGetWay)
        {
            UIItemCommonGrid grid = uiitem.GetGrid<UIItemCommonGrid>();
            grid.SetShowGetWay(showGetWay);
        }
        return uiitem;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="path">资源路径</param>
    public void Release(string path)
    {
        
    }

    /// <summary>
    /// 释放所有
    /// </summary>
    public void ReleaseAll()
    {
       
    }

    public static string BuildCircleIconName(string iconName)
    {
        if(string.IsNullOrEmpty(iconName))
        {
            return string.Empty;
        }
        return string.Format("{0}{1}", iconName, UIDefine.ICON_CIRCLE_SUFFIX);
    }

    public static string GetIconName(string iconName,bool circleStyle = false)
    {
        if (circleStyle)
        {
            string tempIconName = BuildCircleIconName(iconName);
            iconName = tempIconName;
        }
        return iconName;
    }

    /// <summary>
    /// 通过文件名称获取资源ID
    /// </summary>
    /// <param name="isAtlas">true:图集 false:texture</param>
    /// <param name="iconName"></param>
    /// <param name="circleStyle"></param>
    /// <returns></returns>
    public uint GetResIDByFileName(bool isAtlas ,string iconName, bool circleStyle = false)
    {
        int resID = 0;
        iconName = GetIconName(iconName, circleStyle);
        if (string.IsNullOrEmpty(iconName))
        {
            return 0;
        }
        if ((isAtlas && mdicDynamicAtlas.TryGetValue(iconName, out resID))
            || (!isAtlas && mdicDynamicIcons.TryGetValue(iconName,out resID)))
        {
            return (uint)resID;
        }
        return 0;
    }


    private void LoadAtlasJsonFile()
    {
        mdicDynamicAtlas = new Dictionary<string, int>();
        JsonNode root = RareJson.ParseJsonFile("ui/atlas.json");
        if (root == null)
        {
            Engine.Utility.Log.Error("UIManager 解析{0}文件失败!", "ui/atlas.json");
            return;
        }

        JsonArray icons = (JsonArray)root["Icons"];
        for (int i = 0; i < icons.Count; i++)
        {
            JsonObject obj = (JsonObject)icons[i];
            if (obj != null)
            {

                string atlas = obj["name"];
                int resID = obj["resID"];
                JsonArray sprites = (JsonArray)obj["sprites"];
                if (null == sprites)
                {
                    continue;
                }
                for (int k = 0; k < sprites.Count; k++)
                {
                    JsonObject spriteobj = (JsonObject)sprites[k];

                    string spriteName = spriteobj[k.ToString()];
                    if (!mdicDynamicAtlas.ContainsKey(spriteName))
                    {
                        mdicDynamicAtlas.Add(spriteName, resID);
                    }
                    else
                    {
                        Engine.Utility.Log.Error("Read Icon Json error ,exist icon:{0} in atlas1:{1} atlas2:{2}", spriteName, atlas, mdicDynamicAtlas[spriteName]);
                        continue;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 加载图片文件
    /// </summary>
    private void LoadIconJsonFile()
    {
        mdicDynamicIcons = new Dictionary<string, int>();
        JsonNode root = RareJson.ParseJsonFile("ui/icon.json");
        if (root == null)
        {
            Engine.Utility.Log.Error("UIManager 解析{0}文件失败!", "ui/icon.json");
            return;
        }

        JsonArray icons = (JsonArray)root["Icons"];
        for (int i = 0; i < icons.Count; i++)
        {
            JsonObject obj = (JsonObject)icons[i];
            if (obj != null)
            {
             
                string iconName = obj["name"];
                int resID = obj["resID"];
                if (mdicDynamicIcons.ContainsKey(iconName))
                {
                    Engine.Utility.Log.Error("UIManager->LoadIconJsonFile exist icon:{0}", iconName);
                    continue;
                    
                }
                mdicDynamicIcons.Add(iconName, resID);
            }
        }
    }

    #region LowMemeryWarning(低内存警告)

    /// <summary>
    /// 释放没有使用的资源
    /// </summary>
    private void DoLowMemory()
    {
        DataManager.Manager<CMResourceMgr>().OnLowMemory();
        GameApp.Instance().UnloadUnusedAsset(true);
    }

    /// <summary>
    /// 低内存回调
    /// </summary>
    private void OnLowMemory()
    {
        //TODO: LowMemery
        DoLowMemory();
    }
    #endregion
}