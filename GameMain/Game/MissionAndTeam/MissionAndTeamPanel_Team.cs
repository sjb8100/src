using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class MissionAndTeamPanel : UIPanelBase
{

    /// <summary>
    /// grid缓存
    /// </summary>
    List<GameObject> m_lstMemberGridCache = new List<GameObject>();

    List<UILeftTeamMemberGrid> m_lstMemberGrid = new List<UILeftTeamMemberGrid>();

    stTeamMemberBtn m_teamMemberBtninfo;


    /// <summary>
    /// 队员信息list
    /// </summary>
    List<TeamMemberInfo> m_listTeamMember = new List<TeamMemberInfo>();

    void DoGameEvent(int eventID, object param)
    {
        //ROBOTCOMBAT_START = 8001,           // 开始挂机
        //ROBOTCOMBAT_STOP,                   // 停止挂机
        //ROBOTCOMBAT_PAUSE,                  // 暂停挂机

        if (eventID == (int)GameEventID.SKILLBUTTON_CLICK || eventID == (int)GameEventID.ROBOTCOMBAT_START || eventID == (int)GameEventID.JOYSTICK_PRESS)
        {
            if (m_trans_TeamMemberBtnRoot.gameObject.activeSelf == true)
            {
                m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
            }
        }

        else if (eventID == (int)GameEventID.ROBOTCOMBAT_COPYKILLWAVE)
        {
            UpdateCopyTarget();

            stCopySkillWave copyWave = (stCopySkillWave)param;
            UpdateCopyTargetGrid(copyWave.waveId);
        }

        else if (eventID == (int)GameEventID.COPY_REWARDEXP)
        {
            stCopyRewardExp copyRewardExp = (stCopyRewardExp)param;
            UpdateCopyTargetGridExp(copyRewardExp.exp);
        }

        else if (eventID == (int)GameEventID.UIEVENT_WORLDBOSSDAMRANKREFRESH)
        {
            UpdateCopyTargetGridWorldJuBaoRankAndDamage();
        }

        else if (eventID == (int)GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH)
        {
            UpdateCopyTargetGridWorldJuBaoGuWuAdd();
        }

        else if (eventID == (int)GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED)
        {

        }

        else if (eventID == (int)GameEventID.UIEVENTGUIDECOMPLETE)
        {

        }

        else if (eventID == (int)GameEventID.UIEVENT_UPDATEITEM)
        {
            //跟新复活卡数量
            if (DataManager.Manager<AnswerManager>().InAnswerCopy)
            {
                ItemDefine.UpdateItemPassData passData = (ItemDefine.UpdateItemPassData)param;
                if (passData.BaseId ==  DataManager.Manager<AnswerManager>().FuHuoCardId)
                {
                    InitAnswer();
                }
            }
        }
    }

    void InitTeamRoot()
    {
        if (TDManager.IsJoinTeam == false)//无队伍
        {
            m_trans_TeamChoiceBtn.gameObject.SetActive(true);
            m_trans_MemberListRoot.gameObject.SetActive(false);
        }
        else
        {   //有队伍
            m_trans_TeamChoiceBtn.gameObject.SetActive(false);
            m_trans_MemberListRoot.gameObject.SetActive(true);
        }
    }

    void UpdateTeamList()
    {
        if (m_BtnStatus == BtnStatus.Team)
        {
            if (TDManager.IsJoinTeam == true)
            {
                m_trans_TeamChoiceBtn.gameObject.SetActive(false);
                m_trans_MemberListRoot.gameObject.SetActive(true);
                //此处添加好友列表
                UpdateTeamListGrid();
                //组队人数
                string count = TDManager.GetMemberCount() == 0 ? "" : TDManager.GetMemberCount().ToString() + "/" + TeamDataManager.TeamMemberMax;
                m_label_TeamNum.text = count;
            }
            else
            {
                m_label_TeamNum.text = string.Empty;
                m_trans_TeamChoiceBtn.gameObject.SetActive(true);
                m_trans_MemberListRoot.gameObject.SetActive(false);

                //缓存
                ReleaseGrid();
            }
            //红点提示
            NewApplyWarrning();
        }
    }

    void UpdateTeamListGrid()
    {
        if (this.gameObject.activeSelf == false) return;
        if (m_widget_team.gameObject.activeSelf == false) return;

        ReleaseGrid();

        //队员
        // List<TeamMemberInfo> tempList = TDManager.GetMemberListWithoutMe();
        TDManager.GetMemberListWithoutMe(ref m_listTeamMember);

        for (int i = 0; i < m_listTeamMember.Count; i++)
        {
            UILeftTeamMemberGrid leftTeamMemberGrid = GetMemberGrid();

            leftTeamMemberGrid.transform.parent = m_grid_MemberListGrid.transform;
            leftTeamMemberGrid.transform.localScale = Vector3.one;
            leftTeamMemberGrid.transform.localPosition = new Vector3(0, -60.5f * i, 0);
            leftTeamMemberGrid.transform.localRotation = Quaternion.identity;
            leftTeamMemberGrid.gameObject.SetActive(true);

            leftTeamMemberGrid.SetGridData(m_listTeamMember[i]);
            leftTeamMemberGrid.SetName(m_listTeamMember[i].name);
            leftTeamMemberGrid.SetLv(m_listTeamMember[i].lv);
            leftTeamMemberGrid.SetIcon(m_listTeamMember[i].job);
            bool isLeader = DataManager.Manager<TeamDataManager>().IsLeader(m_listTeamMember[i].id);
            leftTeamMemberGrid.SetLeaderMark(isLeader);

            m_lstMemberGrid.Add(leftTeamMemberGrid);
        }

        //“+”按钮
        if (TDManager.MainPlayerIsLeader())
        {
            if (m_listTeamMember.Count < 4)
            {
                InitAddBtn(m_listTeamMember.Count);
            }
            else
            {
                //队伍满了就不要“+”按钮
                Transform t = m_grid_MemberListGrid.GetChild(5);
                m_grid_MemberListGrid.RemoveChild(t);
            }
        }

        //跟随按钮
        if (m_listTeamMember.Count >= 1)
        {
            if (TDManager.MainPlayerIsLeader())
            {
                if (m_listTeamMember.Count < 4)
                {
                    //队伍没满
                    InitFollowBtn(m_listTeamMember.Count + 1);
                }
                else
                {
                    //队伍满了
                    InitFollowBtn(m_listTeamMember.Count);
                }
            }
            else
            {
                InitFollowBtn(m_listTeamMember.Count);
            }
        }

        //m_grid_MemberListGrid.Reposition();

    }

    void InitAddBtn(int count)
    {
        UnityEngine.Object o = UIManager.GetResGameObj(GridID.Uiteamaddgrid);
        GameObject go = Instantiate(o) as GameObject;

        go.transform.parent = m_grid_MemberListGrid.transform;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, -60.5f * (count), 0);
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        UITeamAddGrid grid = go.GetComponent<UITeamAddGrid>();
        if (grid == null)
        {
            grid = go.AddComponent<UITeamAddGrid>();
        }

        grid.RegisterUIEventDelegate(OnTeamAddGridUIEvent);
    }

    private void OnTeamAddGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamInvitePanel);//打开添加队员界面
        }
    }

    /// <summary>
    /// 初始化跟随按钮
    /// </summary>
    void InitFollowBtn(int count)
    {
        UnityEngine.Object obj = UIManager.GetResGameObj(GridID.Uiteamfollowgrid);
        GameObject go = Instantiate(obj) as GameObject;

        go.transform.parent = m_grid_MemberListGrid.transform;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, -60.5f * (count), 0);
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        UITeamFollowGrid teamFollowGrid = go.GetComponent<UITeamFollowGrid>();
        if (teamFollowGrid == null)
        {
            teamFollowGrid = go.AddComponent<UITeamFollowGrid>();
        }

        teamFollowGrid.Init();
    }

    void NewApplyWarrning()
    {
        //红点提示
        if (TDManager.HaveNewApplyMember)
        {
            m_sprite_btnTeam_warrning.gameObject.SetActive(true);
        }
        else
        {
            m_sprite_btnTeam_warrning.gameObject.SetActive(false);
        }
    }

    void ReleaseGrid()
    {
        //加入缓存
        for (int i = 0; i < m_lstMemberGrid.Count; i++)
        {
            m_lstMemberGrid[i].transform.parent = m_trans_MemberGridCache;
            m_lstMemberGrid[i].gameObject.SetActive(false);
            m_lstMemberGridCache.Add(m_lstMemberGrid[i].gameObject);
        }

        //清除
        m_grid_MemberListGrid.transform.DestroyChildren();
        m_lstMemberGrid.Clear();
    }

    /// <summary>
    /// 获取MemberGrid
    /// </summary>
    /// <returns></returns>
    UILeftTeamMemberGrid GetMemberGrid()
    {
        GameObject obj = null;
        UILeftTeamMemberGrid grid = null;

        //有缓存先取缓存
        if (m_lstMemberGridCache.Count > 0)
        {
            obj = m_lstMemberGridCache[0];
            m_lstMemberGridCache.RemoveAt(0);

            grid = obj.GetComponent<UILeftTeamMemberGrid>();
            if (grid == null)
            {
                grid = obj.AddComponent<UILeftTeamMemberGrid>();
            }

            return grid;
        }

        //无缓存  立即创建
        GameObject resObj = UIManager.GetResGameObj(GridID.Uileftteammembergrid) as GameObject;
        obj = Instantiate(resObj) as GameObject;

        grid = obj.GetComponent<UILeftTeamMemberGrid>();
        if (grid == null)
        {
            grid = obj.AddComponent<UILeftTeamMemberGrid>();
        }

        return grid;
    }

    /// <summary>
    /// 显示成员按钮
    /// </summary>
    /// <param name="param"></param>
    void ShowTeamMemberBtn(object param)
    {
        stTeamMemberBtn data = (stTeamMemberBtn)param;

        if (m_trans_TeamMemberBtnRoot.gameObject.activeSelf == true && data.id == this.m_teamMemberBtninfo.id)
        {
            m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
            return;
        }
        else
        {
            m_trans_TeamMemberBtnRoot.gameObject.SetActive(true);
        }

        this.m_teamMemberBtninfo = data;

        m_trans_TeamMemberBtnRoot.position = new Vector3(m_trans_TeamMemberBtnRoot.position.x, m_teamMemberBtninfo.pos_y, m_trans_TeamMemberBtnRoot.position.z);
        m_trans_TeamMemberBtnRoot.localPosition = new Vector3(m_trans_TeamMemberBtnRoot.localPosition.x, m_trans_TeamMemberBtnRoot.localPosition.y - 100, m_trans_TeamMemberBtnRoot.localPosition.z);

        if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
        {
            m_sprite_teamMember_bg.width = 166;
            m_sprite_teamMember_bg.height = 260;
            m_btn_btn_giveleader.gameObject.SetActive(true);
            m_btn_btn_kickedoutteam.gameObject.SetActive(true);
        }
        else
        {
            m_sprite_teamMember_bg.width = 166;
            m_sprite_teamMember_bg.height = 160;
            m_btn_btn_giveleader.gameObject.SetActive(false);
            m_btn_btn_kickedoutteam.gameObject.SetActive(false);
        }
    }

    void onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        RoleRelation data = new RoleRelation() { uid = m_teamMemberBtninfo.id, name = m_teamMemberBtninfo.name };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FriendPanel);
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eChatWithPlayer, data);

        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
    }

    void onClick_Btn_lookmessage_Btn(GameObject caster)
    {

        NetService.Instance.Send(new GameCmd.stRequestViewRolePropertyUserCmd_C()
        {
            zoneid = 0,
            dwUserid = m_teamMemberBtninfo.id,
            mycharid = m_teamMemberBtninfo.id,
        });

        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
    }

    void onClick_Btn_addfriend_Btn(GameObject caster)
    {
        if (Client.ClientGlobal.Instance().IsMainPlayer(m_teamMemberBtninfo.id))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Friend_Friend_bunengtianjiaziji);//不能添加自己
            return;
        }
        List<RoleRelation> list;
        if (DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out list))
        {
            uint maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("MAX_FRIEND_SIZE");

            if (list.Count >= maxNum)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Friend_Friend_haoyoushuliangchaoguoshangxian);//好友数量超过上限
                return;
            }
        }

        DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, m_teamMemberBtninfo.id);
        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
    }

    void onClick_Btn_giveleader_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqChangeLeader(this.m_teamMemberBtninfo.id);
        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
    }

    void onClick_Btn_kickedoutteam_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqKickTeamMember(this.m_teamMemberBtninfo.id);
        m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
    }

    void OnClickTeamMemberBtnClose(GameObject go)
    {
        if (m_trans_TeamMemberBtnRoot.gameObject.activeSelf)
        {
            m_trans_TeamMemberBtnRoot.gameObject.SetActive(false);
        }
    }
}

