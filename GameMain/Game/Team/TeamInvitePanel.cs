using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class TeamInvitePanel : UIPanelBase
{

    List<TeamDataManager.People> m_peopleList = new List<TeamDataManager.People>();

    private UIGridCreatorBase m_TeamInviteGridCreator = null;

    TeamDataManager.PeopleType m_ePeopletype = TeamDataManager.PeopleType.Firend;//好友

    TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }

    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        DataManager.Manager<RelationManager>().ValueUpdateEvent += OnValueUpdateEventArgs;

        InitTabBtn();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eTeamInvitePeopleList)
        {
            CreateGrids();
        }
        return true;
    }

    protected override void OnHide()
    {
        DataManager.Manager<RelationManager>().ValueUpdateEvent -= OnValueUpdateEventArgs;

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if ("updateFriendLevel" == v.key && m_ePeopletype == TeamDataManager.PeopleType.Firend)
        {
            CreateGrids();
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_TeamInviteGridCreator != null)
        {
            m_TeamInviteGridCreator.Release(depthRelease);
        }
        
    }

    #endregion

    #region method

    void InitWidget()
    {
        if (m_trans_scrollView != null)
        {
            m_TeamInviteGridCreator = m_trans_scrollView.GetComponent<UIGridCreatorBase>();
            if (m_TeamInviteGridCreator == null)
            {
                m_TeamInviteGridCreator = m_trans_scrollView.gameObject.AddComponent<UIGridCreatorBase>();
            }
            if (m_TeamInviteGridCreator != null)
            {
                //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiteaminvitegrid) as UnityEngine.GameObject;
                m_TeamInviteGridCreator.gridContentOffset = new UnityEngine.Vector2(-147, 0);
                m_TeamInviteGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                m_TeamInviteGridCreator.gridWidth = 295;
                m_TeamInviteGridCreator.gridHeight = 103;
                m_TeamInviteGridCreator.rowcolumLimit = 2;

                m_TeamInviteGridCreator.RefreshCheck();
                m_TeamInviteGridCreator.Initialize<UITeamInviteGrid>(m_sprite_UITeamInviteGrid.gameObject,  OnTeamInviteGridDataUpdate, OnTeamInviteGridUIEvent);
            }
        }
    }

    void CreateGrids()
    {
        m_peopleList = TDManager.GetInvitePeopleListByType(m_ePeopletype);
        if (m_peopleList != null)
        {
            m_TeamInviteGridCreator.CreateGrids((null != m_peopleList) ? m_peopleList.Count : 0);
        }
    }


    private void OnTeamInviteGridDataUpdate(UIGridBase data, int index)
    {
        if (null != m_peopleList && index < m_peopleList.Count)
        {
            UITeamInviteGrid grid = data as UITeamInviteGrid;
            if (grid == null)
            {
                return;
            }

            grid.SetGridData(m_peopleList[index]);
            grid.SetIcon(m_peopleList[index].profession);
        }
    }

    private void OnTeamInviteGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UITeamInviteGrid grid = data as UITeamInviteGrid;
            if (grid == null)
            {
                return;
            }

            if (param != null)
            {
                int inviteBtnIndex = 1;
                int btnIndex = (int)param;
                if (btnIndex == inviteBtnIndex)
                {
                    TDManager.ReqInviteTeam(grid.m_people.id, grid.m_people.name);

                    List<TeamDataManager.People> list = TDManager.GetInvitePeopleListByType(grid.m_people.peopleType);
                    TeamDataManager.People people = list.Find((TeamDataManager.People p) => { return p.id == grid.m_people.id; });
                    if (people != null)
                    {
                        people.alreadyInvite = true;
                        grid.m_people.alreadyInvite = true;
                    }
                }
            }
            else
            {
                if (grid.m_people.peopleType == TeamDataManager.PeopleType.Clan)
                {
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.m_people.id, PlayerOpreatePanel.ViewType.Clan);
                }
                else if (grid.m_people.peopleType == TeamDataManager.PeopleType.Firend)
                {
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.m_people.id, PlayerOpreatePanel.ViewType.AddRemove_Contact);
                }
                else
                {
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(grid.m_people.id, PlayerOpreatePanel.ViewType.Normal);
                }
            }
        }
    }

    /// <summary>
    /// 初始化tab 按钮
    /// </summary>
    void InitTabBtn()
    {
        switch (m_ePeopletype)
        {
            case TeamDataManager.PeopleType.Firend:
                {
                    onClick_Friend_Btn(null);
                }; break;
            case TeamDataManager.PeopleType.Clan:
                {
                    onClick_Clan_Btn(null);
                }; break;
            case TeamDataManager.PeopleType.Nearby:
                {
                    onClick_Near_Btn(null);
                }; break;
        }
    }


    #endregion



    #region click

    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Friend_Btn(GameObject caster)
    {
        m_TeamInviteGridCreator.ClearAll();

        Protocol.Instance.ReqFriendLevel();//消息返回后再创建

        m_ePeopletype = TeamDataManager.PeopleType.Firend;
        CreateGrids();
    }

    void onClick_Clan_Btn(GameObject caster)
    {
        if (DataManager.Manager<ClanManger>().IsJoinClan == false)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Clan_Commond_haiweijiarushizu);//还未加入氏族
        }
        m_TeamInviteGridCreator.ClearAll();
        m_ePeopletype = TeamDataManager.PeopleType.Clan;
        CreateGrids();
    }

    void onClick_Near_Btn(GameObject caster)
    {
        m_TeamInviteGridCreator.ClearAll();
        m_ePeopletype = TeamDataManager.PeopleType.Nearby;

        //从服务器获取人物数据（先注释，改为从0屏获取数据 ）
        // DataManager.Manager<TeamDataManager>().ReqPeopleListByType(GameCmd.TeamInvite.TeamInvite_Nearby);

        CreateGrids();
    }
    #endregion
}

