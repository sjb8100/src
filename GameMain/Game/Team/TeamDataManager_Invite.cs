using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


partial class TeamDataManager
{
    List<GameCmd.stInviteTeammateRelationUserCmd_S.Data> m_InvitePeopleList = new List<GameCmd.stInviteTeammateRelationUserCmd_S.Data>();//从服务器获取的人员list

    public enum PeopleType
    {
        Firend,  //好友
        Clan,    //氏族
        Nearby  //附近的人
    }

    public class People
    {
        public PeopleType peopleType;
        public uint id;
        public string name;
        public uint lv;
        public bool online;
        public uint profession;
        public bool alreadyInvite;  //是否已经邀请
    }

    /// <summary>
    /// 用于附近的人
    /// </summary>
    List<Client.IPlayer> m_lstPlayer = new List<Client.IPlayer>();

    /// <summary>
    /// 好友数据
    /// </summary>
    /// <returns></returns>
    public List<People> GetFriendPeopleList()
    {
        List<People> peopleList = new List<People>();
        List<RoleRelation> friendList = null;
        DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out friendList);
        if (friendList != null)
        {
            for (int i = 0; i < friendList.Count; i++)
            {
                if (Client.ClientGlobal.Instance().IsMainPlayer(friendList[i].uid))//排除自己
                {
                    continue;
                }

                if (DataManager.Manager<TeamDataManager>().IsMember(friendList[i].uid))//排除已经加入的队员
                {
                    continue;
                }

                if (friendList[i].online)//在线的
                {
                    peopleList.Add(new People
                    {
                        peopleType = PeopleType.Firend,
                        id = friendList[i].uid,
                        name = friendList[i].name,
                        lv = friendList[i].level,
                        online = friendList[i].online,
                        profession = friendList[i].profession,
                        alreadyInvite = false
                    });
                }
            }
        }

        return peopleList;
    }

    /// <summary>
    /// 氏族成员数据
    /// </summary>
    /// <returns></returns>
    public List<People> GetClanPeopleList()
    {
        List<People> peopleList = new List<People>();
        if (DataManager.Manager<ClanManger>().IsJoinClan)
        {
            ClanDefine.LocalClanInfo claninfo = DataManager.Manager<ClanManger>().ClanInfo;

            if (claninfo != null && claninfo.MemberList!= null)
            {
                for (int i = 0; i < claninfo.MemberList.member.Count; i++)
                {
                    if (Client.ClientGlobal.Instance().IsMainPlayer(claninfo.MemberList.member[i].id))//排除自己
                    {
                        continue;
                    }

                    if (DataManager.Manager<TeamDataManager>().IsMember(claninfo.MemberList.member[i].id))//排除已经加入的队员
                    {
                        continue;
                    }

                    if (claninfo.MemberList.member[i].is_online == 0) //排除不在线的
                    {
                        continue;
                    }

                    peopleList.Add(new People
                    {
                        peopleType = PeopleType.Firend,
                        id = claninfo.MemberList.member[i].id,
                        name = claninfo.MemberList.member[i].name,
                        lv = claninfo.MemberList.member[i].level,
                        online = claninfo.MemberList.member[i].is_online == 1 ? true : false,
                        profession = claninfo.MemberList.member[i].job,
                        alreadyInvite = false
                    });
                }
            }
        }
        else
        {
            //未加入氏族

        }

        return peopleList;
    }

    /// <summary>
    /// 附近的人（从服务器获取数据，当前地图的人）先注释，改为获取9屏幕
    /// </summary>
    /// <returns></returns>
    /*
    public List<People> GetNearbyPeopleList()
    {
        List<People> peopleList = new List<People>();

        for (int i = 0; i < m_InvitePeopleList.Count; i++)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(m_InvitePeopleList[i].id))//排除自己
            {
                continue;
            }

            if (DataManager.Manager<TeamDataManager>().IsMember(m_InvitePeopleList[i].id))//排除已经加入的队员
            {
                continue;
            }

            peopleList.Add(new People
            {
                peopleType = PeopleType.Nearby,
                id = m_InvitePeopleList[i].id,
                name = m_InvitePeopleList[i].name,
                profession = m_InvitePeopleList[i].job,
                lv = m_InvitePeopleList[i].level,
                alreadyInvite = false
            });
        }
        return peopleList;
    }
    */

    /// <summary>
    /// 附近的人（9屏数据）
    /// </summary>
    /// <returns></returns>
    public List<People> GetNearbyPeopleList()
    {
        List<People> peopleList = new List<People>();

        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

        m_lstPlayer.Clear();
        if (es != null)
        {
            es.FindAllEntity<Client.IPlayer>(ref m_lstPlayer);
        }

        for (int i = 0; i < m_lstPlayer.Count; i++)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(m_lstPlayer[i]))//排除自己
            {
                continue;
            }

            if (DataManager.Manager<TeamDataManager>().IsMember(m_lstPlayer[i].GetID()))//排除已经加入的队员
            {
                continue;
            }

            peopleList.Add(new People
            {
                peopleType = PeopleType.Nearby,
                id = m_lstPlayer[i].GetID(),
                name = m_lstPlayer[i].GetName(),
                profession = (uint)m_lstPlayer[i].GetProp((int)Client.PlayerProp.Job),
                lv = (uint)m_lstPlayer[i].GetProp((int)Client.CreatureProp.Level),
                alreadyInvite = false
            });
        }
        return peopleList;
    }

    public List<People> GetInvitePeopleListByType(PeopleType type)
    {
        if (type == PeopleType.Firend)
        {
            return GetFriendPeopleList();
        }

        if (type == PeopleType.Clan)
        {
            return GetClanPeopleList();
        }

        if (type == PeopleType.Nearby)
        {
            return GetNearbyPeopleList();
        }

        Engine.Utility.Log.Error("没有取到数据！！！");
        return null;
    }

    #region Net

    /// <summary>
    /// 请求可以邀请的人的list
    /// </summary>
    /// <param name="type">TeamInvite_Friend 为好友， TeamInvite_Nearby 为附近的人</param>
    public void ReqPeopleListByType(GameCmd.TeamInvite type)
    {
        GameCmd.stInviteTeammateRelationUserCmd_C cmd = new GameCmd.stInviteTeammateRelationUserCmd_C();
        cmd.type = (uint)type;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 获取人员list 用于邀请到队伍中
    /// </summary>
    /// <param name="cmd"></param>
    public void OnPeopleListByType(GameCmd.stInviteTeammateRelationUserCmd_S cmd)
    {
        m_InvitePeopleList = cmd.data;
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamInvitePanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamInvitePanel, UIMsgID.eTeamInvitePeopleList, null);
        }
    }

    #endregion

}

