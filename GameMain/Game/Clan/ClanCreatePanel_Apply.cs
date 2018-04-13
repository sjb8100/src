/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanCreatePanel_ApplySupport
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:12:35 AM
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
    //当前选中id
    private uint m_uint_selectId = 0;
    //当前列表信息
    private List<ClanDefine.LocalClanInfo> m_lst_clanTempInfos = null;
    //当前选中氏族的id
    private ClanDefine.LocalClanInfo ClanData
    {
        get
        {
            if (null != m_lst_clanTempInfos)
            {
                foreach (ClanDefine.LocalClanInfo info in m_lst_clanTempInfos)
                {
                    if (null == info)
                    {
                        continue;
                    }
                    if (info.Id == m_uint_selectId)
                    {
                        return info;
                    }
                }
            }
            return null;
        }
    }
    #endregion

    #region init
    private void InitApply()
    {
        if (IsInitMode(ClanCreateMode.Apply))
        {
            return;
        }
        SetInitMode(ClanCreateMode.Apply);
        
        if (null == m_lst_clanTempInfos)
        {
            m_lst_clanTempInfos = new List<ClanDefine.LocalClanInfo>();
        }

        if (null != m_ctor_ClanApplyScrollView)
        {
            GameObject resObj = m_trans_UIClanGrid.gameObject;
            m_ctor_ClanApplyScrollView.Initialize<UIClanGrid>(resObj, OnUIGridUpdate, OnUIGridEventDlg);
        }

        if (null != m_input_SearchInput)
        {
//            m_input_SearchInput.defaultColor = Color.gray;
            m_input_SearchInput.onChange.Add(new EventDelegate(() =>
            {
                m_str_inputSeachClanInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_SearchInput.value
                    , TextManager.CONST_NAME_MAX_WORDS);
                m_input_SearchInput.value = m_str_inputSeachClanInfo;
            }));
            m_input_ClanNameInput.onSubmit.Add(new EventDelegate(() =>
            {
                m_str_inputSeachClanInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_SearchInput.value
                     , TextManager.CONST_NAME_MAX_WORDS);
                m_input_SearchInput.value = m_str_inputSeachClanInfo;
            }));
        }
    }

    /// <summary>
    /// 构建申请支持
    /// </summary>
    private void BuildApplySupport()
    {
        RebuildClanList((int)GameCmd.eGetClanType.GCT_TempFormal);
    }

    //更新
    private void UpdateApply()
    {
        UpdateApplyGG();
    }

    /// <summary>
    /// 更新公告
    /// </summary>
    private void UpdateApplyGG()
    {
        if (null != m_label_ApplyGGInfo)
        {
            m_label_ApplyGGInfo.text = (null != ClanData) ? ClanData.GG : "";
        }
    }

    /// <summary>
    ///重建氏族列表 
    /// </summary>
    private void RebuildClanList(int type)
    {
        if (IsMode(ClanCreateMode.Support) || IsMode(ClanCreateMode.Apply))
        {
            bool filter = false;
            switch(type)
            {
                case (int)GameCmd.eGetClanType.GCT_Temp:
                case (int)GameCmd.eGetClanType.GCT_Formal:
                case (int)GameCmd.eGetClanType.GCT_TempFormal:
                    {
                        filter = false;
                    }
                    break;
                case (int)GameCmd.eGetClanType.GCT_Key_Temp:
                case (int)GameCmd.eGetClanType.GCT_Key_Formal:
                case (int)GameCmd.eGetClanType.GCT_Key_TempFormal:
                    {
                        if (!string.IsNullOrEmpty(m_str_inputSeachClanInfo))
                        {
                            filter = true;
                        }
                    }
                    break;
            }
            m_lst_clanTempInfos.Clear();
            if (IsMode(ClanCreateMode.Apply))
            {
                m_lst_clanTempInfos.AddRange(DataManager.Manager<ClanManger>().GetCacheClanInfos(true, filter));
            }else
            {
                m_lst_clanTempInfos.AddRange(DataManager.Manager<ClanManger>().GetCacheClanInfos(false, filter));
            }
            BuildClanInfoUI();
        }
        
    }

    /// <summary>
    /// 构建氏族列表UI
    /// </summary>
    private void BuildClanInfoUI()
    {
        if ( IsMode(ClanCreateMode.Apply) && null != m_ctor_ClanApplyScrollView)
        {
            m_ctor_ClanApplyScrollView.CreateGrids(m_lst_clanTempInfos.Count);
            if (m_lst_clanTempInfos.Count != 0)
            {
                SetSelectClan(m_lst_clanTempInfos[0]);
            }
        }
        else if (IsMode(ClanCreateMode.Support) && null != m_ctor_ClanSupportScrollView)
        {

            m_ctor_ClanSupportScrollView.CreateGrids(m_lst_clanTempInfos.Count);
            if (m_lst_clanTempInfos.Count != 0)
            {
                SetSelectSupportClan(m_lst_clanTempInfos[0]);
            }
        }
    }

    #endregion

    #region Op

    /// <summary>
    /// 加入当前选中氏族
    /// </summary>
    private void ApplyClan()
    {
        if (null == ClanData)
        {
            TipsManager.Instance.ShowTips("无效氏族信息");
            return;
        }
        DataManager.Manager<ClanManger>().ApplyJoinClan(m_uint_selectId);
    }

    /// <summary>
    /// 一键申请
    /// </summary>
    private void QuickApplyClan()
    {
        m_mgr.QuickJoinClan();
    }

    /// <summary>
    /// 联系族长
    /// </summary>
    private void ContactShaikh()
    {
        ClanDefine.LocalClanInfo localClanInfo = ClanData;
        if (null == localClanInfo)
        {
            TipsManager.Instance.ShowTips("无效氏族信息");
            return ;
        }
        GameCmd.stClanMemberInfo member = localClanInfo.GetMemberInfo(localClanInfo.Creator);
        if (null == member)
        {
            TipsManager.Instance.ShowTips("氏族族长信息错误");
            return;
        }

        RoleRelation data = new RoleRelation() { uid = member.id, name = member.name };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FriendPanel);
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eChatWithPlayer, data);
    }

    /// <summary>
    /// 更新列表
    /// </summary>
    public void UpdateClanList()
    {
        if (null != m_ctor_ClanApplyScrollView)
        {
            m_ctor_ClanApplyScrollView.UpdateActiveGridData();
        }
    }

    /// <summary>
    /// 支持当前氏族
    /// </summary>
    private void SupportClan()
    {
        if (null == ClanData)
        {
            TipsManager.Instance.ShowTips("无效氏族信息");
            return;
        }
        DataManager.Manager<ClanManger>().SupportClan(m_uint_selectId);
    }

    /// <summary>
    /// 取消支持当前氏族
    /// </summary>
    private void CancelSupportClan()
    {
        if (null == ClanData)
        {
            TipsManager.Instance.ShowTips("无效氏族信息");
            return;
        }
        DataManager.Manager<ClanManger>().CancelSupportClan();

    }

    /// <summary>
    /// 更新格子数据
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUIGridUpdate(UIGridBase grid, int index)
    {
        if (grid is UIClanGrid)
        {
            UIClanGrid clanGrid = grid as UIClanGrid;
            if (m_lst_clanTempInfos.Count > index)
            {
                clanGrid.SetGridData(m_lst_clanTempInfos[index]);
                clanGrid.SetHightLight((m_lst_clanTempInfos[index].Id == m_uint_selectId) ? true : false);
                clanGrid.SetTagEnable(m_mgr.IsApplyClan(m_lst_clanTempInfos[index].Id));
                clanGrid.SetBgSprite(index);
            }
        }else if (grid is UIClanSupportGrid)
        {
            UIClanSupportGrid clanSpGrid = grid as UIClanSupportGrid;
            if (m_lst_clanTempInfos.Count > index)
            {
                clanSpGrid.SetGridData(m_lst_clanTempInfos[index]);
                clanSpGrid.SetBgSprite(index);
                clanSpGrid.SetHightLight((m_lst_clanTempInfos[index].Id == m_uint_selectId) ? true : false);
                clanSpGrid.SetTagEnable(m_mgr.IsJoinClan 
                    && null != m_mgr.ClanInfo
                    && (m_mgr.ClanId == m_lst_clanTempInfos[index].Id) && !DataManager.Manager<ClanManger>().IsClanCreatorSelf);
            }
        }
    }

    /// <summary>
    /// UI格子事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnUIGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (null == data)
        {
            return;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIClanGrid)
                    {
                        UIClanGrid clanGrid = data as UIClanGrid;
                        SetSelectClan(clanGrid.Data);
                    }else if (data is UIClanSupportGrid)
                    {
                        UIClanSupportGrid clanGrid = data as UIClanSupportGrid;
                        SetSelectSupportClan(clanGrid.Data);
                    }
                    else if (data is UITabGrid)
                    {
                        UITabGrid tab = data as UITabGrid;
                        if (tab.Data is ClanCreateMode)
                        {
                            SetMode((ClanCreateMode)tab.Data);
                        }
                    }

                }
                break;
        }
    }
    /// <summary>
    /// 设置当前选中的氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void SetSelectClan(ClanDefine.LocalClanInfo selectInfo)
    {
        if (selectInfo.Id == m_uint_selectId || null == m_ctor_ClanApplyScrollView)
        {
            return;
        }
        //刷新数据
        UIClanGrid grid = m_ctor_ClanApplyScrollView.GetGrid<UIClanGrid>(m_lst_clanTempInfos.IndexOf(ClanData));
        if (null != grid)
        {
            grid.SetHightLight(false);
        }
        grid = m_ctor_ClanApplyScrollView.GetGrid<UIClanGrid>(m_lst_clanTempInfos.IndexOf(selectInfo));
        if (null != grid)
        {
            grid.SetHightLight(true);
        }
        m_uint_selectId = selectInfo.Id;
        UpdateApply();
    }
    #endregion

    #region UIEvent
    void onClick_BtnApplySearch_Btn(GameObject caster)
    {
        SearchClan();
    }

    void onClick_BtnApply_Btn(GameObject caster)
    {
        ApplyClan();
    }

    void onClick_BtnApplyQuick_Btn(GameObject caster)
    {
        QuickApplyClan();
    }

    void onClick_BtnApplyContactShaikh_Btn(GameObject caster)
    {
        ContactShaikh();
    }
    #endregion

}