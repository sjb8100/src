using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


partial class HomeDataManager
{
    #region Net

    /// <summary>
    /// 请求自己或好友家园数据
    /// </summary>
    /// <param name="friendId"></param>
    public void ReqAllUserHomeData(uint friendId)
    {
        stRequestAllHomeUserCmd_C cmd = new stRequestAllHomeUserCmd_C();
        cmd.char_id = friendId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 赞好友
    /// </summary>
    public void ReqZanFriend(uint id)
    {
        stHelpTreeHomeUserCmd_CS c = new stHelpTreeHomeUserCmd_CS() { help_who = id };
        NetService.Instance.Send(c);
    }

    /// <summary>
    /// 可以点赞的
    /// </summary>
    public void ReqHomeFriendStatus() 
    {
        stReqFriendTreeListHomeUserCmd_CS cmd = new stReqFriendTreeListHomeUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 可一点赞的
    /// </summary>
    public void OnHomeFriendStatusChange(stReqFriendTreeListHomeUserCmd_CS cmd)
    {
        List<RoleRelation> friendList = null;
        DataManager.Manager<RelationManager>().GetRelationListByType(RelationType.Relation_Friend, out friendList);
        if (friendList != null)
        {
            for (int i = 0; i< cmd.char_id.Count;i++)
            {
               RoleRelation fd=  friendList.Find((RoleRelation rr) => { return rr.uid == cmd.char_id[i]; });
               if (fd != null)
               {
                   fd.help_tree = true;
               }
               else 
               {
                   fd.help_tree = false;
               }
            }
        }

    }
    #endregion


}

