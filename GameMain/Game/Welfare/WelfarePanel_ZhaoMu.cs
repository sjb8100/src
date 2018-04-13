using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Controller;
using Client;

partial class WelfarePanel
{
    InviteType curInviteType = InviteType.Inviter;

    void ChooseInviteType(InviteType type) 
    {
        DataManager.Manager<WelfareManager>().UpdateWelfareState(1);
        curInviteType = type;
        m_trans_InviterRoot.gameObject.SetActive(type != InviteType.Invited);
        m_trans_InvitedRoot.gameObject.SetActive(type == InviteType.Invited);
        m_lstWelFareData = m_dataManager.GetWelfareDatasBy2Type(WelfareType.FriendInvite, curInviteType);
        m_lstWelFareData.Sort();
        if (m_ctor_FriendInviteScroll != null)
        {
            m_ctor_FriendInviteScroll.CreateGrids(m_lstWelFareData.Count);
        }
    }
    void onClick_InviterBtn_Btn(GameObject caster)
    {
        ChooseInviteType(InviteType.Inviter);
    }
    
    void onClick_InvitedBtn_Btn(GameObject caster)
    {
        ChooseInviteType(InviteType.Invited);
    }
    void onClick_InvitedRechargeBtn_Btn(GameObject caster)
    {
        ChooseInviteType(InviteType.InvitedRecharge);
    }


    void onClick_CopyBtn_Btn(GameObject caster)
    {
        TextEditor te = new TextEditor();
        te.text = m_label_InviteCode.text;
        te.SelectAll();
        te.Copy();
        TipsManager.Instance.ShowTips("已将邀请码复制到剪切板");
    }

    void onClick_ShareBtn_Btn(GameObject caster)
    {
        
    }

    void onClick_InviteListBtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.InviteFriendPanel);
    }

    void onClick_ConfirmInviteBtn_Btn(GameObject caster)
    {
        uint inviteCode = 0;
        uint levelLimit = GameTableManager.Instance.GetGlobalConfig<uint>("InviteLevelLimit");
        if (MainPlayerHelper.GetPlayerLevel() > levelLimit)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Invite_Commond_wufabeizhaomu);
            return;
        }
        if (uint.TryParse(m_input_MyInviteCode.value, out inviteCode))
        {
            NetService.Instance.Send(new GameCmd.stRequestRecruitRelationUserCmd_CS() { user_id = inviteCode });
        }
        else 
        {
            TipsManager.Instance.ShowTips("邀请码输入有误");
        }
    }

    void onClick_ContactInviterBtn_Btn(GameObject caster)
    {
        uint inviterID = DataManager.Manager<WelfareManager>().InviterID;
        DataManager.Instance.Sender.RequestPlayerInfoForOprate(inviterID, PlayerOpreatePanel.ViewType.Normal);
    }

    void ChangeInviteState() 
    {
        uint inviterID = DataManager.Manager<WelfareManager>().InviterID;
        bool isOver =  DataManager.Manager<WelfareManager>().HadBeenInvited;
        uint job = DataManager.Manager<WelfareManager>().InviterProfession;
        m_trans_NoInviter.gameObject.SetActive(!(inviterID != 0 && isOver));
        m_trans_HasInviter.gameObject.SetActive((inviterID != 0 && isOver));
        string msg ="";
        string jobText = "";     
        table.SelectRoleDataBase data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
        if (data != null)
        {
            jobText = data.professionName;
        }
        uint lv = DataManager.Manager<WelfareManager>().InviterLv;
        string name = DataManager.Manager<WelfareManager>().InviterName;
        msg = string.Format("{0}({1} {2})", name, lv+ "级", jobText);
        m_label_InviterInfo.text = msg;

    }

}
