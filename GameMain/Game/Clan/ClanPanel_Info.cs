/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanPanel_Info
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:30:46 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanPanel
{
    #region define
    public enum ClanInfoMode
    {
        None = 0,
        Upgrade = 1,
        Donate = 2,
//        Task = 3,
        DeclareWar = 3,
        Max,
    }
    #endregion

    #region property
    //当前活跃的模式
    private ClanInfoMode m_em_activeInfoMode = ClanInfoMode.None;
    //tabs
    //private Dictionary<ClanInfoMode, UITabGrid> m_dic_infoTabs = null;
    //content
    private Dictionary<ClanInfoMode, Transform> m_dic_infoContents = null;
    //
    //private UIGridCreatorBase taskCreator = null;
    //Dictionary<ClanInfoMode, string> m_dic_tabContents = null;
    List<string> m_list_tabContents = new List<string>(4)
    {
        "氏族升级","氏族捐献","氏族宣战"
    };
    #endregion

    #region Init
    private void InitInfo()
    {
        if (m_ctor_LeftBtnRoot != null)
        {
            m_ctor_LeftBtnRoot.RefreshCheck();
            m_ctor_LeftBtnRoot.Initialize<UIClanTabGrid>(m_trans_UIClanTabGrid.gameObject,OnUpdateUIGrid, OnUIGridEventDlg);
            m_ctor_LeftBtnRoot.CreateGrids(m_list_tabContents.Count);

          
        }     
        switch(m_em_activeInfoMode)
        {
            case ClanInfoMode.DeclareWar:
                InitDeclareWar();
                break;
            case ClanInfoMode.Donate:
                InitDonate();
                break;
            case ClanInfoMode.Upgrade:
                InitUpgrade();
                break;
//             case ClanInfoMode.Task:
//                 InitTask();
//                 break;
        }

    }

    /// <summary>
    /// 构造info
    /// </summary>
    private void BuildInfo()
    {
        SetInfoMode(ClanInfoMode.Upgrade);
        if (m_ctor_LeftBtnRoot != null)
        {
            UIClanTabGrid tab = m_ctor_LeftBtnRoot.GetGrid<UIClanTabGrid>(0);
            if (tab != null)
            {
                tab.SetHightLight(true);              
            }
            if (previous != null && previous != tab)
            {
                previous.SetHightLight(false);
            }
            previous = tab;
        }
      
    }

    
    #endregion

    
    #region Op
    /// <summary>
    /// 设置氏族信息tab状态
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="enable"></param>
    private void SetInfoTabEnable(ClanInfoMode mode,bool enable)
    {
//         UITabGrid tab = null;
//         if ( m_dic_infoTabs.TryGetValue(mode, out tab))
//         {
//             tab.SetHightLight(enable);
//             SetInfoContentVisible(mode, enable);
//         }
    }

    /// <summary>
    /// 设置信息分类组件是否可见
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="visible"></param>
    private void SetInfoContentVisible(ClanInfoMode mode,bool visible)
    {
        Transform ts = null;
        if (null != m_dic_infoContents 
            && m_dic_infoContents.TryGetValue(mode,out ts)
            && ts.gameObject.activeSelf != visible)
        {
            ts.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// 设置氏族信息当前活跃的模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetInfoMode(ClanInfoMode mode)
    {
       
        m_em_activeInfoMode = mode;
        InitInfo();
        switch (mode)
        {
            case ClanInfoMode.DeclareWar:
                BuildDeclareWarList();
                break;
        }
        UpdateInfo();
        UIClanTabGrid tab = m_ctor_LeftBtnRoot.GetGrid<UIClanTabGrid>((int)mode - 1);
        if (tab == null)
        {
            return;
        }
        if (previous == tab)
        {
            return;
        }
        else
        {
            previous.SetHightLight(false);
            tab.SetHightLight(true);
            previous = tab;
        }
    }

    /// <summary>
    /// 跳转氏族商店
    /// </summary>
    private void GoToClanShop()
    {

        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eNone;
        returnData[0].panelid = PanelID.ClanPanel;
        returnData[0].param = null;

        UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
        jumpData.Tabs = new int[1];

        jumpData.Tabs[0] = (int)ShopPanel.TabMode.ShengWang;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShopPanel, jumpData: jumpData);
    }


    UIClanDeclareWarGrid preDeclareWarGrid = null;
    /// <summary>
    /// 氏族信息tab事件委托
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnInfoGridUIDlg(UIEventType type,object data,object param)
    {
        switch(type)
        {
            case UIEventType.Click:
                {
                    if (data is UITabGrid)
                    {
                        UITabGrid tabGrid = (UITabGrid) data;
                        ClanInfoMode mode = (ClanInfoMode)tabGrid.Data;
                         if (mode != ClanInfoMode.None)
                        {
                            SetInfoMode(mode);
                        }
                    }
                    else if (data is UIClanDeclareWarGrid)
                    {
                        UIClanDeclareWarGrid g = data as UIClanDeclareWarGrid;                     
                        if (preDeclareWarGrid != null)
                        {
                            preDeclareWarGrid.SetSelect(false);
                        }
                        g.SetSelect(true);
                        preDeclareWarGrid = g;

                    }
                }
                
                break;
        }
    }

    private void UpdateInfo()
    {
        switch (m_em_activeInfoMode)
        {
            case ClanInfoMode.Donate:
                {
                    UpdateDonate();
                }
                break;
            case ClanInfoMode.Upgrade:
                {
                    UpdateUpgrade();
                }
                break;
            case ClanInfoMode.DeclareWar:
                {
                    UpdateDeclareWar();
                }
                break;
        }
    }
    #endregion

    /// <summary>
    /// 更新详情
    /// </summary>
    private void UpdateDetail()
    {
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        //氏族名称
        if (null != m_label_DetailClanTitle)
        {
            m_label_DetailClanTitle.text = (null != clanInfo) ? clanInfo.Name : "";
        }
        //氏族id
        if (null != m_trans_DetailInfoClanId)
        {
            m_trans_DetailInfoClanId.Find("Value").GetComponent<UILabel>().text 
                = (null != clanInfo) ? clanInfo.Id.ToString() : "";
        }
        //族长
        if (null != m_trans_DetailInfoShaikh)
        {
            GameCmd.stClanMemberInfo shaikh = (null != clanInfo) ? clanInfo.GetMemberInfo(clanInfo.ShaikhId) : null;
            m_trans_DetailInfoShaikh.Find("Value").GetComponent<UILabel>().text
                = (null != shaikh) ? shaikh.name : "";
        }
        //等级
        if (null != m_trans_DetailInfoClanLv)
        {
            m_trans_DetailInfoClanLv.Find("Value").GetComponent<UILabel>().text 
                = (null != clanInfo) ? clanInfo.Lv.ToString() : "";
        }
        //资金
        if (null != m_trans_DetailInfoClanMoney)
        {
            m_trans_DetailInfoClanMoney.Find("Value").GetComponent<UILabel>().text 
                = (null != clanInfo) ? clanInfo.Money.ToString() : "";
        }
        //总族贡
        if (null != m_trans_DetailInfoClanConb)
        {
            m_trans_DetailInfoClanConb.Find("Value").GetComponent<UILabel>().text 
                = (null != clanInfo) ? clanInfo.TotalZG.ToString() : "";
        }
        //战力
        if (null != m_trans_DetailInfoClanFgt)
        {
            m_trans_DetailInfoClanFgt.Find("Value").GetComponent<UILabel>().text
                = (null != clanInfo) ? clanInfo.Fight.ToString() : "";
        }
        //7日族贡
        if (null != m_trans_DetailInfoClanSpT)
        {
            m_trans_DetailInfoClanSpT.Find("Value").GetComponent<UILabel>().text
                = (null != clanInfo) ? clanInfo.SevenDayZG.ToString() : "";
        }
        //人数
        if (null != m_trans_DetailInfoClanNum)
        {
            uint membercount = 0;
            uint totalCount = 0;
            if (null != clanInfo)
            {
                membercount = (uint)clanInfo.MemberCount;
                totalCount = m_mgr.GetClanMemberMaxLimit(clanInfo.Lv);
            }
            m_trans_DetailInfoClanNum.Find("Value").GetComponent<UILabel>().text
                = string.Format("{0}/{1}",membercount,totalCount);
        }
        //日消耗族贡
        if (null != m_trans_DetailInfoClanDaySpT)
        {
            ClanDefine.LocalClanMemberDB db = ClanManger.GetLocalCalnMemberDB(clanInfo.Lv);
            m_trans_DetailInfoClanDaySpT.Find("Value").GetComponent<UILabel>().text
                = (null != db) ? db.CostZiJin.ToString() : "";
        }
        //shengwang
        m_label_ShengWangLabel.text = UserData.Reputation.ToString();
        //公告

        UpdateGG();
    }

    /// <summary>
    /// 公告
    /// </summary>
    private void UpdateGG()
    {
        //公告
        if (null != m_label_DetailGGInfo)
        {
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            m_label_DetailGGInfo.text = (null != clanInfo) ? clanInfo.GG : DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Clan_Commond_shizugonggao);
        }
    }
    /// <summary>
    /// 显示发送gg
    /// </summary>
    private void ShowSendGG()
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonInputPanel, data: new CommonInputPanel.CommonInputPanelData()
        {
            m_str_title = "公告",
            m_uint_maxWordsCount = 50,
            m_str_confirmText = "发送",
            m_str_starTxt = ((null != ClanInfo) ? ClanInfo.GG : ""),
            confimAction = (inputText) =>
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonInputPanel);
                if (string.IsNullOrEmpty(inputText))
                {
                    inputText = "";
                }
                SendGG(inputText);
            },
        });
    }

    /// <summary>
    /// 发送公告
    /// </summary>
    /// <param name="text"></param>
    private void SendGG(string text)
    {
        m_mgr.SendClanGG(text);
    }

    #region Donate
    //捐献数据
    private List<uint> m_list_donateDatas = null;
    private void StructDonateDatas()
    {
        if (null == m_list_donateDatas)
        {
            m_list_donateDatas = new List<uint>();
        }
        m_list_donateDatas.Clear();
        m_list_donateDatas.AddRange(DataManager.Manager<ClanManger>().GetClanDonateList());
    }

    private void BuildDonateList()
    {
        if (null != m_ctor_DonateScrollView)
        {
            m_ctor_DonateScrollView.CreateGrids(m_list_donateDatas.Count);
        }
    }

    private void InitDonate()
    {
        m_trans_DonateContent.gameObject.SetActive(true);
        m_trans_DeclareWarContent.gameObject.SetActive(false);
        m_trans_UpgradeContent.gameObject.SetActive(false);
        m_trans_ActivityScrollViewContent.gameObject.SetActive(false);
        
        if (null != m_ctor_DonateScrollView )
        {
            if (!IsInitMode(ClanPanel.ClanInfoMode.Donate))
            {
                SetInitMode(ClanPanel.ClanInfoMode.Donate);

                m_ctor_DonateScrollView.RefreshCheck();
                m_ctor_DonateScrollView.Initialize<UIClanDonateGrid>(m_trans_UIClanDonateGrid.gameObject,OnUpdateUIGrid, null);
            }
            //获取数据
            StructDonateDatas();
            BuildDonateList();
        }
    }

    /// <summary>
    /// 捐献成功
    /// </summary>
    /// <param name="id"></param>
    private void OnDonateSuccess(uint id)
    {
        int index = (null != m_list_donateDatas && m_list_donateDatas.Contains(id))
            ? m_list_donateDatas.IndexOf(id) : -1;
        if (index >=0 && null != m_ctor_DonateScrollView)
        {
            m_ctor_DonateScrollView.UpdateData(index);
        }
    }

    /// <summary>
    /// 捐献数据改变
    /// </summary>
    private void OnDonateDataChanged()
    {
        StructDonateDatas();
        BuildDonateList();
        //if (null != m_donateCreator)
        //{
        //    m_donateCreator.UpdateActiveGridData();
        //}
    }

    /// <summary>
    /// 更新捐献
    /// </summary>
    private void UpdateDonate()
    {

    }

    /// <summary>
    /// 捐献
    /// </summary>
    /// <param name="id"></param>
    private void DoDonate(uint id)
    {
        m_mgr.ClanDonate(id);
    }
    #endregion

    #region Honor
    //荣誉数据
    private List<GameCmd.stHonorInfo> m_list_honorInfos = null;
    private void InitHonor()
    {
        if (IsInitMode(ClanPanel.ClanMemberMode.Event))
        {
            return;
        }
        SetInitMode(ClanPanel.ClanMemberMode.Event);
        if (null != m_ctor_ClanHonorScrollView)
        {
            m_ctor_ClanHonorScrollView.Initialize<UIClanHonorGrid>(m_trans_UIClanHonorGrid.gameObject
                                    , OnUpdateUIGrid, null);
            //StructDonateDatas();
            //BuildHonorList();
            //请求容易数据
            m_mgr.GetHonorInfosReq();
        }
    }

    /// <summary>
    /// 构造氏族动态数据
    /// </summary>
    private void StructHonorData()
    {
        if (null == m_list_honorInfos)
        {
            m_list_honorInfos = new List<GameCmd.stHonorInfo>();
        }
        m_list_honorInfos.Clear();
        m_list_honorInfos.AddRange(m_mgr.GetHonorInfos());
    }
    /// <summary>
    /// 生成氏族动态列表
    /// </summary>
    private void BuildHonorList()
    {
        if (!IsInitMode(ClanMemberMode.Event))
        {
            return;
        }
        if (null != m_ctor_ClanHonorScrollView)
        {
            m_ctor_ClanHonorScrollView.CreateGrids((null != m_list_honorInfos) ? m_list_honorInfos.Count : 0);
        }
    }

    /// <summary>
    /// 添加氏族动态
    /// </summary>
    /// <param name="info"></param>
    private void OnHonorInfoAdd(GameCmd.stHonorInfo info)
    {
        if (null == info)
        {
            return;
        }
        if (null != m_ctor_ClanHonorScrollView)
        {
            if (m_list_honorInfos.Count > 0 && m_list_honorInfos.Count >= ClanManger.ClanHonorMaxKeepNum)
            {
                m_list_honorInfos.RemoveAt(0);
                m_ctor_ClanHonorScrollView.RemoveData(0);
            }
            m_list_honorInfos.Add(info);
            m_ctor_ClanHonorScrollView.InsertData(m_list_honorInfos.Count - 1);
        }
        
    }

    /// <summary>
    /// 刷新氏族动态
    /// </summary>
    private void OnRefreshHonorInfos()
    {
        StructHonorData();
        BuildHonorList();
    }

    /// <summary>
    /// 更新荣誉
    /// </summary>
    private void UpdateHonor()
    {

    }
    #endregion

    #region Upgrade
    private void OnClanUpgrade()
    {
        UpdateMember();
        UpdateUpgrade();
    }
    private void InitUpgrade()
    {
        m_trans_UpgradeContent.gameObject.SetActive(true);
        m_trans_DonateContent.gameObject.SetActive(false);
        m_trans_DeclareWarContent.gameObject.SetActive(false);
        m_trans_ActivityScrollViewContent.gameObject.SetActive(false);
       
        SetInitMode(ClanPanel.ClanInfoMode.Upgrade);
       
    }
    /// <summary>
    /// 更新升级
    /// </summary>
    private void UpdateUpgrade()
    {
        if (m_mgr.IsJoinClan && m_mgr.IsJoinFormal)
        {
            ClanDefine.LocalClanInfo claninfo = ClanInfo;
            if (null == claninfo)
            {
                Engine.Utility.Log.Error("ClanInfo Error");
                return;
            }
            bool isMax = m_mgr.IsMaxClanLv(claninfo.Lv);
            if (null != m_trans_UpgradeNormal && m_trans_UpgradeNormal.gameObject.activeSelf == isMax)
            {
                m_trans_UpgradeNormal.gameObject.SetActive(!isMax);
            }
            if (null != m_trans_UpgradeMax && m_trans_UpgradeMax.gameObject.activeSelf != isMax)
            {
                m_trans_UpgradeMax.gameObject.SetActive(isMax);
            }
            if (ClanInfo != null)
            {
                m_label_clanlevelnum.text = claninfo.Lv.ToString();
            }  
            if (isMax)
            {
                if (null != m_label_ClanMaxLv)
                {
                    m_label_ClanMaxLv.text = string.Format("氏族等级{0}", claninfo.Lv);
                }
                if (null != m_label_UpgradeMaxInfo)
                {
                    m_label_UpgradeMaxInfo.text = string.Format("成员上限 {0}", m_mgr.GetClanMemberMaxLimit(claninfo.Lv));
                }
            }else
            {
                if (null != m_trans_UpgradeCur)
                {
                    m_trans_UpgradeCur.Find("Title").GetComponent<UILabel>().text = string.Format("氏族等级{0}", claninfo.Lv);
                    m_trans_UpgradeCur.Find("Content").GetComponent<UILabel>().text = string.Format("成员上限 {0}", m_mgr.GetClanMemberMaxLimit(claninfo.Lv));
                }              
                if (null != m_trans_UpgradeNext)
                {
                    m_trans_UpgradeNext.Find("Title").GetComponent<UILabel>().text = string.Format("氏族等级{0}", claninfo.Lv +1);
                    m_trans_UpgradeNext.Find("Content").GetComponent<UILabel>().text = string.Format("成员上限 {0}", m_mgr.GetClanMemberMaxLimit(claninfo.Lv +1));
                }
                table.ClanUpgradeDataBase upDB = GameTableManager.Instance.GetTableItem<table.ClanUpgradeDataBase>(claninfo.Lv + 1);
                if (null != m_label_ClanUpgradeCostZG)
                {
                    m_label_ClanUpgradeCostZG.text = ItemDefine.BuilderStringByHoldAndNeedNum(claninfo.TotalZG,((null != upDB)? upDB.buildLv:0));
                }
                if (null != m_label_ClanUpgradeCostZJ)
                {
                    m_label_ClanUpgradeCostZJ.text = ItemDefine.BuilderStringByHoldAndNeedNum(claninfo.Money,((null != upDB)? upDB.clanMoney : 0));
                }
            }
        }
    }

    #endregion

    #region DeclareWar
    private List<uint> m_lst_Rivalry = null;
    private void OnActivePageSet(int pageIndex)
    {
        //UpdateDeclareList();
    }
//     private void UpdateDeclareList()
//     {
//         bool visible = (null != m_blockScrollview && m_blockScrollview.IsForwardEnable);
//         if (null != m_btn_BtnLeft && m_btn_BtnLeft.gameObject.activeSelf != visible)
//         {
//             m_btn_BtnLeft.gameObject.SetActive(visible);
//         }
//         visible = (null != m_blockScrollview && m_blockScrollview.IsNextEnable);
//         if (null != m_btn_BtnRight && m_btn_BtnRight.gameObject.activeSelf != visible)
//         {
//             m_btn_BtnRight.gameObject.SetActive(visible);
//         }
//     }

    private void InitDeclareWar()
    {
        m_trans_UpgradeContent.gameObject.SetActive(false);
        m_trans_DonateContent.gameObject.SetActive(false);
        m_trans_DeclareWarContent.gameObject.SetActive(true);
        m_trans_ActivityScrollViewContent.gameObject.SetActive(false);
        m_lst_Rivalry = new List<uint>();
        if (m_ctor_DecareWarListScoll != null)
        {
            if (!IsInitMode(ClanPanel.ClanInfoMode.DeclareWar))
            {
                SetInitMode(ClanPanel.ClanInfoMode.DeclareWar);


                m_ctor_DecareWarListScoll.RefreshCheck();
                m_ctor_DecareWarListScoll.Initialize<UIClanDeclareWarGrid>(m_trans_UIClanDeclareWarGrid.gameObject,OnUpdateUIGrid, OnInfoGridUIDlg);

            }
            
        }
        BuildDeclareWarList();
    }

    /// <summary>
    /// 构建敌对势力列表
    /// </summary>
    private void BuildDeclareWarList()
    {
        if (null == m_lst_Rivalry)
        {
            m_lst_Rivalry = new List<uint>();
        }
        int oldCount = m_lst_Rivalry.Count;
        int curCount = 0;
        m_lst_Rivalry.Clear();
        m_lst_Rivalry.AddRange(m_mgr.GetClanRivalryList());
        m_ctor_DecareWarListScoll.CreateGrids(m_lst_Rivalry.Count);
//         curCount = m_lst_Rivalry.Count;
//         int create = (curCount - oldCount);
//         if (null != m_blockScrollview)
//         {
//             if (create > 0)
//             {
//                 for(int i =0 ; i < create;i++)
//                 {
//                     m_blockScrollview.Insert(curCount - create + i);
//                 }
//             }
//             else if (create < 0)
//             {
//                 create = Mathf.Abs(create);
//                 for (int i = 0; i < create; i++)
//                 {
//                     m_blockScrollview.Remove(curCount - 1 - i);
//                 }
//             }
//             m_blockScrollview.UpdateActiveGrid();
//         }
//        UpdateDeclareList();
        UpdateDeclareWar();
    }

    /// <summary>
    /// 宣战信息改变
    /// </summary>
    /// <param name="data"></param>
    private void OnDeclareWarInfoChanged(uint clanId)
    {
//         if (null != m_blockScrollview)
//         {
//             bool remove = (null == m_mgr.GetClanRivalryInfo(clanId)) ? true : false;
//             bool exist = m_lst_Rivalry.Contains(clanId);
//             if (exist)
//             {
//                 int index = m_lst_Rivalry.IndexOf(clanId);
//                 if (remove)
//                 {
//                     m_lst_Rivalry.Remove(clanId);
//                     m_blockScrollview.Remove(index);
//                 }else
//                 {
//                     m_blockScrollview.UpdateData(index);
//                 }
//             }else if (!remove)
//             {
//                 m_lst_Rivalry.Add(clanId);
//                 m_blockScrollview.Insert(m_lst_Rivalry.Count - 1);
//             }
//         }
//         UpdateDeclareWar();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void UpdateDeclareWar()
    {
//         if (null != m_label_DeclareTitle)
//         {
//             m_label_DeclareTitle.text = string.Format("敌对氏族({0})",m_lst_Rivalry.Count) ;
//         }
    }
    #endregion

    #region UIEvent
    void onClick_BtnUpgrade_Btn(GameObject caster)
    {
        m_mgr.ClanUpgrade();
    }
    void onClick_BtnEditorGG_Btn(GameObject caster)
    {
        ShowSendGG();
    }

    void onClick_BtnDeclareWar_Btn(GameObject caster)
    {
        m_mgr.DoDeclareWar();
    }

    void onClick_BtnLeft_Btn(GameObject caster)
    {
//         if (null != m_blockScrollview)
//         {
//             m_blockScrollview.MoveToNext(true);
//             //UpdateDeclareList();
//         }
        
    }

    void onClick_BtnRight_Btn(GameObject caster)
    {
//         if (null != m_blockScrollview)
//         {
//             m_blockScrollview.MoveToNext(false);
//             //UpdateDeclareList();
//         }
    }

    /// <summary>
    /// 氏族领地
    /// </summary>
    /// <param name="caster"></param>
    void onClick_LingDiBtn_Btn(GameObject caster)
    {
        //让大威配到全局表里面，他不配，出问题找大威。
       uint mapId = 2;
       uint clanTerritoryNpcId = 10323;

       Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
       if (cs == null)
       {
           return;
       }
       Client.IController controller = cs.GetActiveCtrl();
       if (controller == null)
       {
           Engine.Utility.Log.Error("IController is null");
           return;
       }

       controller.VisiteNPC(mapId, clanTerritoryNpcId,true);
    }

    void onClick_ShengWangStoreBtn_Btn(GameObject caster)
    {

        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eNone;
        returnData[0].panelid = PanelID.ClanPanel;
        returnData[0].param = null;

        UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
        jumpData.Tabs = new int[1];
        jumpData.Tabs[0] = (int)ShopPanel.TabMode.ShengWang;//积分商城
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShopPanel, jumpData: jumpData);
    }


    #endregion
}