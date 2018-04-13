using System;
using System.Collections.Generic;
using GameCmd;
using Engine.Utility;

public class RoleRelation 
{
    public RelationType type;
    public uint uid;
    public string name;
    public bool online;
    public uint level;
    /// <summary>
    /// 职业
    /// </summary>
    public uint profession;
    public uint degree;
    public uint title;
    public uint mapid;
    public uint lastChatTime;
    public bool help_tree;

    public bool isSys = false;

    public bool isRobot = false;

}

public class RelationManager :BaseModuleData,IManager
{
    Dictionary<RelationType, List<RoleRelation>> m_dicRelation;

    List<RoleRelation> m_lstRecommend = null;
    List<RoleRelation> m_lstRecommendCache = null;
    public List<RoleRelation> RecommendList
    {
        get
        {
            return m_lstRecommend;
        }
    } 
    public bool newFriendReuquest = false;
    public void ClearData()
    {
        foreach (var item in m_dicRelation)
        {
            item.Value.Clear();
        }
        m_lstRecommend.Clear();
        m_lstRecommendCache.Clear();
    }
    public void Initialize()
    {
        m_dicRelation = new Dictionary<RelationType, List<RoleRelation>>();
        m_lstRecommend = new List<RoleRelation>();
        m_lstRecommendCache = new List<RoleRelation>();
        m_dicRelation.Add(RelationType.Relation_Contact, new List<RoleRelation>());
//        m_dicRelation[RelationType.Relation_Contact].Add(new RoleRelation() { isSys = true });
        m_dicRelation.Add(RelationType.Relation_FriendRequest, new List<RoleRelation>());
        m_dicRelation.Add(RelationType.Relation_Friend,new List<RoleRelation>());
        m_dicRelation.Add(RelationType.Relation_Black,new List<RoleRelation>());
        m_dicRelation.Add(RelationType.Relation_Enemy, new List<RoleRelation>());
        m_dicRelation.Add(RelationType.Relation_Interactive, new List<RoleRelation>());

    }

    public void Reset(bool depthClearData = false)
    {
//         foreach (var item in m_dicRelation)
//         {
//             item.Value.Clear();
//         }
//         m_lstRecommendCache.Clear();
//         m_lstRecommend.Clear();
    }

    public void Process(float deltaTime)
    {

    }

    /// <summary>
    /// 获取某一种关系类型所有玩家
    /// </summary>
    /// <param name="type"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public bool GetRelationListByType(RelationType type, out List<RoleRelation> list)
    {
        if (m_dicRelation.TryGetValue(type, out list))
        {
            return true;
        }
        return false;
    }

    public void UpdateContactTimestamp(uint id,uint timestamp)
    {
        if (id != 0)
        {
            List<RoleRelation> friends;
            if (GetRelationListByType(GameCmd.RelationType.Relation_Contact, out friends))
            {
                for (int i = 0; i < friends.Count; i++)
                {
                    if (friends[i].uid == id)
                    {
                        friends[i].lastChatTime = timestamp;
                        friends.Sort(SortRelationsByChatTime);
                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SortList", 0, friends);
                        DispatchValueUpdateEvent(arg);
                        break;
                    }
                }
            }
        }
    }

    int SortRelationsByChatTime(RoleRelation a,RoleRelation b)
    {
        if (a.isSys)
        {
            return -1;
        }
        if (a.lastChatTime > b.lastChatTime )
        {
            return -1;
        }
        else if (a.lastChatTime < b.lastChatTime)
        {
            return 1;
        }
        return 0;
    }
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool AddRelationByType(RelationType type,tFriendBaseData data)
    {
        if (type == RelationType.Relation_Friend)
        {
            if (m_lstRecommend != null)
            {
                for (int i = 0; i < m_lstRecommend.Count; i++)
                {
                    if (m_lstRecommend[i].uid == data.id)
                    {
                        m_lstRecommend.RemoveAt(i);
                        break;
                    }
                }
            }
            if (m_lstRecommendCache != null)
            {
                for (int i = 0; i < m_lstRecommendCache.Count; i++)
                {
                    if (m_lstRecommendCache[i].uid == data.id)
                    {
                        m_lstRecommendCache.RemoveAt(i);
                        if (m_lstRecommend != null && m_lstRecommend.Count  > 0)
                        {
                            m_lstRecommendCache.Add(m_lstRecommend[0]);
                            m_lstRecommend.RemoveAt(0);
                        }
                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SEARCH", 0, m_lstRecommendCache);
                        DispatchValueUpdateEvent(arg);
                        break;
                    }
                }
            }
        }

        List<RoleRelation> lst = null;
        if (m_dicRelation.TryGetValue(type,out lst))
        {
            RoleRelation rec = new RoleRelation()
            {
                type = type,
                uid = data.id, 
                online = data.online == 1, 
                level = data.level, 
                profession = data.profession,
                degree = data.degree,
                title = data.title,
                mapid = data.mapid,
                name = data.username,
                help_tree = data.help_tree,
            };
            bool isOld = false;
            for (int i = 0; i < lst.Count;i++ )
            {
                if (lst[i].uid == data.id)
                {
                    lst[i] = rec;
                    isOld = true;
                }
            }
            if (!isOld)
            {
                lst.Add(rec);
            }
            lst.Sort(SortRelationsByChatTime);
            ValueUpdateEventArgs arg = new ValueUpdateEventArgs(type.ToString(), 0, lst);
            DispatchValueUpdateEvent( arg );
            if (type == RelationType.Relation_FriendRequest)
            {
                newFriendReuquest = true;
            }
            return true;
        }
        else {
            Log.Error(" m_dicRelation no contain type :{0}", type);
        }
        return false;
    }

    public void RemoveRelationByType(RelationType type,uint uid)
    {
        List<RoleRelation> lst = null;
        if (m_dicRelation.TryGetValue(type, out lst))
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].uid == uid)
                {
                    lst.RemoveAt(i);
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs(type.ToString(), 0, lst);
                    DispatchValueUpdateEvent(arg);
                    if (type == RelationType.Relation_Friend)
                    {
                        TipsManager.Instance.ShowTipsById(504);
                    }
                    else if (type == RelationType.Relation_Black)
                    {
                        TipsManager.Instance.ShowTipsById(507);
                    }
                    else if (type == RelationType.Relation_Contact)
                    {
                        TipsManager.Instance.ShowTipsById(509);
                    }
                }
            }
        }
        else
        {
            Log.Error(" m_dicRelation no contain type :{0}", type);
        }
    }

    public void UpdateRelationOnLineState(bool isOnLine,uint id,uint mapid = 0)
    {
        List<RoleRelation> friends;
        if (GetRelationListByType(GameCmd.RelationType.Relation_Friend, out friends))
        {
            foreach (var item in friends)
            {
                if (item.uid != id)
                {
                    continue;
                }
                
                item.mapid = mapid;
                if (item.online != isOnLine)
                {
                    if (isOnLine)
                    {
                        TipsManager.Instance.ShowTipsById(500, item.name);
                    }
                    else
                    {
                        TipsManager.Instance.ShowTipsById(501, item.name);
                    }
                }

                item.online = isOnLine;
                break;
            }
        }
    }

    public void UpdateRequestSearch(List<tFriendBaseData> userdata)
    {
        List<RoleRelation> lst = new List<RoleRelation>();

        foreach (var data in userdata)
        {
            lst.Add(new RoleRelation()
            {
                uid = data.id,
                online = data.online == 1,
                level = data.level,
                profession = data.profession,
                degree = data.degree,
                title = data.title,
                mapid = data.mapid,
                name = data.username,
            });      
        }
        Engine.Utility.Log.LogGroup("ZCX", "UpdateRequestSearch :{0}", userdata.Count);
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SEARCH", 0, lst);
        DispatchValueUpdateEvent(arg);
    }

    public bool IsMyFriend(uint uid)
    {
        return IsInRelationList(GameCmd.RelationType.Relation_Friend,uid);;
    }

    public bool IsInRelationList(RelationType type, uint uid)
    {
        List<RoleRelation> datas;
        if (GetRelationListByType(type, out datas))
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].uid == uid)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsMyBlack(uint uid)
    {
        return IsInRelationList(GameCmd.RelationType.Relation_Black,uid);
    }

    public void AddRecommendList(List<tFriendBaseData> userdata)
    {
        if (m_lstRecommend == null)
        {
            return;
        }
        m_lstRecommend.Clear();

        foreach (var data in userdata)
        {
            m_lstRecommend.Add(new RoleRelation()
            {
                uid = data.id,
                online = data.online == 1,
                level = data.level,
                profession = data.profession,
                degree = data.degree,
                title = data.title,
                mapid = data.mapid,
                name = data.username,
            });
        }
        Engine.Utility.Log.LogGroup("ZCX", "AddRecommendList :{0}", userdata.Count);
        List<RoleRelation> lstdata = new List<RoleRelation>();

        m_lstRecommendCache.Clear();
        for (int i = 0;  i < 5 && m_lstRecommend.Count > 0; i++)
        {
            lstdata.Add(m_lstRecommend[0]);
            m_lstRecommendCache.Add(m_lstRecommend[0]);
            m_lstRecommend.RemoveAt(0);
        }

        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SEARCH", 0, lstdata);
        DispatchValueUpdateEvent(arg);
    }

    public bool CheckRecommendList()
    {
        if (m_lstRecommend.Count > 0)
        {
            List<RoleRelation> lstdata = new List<RoleRelation>();
            m_lstRecommendCache.Clear();
            for (int i = 0; i < 5 && m_lstRecommend.Count > 0; i++)
            {
                lstdata.Add(m_lstRecommend[0]);
                m_lstRecommendCache.Add(m_lstRecommend[0]);
                m_lstRecommend.RemoveAt(0);
            }

            ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SEARCH", 0, lstdata);
            DispatchValueUpdateEvent(arg);

            return true;
        }
        else
        {
            ValueUpdateEventArgs arg = new ValueUpdateEventArgs("SEARCH", 0, m_lstRecommendCache);
            DispatchValueUpdateEvent(arg);
        }

        return false;
    }

    public void RefreshFriendLevel(uint nUID,uint nLevel,bool online)
    {
        List<RoleRelation> datas;
        if (GetRelationListByType(RelationType.Relation_Friend, out datas))
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].uid == nUID)
                {
                    datas[i].level = nLevel;
                    datas[i].online = online;
                }
            }
            datas.Sort(FriendSort);
        }
    }

    int FriendSort(RoleRelation left, RoleRelation right) 
    {
        int leftNum = left.online ? 1:0;
        int rightNum = right.online ? 1:0;
        // -  在线优先   +  离线优先
        return rightNum - leftNum;
    }
    public void DispatchFriendLevel()
    {
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("updateFriendLevel", null, null);
        DispatchValueUpdateEvent(arg);
    }
    #region BlackName
    /// <summary>
    /// 屏蔽操作
    /// </summary>
    /// <param name="uid"></param>
    public void ShiledPlayer(uint uid)
    {
        if (!IsMyBlack(uid))
        {
            //拉黑
            DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Black, uid);
        }else
        {
            //取消拉黑
            DataManager.Instance.Sender.RequestKickRelationUser(GameCmd.RelationType.Relation_Black, uid);
        }
    }
    #endregion

}
