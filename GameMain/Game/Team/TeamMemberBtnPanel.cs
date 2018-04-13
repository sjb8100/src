using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class TeamMemberBtnPanel : UIPanelBase
{
    MyTeamGridData myTeamGridData;

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_widget_close.gameObject).onClick = OnClickClose;
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        this.myTeamGridData = data as MyTeamGridData;

        if (this.myTeamGridData != null)
        {
            m_trans_content.position = this.myTeamGridData.pos;
        }
        else
        {
            Engine.Utility.Log.Error("好友数据MyTeamGridData 为null  !!!");
        }

        uint MainPlayerID = ClientGlobal.Instance().MainPlayer.GetID();
        if (IsLeader(MainPlayerID))   //本人是队长   有5个按钮
        {
            m_sprite_bg.width = 166;
            m_sprite_bg.height = 260;
            m_btn_btn_sendmessage.gameObject.SetActive(true);
            m_btn_btn_lookmessage.gameObject.SetActive(true);
            m_btn_btn_addfriend.gameObject.SetActive(true);
            m_btn_btn_giveleader.gameObject.SetActive(true);
            m_btn_btn_kickedoutteam.gameObject.SetActive(true);
        }
        else                               //本人不是队长，3个按钮
        {
            m_sprite_bg.width = 166;
            m_sprite_bg.height = 160;
            m_btn_btn_sendmessage.gameObject.SetActive(true);
            m_btn_btn_lookmessage.gameObject.SetActive(true);
            m_btn_btn_addfriend.gameObject.SetActive(true);
            m_btn_btn_giveleader.gameObject.SetActive(false);
            m_btn_btn_kickedoutteam.gameObject.SetActive(false);
        }

    }

    protected override void OnShow(object data)
    {

    }

    protected override void OnHide()
    {
        base.OnHide();
    }
    #endregion


    #region method

    bool IsLeader(uint id)
    {
        return DataManager.Manager<TeamDataManager>().IsLeader(id);
    }
    #endregion

    #region Click

    void onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        RoleRelation data = new RoleRelation() { uid = this.myTeamGridData.teamMemberInfo.id, name = this.myTeamGridData.teamMemberInfo.name };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FriendPanel);
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eChatWithPlayer, data);
        HideSelf();
    }

    void onClick_Btn_lookmessage_Btn(GameObject caster)
    {
        NetService.Instance.Send(new GameCmd.stRequestViewRolePropertyUserCmd_C()
        {
            zoneid = 0,
            dwUserid = this.myTeamGridData.teamMemberInfo.id,
            mycharid = this.myTeamGridData.teamMemberInfo.id,
        });

        HideSelf();
    }

    void onClick_Btn_addfriend_Btn(GameObject caster)
    {
        if (Client.ClientGlobal.Instance().IsMainPlayer(myTeamGridData.teamMemberInfo.id))
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

        DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, myTeamGridData.teamMemberInfo.id);


        HideSelf();
    }

    void onClick_Btn_giveleader_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqChangeLeader(this.myTeamGridData.teamMemberInfo.id);
        HideSelf();
    }

    void onClick_Btn_kickedoutteam_Btn(GameObject caster)
    {
        DataManager.Manager<TeamDataManager>().ReqKickTeamMember(this.myTeamGridData.teamMemberInfo.id);
        HideSelf();
    }

    void OnClickClose(GameObject caster)
    {
        HideSelf();
    }

    #endregion

}
