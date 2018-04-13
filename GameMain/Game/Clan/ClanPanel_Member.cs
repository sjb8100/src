/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanPanel_Member
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:31:39 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using GameCmd;
partial class ClanPanel
{
    #region Define
    public enum ClanMemberMode
    {
        None,
        Member,
        Apply,
        Event,
    }
    #endregion
    
    #region property
    private ClanMemberMode m_em_clanMemberMode = ClanMemberMode.None;
    private Dictionary<ClanMemberMode, UITabGrid> m_dic_clanMemberTabs;
    private UIGridCreatorBase m_applyCreator = null;
    private UIGridCreatorBase m_memberCreator = null;
    //显示申请红点提示
    private bool m_bool_showApplyRedPoint = false;
    private bool m_bool_showSkillRedPoint = false;
    #endregion

    #region Op
    private void UpdateMember()
    {
        if (null != m_label_MemberONT)
        {
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            ClanDefine.LocalClanMemberDB db = ClanManger.GetLocalCalnMemberDB(clanInfo.Lv);
            m_label_MemberONT.text = string.Format("成员数量：{0}/{1}/{2}", clanInfo.OnLineMemberCount
                , clanInfo.MemberCount, ((null != db) ? db.MaxMember : 0));
        }
        BuildMemberList();
    }

    private void BuildMemberList()
    {
        switch (m_em_clanMemberMode)
        {
            case ClanMemberMode.Member:
                BuildMemberDatas();
                break;
            case ClanMemberMode.Apply:

                GetClanApplyInfoList();

                break;
            case ClanMemberMode.Event:
                InitHonor();
                break;
        }     
    }
    private void SetMemberMode(ClanMemberMode mode,bool force = false)
    {
        if (m_em_clanMemberMode != mode || force)
        {
            SetMemberModeEnable(m_em_clanMemberMode, false);
            m_em_clanMemberMode = mode;
            SetMemberModeEnable(m_em_clanMemberMode, true);
            InitMember();
            BuildMemberList();
        }
    }

    /// <summary>
    /// 是否为当前成员模式
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public bool IsMemberMode(ClanMemberMode mode)
    {
        return m_em_clanMemberMode == mode;
    }

    /// <summary>
    /// 设置氏族申请红点提示
    /// </summary>
    /// <param name="enable"></param>
    public void SetApplyRedPoint()
    {
        UpdateApplyRedPoint();
    }

    private void UpdateApplyRedPoint()
    {
        UITabGrid tabGrid = null;
        if (IsPanelMode(ClanPanelMode.Member))
        {
            m_dic_clanMemberTabs[ClanMemberMode.Apply].SetRedPointStatus(m_bool_showApplyRedPoint);
        }
        Dictionary<int, UITabGrid> dicTabs = null;
        if(dicUITabGrid.TryGetValue(1,out dicTabs))
        {
            if (dicTabs != null && dicTabs.TryGetValue((int)ClanPanelMode.Member, out tabGrid))
            {
                tabGrid.SetRedPointStatus(m_bool_showApplyRedPoint);
 
            }
        }

        //已读后隐藏主界面氏族红点显示
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Clan,
            direction = (int)WarningDirection.Left,
            bShowRed = m_bool_showApplyRedPoint,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    private void UpdateSkillRedPoint()
    {
        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            if (dicTabs != null && dicTabs.TryGetValue((int)ClanPanelMode.Skill, out tabGrid))
            {
                tabGrid.SetRedPointStatus(m_bool_showSkillRedPoint);

            }
        }
    }

    private void SetMemberModeEnable(ClanMemberMode mode,bool visible)
    {
        UITabGrid grid = (m_dic_clanMemberTabs.ContainsKey(m_em_clanMemberMode))
                ? m_dic_clanMemberTabs[m_em_clanMemberMode] : null;
        if (null != grid)
        {
            grid.SetHightLight(visible);
        }
        SetMemberModeWidgetVisble(mode, visible);
    }

    private void SetMemberModeWidgetVisble(ClanMemberMode mode, bool visible)
    {
        bool cansee = (visible && mode == ClanMemberMode.Member);
        bool isEventMode = (mode ==  ClanMemberMode.Event);
        m_trans_MemberArea_Event.gameObject.SetActive(isEventMode);
        m_trans_MemberArea_Member.gameObject.SetActive(!isEventMode);
        //成员
        if (null != m_trans_MemberScrollViewContent
            && m_trans_MemberScrollViewContent.gameObject.activeSelf != cansee)
        {
            m_trans_MemberScrollViewContent.gameObject.SetActive(cansee);
        }
        if (null != m_trans_MemberTitleM
            && m_trans_MemberTitleM.gameObject.activeSelf != cansee)
        {
            m_trans_MemberTitleM.gameObject.SetActive(cansee);
        }
        if (null != m_btn_BtnChangeClan
            && m_btn_BtnChangeClan.gameObject.activeSelf != cansee)
        {
            m_btn_BtnChangeClan.gameObject.SetActive(cansee);
        }
        if (null != m_btn_BtnQuitClan
            && m_btn_BtnQuitClan.gameObject.activeSelf != cansee)
        {
            m_btn_BtnQuitClan.gameObject.SetActive(cansee);
        }
        if (null != m_btn_BtnMassSendMsg
            && m_btn_BtnMassSendMsg.gameObject.activeSelf != cansee)
        {
            m_btn_BtnMassSendMsg.gameObject.SetActive(cansee);
        }

        cansee = !cansee;
        //申请
        if (null != m_trans_MemberApplyScrollViewContent
            && m_trans_MemberApplyScrollViewContent.gameObject.activeSelf != cansee)
        {
            m_trans_MemberApplyScrollViewContent.gameObject.SetActive(cansee);
        } 
        if (null != m_trans_MemberTitleA
            && m_trans_MemberTitleA.gameObject.activeSelf != cansee)
        {
            m_trans_MemberTitleA.gameObject.SetActive(cansee);
        }
        if (null != m_btn_BtnClear
            && m_btn_BtnClear.gameObject.activeSelf != cansee)
        {
            m_btn_BtnClear.gameObject.SetActive(cansee);
        }

        //事件

    }

    #endregion

    #region InitMember
    //当前选中成员数据
    private stClanMemberInfo m_uint_selectmemberid = null;
    private List<GameCmd.stClanMemberInfo> m_list_memberdatas = new List<GameCmd.stClanMemberInfo>();
    
    /// <summary>
    /// 构造成员
    /// </summary>
    private  void BuildMember()
    {
        SetMemberMode(ClanMemberMode.Member,true);
    }
    //构造成员数据
    private void BuildMemberDatas()
    {
        if (!IsPanelMode(ClanPanelMode.Member) || !IsInitMode(ClanMemberMode.Member))
        {
            return;
        }
     
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null != clanInfo)
        {
            m_list_memberdatas = DataManager.Manager<ClanManger>().MemberSortByType(DataManager.Manager<ClanManger>().curMemberSortType, true);              
            if (!m_list_memberdatas.Contains(m_uint_selectmemberid) && m_list_memberdatas.Count > 0)
            {
                m_uint_selectmemberid = m_list_memberdatas[0];
            }
            m_list_memberdatas.Sort(ClanDefine.ClanMemberSort);
            m_memberCreator.CreateGrids(m_list_memberdatas.Count);
            if (null != m_label_MemberONT)
            {
                ClanDefine.LocalClanMemberDB db = ClanManger.GetLocalCalnMemberDB(clanInfo.Lv);
                m_label_MemberONT.text = string.Format("成员数量：{0}/{1}/{2}", clanInfo.OnLineMemberCount
                    , clanInfo.MemberCount, ((null != db) ? db.MaxMember : 0));
            }
        }
       
    }

    void RefreshMemberList(uint id) 
    {
        int preNums = m_list_memberdatas.Count;
        int nowNums = ClanInfo.MemberList.member.Count;
        int addNums = nowNums - preNums;
        if (addNums == 0)
        {
            //刷新
        }
        else if (addNums < 0)
        {
            addNums = Mathf.Abs(addNums);
            for (int j = 0; j < m_list_memberdatas.Count; j++)
            {
                if (m_list_memberdatas[j].id == id)
                {
                    m_list_memberdatas.RemoveAt(j);
                }
            }
            //删除
            for (int i = 0; i < addNums; i++)
            {
                m_memberCreator.RemoveData(preNums - i - 1);
            }
        }
        else
        {
            m_list_memberdatas = DataManager.Manager<ClanManger>().MemberSortByType(DataManager.Manager<ClanManger>().curMemberSortType, true);   
            //添加
            for (int i = 0; i < addNums; i++)
            {
                m_memberCreator.InsertData(preNums + i);
            }
        }
        m_memberCreator.UpdateActiveGridData();
    }

    private void SetSelectMemberId(stClanMemberInfo selectMember)
    {
        if (m_uint_selectmemberid == selectMember
            || null == m_memberCreator)
        {
            return;
        }

        //刷新数据
        UIClanMemberGrid grid = (m_list_memberdatas.Contains(m_uint_selectmemberid))
            ? m_memberCreator.GetGrid<UIClanMemberGrid>(m_list_memberdatas.IndexOf(m_uint_selectmemberid)) : null;
        if (null != grid)
        {
            grid.SetHightLight(false);
        }
        m_uint_selectmemberid = selectMember;
        grid = (m_list_memberdatas.Contains(m_uint_selectmemberid))
            ? m_memberCreator.GetGrid<UIClanMemberGrid>(m_list_memberdatas.IndexOf(m_uint_selectmemberid)) : null;
        if (null != grid)
        {
            grid.SetHightLight(true);
        }
        
    }

    /// <summary>
    /// 构建成员列表
    /// </summary>
    private void BuildApplyList()
    {
        if (!IsInitMode(ClanPanel.ClanMemberMode.Apply))
        {
            return;
        }

        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        m_list_applyUserIds.Clear();
        m_list_applyUserIds.AddRange(m_mgr.GetClanApplyUserIds());
        curApplySortType = MemberSortType.None;
        //获取数据
        if (null != clanInfo && null != m_applyCreator)
        {
            m_applyCreator.CreateGrids(m_list_applyUserIds.Count);
        }
    }
    

    private void InitMember()
    {
        if (!IsInitMode(ClanPanelMode.Member))
        {
            m_dic_clanMemberTabs = new Dictionary<ClanMemberMode, UITabGrid>();
            UITabGrid tabGrid = null;
            if (null != m_trans_MemberTab)
            {
                tabGrid = m_trans_MemberTab.GetComponent<UITabGrid>();
                if (null == tabGrid)
                {
                    tabGrid = m_trans_MemberTab.gameObject.AddComponent<UITabGrid>();
                }
                tabGrid.SetDepth(5);
                tabGrid.SetHightLight(false);
                tabGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        SetMemberMode(ClanMemberMode.Member);
                    }
                });
            }
            m_dic_clanMemberTabs.Add(ClanMemberMode.Member, tabGrid);

            if (null != m_trans_ApplyTab)
            {
                tabGrid = m_trans_ApplyTab.GetComponent<UITabGrid>();
                if (null == tabGrid)
                {
                    tabGrid = m_trans_ApplyTab.gameObject.AddComponent<UITabGrid>();
                }
                tabGrid.SetDepth(4);
                tabGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        SetMemberMode(ClanMemberMode.Apply);
                    }
                });
                tabGrid.SetHightLight(false);
            }
            m_dic_clanMemberTabs.Add(ClanMemberMode.Apply, tabGrid);


            if (null != m_trans_EventTab)
            {
                tabGrid = m_trans_EventTab.GetComponent<UITabGrid>();
                if (null == tabGrid)
                {
                    tabGrid = m_trans_EventTab.gameObject.AddComponent<UITabGrid>();
                }
                tabGrid.SetDepth(4);
                tabGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        SetMemberMode(ClanMemberMode.Event);
                    }
                });
                tabGrid.SetHightLight(false);
            }
            m_dic_clanMemberTabs.Add(ClanMemberMode.Event, tabGrid);
            SetInitMode(ClanPanelMode.Member);
        }

        if (!IsInitMode(ClanMemberMode.Member)
            && m_em_clanMemberMode == ClanMemberMode.Member)
        {
            SetInitMode(ClanMemberMode.Member);
            if (null != m_trans_MemberScrollView && null == m_memberCreator)
            {
                m_memberCreator = m_trans_MemberScrollView.gameObject.AddComponent<UIGridCreatorBase>();
                GameObject obj = m_trans_UIClanMemberGrid.gameObject;
                m_memberCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_memberCreator.gridContentOffset = new UnityEngine.Vector2(0, 170);
                m_memberCreator.gridWidth = 1066;
                m_memberCreator.gridHeight = 64;
                m_memberCreator.RefreshCheck();
                m_memberCreator.Initialize<UIClanMemberGrid>(obj, OnUpdateUIGrid, OnUIGridEventDlg);

            }
        }
        if (!IsInitMode(ClanMemberMode.Apply)
           && m_em_clanMemberMode == ClanMemberMode.Apply)
        {
            SetInitMode(ClanMemberMode.Apply);
            if (null != m_trans_MemberApplyScrollView && null == m_applyCreator)
            {
                m_applyCreator = m_trans_MemberApplyScrollView.gameObject.AddComponent<UIGridCreatorBase>();
                GameObject obj = m_trans_UIClanApplyGrid.gameObject;
                m_applyCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                m_applyCreator.gridContentOffset = new UnityEngine.Vector2(0, 170);
                m_applyCreator.gridWidth = 1066;
                m_applyCreator.gridHeight = 71;
                m_applyCreator.RefreshCheck();
                m_applyCreator.Initialize<UIClanApplyGrid>(obj, OnUpdateUIGrid, OnUIGridEventDlg);
            }
        }
        ////请求申请列表
        //GetClanApplyInfoList();
        if (null != m_trans_MemmberInviteContent
            && m_trans_MemmberInviteContent.gameObject.activeSelf)
        {
            m_trans_MemmberInviteContent.gameObject.SetActive(false);
        }
    }


    #endregion

    #region GetClanApplyInfoList
    private List<uint> m_list_applyUserIds = new List<uint>();
// 
//      <summary>
//      申请加入氏族玩家列表改变
//      </summary>
     private void OnClanApplyInfoChanged()
     {
         if (!(IsInitMode(ClanPanelMode.Member) && IsMemberMode(ClanMemberMode.Apply)))
         {
             return;
         }
         if (null == m_applyCreator)
         {
             return;
         }

         List<uint> tempApplyInfos = new List<uint>();
         tempApplyInfos.AddRange(m_mgr.GetClanApplyUserIds());
 
         int preNums = m_list_applyUserIds.Count;
         int addNums = tempApplyInfos.Count - m_list_applyUserIds.Count;
   
         if (addNums == 0)
         {
             //刷新
         }else if (addNums < 0)
         {
             addNums = Mathf.Abs(addNums);
             for (int m = 0; m < m_list_applyUserIds.Count; m++)
             {
                 if (!tempApplyInfos.Contains(m_list_applyUserIds[m]))
                 {
                     m_list_applyUserIds.RemoveAt(m);
                 }
             }
             //删除
             for (int i = 0; i < addNums; i++)
             {
                 m_applyCreator.RemoveData(preNums - i-1);
             }
         }
         else
         {
             for (int m = 0; m < tempApplyInfos.Count; m++)
             {
                 if (!m_list_applyUserIds.Contains(tempApplyInfos[m]))
                 {
                     m_list_applyUserIds.Add(tempApplyInfos[m]);
                 }
             }

             //添加
             for(int i = 0;i < addNums;i++)
             {
                 m_applyCreator.InsertData(preNums + i);
             }
         }
         //刷新
         m_applyCreator.UpdateActiveGridData();
     }
    /// <summary>
    /// 从服务端请求申请加入氏族列表
    /// </summary>
    private void GetClanApplyInfoList()
    {
        m_mgr.GetClanApplyListReq();
    }

    /// <summary>
    /// 处理玩家申请
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="agree"></param>
    private void DealClanApplyInfo(uint userId,bool agree)
    {
        m_mgr.DealClanApplyReq(userId, agree);
    }

    /// <summary>
    /// 队伍邀请
    /// </summary>
    private void ShowInviteTeamUI()
    {
        List<TeamMemberInfo> teamMembers = DataManager.Manager<TeamDataManager>().TeamMemberList;
        if (null == teamMembers || teamMembers.Count == 0)
        {
            TipsManager.Instance.ShowTips("你没在队伍中");
            return;
        }
        List<InvitePanel.InviteData> invitedatas = new List<InvitePanel.InviteData>();
        InvitePanel.InviteData inviteData;
        foreach (TeamMemberInfo info in teamMembers)
        {
            if (info.id == DataManager.Instance.UserId)
            {
                //过滤自己
                continue;
            }
            inviteData = new InvitePanel.InviteData()
            {
                userId = info.id,
                name = info.name,
                lv = (int)info.lv,
                icon = "",
            };
            invitedatas.Add(inviteData);
        }
        InvitePanel.InvitePanelData panelData = new InvitePanel.InvitePanelData()
        {
            title = "队伍",
            inviteDatas = invitedatas,
            inviteBtnClickAction = DoInviteJoinClan,
            isClanInvite = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.InvitePanel, data: panelData);
    }

    /// <summary>
    /// 好友邀请
    /// </summary>
    private void ShowInviteFriendUI()
    {
        List<RoleRelation> friendInfos = null;
        DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out friendInfos);
        if (null == friendInfos || friendInfos.Count == 0)
        {
            TipsManager.Instance.ShowTips("你现在还没有好友");
            return;
        }
        List<InvitePanel.InviteData> invitedatas = new List<InvitePanel.InviteData>();
        InvitePanel.InviteData inviteData;
        foreach (RoleRelation info in friendInfos)
        {
            inviteData = new InvitePanel.InviteData()
            {
                userId = info.uid,
                name = info.name,
                lv = (int)info.level,
                icon = "",

            };
            invitedatas.Add(inviteData);
        }
        InvitePanel.InvitePanelData panelData = new InvitePanel.InvitePanelData()
        {
            title = "好友",
            inviteDatas = invitedatas,
            inviteBtnClickAction = DoInviteJoinClan,
            isClanInvite = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.InvitePanel, data: panelData);
    }
    /// <summary>
    /// 邀请加入氏族
    /// </summary>
    /// <param name="inviteData"></param>
    private void DoInviteJoinClan(InvitePanel.InviteData inviteData)
    {
        if (null != inviteData)
        {
            m_mgr.InviteJoinClan(inviteData.userId);
        }
    }

    #endregion

    #region UIEvent
    void onClick_BtnQuitClan_Btn(GameObject caster)
    {
        string msg = "确定要退出氏族吗？";
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, msg, () =>
            {
                m_mgr.QuitClan();
            }, okstr: "确定");
    }

    void onClick_BtnChangeClan_Btn(GameObject caster)
    {
        m_mgr.ChangeClan();
    }

    void onClick_BtnInvite_Btn(GameObject caster)
    {
        if (null != m_trans_MemmberInviteContent)
        {
            m_trans_MemmberInviteContent.gameObject.SetActive(!m_trans_MemmberInviteContent.gameObject.activeSelf);
        }
    }

    void onClick_BtnMassSendMsg_Btn(GameObject caster)
    {
        m_mgr.MassBroadCastMsg();
    }

    void onClick_BtnClear_Btn(GameObject caster)
    {
        m_mgr.ClearClanApplyListReq();
    }

    //好友
    void onClick_BtnFriend_Btn(GameObject caster)
    {
        ShowInviteFriendUI();
    }

    //队伍
    void onClick_BtnNearby_Btn(GameObject caster)
    {
        ShowInviteTeamUI();
    }

    void onClick_MemberInviteCollider_Btn(GameObject caster)
    {
        if (null != m_trans_MemmberInviteContent && m_trans_MemmberInviteContent.gameObject.activeSelf)
        {
            m_trans_MemmberInviteContent.gameObject.SetActive(false);
        }
    }
    #endregion

    #region MemberSort

    MemberSortType curApplySortType = MemberSortType.None;
    bool ApplyReverse = false;

    void MemberSortByType(MemberSortType type) 
    {
        m_list_memberdatas = DataManager.Manager<ClanManger>().MemberSortByType(type);
        m_memberCreator.CreateGrids(m_list_memberdatas.Count);
    }

    /// <summary>
    /// 申请列表排序
    /// </summary>
    /// <param name="type"></param>
    void ApplyMemberSortByType(MemberSortType type) 
    {
        if (m_list_applyUserIds.Count == 0)
        {
            return;
        }
        ApplyReverse = curApplySortType == type;
        if (ApplyReverse)
        {
            m_list_applyUserIds.Reverse();
        }
        else
        {
            bool isNeedExchange = false;
            uint temp = 0;
            stRequestListClanUserCmd_S.Data applyData_i = null;
            stRequestListClanUserCmd_S.Data applyData_j = null;
            int length = m_list_applyUserIds.Count - 1;
            for (int i = 0; i < length; i++)
            {
                applyData_i = m_mgr.GetClanApplyUserInfo(m_list_applyUserIds[i]);
                if (applyData_i == null)
                {
                    break;
                }
                for (int j = length; j > i; j--)
                {                 
                    applyData_j =  m_mgr.GetClanApplyUserInfo(m_list_applyUserIds[j]);
                    if (applyData_j == null)
                    {
                        break;
                    }
                    switch (type)
                    {
                        case MemberSortType.None:              
                            break;
                        case MemberSortType.Profession:
                            isNeedExchange = applyData_i.jov < applyData_j.jov;
                            break;
                        case MemberSortType.Name:
                            isNeedExchange = applyData_i.name.CompareTo(applyData_j.name) < 0;
                            break;
                        case MemberSortType.Lv:
                            isNeedExchange = applyData_i.level < applyData_j.level;
                            break;
                        case MemberSortType.Fight:
                            isNeedExchange = applyData_i.fight < applyData_j.fight;
                            break;
                    }
                    if (isNeedExchange)
                    {
                        temp = m_list_applyUserIds[j];
                        m_list_applyUserIds[j] = m_list_applyUserIds[i];
                        m_list_applyUserIds[i] = temp;
                    }
                }
            }
            curApplySortType = type;
        }
        m_applyCreator.CreateGrids(m_list_applyUserIds.Count);
    }
    
    void onClick_TabProfBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Profession);
    }

    void onClick_TabNameBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Name);
    }

    void onClick_TabLvBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Lv);
    }

    void onClick_TabHonorBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Donate);
    }

    void onClick_TabDutyBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Duty);
    }

    void onClick_TabJoinTimeBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.Fight);
    }

    void onClick_TabOutLineTimeBtn_Btn(GameObject caster)
    {
        MemberSortByType(MemberSortType.OnLineTime);
    }



    void onClick_ApplyProfBtn_Btn(GameObject caster)
    {
        ApplyMemberSortByType(MemberSortType.Profession);
    }

    void onClick_ApplyNameBtn_Btn(GameObject caster)
    {
        ApplyMemberSortByType(MemberSortType.Name);
    }

    void onClick_ApplyLvBtn_Btn(GameObject caster)
    {
        ApplyMemberSortByType(MemberSortType.Lv);
    }

    void onClick_ApplyFightBtn_Btn(GameObject caster)
    {
        ApplyMemberSortByType(MemberSortType.Fight);
    }

    #endregion
}
