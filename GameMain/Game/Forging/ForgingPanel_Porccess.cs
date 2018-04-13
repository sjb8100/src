/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ForgingPanel_Porccess
 * 版本号：  V1.0.0.0
 * 创建时间：11/10/2016 3:48:22 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ForgingPanel
{
    #region define
    //锻造装备属性索引
    public enum ForgingProccessPropertyIndex
    {
        None = -1,
        One = 0,
        Two,
        Three,
        Four,
        Five,
        Max,
    }

    /// <summary>
    /// 锻造加工模式
    /// </summary>
    public enum ForgingProccessMode
    {
        //提升
        Promote = 1,
        //移除
        Remove = 2,
        //提取
        Fetch = 3,
        Max,
    }

    #endregion

    #region property
    //当前加工模式
    private ForgingProccessMode m_em_fpm = ForgingProccessMode.Promote;
    //选中符石id
    private uint m_uint_selectRSBaseId = 0;
    //选中符石
    private RuneStone SelectRS
    {
        get
        {
            if (m_uint_selectRSBaseId != 0)
            {
                RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(m_uint_selectRSBaseId,ItemDefine.ItemDataType.RuneStone);
                return (rs.IsBaseReady) ? rs : null;
            }
            return null;
        }
    }
    //当前选中的属性id
    public uint SelectProccessAttrId
    {
        get
        {
            if (IsSelectEquipAttr())
            {
                GameCmd.PairNumber attr = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
                if (null != attr)
                {
                    return attr.id;
                }
            }
            return 0;
        }
    }

    //当前勾选属性索引
    private ForgingProccessPropertyIndex m_em_selectEquipAttrIndex = ForgingProccessPropertyIndex.None;
    //加工tabs
    private Dictionary<ForgingProccessMode, UITabGrid> m_dic_fpTabs = null;
    //加工属性格子Cur
    private Dictionary<ForgingProccessPropertyIndex, UIEquipPropertyGrid> m_dic_propCurGrids = null;
    //加工属性格子Next
    private Dictionary<ForgingProccessPropertyIndex, UIEquipPropertyGrid> m_dic_propNextGrids = null;
    //加工checkboxToggle
    private Dictionary<ForgingProccessPropertyIndex, UIToggle> m_dic_propCBs = null;
    private UIItemGrowShowGrid m_processGrowShow;
    private UIProccessGrid m_processRsGrowShow;
    #endregion

    #region InitProccess

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clearIndex"></param>
    /// <param name="clearSelectRsId"></param>
    private void ResetProccess(bool clearIndex = true,bool clearSelectRsId  = true)
    {
        if (clearIndex)
        {
            m_em_selectEquipAttrIndex = ForgingProccessPropertyIndex.None;
            if (null != m_dic_propCBs)
            {
                UIToggle toggle = null;
                for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One
                ; i < ForgingProccessPropertyIndex.Max; i++)
                {
                    if (m_dic_propCBs.TryGetValue(i, out toggle)
                        && null != toggle && toggle.value)
                    {
                        toggle.Set(false);
                    }
                }
            }
        }

        if (clearSelectRsId)
        {
            m_uint_selectRSBaseId = 0;
        }
    }

    private void InitProccessWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.Proccess))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.Proccess, true);
        //装备
        GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemgrowshowgrid) as GameObject;
        if (null != preObj)
        {
            GameObject cloneObj = null;
            if (null != m_trans_ProcessInfoRoot && null == m_processGrowShow)
            {
                cloneObj = NGUITools.AddChild(m_trans_ProcessInfoRoot.gameObject, preObj);
                if (null != cloneObj)
                {
                    m_processGrowShow = cloneObj.GetComponent<UIItemGrowShowGrid>();
                    if (null == m_processGrowShow)
                    {
                        m_processGrowShow = cloneObj.AddComponent<UIItemGrowShowGrid>();
                    }
                    m_processGrowShow.RegisterUIEventDelegate(OnUIEventCallback);
                }
            }

            preObj = UIManager.GetResGameObj(GridID.Uiproccessgrid) as GameObject;
            if (null != m_trans_ProcessCostRoot && null == m_processRsGrowShow)
            {
                cloneObj = NGUITools.AddChild(m_trans_ProcessCostRoot.gameObject, preObj);
                if (null != cloneObj)
                {
                    m_processRsGrowShow = cloneObj.GetComponent<UIProccessGrid>();
                    if (null == m_processRsGrowShow)
                    {
                        m_processRsGrowShow = cloneObj.AddComponent<UIProccessGrid>();
                    }
                    m_processRsGrowShow.RegisterUIEventDelegate((eventType, data, param) =>
                        {
                            if (eventType == UIEventType.Click)
                            {
                                if (data is UIItemInfoGrid)
                                {
                                    UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                                    if (null != infoGrid && infoGrid.NotEnough && null != param && param is uint)
                                    {
                                        ShowItemGet((uint)param);
                                    }
                                }
                                else if (data is UIProccessGrid)
                                {
                                    if (IsPanelMode(ForgingPanelMode.Proccess))
                                    {
                                        if (!IsProccessMode(ForgingProccessMode.Fetch))
                                        {
                                            if (null != param)
                                            {
                                                m_uint_selectRSBaseId = 0;
                                                m_processRsGrowShow.SetGridData(m_uint_selectRSBaseId, needUnload: true);
                                            }else
                                            {
                                                OnSelectProcessRuneStone();
                                            }
                                        }
                                    }
                                }
                            }
                        });
                }
            }
        }
        

        if (null == m_dic_fpTabs)
        {
            m_dic_fpTabs = new Dictionary<ForgingProccessMode, UITabGrid>();
        }
        m_dic_fpTabs.Clear();
        Transform ts = null;
        if (null != m_trans_ProcessTabs)
        {
            UITabGrid tGrid = null;
            for(ForgingProccessMode i = ForgingProccessMode.Promote ; i < ForgingProccessMode.Max;i++)
            {
                ts = m_trans_ProcessTabs.Find(i.ToString() + "Tab");
                if (null == ts)
                {
                    continue;
                }
                tGrid = ts.GetComponent<UITabGrid>();
                if (null == tGrid)
                {
                    tGrid = ts.gameObject.AddComponent<UITabGrid>();
                }
                tGrid.SetGridData(i);
                tGrid.RegisterUIEventDelegate(OnUIEventCallback);
                tGrid.SetHightLight(false);
                m_dic_fpTabs.Add(i, tGrid);
            }
        }
        if (null == m_dic_propCurGrids)
        {
            m_dic_propCurGrids = new Dictionary<ForgingProccessPropertyIndex, UIEquipPropertyGrid>();
        }
        m_dic_propCurGrids.Clear();
        if (null == m_dic_propNextGrids)
        {
            m_dic_propNextGrids = new Dictionary<ForgingProccessPropertyIndex, UIEquipPropertyGrid>();
        }
        m_dic_propNextGrids.Clear();
        if (null == m_dic_propCBs)
        {
            m_dic_propCBs = new Dictionary<ForgingProccessPropertyIndex, UIToggle>();
        }
        m_dic_propCBs.Clear();

        UIEquipPropertyGrid uPGrid = null;
        UIToggle toggle = null;
        //属性格子
        for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One
            ; i < ForgingProccessPropertyIndex.Max;i++ )
        {
            if (null != m_trans_ProccessPropertyContentRoot)
            {
                //Cur
                ts = Util.findTransform(m_trans_ProccessPropertyContentRoot, i.ToString() + "CPP");
                if (null != ts)
                {
                    uPGrid = ts.GetComponent<UIEquipPropertyGrid>();
                    if (null == uPGrid)
                    {
                        uPGrid = ts.gameObject.AddComponent<UIEquipPropertyGrid>();
                    }
                    m_dic_propCurGrids.Add(i, uPGrid);
                }
                //Nex
                ts = Util.findTransform(m_trans_ProccessPropertyContentRoot, i.ToString() + "NPP");
                if (null != ts)
                {
                    uPGrid = ts.GetComponent<UIEquipPropertyGrid>();
                    if (null == uPGrid)
                    {
                        uPGrid = ts.gameObject.AddComponent<UIEquipPropertyGrid>();
                    }
                    m_dic_propNextGrids.Add(i, uPGrid);
                }
                //CheckBox
                ts = Util.findTransform(m_trans_ProccessPropertyContentRoot, i.ToString() + "CB");
                if (null != ts)
                {
                    toggle = ts.GetComponent<UIToggle>();
                    if (null == uPGrid)
                    {
                        toggle = ts.gameObject.AddComponent<UIToggle>();
                    }

                    m_dic_propCBs.Add(i, toggle);
                }
            }
        }

        SetProccessMode(ForgingProccessMode.Promote, true);
        //InitPromote();
        //InitRemove();
        //InitFetch();
    }

    #endregion

    #region Promote

    //提升UI缓存
    private List<UIEquipPropertyPromoteGrid> m_dic_promoteGridTrans 
        = new List<UIEquipPropertyPromoteGrid>();
    /// <summary>
    /// 属性提升成功
    /// </summary>
    /// <param name="ret"></param>
    private void OnPromoteComplete(bool success)
    {
        bool clearRs = true;
        GameCmd.PairNumber selectAttrPair = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
        if (null != selectAttrPair)
        {
            //筛选满足条件符石
            uint selectAttrGrade = emgr.GetAttrGrade(selectAttrPair);
           
            if (m_uint_selectRSBaseId != 0)
            {
                RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(m_uint_selectRSBaseId, ItemDefine.ItemDataType.RuneStone);
                int holdNum = imgr.GetItemNumByBaseId(m_uint_selectRSBaseId);
                clearRs = rs.Grade < selectAttrGrade || holdNum <= 0;
            }
        }
        ResetProccess(clearIndex: false,clearSelectRsId:clearRs);
        string tips = "恭喜，提升属性成功";
        if (!success)
        {
            tips = "很遗憾，属性提升失败";
        }
        UpdateProccess(Data);
        TipsManager.Instance.ShowTips(tips);
    }

    /// <summary>
    /// 点击属性提升符石选择
    /// </summary>
    private void OnSelectProcessRuneStone()
    {
        if (!IsSelectEquipAttr()|| DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemChoosePanel))
        {
            TipsManager.Instance.ShowTips("请勾选需要提升的属性");
            return;
        }

        GameCmd.PairNumber selectAttrPair = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
        if (null == selectAttrPair)
        {
            Engine.Utility.Log.Error("Get AttrPair faield,promoteid:{0}", 0);
            return;
        }
        uint selectAttrGrade = emgr.GetAttrGrade(selectAttrPair);
        bool filterEqual = IsProccessMode(ForgingProccessMode.Promote);
        //筛选满足条件符石
        List<uint> baseIdList = emgr.GetMatchProperyRunestone(selectAttrPair, filterEqual);

        UILabel desLabel = null;
        Action<UILabel> desLabAction = (des) =>
        {
            desLabel = des;
            if (null != desLabel)
            {
                if (filterEqual)
                {
                    desLabel.text = m_tmgr.GetLocalText(LocalTextType.Local_TXT_Rune_1);
                }
                else
                {
                    desLabel.text = m_tmgr.GetLocalText(LocalTextType.Local_TXT_Rune_2);
                }
            }
                
        };
        uint tempSelectGemRuneStoneId = 0;
        UIRSInfoSelectGrid selectGemRuneStoneGrid = null;
        UIEventDelegate eventDlg = (eventTye, grid, param) =>
        {
            if (eventTye == UIEventType.Click)
            {
                UIRSInfoSelectGrid tempGrid = grid as UIRSInfoSelectGrid;
                RuneStone tempRs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(tempGrid.BaseId, ItemDefine.ItemDataType.RuneStone);
                if (tempSelectGemRuneStoneId == tempGrid.BaseId)
                {
                    tempSelectGemRuneStoneId = 0;
                    tempGrid.SetHightLight(false);
                }
                else if (tempGrid.BaseId != 0)
                {
                    bool enable = (!filterEqual) ? (tempRs.Grade >= selectAttrGrade) : (tempRs.Grade > selectAttrGrade);
                    if (enable)
                    {
                        tempSelectGemRuneStoneId = tempGrid.BaseId;
                        if (null != selectGemRuneStoneGrid)
                            selectGemRuneStoneGrid.SetHightLight(false);
                        tempGrid.SetHightLight(true);
                        selectGemRuneStoneGrid = tempGrid;
                    }else if (filterEqual)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Rune_Promote1);
                    }else
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Rune_Eliminate1);
                    }
                }

            }
        };

        ItemChoosePanel.ItemChooseShowData showData = new ItemChoosePanel.ItemChooseShowData()
        {
            createNum = baseIdList.Count,
            title = "选择符石",
            nullTips = "您未拥有该类符石",
            cloneTemp = m_trans_UIRSInfoSelectGrid.gameObject,
            onChooseCallback = () =>
            {
                if (tempSelectGemRuneStoneId != m_uint_selectRSBaseId)
                {
                    m_uint_selectRSBaseId = tempSelectGemRuneStoneId;
                    UpdateProccess(Data);
                }
            },
            gridCreateCallback = (obj, index) =>
            {
                if (index >= baseIdList.Count)
                {
                    Engine.Utility.Log.Error("OnSelectPromoteRuneStone error,index out of range");
                }
                else
                {
                    table.ItemDataBase tempItemDataBase = imgr.GetLocalDataBase<table.ItemDataBase>(baseIdList[index]);
                    table.RunestoneDataBase tempRuneStoneDataBase = imgr.GetLocalDataBase<table.RunestoneDataBase>(baseIdList[index]);
                    if (null == tempItemDataBase || null == tempRuneStoneDataBase)
                    {
                        Engine.Utility.Log.Error("OnSelectPromoteRuneStone error,get itemdatabase or tempItemDataBase null baseid:{0}", baseIdList[index]);
                    }
                    else
                    {
                        UIRSInfoSelectGrid gemRuneStoneGrid = obj.GetComponent<UIRSInfoSelectGrid>();
                        if (null == gemRuneStoneGrid)
                            gemRuneStoneGrid = obj.AddComponent<UIRSInfoSelectGrid>();
                        gemRuneStoneGrid.RegisterUIEventDelegate(eventDlg);
                        RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(baseIdList[index], ItemDefine.ItemDataType.RuneStone);
                        bool select = (baseIdList[index] == m_uint_selectRSBaseId);
                        bool enable = (!filterEqual) ? (rs.Grade >= selectAttrGrade) : (rs.Grade > selectAttrGrade);
                        gemRuneStoneGrid.SetGridViewData(baseIdList[index], select, enable);
                        if (select)
                        {
                            selectGemRuneStoneGrid = gemRuneStoneGrid;
                            tempSelectGemRuneStoneId = baseIdList[index];
                        }

                    }
                }

            },
            desPassCallback = desLabAction,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ItemChoosePanel, data: showData);
    }
   
    private void DoPromote()
    {
        string tips = "";
        bool success = true;
        if (!emgr.IsItemHaveAdditiveAttrOrGemHole(selectEquipId, false))
        {
            success = false;
            tips = "当前装备没有附加属性，无法进行提升操作";
        }
        else if (!IsSelectEquipAttr())
        {
            success = false;
            tips = "请勾选要提升的属性";
        }
        else if (m_uint_selectRSBaseId == 0)
        {
            success = false;
            tips = "请选择符石";
        }
        if (!success)
        {
            TipsManager.Instance.ShowTips(tips);
            return;
        }
        
        GameCmd.PairNumber pair = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
        emgr.EquipPropertyPromote(selectEquipId, m_uint_selectRSBaseId, ((null != pair) ? pair.id : 0));
    }
    #endregion

    #region Remove
    /// <summary>
    ///属性消除 
    /// </summary>
    /// <param name="ret"></param>
    private void OnRemoveComplete()
    {
        ResetProccess();
        UpdateProccess(Data);
    }
    private void DoRemove()
    {
        string tips = "";
        bool success = true;
        if (!emgr.IsItemHaveAdditiveAttrOrGemHole(selectEquipId))
        {
            success = false;
            tips = "当前装备没有附加属性，无法进行消除操作";
        }
        else if (!IsSelectEquipAttr())
        {
            success = false;
            tips = "请先勾选要消除的附加属性";
        }
        else if (m_uint_selectRSBaseId == 0)
        {
            success = false;
            tips = "请选择符石";
        }
        else if (imgr.GetItemNumByBaseId(m_uint_selectRSBaseId) == 0)
        {
            success = false;
            tips = "符石数量不足";
        }

        if (!success)
        {
            TipsManager.Instance.ShowTips(tips);
            return;
        }
        
        GameCmd.PairNumber pair = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
        emgr.EquipPropertyRemove(selectEquipId, m_uint_selectRSBaseId, ((null != pair) ? pair.id : 0));
    }
    #endregion

    #region Fetch
    // <summary>
    /// 属性提升成功
    /// </summary>
    /// <param name="ret"></param>
    private void OnFetchComplete(uint stoneId)
    {
        ResetProccess();
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RuneStoneInvokeCompletePanel,data:stoneId);
        UpdateProccess(Data);
    }

    private void DoFetch()
    {
        if (!emgr.IsItemHaveAdditiveAttrOrGemHole(selectEquipId))
        {
            TipsManager.Instance.ShowTips("当前装备没有附加属性，无法进行消除操作");
            return;
        }

        if (emgr.IsWearEquip(selectEquipId))
        {
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.Ok, "提取只可对背包中道具操作！",null);
            return;
        }
        DataManager.Manager<EquipManager>().ActiveRuneStone(selectEquipId);
    }
    #endregion

    #region Op
    /// <summary>
    /// 设置选中装备属性索引(toggle切换)
    /// </summary>
    /// <param name="index"></param>
    private void SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex index)
    {
        m_em_selectEquipAttrIndex = (index != m_em_selectEquipAttrIndex)
            ? index : ForgingProccessPropertyIndex.None;
        //重置选着符石id
        m_uint_selectRSBaseId = 0;
        //if (IsProccessMode(ForgingProccessMode.Remove)
        //    && m_em_selectEquipAttrIndex != ForgingProccessPropertyIndex.None)
        //{
        //    GameCmd.PairNumber attr = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex, true);
        //    if (null != attr)
        //    {
        //        //重新计算满足条件的符石id
        //        m_uint_selectRSBaseId = emgr.GetMatchRunStoneByAttr(GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex));
        //    }
        //}
        UpdateProccess(Data);
    }

    /// <summary>
    /// 是否勾选装备属性
    /// </summary>
    /// <returns></returns>
    private bool IsSelectEquipAttr()
    {
        return (m_em_selectEquipAttrIndex != ForgingProccessPropertyIndex.None);
    }

    /// <summary>
    /// 根据索引获取属性id
    /// </summary>
    /// <param name="index">属性索引</param>
    /// <param name="includeHole">是否包括宝石孔属性</param>
    /// <returns></returns>
    public GameCmd.PairNumber GetEquipAttrByIndex(int index,bool includeHole = false)
    {
        List<GameCmd.PairNumber> attrs = GetEquipAttrs(false);

        return (null != attrs && attrs.Count > index && index >= 0) ? attrs[index] : null;
    }

    /// <summary>
    /// 获取装备属性列表
    /// </summary>
    /// <param name="includeHole">是否包括宝石孔属性</param>
    /// <returns></returns>
    public List<GameCmd.PairNumber> GetEquipAttrs(bool includeHole = false)
    {
        List<GameCmd.PairNumber> attrs = new List<GameCmd.PairNumber>();
        Equip equip = Data;
        if (null != equip)
        {
            attrs.AddRange(equip.GetAdditiveAttr());
            if (includeHole)
            {
                uint attrValue = 0;
                if (equip.TryGetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_HoleNum, out attrValue))
                {

                    attrs.Add(new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_HoleNum,
                        value = attrValue,
                    });
                }
            }
        }
        return attrs;
    }

    /// <summary>
    /// 获取选中装备的属性数量
    /// </summary>
    /// <param name="includeHole">是否包括宝石孔属性</param>
    /// <returns></returns>
    public int GetEquipAttrCount(bool includeHole = false)
    {
        return GetEquipAttrs(false).Count;
    }
    /// <summary>
    /// 设置锻造加工模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetProccessMode(ForgingProccessMode mode,bool force = false)
    {
        if (this.m_em_fpm == mode && !force)
            return;

        UITabGrid grid = null;
        if (null != m_dic_fpTabs && m_dic_fpTabs.TryGetValue(m_em_fpm, out grid))
        {
            grid.SetHightLight(false);
        }
        this.m_em_fpm = mode;
        if (null != m_dic_fpTabs && m_dic_fpTabs.TryGetValue(m_em_fpm, out grid))
        {
            grid.SetHightLight(true);
        }
        ResetProccess();
        UpdateProccess(Data);
    }


    /// <summary>
    /// 是否为当前加工模式
    /// </summary>
    /// <param name="mode"></param>
    private bool IsProccessMode(ForgingProccessMode mode)
    {
        return (m_em_fpm == mode);
    }

    #endregion

    #region Update

    /// <summary>
    /// 隐藏组件
    /// </summary>
    private void DisableProccessGrid()
    {
        for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One
            ; i < ForgingProccessPropertyIndex.Max; i++)
        {
            if (m_dic_propCurGrids.ContainsKey(i) && m_dic_propCurGrids[i].Visible)
            {
                m_dic_propCurGrids[i].SetVisible(false);
            }
            if (m_dic_propNextGrids.ContainsKey(i) && m_dic_propNextGrids[i].Visible)
            {
                m_dic_propNextGrids[i].SetVisible(false);
            }
            if (m_dic_propCBs.ContainsKey(i) && m_dic_propCBs[i].gameObject.activeSelf)
            {
                m_dic_propCBs[i].gameObject.SetActive(false);
                m_dic_propCBs[i].value = false;
            }
        }
        if (null != m_trans_ArrowPromote && m_trans_ArrowPromote.gameObject.activeSelf)
        {
            m_trans_ArrowPromote.gameObject.SetActive(false);
        }
        if (null != m_label_ProccessTips && m_label_ProccessTips.gameObject.activeSelf)
        {
            m_label_ProccessTips.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 刷新加工消费
    /// </summary>
    private void UpdateProccessCost()
    {
        if (null != m_trans_ProccessCost)
        {
            uint proccessCost = 0;
            if (IsProccessMode(ForgingProccessMode.Fetch))
            {
                proccessCost = emgr.RuneStoneActivationCost;
            }
            else 
            {
                GameCmd.PairNumber selectAttrPair = GetEquipAttrByIndex((int)m_em_selectEquipAttrIndex);
                if (null != selectAttrPair)
                {
                    uint matchRunestoneID = emgr.GetMatchAttrRuneStone(selectAttrPair);
                    RuneStone rs = imgr.GetTempBaseItemByBaseID<RuneStone>(matchRunestoneID,ItemDefine.ItemDataType.RuneStone);
                    if (IsProccessMode(ForgingProccessMode.Remove))
                    {
                        proccessCost = (null != rs) ? rs.RemoveCost : 0;
                    }
                    else if (IsProccessMode(ForgingProccessMode.Promote))
                    {
                        proccessCost = (null != rs) ? rs.PromoteCost : 0;
                    }
                }
            }
            
            UICurrencyGrid currencyGrid = m_trans_ProccessCost.GetComponent<UICurrencyGrid>();
            if (null == currencyGrid)
                currencyGrid = m_trans_ProccessCost.gameObject.AddComponent<UICurrencyGrid>();
            currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold), proccessCost));
        }
    }
    /// <summary>
    /// 刷新加工按钮状态
    /// </summary>
    private void UpdateProccessBtnStatus()
    {
        bool cansee = IsProccessMode(ForgingProccessMode.Promote);
        if (null != m_btn_BtnPromote 
            && m_btn_BtnPromote.gameObject.activeSelf != cansee)
        {
            m_btn_BtnPromote.gameObject.SetActive(cansee);
        }
        cansee = IsProccessMode(ForgingProccessMode.Remove);
        if (null != m_btn_BtnRemove
            && m_btn_BtnRemove.gameObject.activeSelf != cansee)
        {
            m_btn_BtnRemove.gameObject.SetActive(cansee);
        }
        cansee = IsProccessMode(ForgingProccessMode.Fetch);
        if (null != m_btn_BtnFetch
            && m_btn_BtnFetch.gameObject.activeSelf != cansee)
        {
            m_btn_BtnFetch.gameObject.SetActive(cansee);
        }
    }

    /// <summary>
    /// 刷新加工panel
    /// </summary>
    /// <param name="equip"></param>
    internal void UpdateProccess(Equip equip)
    {
        if (null == equip)
        {
            return;
        }
        bool canProccess = equip.CanProccess;
        if (null != m_processGrowShow)
        {
            m_processGrowShow.SetGridData(equip.QWThisID);
        }
        
        if (null != m_label_ProccessWarningTxt && m_label_ProccessWarningTxt.gameObject.activeSelf == canProccess)
        {
            m_label_ProccessWarningTxt.gameObject.SetActive(!canProccess);
        }
        if (null != m_trans_ProccessBottom && m_trans_ProccessBottom.gameObject.activeSelf != canProccess)
        {
            m_trans_ProccessBottom.gameObject.SetActive(canProccess);
        }
        if (canProccess)
        {
            DisableProccessGrid();
            UpdateProccessCost();
            UpdateProccessBtnStatus();
            if (IsProccessMode(ForgingProccessMode.Promote))
            {
                UpdatePromote();
            }
            else if (IsProccessMode(ForgingProccessMode.Remove))
            {
                UpdateRemove();
            }
            else if (IsProccessMode(ForgingProccessMode.Fetch))
            {
                UpdateFetch();
            }
        }
    }

    /// <summary>
    /// 更新提升
    /// </summary>
    internal void UpdatePromote()
    {
        bool isSelectRS = (m_uint_selectRSBaseId != 0);
        RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(m_uint_selectRSBaseId, ItemDefine.ItemDataType.RuneStone);
        uint num = (isSelectRS) ? (uint)imgr.GetItemNumByBaseId(m_uint_selectRSBaseId) : 0;
        if (null != m_processRsGrowShow)
        {
            m_processRsGrowShow.SetGridData(m_uint_selectRSBaseId,needUnload:true);
        }
        int attrCount = GetEquipAttrCount();
        if (attrCount > 0)
        {
            EquipDefine.LocalAttrData attr = null;
            EquipDefine.LocalAttrData nextAttr = null;
            GameCmd.PairNumber attrPair = null;
            ColorType cType = ColorType.JZRY_Txt_Black;
            bool choose = false;
            Vector3 tempPos = Vector3.zero;
            for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One;
                i < (ForgingProccessPropertyIndex)attrCount; i++)
            {
                cType = ColorType.JZRY_Txt_Black;
                choose = IsSelectEquipAttr() && (m_em_selectEquipAttrIndex == i);
                attrPair = GetEquipAttrByIndex((int)i);
                attr = EquipDefine.LocalAttrData.Create(attrPair);
                if (m_dic_propCurGrids.ContainsKey(i))
                {
                    m_dic_propCurGrids[i].SetVisible(true);
                    m_dic_propCurGrids[i].SetGridView(ColorManager.GetColorString(cType, attr.AttrDes), attr.IsMaxGrade, true
                    , attr.Grade);
                }

                if (m_dic_propNextGrids.ContainsKey(i))
                {
                    cType = (choose) ? ColorType.JZRY_Green : ColorType.JZRY_Txt_Black;
                    m_dic_propNextGrids[i].SetVisible(true);
                    if (attr.IsMaxGrade)
                    {
                        m_dic_propNextGrids[i].SetGridView("", false, false);
                        m_dic_propNextGrids[i].SetProperty("已提升至最大等级");
                    }else
                    {
                        nextAttr = EquipDefine.LocalAttrData.Create((null != attr) ? attr.NextEffectId : 0);
                        if (null != nextAttr)
                        {
                            m_dic_propNextGrids[i].SetGridView(
                                ColorManager.GetColorString(cType, nextAttr.AttrDes), nextAttr.IsMaxGrade, true, nextAttr.Grade);
                        }
                        else
                        {
                            Engine.Utility.Log.Error("Forgingpanel_proccess UpdatePromote nextAttr null");
                        }
                    }
                    
                    if (null != m_trans_ArrowPromote && choose && isSelectRS)
                    {
                        if (!m_trans_ArrowPromote.gameObject.activeSelf)
                        {
                            m_trans_ArrowPromote.gameObject.SetActive(true);
                        }
                        tempPos = m_trans_ArrowPromote.position;
                        tempPos.y = m_dic_propCurGrids[i].CacheTransform.position.y;
                        m_trans_ArrowPromote.transform.position = tempPos;
                        if (null != m_label_ArrowPromoteTxt)
                        {
                            uint differ = (null != attr && null != rs) ? (rs.Grade - attr.Grade) : 0;
                            m_label_ArrowPromoteTxt.text = string.Format("成功率{0}%", emgr.GetPropertyPromoteProp(differ));
                        }
                    }
                }

                if (m_dic_propCBs.ContainsKey(i) && !m_dic_propCBs[i].gameObject.activeSelf)
                {
                    m_dic_propCBs[i].gameObject.SetActive(true);
                    m_dic_propCBs[i].Set(choose);
                }
            }
            
        }
    }

    /// <summary>
    /// 更新消除
    /// </summary>
    internal void UpdateRemove()
    {
        bool isSelectRS = (m_uint_selectRSBaseId != 0);
        RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(m_uint_selectRSBaseId, ItemDefine.ItemDataType.RuneStone);
        uint num = (isSelectRS) ? (uint)imgr.GetItemNumByBaseId(m_uint_selectRSBaseId) : 0;
        if (null != m_processRsGrowShow)
        {
            m_processRsGrowShow.SetGridData(m_uint_selectRSBaseId,needUnload:true);
        }
        int attrCount = GetEquipAttrCount(false);
        if (attrCount > 0)
        {
            EquipDefine.LocalAttrData attr = null;
            GameCmd.PairNumber attrPair = null;
            ColorType cType = ColorType.JZRY_Txt_Black;
            bool choose = false;
            for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One; 
                i < (ForgingProccessPropertyIndex)attrCount; i++)
            {
                choose = IsSelectEquipAttr() && (m_em_selectEquipAttrIndex == i);
                cType = (choose) ? ColorType.JZRY_Txt_Red : ColorType.JZRY_Txt_Black;
                attrPair = GetEquipAttrByIndex((int)i,true);
                if (m_dic_propCurGrids.ContainsKey(i) && !m_dic_propCurGrids[i].Visible)
                {
                    m_dic_propCurGrids[i].SetVisible(true);
                    attr = EquipDefine.LocalAttrData.Create(attrPair);
                    m_dic_propCurGrids[i].SetGridView(ColorManager.GetColorString(cType, attr.AttrDes), attr.IsMaxGrade, true
                    , attr.Grade);
                }
                
                if (m_dic_propCBs.ContainsKey(i) && !m_dic_propCBs[i].gameObject.activeSelf)
                {
                    m_dic_propCBs[i].gameObject.SetActive(true);
                    m_dic_propCBs[i].Set(choose);
                }

                bool showTips = emgr.IsItemHaveAdditiveAttrOrGemHole(selectEquipId);
                if (null != m_label_ProccessTips && m_label_ProccessTips.gameObject.activeSelf != showTips)
                {
                    m_label_ProccessTips.gameObject.SetActive(showTips);
                    if (showTips)
                    {
                        m_label_ProccessTips.text = "勾选属性消除后消失";
                    }
                }
            }
        }
    }

    /// <summary>
    /// 更新提取
    /// </summary>
    internal void UpdateFetch()
    {
        RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(emgr.InactiveRuneStoneBaseId, ItemDefine.ItemDataType.RuneStone);
        uint num = (uint)imgr.GetItemNumByBaseId(emgr.InactiveRuneStoneBaseId);
        if (null != m_processRsGrowShow)
        {
            m_processRsGrowShow.SetGridData(rs.BaseId);
        }
        int attrCount = GetEquipAttrCount(true);
        if (attrCount >0)
        {
            EquipDefine.LocalAttrData attr = null;
            GameCmd.PairNumber attrPair = null;
            for (ForgingProccessPropertyIndex i = ForgingProccessPropertyIndex.One
            ; i < (ForgingProccessPropertyIndex) attrCount; i++)
            {
                attrPair = GetEquipAttrByIndex((int)i,true);
                if (m_dic_propCurGrids.ContainsKey(i) && !m_dic_propCurGrids[i].Visible)
                {
                    m_dic_propCurGrids[i].SetVisible(true);
                    attr = EquipDefine.LocalAttrData.Create(attrPair);
                    m_dic_propCurGrids[i].SetGridView(ColorManager.GetColorString(ColorType.JZRY_Txt_Black, attr.AttrDes), attr.IsMaxGrade, true
                    , attr.Grade);
                    
                }
            }
            bool showTips = emgr.IsItemHaveAdditiveAttrOrGemHole(selectEquipId);
            if (null != m_label_ProccessTips && m_label_ProccessTips.gameObject.activeSelf != showTips)
            {
                m_label_ProccessTips.gameObject.SetActive(showTips);
                if (showTips)
                {
                    m_label_ProccessTips.text = "提取后此装备消失";
                }
            }
        }
        
    }
    #endregion

    #region UIEvent
    void onClick_BtnFetch_Btn(GameObject caster)
    {
        DoFetch();
    }

    void onClick_BtnPromote_Btn(GameObject caster)
    {
        DoPromote();
    }

    void onClick_BtnRemove_Btn(GameObject caster)
    {
        DoRemove();
    }

    void onClick_OneCB_Btn(GameObject caster)
    {
        SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex.One);
    }

    void onClick_TwoCB_Btn(GameObject caster)
    {
        SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex.Two);
    }

    void onClick_ThreeCB_Btn(GameObject caster)
    {
        SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex.Three);
    }

    void onClick_FourCB_Btn(GameObject caster)
    {
        SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex.Four);
    }

    void onClick_FiveCB_Btn(GameObject caster)
    {
        SetSetlectEquipAttrIndex(ForgingProccessPropertyIndex.Five);
    }

    #endregion
}