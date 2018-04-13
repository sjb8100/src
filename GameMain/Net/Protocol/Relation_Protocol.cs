using System;
using System.Collections.Generic;
using GameCmd;
using Common;
using Engine.Utility;

partial class Protocol
{
    /*	stRequestAddRelationUserCmd_CS = 3;
        stAnswerAddRelationUserCmd_C = 4;
        stAddRelationUserCmd_S = 5;
        stRequestRemoveRelationUserCmd = 6;
        stRequestKickRelationUserCmd_C = 7;
        stRemoveRelationUserCmd_S = 8;
        stOnlineRelationUserCmd_S = 9;
        stOfflineRelationUserCmd_S = 10;
        stAddListRelationUserCmd_S = 11;
        stRequestSaveEnemyRelationUserCmd_C = 12;
        stRequestCancelSaveEnemyRelationUserCmd_C = 13;
     */

    RelationManager relationManager
    {
        get { return DataManager.Manager<RelationManager>(); }
    }
    #region recive
    [Execute]//邀请加入某个社会关
    public void Excute(stRequestAddRelationUserCmd_CS cmd)
    {
        Engine.Utility.Log.Error("邀请加入某个社会关系");
        //GameCmd.stCommonMChatUserCmd
        if (cmd.type == RelationType.Relation_FriendRequest)
        {
            DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(new GameCmd.stCommonMChatUserCmd_CS()
            {
                dwOPDes = 0,
                szInfo = ChatDataManager.GetAddFriendHrefString(cmd.requestname, cmd.id),
                szOPDes = cmd.requestname,
                byChatType = CHATTYPE.CHAT_SYS,
            });
        }
    }

    [Execute]//添加某个社会关系
    public void Excute(stAddRelationUserCmd_S cmd)
    {
        tFriendBaseData data = null;
        switch (cmd.type)
        {
            case RelationType.Relation_Friend:
                data = cmd.frdata;
                if (string.IsNullOrEmpty(data.username))
                {
                    TipsManager.Instance.ShowTips("添加好友成功 但是名字是空的！");
                    UnityEngine.Debug.LogError("添加好友成功 但是名字是空的！");
                }
                else
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Friend_Friend_tianjiachenggong, data.username);
                }
                break;
            case RelationType.Relation_Black:
                data = cmd.bldata;
                TipsManager.Instance.ShowTips(LocalTextType.Friend_BlackList_pingbichenggong);
                break;
            case RelationType.Relation_Enemy:
                data = cmd.endata;
                break;
            case RelationType.Relation_Contact:
                data = cmd.strangerdata;
                    break;
            case RelationType.Relation_FriendRequest:
                    data = cmd.friendrequestdata;
                break;
            case RelationType.Relation_Interactive:
                data = cmd.interactive;
                break;
        }

        if (data != null)
        {
            relationManager.AddRelationByType(cmd.type, data);
        }
        else
        {
            Engine.Utility.Log.Error("添加某个社会关系 数据为空");
        }
    }

    [Execute]//删除某个社会关系
    public void Excute(stRemoveRelationUserCmd_S cmd)
    {
        relationManager.RemoveRelationByType(cmd.type, cmd.id);
    }

    [Execute]
    public void Excute(stOnlineRelationUserCmd_S cmd)
    {
        relationManager.UpdateRelationOnLineState(true, cmd.id, cmd.mapid);
    }

    [Execute]
    public void Excute(stOfflineRelationUserCmd_S cmd)
    {
        relationManager.UpdateRelationOnLineState(false, cmd.id);
    }

    [Execute]
    public void Excute(stAddListRelationUserCmd_S cmd)
    {
        switch (cmd.type)
        {
            case RelationType.Relation_Black:
                for (int i = 0; i < cmd.bldata.Count; i++)
                    relationManager.AddRelationByType(cmd.type, cmd.bldata[i]);
                break;
            case RelationType.Relation_Contact:
                for (int i = 0; i < cmd.contactdata.Count; i++)
                    relationManager.AddRelationByType(cmd.type, cmd.contactdata[i]);
                break;
            case RelationType.Relation_Enemy:
                for (int i = 0; i < cmd.endata.Count; i++)
                    relationManager.AddRelationByType(cmd.type, cmd.endata[i]);      
                break;
            case RelationType.Relation_Friend:
                for (int i = 0; i < cmd.frdata.Count; i++)
                    relationManager.AddRelationByType(cmd.type, cmd.frdata[i]);
                break;
            case RelationType.Relation_FriendRequest:
                for (int i = 0; i < cmd.friendrequestdata.Count; i++)
                   relationManager.AddRelationByType(cmd.type, cmd.friendrequestdata[i]);
                break;
            case RelationType.Relation_Interactive:
                for (int i = 0; i < cmd.interactivedata.Count; i++)
                    relationManager.AddRelationByType(cmd.type, cmd.interactivedata[i]);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 查找好友列表返回
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stReturnSearchUserRelationUserCmd_S cmd)
    {
        relationManager.UpdateRequestSearch(cmd.userdata);
    }

    /// <summary>
    /// 推荐好友列表
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stRecommendFriendListRelationUserCmd_S cmd)
    {
        relationManager.AddRecommendList(cmd.redata);
    }

    [Execute]
    public void Execute(stRefreshFriendLevRelationUserCmd_S cmd)
    {
        for (int i = 0; i < cmd.data.Count; i++)
        {
            relationManager.RefreshFriendLevel(cmd.data[i].userid, cmd.data[i].level,cmd.data[i].online);            
        }
        relationManager.DispatchFriendLevel();
    }
    #endregion 


    #region Send
    //------------------------------------------------
    public void RequestKickRelationUser(RelationType rtype, uint uid)
    {
        NetService.Instance.Send(new GameCmd.stRequestKickRelationUserCmd_C()
        {
            type = rtype,
            id = uid,
        });
    }

    /// <summary>
    /// 清空申请列表
    /// </summary>
    public void ClearRequestFriendList()
    {
        List<RoleRelation> lst = null;
        
        if (DataManager.Manager<RelationManager>().GetRelationListByType(RelationType.Relation_FriendRequest,out lst))
        {
            for (int i = 0; i < lst.Count; i++)
            {
                RequestKickRelationUser(RelationType.Relation_FriendRequest, lst[i].uid);
            }
        }
    }
    /// <summary>
    /// 申请查询添加好友
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="strname"></param>
    public void RequestSearchUserRelation(uint uid, string strname)
    {
        NetService.Instance.Send(new stRequestSearchUserRelationUserCmd_C() { id = uid, name = strname });
    }

    /// <summary>
    /// 添加关系
    /// </summary>
    public void RequestAddRelation(RelationType rtype,uint uid)
    {
        NetService.Instance.Send(new GameCmd.stRequestAddRelationUserCmd_CS()
        {
            type = rtype,
            id = uid,
        });
    }

    /// <summary>
    /// 请求推荐好友列表
    /// </summary>
    public void ReqRecomFriendList()
    {
        NetService.Instance.Send(new GameCmd.stReqRecomFriendListRelationUserCmd_C() { });
    }

    public void ReqFriendLevel()
    {
        NetService.Instance.Send(new GameCmd.stRequestFriendLevRelationUserCmd_C() { });
    }
    #endregion
}
