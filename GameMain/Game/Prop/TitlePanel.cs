using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine;
using System.Collections;

//*******************************************************************************************
//	创建日期：	2016-11-1   16:09
//	文件名称：	TitlePanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	称号
//*******************************************************************************************
partial class PropPanel
{
    #region property


    UISecondTabCreatorBase m_secondsTabCreator;   //左侧称号(带二级页签)

    UIGridCreatorBase m_ForeverPropGridCreator;    //附加属性（永久属性）
    UIGridCreatorBase m_ActivatePropGridCreator;  // 激活属性

    Dictionary<uint, List<uint>> m_titleDic = null;//key: 小类 value: 里面的称号 id list
    List<uint> m_lstTitleData = new List<uint>();  //当前的grid 的list
    List<uint> m_lstTitleCategory = new List<uint>(); //称号类别list

    uint m_selectTitleTypeId; //小类Id
    uint m_selectTitleId;    //选中的
    uint m_wearTitleId;      //佩戴的
    uint m_activateTitleId;  //激活的
    //uint m_newTitleId;       //新的

    TitleDataBase m_titleDataBase = null;    //表格中数据
    stTitleData m_titleData = null; //服务器下发的数据

    List<TitleDataBase> m_lstTableTitle = null;  //称号表格

    uint m_SelectCategoryItem = 0;  //选中的类别

    uint itemIdForBuy = 0;//徽章，用于重置称号使用次数

    IRenerTextureObj rtObj = null;//称号特效

    //缓存
    List<GameObject> m_lstTitleTypeGridCache = new List<GameObject>();
    List<GameObject> m_lstTitleSecondTypeGridCache = new List<GameObject>();

    /// <summary>
    /// 时效性
    /// </summary>
    public enum Timeliness
    {
        Forever = 0,  //永远
        TimeLimit,//限时
        NumLimit,//限次
        TimeNumLimit,//限时限次
    }

    Timeliness m_eTitleTimeliness;

    TitleManager TManager
    {
        get
        {
            return DataManager.Manager<TitleManager>();
        }
    }
    #endregion


    #region method

    /// <summary>
    /// 初始化称号
    /// </summary>
    void InitTitle()
    {
        InitTitleData();      //1、 数据

        InitTitleWidget();     //2、组件初始化
        CreateTopTabs();      //上方可切换也签
    }


    /// <summary>
    /// 初始化称号数据
    /// </summary>
    void InitTitleData()
    {
        //TManager.InitTitleDic();

        m_lstTitleData = TManager.GetTitleTypeList(m_SelectCategoryItem);
        m_titleDic = TManager.GetTitleTypeDic(m_SelectCategoryItem);

        m_wearTitleId = TManager.WearTitleId;
        m_activateTitleId = TManager.ActivateTitleId;

        m_selectTitleId = 0;

        itemIdForBuy = GameTableManager.Instance.GetGlobalConfig<uint>("ItemAddCountTitle"); //徽章，用于重置称号使用次数
    }

    /// <summary>
    /// 初始化顶部的grids
    /// </summary>
    void InitTitleWidget()
    {
        //上面的页签
        if (m_ctor_TitleCategoryScrollview != null)
        {
           // UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uititlecategorygrid) as UnityEngine.GameObject;
            m_ctor_TitleCategoryScrollview.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor_TitleCategoryScrollview.gridWidth = 141;
            m_ctor_TitleCategoryScrollview.gridHeight = 50;
            m_ctor_TitleCategoryScrollview.RefreshCheck();

            m_ctor_TitleCategoryScrollview.Initialize<UITitleCategoryGrid>(m_trans_UITitleCategorygrid.gameObject,  OnGridDataUpdate, OnGridUIEvent);
        }

        //左侧的称号列表（带二级页签）
        if (m_scrollview_TitleListScrollView != null)
        {
            m_secondsTabCreator = m_scrollview_TitleListScrollView.gameObject.GetComponent<UISecondTabCreatorBase>();
            if (m_secondsTabCreator == null)
            {
                m_secondsTabCreator = m_scrollview_TitleListScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();
                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uititlesecondtypegrid) as GameObject;

                m_secondsTabCreator.Initialize<UITitleSecondTypeGrid>(cloneFTemp, cloneSTemp, OnUpdateTitileCtrTypeGrid, OnUpdateTitleSecondGrid, OnTitileCtrTypeGridUIEventDlg);
            }
        }

        //附加属性
        if (m_trans_AttachedScrollViewContent != null)
        {
            m_ForeverPropGridCreator = m_trans_AttachedScrollViewContent.GetComponent<UIGridCreatorBase>();
            if (m_ForeverPropGridCreator == null)
            {
                m_ForeverPropGridCreator = m_trans_AttachedScrollViewContent.gameObject.AddComponent<UIGridCreatorBase>();

                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uititleforeverpropgrid) as UnityEngine.GameObject;
                m_ForeverPropGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
                m_ForeverPropGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_ForeverPropGridCreator.gridWidth = 250;
                m_ForeverPropGridCreator.gridHeight = 32;

                m_ForeverPropGridCreator.RefreshCheck();
                m_ForeverPropGridCreator.Initialize<UITitleForeverPropGrid>((uint)GridID.Uititleforeverpropgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGridDataUpdate, OnGridUIEvent);
            }
        }

        //激活属性
        if (m_trans_UseScrollViewContent != null)
        {
            m_ActivatePropGridCreator = m_trans_UseScrollViewContent.GetComponent<UIGridCreatorBase>();
            if (m_ActivatePropGridCreator == null)
            {
                m_ActivatePropGridCreator = m_trans_UseScrollViewContent.gameObject.AddComponent<UIGridCreatorBase>();

                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uititleactivatepropgrid) as UnityEngine.GameObject;
                m_ActivatePropGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
                m_ActivatePropGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_ActivatePropGridCreator.gridWidth = 250;
                m_ActivatePropGridCreator.gridHeight = 32;

                m_ActivatePropGridCreator.RefreshCheck();
                m_ActivatePropGridCreator.Initialize<UITitleActivatePropGrid>((uint)GridID.Uititleactivatepropgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGridDataUpdate, OnGridUIEvent);
            }
        }
    }

    /// <summary>
    /// 跟新格子数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UITitleCategoryGrid)
        {
            if (null != m_lstTitleCategory && index < m_lstTitleCategory.Count)
            {
                UITitleCategoryGrid grid = data as UITitleCategoryGrid;
                if (grid != null)
                {
                    grid.SetGridData(m_lstTitleCategory[index]);

                    //name
                    List<uint> ownlist = TManager.GetOwnTitleListByType(m_lstTitleCategory[index]);
                    string name = string.Format("{0}{1}", TManager.TitleCategoryDic[m_lstTitleCategory[index]], ownlist.Count > 0 ? "(" + ownlist.Count + ")" : "");
                    grid.SetName(name);

                    //newMark
                    List<uint> titleList = TManager.GetTitleTypeList(m_lstTitleCategory[index]);
                    bool haveNew = HaveNewTilteInList(titleList);
                    grid.SetNewMark(haveNew);
                }
            }
        }

        //附加属性（永久属性）
        if (data is UITitleForeverPropGrid)
        {
            List<uint> foreverAddList = TManager.GetForeverAddList(this.m_selectTitleId);
            if (foreverAddList != null && index < foreverAddList.Count)
            {
                UITitleForeverPropGrid grid = data as UITitleForeverPropGrid;
                if (grid != null)
                {
                    grid.SetGridData(foreverAddList[index]);
                }
            }
        }

        //激活属性
        if (data is UITitleActivatePropGrid)
        {
            List<uint> activateAddList = TManager.GetActivateAddList(this.m_selectTitleId);
            if (activateAddList != null && index < activateAddList.Count)
            {
                UITitleActivatePropGrid grid = data as UITitleActivatePropGrid;
                if (grid != null)
                {
                    grid.SetGridData(activateAddList[index]);
                }
            }
        }

    }

    /// <summary>
    /// 点击
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UITitleCategoryGrid)
            {
                UITitleCategoryGrid grid = data as UITitleCategoryGrid;
                if (grid != null)
                {
                    SetSelectTitleCategoryItem(grid.m_typeId);
                }
            }
        }
    }

    /// <summary>
    /// 5个可切换的称号
    /// </summary>
    void CreateTopTabs()
    {
        m_lstTitleCategory = TManager.TitleCategoryList;

        if (m_ctor_TitleCategoryScrollview != null)
        {
            m_ctor_TitleCategoryScrollview.CreateGrids((null != m_lstTitleCategory) ? m_lstTitleCategory.Count : 0);
        }

        SetSelectTitleCategoryItem(TManager.TitleCategoryList[0]);//默认选中第一个
    }

    /// <summary>
    /// 选中的类别grid
    /// </summary>
    /// <param name="titleTypeId"></param>
    void SetSelectTitleCategoryItem(uint titleTypeId)
    {
        UITitleCategoryGrid grid = m_ctor_TitleCategoryScrollview.GetGrid<UITitleCategoryGrid>(m_lstTitleCategory.IndexOf(this.m_SelectCategoryItem));
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = m_ctor_TitleCategoryScrollview.GetGrid<UITitleCategoryGrid>(m_lstTitleCategory.IndexOf(titleTypeId));
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_SelectCategoryItem = titleTypeId;
        m_lstTitleData = TManager.GetTitleTypeList(this.m_SelectCategoryItem);
        m_titleDic = TManager.GetTitleTypeDic(this.m_SelectCategoryItem);

        CreateTitleGrids();  //3、左侧页签

        SelectFirstTitle();  //选中第一个称号
    }

    /// <summary>
    /// 创建左侧页签
    /// </summary>
    void CreateTitleGrids()
    {
        List<int> list = new List<int>();
        Dictionary<uint, List<uint>>.Enumerator enumerator = m_titleDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            list.Add(enumerator.Current.Value.Count);
        }

        m_secondsTabCreator.CreateGrids(list);
    }

    /// <summary>
    /// 选中第一个称号
    /// </summary>
    void SelectFirstTitle()
    {
        Dictionary<uint, List<uint>>.KeyCollection kc = m_titleDic.Keys;
        List<uint> keyIdList = kc.ToList<uint>();
        if (keyIdList.Count > 0)
        {
            SetSelectCtrTypeGrid(keyIdList[0], true);
            List<uint> titleIdList;
            if (m_titleDic.TryGetValue(keyIdList[0], out titleIdList))
            {
                if (titleIdList.Count > 0)
                {
                    SetSelectSecondType(keyIdList[0], titleIdList[0]);
                }
            }
        }
    }

    /// <summary>
    /// 跟新一级页签数据
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="index"></param>
    private void OnUpdateTitileCtrTypeGrid(UIGridBase gridBase, int index)
    {
        if (m_titleDic != null && m_titleDic.Keys.Count > index)
        {
            UICtrTypeGrid grid = gridBase as UICtrTypeGrid;
            if (grid == null)
            {
                return;
            }

            Dictionary<uint, List<uint>>.KeyCollection kc = m_titleDic.Keys;
            List<uint> keyIdList = kc.ToList<uint>();
            uint keyId = keyIdList[index];

            List<uint> secondIdList;
            if (m_titleDic.TryGetValue(keyId, out secondIdList))
            {
                if (secondIdList.Count == 0)
                {
                    return;
                }
                TitleDataBase tdb = m_lstTableTitle.Find((d) => { return d.dwID == secondIdList[0]; });

                //数据
                grid.SetData(keyId, tdb != null ? tdb.secondTypeName : "", secondIdList.Count);

                //红点
                bool haveNewTitle = HaveNewTilteInList(secondIdList);
                grid.SetRedPointStatus(haveNewTitle);
            }
        }

    }

    /// <summary>
    /// 更新二级页签数据
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="id"></param>
    /// <param name="index"></param>
    private void OnUpdateTitleSecondGrid(UIGridBase gridBase, object id, int index)
    {
        UITitleSecondTypeGrid grid = gridBase as UITitleSecondTypeGrid;
        if (grid == null)
        {
            return;
        }

        List<uint> secondIdList;
        if (m_titleDic.TryGetValue((uint)id, out secondIdList))
        {
            if (secondIdList.Count > index)
            {
                grid.SetGridData((uint)id, secondIdList[index]);

                grid.SetSelect(secondIdList[index] == m_selectTitleId);
                bool isNewTitle = TManager.NewTitleIdList.Contains(secondIdList[index]);
                grid.SetRedPointStatus(isNewTitle);

            }
        }
    }

    /// <summary>
    /// grid事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnTitileCtrTypeGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            //一级页签
            if (data is UICtrTypeGrid)
            {

                UICtrTypeGrid grid = data as UICtrTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectCtrTypeGrid((uint)grid.ID);

                //默认选中二级页签第一个
                List<uint> secondIdList;
                if (m_titleDic.TryGetValue((uint)grid.ID, out secondIdList))
                {
                    if (secondIdList.Count > 0)
                    {
                        uint secondId = secondIdList[0];
                        SetSelectSecondType((uint)grid.ID, secondId);
                    }
                }
            }

            //二级页签
            if (data is UITitleSecondTypeGrid)
            {
                UITitleSecondTypeGrid grid = data as UITitleSecondTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectSecondType(grid.FirstKeyId, grid.TitleId);
            }
        }
    }

    /// <summary>
    /// 选中一级页签
    /// </summary>
    /// <param name="id"></param>
    /// <param name="instant"></param>
    void SetSelectCtrTypeGrid(uint id, bool instant = false)
    {
        Dictionary<uint, List<uint>>.KeyCollection kc = m_titleDic.Keys;
        List<uint> keyIdList = kc.ToList<uint>();

        List<uint> titleIdList;

        if (id == m_selectTitleTypeId)
        {
            if (m_titleDic.TryGetValue(m_selectTitleTypeId, out titleIdList) && m_secondsTabCreator.IsOpen(keyIdList.IndexOf(m_selectTitleTypeId))) //以前选中的
            {
                m_secondsTabCreator.Close(keyIdList.IndexOf(m_selectTitleTypeId), true);
                return;
            }
        }

        if (m_titleDic.TryGetValue(m_selectTitleTypeId, out titleIdList)) //以前选中的
        {
            m_secondsTabCreator.Close(keyIdList.IndexOf(m_selectTitleTypeId), true);
        }

        if (m_titleDic.TryGetValue(id, out titleIdList))    //现在选中的
        {
            m_secondsTabCreator.Open(keyIdList.IndexOf(id), instant);
        }

        m_selectTitleTypeId = id;
    }

    /// <summary>
    /// 设置选中二级分页
    /// </summary>
    /// <param name="firstKeyId"></param>
    /// <param name="titleId"></param>
    /// <param name="force"></param>
    private void SetSelectSecondType(uint firstKeyId, uint titleId, bool force = false)
    {
        UITitleSecondTypeGrid grid = GetTitleSecondTypeGrid(m_selectTitleTypeId, m_selectTitleId);
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = GetTitleSecondTypeGrid(firstKeyId, titleId);
        if (grid != null)
        {
            //选中高亮
            grid.SetSelect(true);

            //取消grid的红点
            grid.SetRedPointStatus(false);
        }

        this.m_selectTitleId = titleId;

        InitTitleInfoUI();

        //取消红点提示
        CancelNewTitleRedPoint(firstKeyId, titleId);
    }

    UITitleSecondTypeGrid GetTitleSecondTypeGrid(uint firstKeyId, uint titleId)
    {
        List<uint> firstKeyIdList = m_titleDic.Keys.ToList<uint>();
        List<uint> titleIdList;
        UITitleSecondTypeGrid grid;
        if (m_titleDic.TryGetValue(firstKeyId, out titleIdList))
        {
            grid = m_secondsTabCreator.GetGrid<UITitleSecondTypeGrid>(firstKeyIdList.IndexOf(firstKeyId), titleIdList.IndexOf(titleId));
            if (grid != null)
            {
                return grid;
            }
        }
        return null;
    }


    /// <summary>
    /// 右侧称呼详细信息
    /// </summary>
    /// <param name="titleId"></param>
    void InitTitleInfoUI()
    {
        m_titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(this.m_selectTitleId);     //表格中数据
        m_titleData = TManager.OwnedTitleList.Find((data) => { return data.dwID == this.m_selectTitleId; }); //服务器下发的数据

        //当前称号详细信息
        if (m_titleDataBase != null)
        {
            //称号UI展示
            InitTitleDisplay(m_titleDataBase);

            //时效
            InitTimelinessUI(m_titleDataBase.timeliness, m_titleData);

            //描述
            m_label_TitleDescription.text = m_titleDataBase.strDesc;

            //永久加成
            CreateForeverAddUI();

            //激活加成
            CreateActivateAddUI();

            //佩戴和激活按钮初始化
            InitWearAndActivateUI();
        }
    }

    /// <summary>
    /// 称号展示
    /// </summary>
    void InitTitleDisplay(TitleDataBase titleDataBase)
    {
        if (m_titleDataBase.UIState == 0)
        {
            m_label_TitleLabel.gameObject.SetActive(true);
            m_trans_TitleFx.gameObject.SetActive(false);
            m_label_TitleLabel.text = titleDataBase.TextUI;
        }

        if (m_titleDataBase.UIState == 1)
        {
            m_label_TitleLabel.gameObject.SetActive(false);
            m_trans_TitleFx.gameObject.SetActive(true);

            uint fxId = m_titleDataBase.FxUI;

            //特效
            UIParticleWidget wight = m_trans_TitleFx.GetComponent<UIParticleWidget>();
            if (null == wight)
            {
                wight = m_trans_TitleFx.gameObject.AddComponent<UIParticleWidget>();
                wight.depth = 100;
            }

            if (wight != null)
            {
                wight.ReleaseParticle();
                wight.AddParticle(fxId);
            }
        }
    }

    void CreateEffectEvent(IEffect obj)
    {
        string s = "";
        Node node = obj.GetNode();
        if (node == null)
        {
            return;
        }

        Transform fxTransf = node.GetTransForm();
        if (fxTransf == null)
        {
            return;
        }

        if (m_trans_TitleFx.transform.childCount == 0)
        {
            fxTransf.parent = m_trans_TitleFx;
            fxTransf.localPosition = Vector3.zero;
            fxTransf.localRotation = Quaternion.identity;
            fxTransf.localScale = Vector3.one;
        }

    }

    /// <summary>
    /// 时效UI1
    /// </summary>
    void InitTimelinessUI(uint timeliness, stTitleData titleData)
    {
        this.m_eTitleTimeliness = (Timeliness)timeliness;
        if (m_eTitleTimeliness == Timeliness.Forever)//永久
        {
            m_widget_Permanent.gameObject.SetActive(true);//永久
            m_widget_LimitedTime.gameObject.SetActive(false);//限时
            m_widget_LimitedNumber.gameObject.SetActive(false);//限次
            m_widget_NumAndTime.gameObject.SetActive(false); //
        }

        if (m_eTitleTimeliness == Timeliness.TimeLimit)//限时
        {
            m_widget_Permanent.gameObject.SetActive(false);//永久
            m_widget_LimitedTime.gameObject.SetActive(true);//限时
            m_widget_LimitedNumber.gameObject.SetActive(false);//限次
            m_widget_NumAndTime.gameObject.SetActive(false); //
            if (m_titleData != null)
            {
                m_label_LimitedTimeLbl.text = StringUtil.GetStringBySeconds(m_titleData.dwTime);
            }
            else
            {
                m_label_LimitedTimeLbl.text = "0";
            }
        }

        if (m_eTitleTimeliness == Timeliness.NumLimit)//限次
        {
            m_widget_Permanent.gameObject.SetActive(false);//永久
            m_widget_LimitedTime.gameObject.SetActive(false);//限时
            m_widget_LimitedNumber.gameObject.SetActive(true);//限次
            m_widget_NumAndTime.gameObject.SetActive(false); //
            if (m_titleData != null)
            {
                m_label_LimitedNumberLbl.text = m_titleData.dwCount.ToString();
            }
            else
            {
                m_label_LimitedNumberLbl.text = "0";
            }
        }

        if (m_eTitleTimeliness == Timeliness.TimeNumLimit)//限时限次
        {
            m_widget_Permanent.gameObject.SetActive(false);//永久
            m_widget_LimitedTime.gameObject.SetActive(false);//限时
            m_widget_LimitedNumber.gameObject.SetActive(false);//限次
            m_widget_NumAndTime.gameObject.SetActive(true); //限时限次

            if (m_titleData != null)
            {
                m_label_UseNumberLbl.text = m_titleData.dwCount.ToString();
                m_label_UseTimeLbl.text = StringUtil.GetStringBySeconds(m_titleData.dwTime);
            }
            else
            {
                m_label_UseNumberLbl.text = "0";
                m_label_UseTimeLbl.text = "0";
            }
        }
    }

    /// <summary>
    /// 永久加成UI
    /// </summary>
    void CreateForeverAddUI()
    {
        //战斗力
        m_label_AttachedFighting.text = TManager.GetForeverAddFight(this.m_selectTitleId).ToString();
        List<uint> foreverAddList = TManager.GetForeverAddList(this.m_selectTitleId);

        if (m_ForeverPropGridCreator != null)
        {
            m_ForeverPropGridCreator.CreateGrids(foreverAddList != null ? foreverAddList.Count : 0);
        }
    }

    /// <summary>
    /// 激活加成UI
    /// </summary>
    void CreateActivateAddUI()
    {
        m_label_UseFighting.text = TManager.GetActivateAddFight(this.m_selectTitleId).ToString();
        List<uint> activateAddList = TManager.GetActivateAddList(this.m_selectTitleId);

        if (m_ActivatePropGridCreator != null)
        {
            m_ActivatePropGridCreator.CreateGrids(activateAddList != null ? activateAddList.Count : 0);
        }
    }


    void InitWearAndActivateUI()
    {
        bool isHave = false;
        if (m_titleData != null)
        {
            m_btn_btn_WearTitle.isEnabled = true;
            m_btn_btn_UseTitle.isEnabled = true;
            m_btn_btn_WearAndUseTitle.isEnabled = true;
            m_btn_btn_UseNumAdd.isEnabled = true;
            m_btn_btn_LimitedNumberAdd.isEnabled = true;
        }
        else
        {
            m_btn_btn_WearTitle.isEnabled = false;
            m_btn_btn_UseTitle.isEnabled = false;
            m_btn_btn_WearAndUseTitle.isEnabled = false;
            m_btn_btn_UseNumAdd.isEnabled = false;
            m_btn_btn_LimitedNumberAdd.isEnabled = false;
        }
    }
    private CMResAsynSeedData<CMTexture> m_CASD = null;
    /// <summary>
    /// 增加使用次数UI
    /// </summary>
    void InitAddUseTimesUI()
    {
        ItemDataBase itemDataBase = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemIdForBuy);
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemIdForBuy);
        //icon
        UITexture sp = m_btn_ptguiyuan_icon.GetComponent<UITexture>();
        UIManager.GetTextureAsyn(itemDataBase.itemIcon, ref m_CASD, () =>
        {
            if (null != sp)
            {
                sp.mainTexture = null;
            }
        }, sp);

        // name , num , des
        m_label_ptguiyuan_name.text = itemDataBase.itemName;
        m_label_ptguiyuan_number.text = string.Format("{0}/1", count);
        m_label_description.text = itemDataBase.description;
    }

    void AddUseTimes(bool useDianJunAutoBuy, int num)
    {
        TManager.ReqAddTitleUseTimes(this.m_selectTitleId, itemIdForBuy);
    }

    /// <summary>
    /// 佩戴的
    /// </summary>
    /// <param name="titleId"></param>
    public void SetWearTitleItem(uint titleId)
    {
        if (this.m_wearTitleId == titleId)
        {
            return;
        }

        UITitleSecondTypeGrid grid = GetTitleSecondTypeGrid(m_selectTitleTypeId, m_selectTitleId);
        if (titleId != 0)
        {
            if (grid != null)
            {
                grid.SetWearMark(true);
            }
        }
        else
        {
            grid.SetWearMark(false);
        }
        this.m_wearTitleId = titleId;
    }

    /// <summary>
    /// 激活的
    /// </summary>
    /// <param name="titleId"></param>
    public void SetActivateTitleItem(uint titleId)
    {
        if (this.m_activateTitleId == titleId)
        {
            return;
        }

        UITitleSecondTypeGrid grid = GetTitleSecondTypeGrid(m_selectTitleTypeId, m_selectTitleId);
        if (titleId != 0)
        {
            if (grid != null)
            {
                grid.SetActivateMark(true);
            }
        }
        else
        {
            grid.SetActivateMark(false);
        }

        this.m_activateTitleId = titleId;
    }

    /// <summary>
    /// 取消红点提示
    /// </summary>
    /// <param name="titleId"></param>
    public void CancelNewTitleRedPoint(uint firstKeyId, uint titleId)
    {
        //1、 清数据
        if (TManager.IsNewTitle(titleId))
        {
            TManager.NewTitleIdList.Remove(titleId);

            //检查主界面红点提示
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.TITLE_NEWTITLE,
                direction = (int)WarningDirection.Left,
                bShowRed = TManager.HaveNewTitle(),
            };
            EventEngine.Instance().DispatchEvent((int)GameEventID.MAINPANEL_SHOWREDWARING, st);
        }

        //2、 下面的一级页签
        if (m_secondsTabCreator != null)
        {
            List<uint> titleIdList;
            if (m_titleDic.TryGetValue(firstKeyId, out titleIdList))
            {
                bool haveNewTitle = HaveNewTilteInList(titleIdList);

                List<uint> firstKeyIdList = m_titleDic.Keys.ToList<uint>();
                UICtrTypeGrid ctrTypeGrid = m_secondsTabCreator.GetGrid<UICtrTypeGrid>(firstKeyIdList.IndexOf(firstKeyId));
                if (ctrTypeGrid != null)
                {
                    ctrTypeGrid.SetRedPointStatus(haveNewTitle);
                }
            }
        }

        //3、最上面的grid
        SetTopCategoryGridNewMark(HaveNewTilteInList(m_lstTitleData));
    }

    void UpdateUseTimes(uint titleId)
    {
        if (this.m_selectTitleId == titleId)
        {
            m_titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(this.m_selectTitleId);     //表格中数据
            m_titleData = TManager.OwnedTitleList.Find((data) => { return data.dwID == this.m_selectTitleId; }); //服务器下发的数据

            //当前称号详细信息
            if (m_titleDataBase != null)
            {
                InitTimelinessUI(m_titleDataBase.timeliness, m_titleData);  //时效
            }
        }
    }

    /// <summary>
    /// list中是否有新的未查看的title
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    bool HaveNewTilteInList(List<uint> list)
    {
        for (int i = 0; i < TManager.NewTitleIdList.Count; i++)
        {
            if (list.Contains(TManager.NewTitleIdList[i]) == true)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 设置上面的也签红点提示
    /// </summary>
    /// <param name="go"></param>
    /// <param name="b"></param>
    void SetTopCategoryGridNewMark(bool b)
    {
        UITitleCategoryGrid grid = m_ctor_TitleCategoryScrollview.GetGrid<UITitleCategoryGrid>(m_lstTitleCategory.IndexOf(this.m_SelectCategoryItem));
        if (grid != null)
        {
            grid.SetNewMark(b);
        }
    }


    #endregion


    #region mono

    float tempTime = 0f;
    void Update()
    {
        // 限时  限时限次
        if (this.m_eTitleTimeliness == Timeliness.TimeNumLimit || this.m_eTitleTimeliness == Timeliness.TimeLimit)
        {
            tempTime += Time.deltaTime;
            if (tempTime > 0.95f)
            {
                if (this.m_titleData != null && this.m_titleData.dwTime >= 0)
                {
                    //限时 时间更新
                    if (m_widget_LimitedTime.gameObject.activeSelf)
                    {
                        m_label_LimitedTimeLbl.text = StringUtil.GetStringBySeconds(this.m_titleData.dwTime);
                    }

                    //限时限次 里面的时间更新
                    if (m_widget_NumAndTime.gameObject.activeSelf)
                    {
                        m_label_UseTimeLbl.text = StringUtil.GetStringBySeconds(this.m_titleData.dwTime);
                    }
                }

                //清零
                tempTime = 0;
            }

        }
    }

    #endregion


    #region click

    /// <summary>
    /// 累加属性
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_ViewProp_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TitleAddPropertyPanel);
    }


    /// <summary>
    /// 限时限次  增加使用次数
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_UseNumAdd_Btn(GameObject caster)
    {
        //使用使用徽章界面
        string title = "使用徽章";
        uint itemId = itemIdForBuy;

        TipsManager.Instance.ShowUseItemWindow(title, itemId, AddUseTimes, null, setNum: false);

    }

    /// <summary>
    /// 限次，增加使用次数
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_LimitedNumberAdd_Btn(GameObject caster)
    {
        //使用使用徽章界面
        string title = "使用徽章";
        uint itemId = itemIdForBuy;

        TipsManager.Instance.ShowUseItemWindow(title, itemId, AddUseTimes, null, setNum: false);
    }


    void onClick_Ptguiyuan_icon_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel);
        HideSelf();
    }


    /// <summary>
    /// 增加使用次数  取消
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_quxiao_Btn(GameObject caster)
    {
    }

    /// <summary>
    /// 增加使用次数  确定
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_queding_Btn(GameObject caster)
    {
    }

    /// <summary>
    /// 增加使用次数  自动补足
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Zidongbuzu_Btn(GameObject caster)
    {

    }


    /// <summary>
    /// 佩戴称号
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_WearTitle_Btn(GameObject caster)
    {
        if (m_selectTitleId != TManager.WearTitleId)
        {
            TManager.ReqSelectTitle(this.m_selectTitleId);//佩戴称号
        }
        else
        {
            TManager.ReqSelectTitle(0);//取消佩戴称号
        }
    }


    bool isActivate = true;
    /// <summary>
    /// 使用称号
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_UseTitle_Btn(GameObject caster)
    {
        if (m_selectTitleId != TManager.ActivateTitleId)
        {
            TManager.ReqActivateTitle(this.m_selectTitleId);//激活称号
        }
        else
        {
            TManager.ReqActivateTitle(0);//取消激活称号
        }
    }


    bool isWearAndActivate = true;
    /// <summary>
    /// 佩戴并使用称号
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_WearAndUseTitle_Btn(GameObject caster)
    {

        if (m_selectTitleId != TManager.WearTitleId || m_selectTitleId != TManager.ActivateTitleId)
        {
            TManager.ReqSelectTitle(this.m_selectTitleId);//佩戴称号
            TManager.ReqActivateTitle(this.m_selectTitleId);//激活称号
        }
        else
        {
            TManager.ReqSelectTitle(0);//取消佩戴称号
            TManager.ReqActivateTitle(0);//取消激活称号
        }
    }


    void UpdateTimeLabel()
    {
        if (m_titleData != null)
        {
            if (m_titleData.dwTime > 0)
            {

            }

        }
    }

    void RefreshTitleGrids(uint titleId)
    {
        if (m_titleData == null || m_titleDic == null || m_secondsTabCreator == null)
        {
            return;
        }

        if (m_lstTitleData.Contains(titleId) == false)
        {
            return;
        }

        TManager.OrderTitleIdDic(titleId, ref m_titleDic);

        Dictionary<uint, List<uint>>.Enumerator enumerator = m_titleDic.GetEnumerator();
        Dictionary<uint, List<uint>>.KeyCollection kc = m_titleDic.Keys;
        List<uint> firstKeyIdList = kc.ToList<uint>();
        for (int i = 0; i < firstKeyIdList.Count; i++)
        {
            List<uint> secondKeyIdList = m_titleDic[firstKeyIdList[i]];
            for (int j = 0; j < secondKeyIdList.Count; j++)
            {
                m_secondsTabCreator.UpdateIndex(i, j);
            }
        }
    }
    #endregion

    void ReleaseTitle(bool depthRelease)
    {
        if (m_ctor_TitleCategoryScrollview != null)
        {
            m_ctor_TitleCategoryScrollview.Release(depthRelease);
        }

        if (m_ForeverPropGridCreator != null)
        {
            m_ForeverPropGridCreator.Release(depthRelease);
        }

        if (m_ActivatePropGridCreator != null)
        {
            m_ActivatePropGridCreator.Release(depthRelease);
        }
    }
}

