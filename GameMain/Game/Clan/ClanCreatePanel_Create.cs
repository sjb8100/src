/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanCreatePanel_Create
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:13:35 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanCreatePanel
{
    #region property
    //输入的名称
    private string m_str_inputClanName = "";
    //输入氏族公告
    private string m_str_inputGGInfo = "请输入氏族公告";
    //输入查询信息
    private string m_str_inputSeachClanInfo = "";
    //发送置顶公告消耗
    private UIItemGrowCostGrid m_createGrowShow = null;
    #endregion

    #region init
    private void InitCreate()
    {
        if (!IsInitMode(ClanCreateMode.Create))
        {
            if (null != m_label_CreateTipsTipsInfo)
            {
                string notice = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Clan_Commond_chuangjianshizugonggao,
                    ClanManger.ClanUnlockLevel,
                    ClanManger.BuildClanCostCopper,
                    ClanManger.BuildClanCostGold,
                    ClanManger.TempClanLastMinute / 60,
                     ClanManger.TempClanSupporter
                    );
                m_label_CreateTipsTipsInfo.text = notice;
            }

            if (null != m_input_ClanNameInput)
            {
                m_input_ClanNameInput.onChange.Add(new EventDelegate(() =>
                {
                    m_str_inputClanName = TextManager.GetTextByWordsCountLimitInUnicode(m_input_ClanNameInput.value
                        , TextManager.CONST_NAME_MAX_WORDS);
                    m_input_ClanNameInput.value = m_str_inputClanName;
                }));
                m_input_ClanNameInput.onSubmit.Add(new EventDelegate(() =>
                {
                    m_str_inputClanName = TextManager.GetTextByWordsCountLimitInUnicode(m_input_ClanNameInput.value
                         , TextManager.CONST_NAME_MAX_WORDS);
                    m_input_ClanNameInput.value = m_str_inputClanName;
                }));
            }

            SetInitMode(ClanCreateMode.Create);
        }
        
        if (!m_mgr.IsJoinClan)
        {
            UICurrencyGrid cGrid = null;
            if (null != m_trans_TempClanCostWQ)
            {
                cGrid = m_trans_TempClanCostWQ.GetComponent<UICurrencyGrid>();
                if (null == cGrid)
                {
                    cGrid = m_trans_TempClanCostWQ.gameObject.AddComponent<UICurrencyGrid>();
                }
                if (null != cGrid)
                {

//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                         MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_MoneyTicket)
//                         , ClanManger.BuildClanCostCopper));
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                                "tubiao_huobi_tong"
//                               , ClanManger.BuildClanCostCopper));
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                                null
//                               , ClanManger.BuildClanCostCopper));
                    cGrid.UpdateNum((int)ClanManger.BuildClanCostCopper);
                }
            }

            if (null != m_trans_TempClanCosCoin)
            {
                cGrid = m_trans_TempClanCosCoin.GetComponent<UICurrencyGrid>();
                if (null == cGrid)
                {
                    cGrid = m_trans_TempClanCosCoin.gameObject.AddComponent<UICurrencyGrid>();
                }
                if (null != cGrid)
                {
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                         MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_MoneyTicket)
//                         , ClanManger.BuildClanCostGold));
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                        "tubiao_huobi_jin"
//                        , ClanManger.BuildClanCostGold));
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                       null
//                       , ClanManger.BuildClanCostGold));
                    cGrid.UpdateNum((int)ClanManger.BuildClanCostGold);
                }
            }
        }else
        {
            
        }
    }

    /// <summary>
    /// 构建申请支持
    /// </summary>
    private void BuildCreate()
    {

    }
    //更新
    private void UpdateCreate()
    {
        if (null != m_input_ClanNameInput && m_input_ClanNameInput.gameObject.activeSelf == m_mgr.IsJoinClan)
        {
            m_input_ClanNameInput.gameObject.SetActive(!m_mgr.IsJoinClan);
        }

        if (null != m_label_TmpClanName && m_label_TmpClanName.gameObject.activeSelf != m_mgr.IsJoinClan)
        {
            m_label_TmpClanName.gameObject.SetActive(m_mgr.IsJoinClan);
        }

        if (null != m_trans_ClanCost && m_trans_ClanCost.gameObject.activeSelf == m_mgr.IsJoinClan)
        {
            m_trans_ClanCost.gameObject.SetActive(!m_mgr.IsJoinClan);
        }

        if (null != m_trans_ClanCostItemRoot && m_trans_ClanCostItemRoot.gameObject.activeSelf != m_mgr.IsJoinClan)
        {
            m_trans_ClanCostItemRoot.gameObject.SetActive(m_mgr.IsJoinClan);
        }

        if (null != m_btn_BtnNotice && m_btn_BtnNotice.gameObject.activeSelf != m_mgr.IsJoinClan)
        {
            m_btn_BtnNotice.gameObject.SetActive(m_mgr.IsJoinClan);
        }

        if (null != m_btn_BtnCreateTempClan && m_btn_BtnCreateTempClan.gameObject.activeSelf == m_mgr.IsJoinClan)
        {
            m_btn_BtnCreateTempClan.gameObject.SetActive(!m_mgr.IsJoinClan);
        }

        if (null != m_trans_TempClanInfo && m_trans_TempClanInfo.gameObject.activeSelf != m_mgr.IsJoinClan)
        {
            m_trans_TempClanInfo.gameObject.SetActive(m_mgr.IsJoinClan);
        }
        if (m_mgr.IsJoinClan)
        {
            if (!m_mgr.IsJoinFormal
            && m_mgr.IsClanCreatorSelf)
                OnUpdateSupportNum(m_mgr.ClanInfo.MemberCount);
            UpdateHorn();
            m_label_TmpClanName.text = (null != m_mgr.ClanInfo) ? m_mgr.ClanInfo.Name : "";

        }else
        {
            UICurrencyGrid cGrid = null;
            if (null != m_trans_TempClanCostWQ)
            {
                cGrid = m_trans_TempClanCostWQ.GetComponent<UICurrencyGrid>();
                if (null != cGrid)
                {
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                         MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_MoneyTicket)
//                         , ClanManger.BuildClanCostCopper));
                    cGrid.UpdateNum((int)ClanManger.BuildClanCostCopper);
                }
            }

            if (null != m_trans_TempClanCosCoin)
            {
                cGrid = m_trans_TempClanCosCoin.GetComponent<UICurrencyGrid>();
                if (null != cGrid)
                {
//                     cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
//                         MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold)
//                         , ClanManger.BuildClanCostGold));
                    cGrid.UpdateNum((int)ClanManger.BuildClanCostGold);
                }
            }
        }
        UpdateCreateGG();
    }


    #endregion


    #region Op

    /// <summary>
    /// 创建氏族
    /// </summary>
    private void CreateClan()
    {
        DataManager.Manager<ClanManger>().CreateClan(m_str_inputClanName, m_str_inputGGInfo);
    }

    /// <summary>
    /// 发送置顶公告
    /// </summary>
    private void SendTopGG()
    {
        m_mgr.SendTopClanGG();
    }

    /// <summary>
    /// 查询氏族信息
    /// </summary>
    private void SearchClan()
    {
        if (string.IsNullOrEmpty(m_str_inputSeachClanInfo))
        {
            DataManager.Manager<ClanManger>().GetClanInfoList(GameCmd.eGetClanType.GCT_TempFormal, 1);
            return;
        }
        if (IsMode(ClanCreateMode.Apply) || IsMode(ClanCreateMode.Support))
        {
            GameCmd.eGetClanType getclanType =  GameCmd.eGetClanType.GCT_Key_Formal;
            if (IsMode(ClanCreateMode.Support))
            {
                getclanType = GameCmd.eGetClanType.GCT_Key_Temp;
            }
            DataManager.Manager<ClanManger>().SearchClans(getclanType, m_str_inputSeachClanInfo, 1);
        }
        
    }


    #endregion

    #region Update

    private void UpdateCreateGG()
    {
        if (null != m_label_CreateGGInfo)
        {
            m_label_CreateGGInfo.text = (m_mgr.IsJoinClan && null != m_mgr.ClanInfo) ? m_mgr.ClanInfo.GG : m_str_inputGGInfo;
        }
    }

    /// <summary>
    /// 响应支持人数
    /// </summary>
    /// <param name="num"></param>
    private void OnUpdateSupportNum(int num)
    {
        string str = "当前支持人数{0}/{1}";
        str = string.Format(str, num, ClanManger.TempClanSupporter);
        if (null != m_label_TempClanSupNum)
        {
            m_label_TempClanSupNum.text = str;
        }
    }

    /// <summary>
    /// 更新喇叭数量
    /// </summary>
    private void UpdateHorn()
    {
        if (IsInitMode(ClanCreateMode.Create) && m_mgr.IsJoinClan )
       {
           if (null != m_trans_ClanCostItemRoot)
            {
               if (null == m_createGrowShow)
               {
                   GameObject prefabObj = m_trans_UIItemGrowCostGrid.gameObject;
                   if (null != prefabObj)
                   {
                       GameObject cloneObj = NGUITools.AddChild(m_trans_ClanCostItemRoot.gameObject, prefabObj);
                       if (null != cloneObj)
                       {
                           m_createGrowShow = cloneObj.GetComponent<UIItemGrowCostGrid>();
                           if (null == m_createGrowShow)
                           {
                               m_createGrowShow = cloneObj.AddComponent<UIItemGrowCostGrid>();
                               m_createGrowShow.RegisterUIEventDelegate(OnUIEventCallback);
                           }
                       }
                   }
               }
               if (null != m_createGrowShow)
               {
                   m_createGrowShow.SetGridData(ClanManger.PublicClanTopGGCostItemId
                    , 1);
               }
            }
       }
    }
    private void OnUIEventCallback(UIEventType eventType, object data, object parms)
    {
        if (null == data)
        {
            return;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid itemInfoGrid = data as UIItemInfoGrid;
                        if (itemInfoGrid.NotEnough && null != parms && parms is uint)
                        {
                            ShowItemGet((uint)parms);
                        }
                    }
                    break;
                
                
                }
        }
    }
    /// <summary>
    /// 显示获取
    /// </summary>
    /// <param name="baseId"></param>
    private void ShowItemGet(uint baseId)
    {
        TipsManager.Instance.ShowItemTips(baseId); ;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: baseId);
    }
    /// <summary>
    /// 获取UI返回数据
    /// </summary>
    /// <returns></returns>
    private ReturnBackUIData[] GetReturnBackUIData()
    {
        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eShowUI;
        returnData[0].panelid = PanelID.ClanCreatePanel;
        ClanCreateMode tabMode = m_em_clanCreateMode;
        int[] tab = new int[2];
        tab[0] = (int)tabMode;
        ReturnBackUIMsg backUIMsg = new ReturnBackUIMsg();
        backUIMsg.tabs = tab;
        returnData[0].param = backUIMsg;
        return returnData;
    }

    /// <summary>
    /// 显示发送gg
    /// </summary>
    private void ShowSendGG()
    {
        string starTxt = m_str_inputGGInfo;
        if (m_mgr.IsJoinClan && null != m_mgr.ClanInfo)
        {
            starTxt = m_mgr.ClanInfo.GG;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonInputPanel, data: new CommonInputPanel.CommonInputPanelData()
        {
            m_str_title = "公告",
            m_uint_maxWordsCount = 50,
            m_str_confirmText = "保存",
            m_str_starTxt = starTxt,
            confimAction = (inputText) =>
            {
                m_str_inputGGInfo = (string.IsNullOrEmpty(inputText)) ? "" : inputText;
                SendGG(inputText);
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonInputPanel);
            },
        });
    }

    /// <summary>
    /// 发送公告
    /// </summary>
    /// <param name="msg"></param>
    private void SendGG(string msg)
    {
       if (!m_mgr.IsJoinClan)
       {
           UpdateGG();
       }
       else
       {
           m_mgr.SendClanGG(msg);
       }
    }
    //更新刷新间隔
    public const float REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新剩余的时间
    private float next_refresh_left_time = 0;
    //剩余时间
    private long m_l_leftSeconds = 0;
    private void ImmediatelyRefreshLeftTime()
    {
        next_refresh_left_time = 0;
    }

    void Update()
    {
        if (IsMode(ClanCreateMode.Create) 
            && IsInitMode(ClanCreateMode.Create) 
            && m_mgr.IsJoinClan
            && null != m_mgr.ClanInfo)
        {
            
            next_refresh_left_time -= Time.deltaTime;
            if (next_refresh_left_time <= Mathf.Epsilon)
            {
                next_refresh_left_time = REFRESH_LEFTTIME_GAP;
                ScheduleDefine.ScheduleUnit.IsInCycleDateTme(
                    DateTimeHelper.Instance.Now
                    , (m_mgr.ClanInfo.CreateTime + (uint)(ClanManger.TempClanLastMinute * 60))
                    , DateTimeHelper.Instance.Now, out m_l_leftSeconds);
                if (null != m_label_TempClanLeftTime)
                {
                    m_label_TempClanLeftTime.text = string.Format("{0}{1}", "创建氏族剩余时间："
                        , DateTimeHelper.ParseTimeSeconds((int)m_l_leftSeconds));
                }
            }
        }
    }
    #endregion

    #region UIEvent
    void onClick_ClanNoticeEditBtn_Btn(GameObject caster)
    {
        if (IsMode(ClanCreateMode.Create))
        {
            ShowSendGG();
        }
            
    }

    void onClick_BtnCreateTempClan_Btn(GameObject caster)
    {
        CreateClan();
    }

    void onClick_BtnNotice_Btn(GameObject caster)
    {
        SendTopGG();
    }

    #endregion
      public override bool OnMsg(UIMsgID msgid, object param)
      {   
          if (msgid == UIMsgID.eShowUI)
        {
            ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
            if (showInfo != null)
            {
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }
            }

        }
        return base.OnMsg(msgid, param);
    }
}