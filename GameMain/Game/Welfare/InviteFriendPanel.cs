using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;

partial class InviteFriendPanel : UIPanelBase
{
    List<BaseUserInfo> infos = null;
    uint InviteMaxNum = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        GameObject obj = m_sprite_UIWelfareInviteFriendGrid.gameObject;
        if (m_ctor_PlayListScroll != null)
        {
            m_ctor_PlayListScroll.gridContentOffset = new UnityEngine.Vector2(0, 115);
            m_ctor_PlayListScroll.RefreshCheck();
        }
        m_ctor_PlayListScroll.Initialize<UIWelfareInviteFriendGrid>(obj, OnUpdateInviteList, OnClickGrid);
        InviteMaxNum = GameTableManager.Instance.GetGlobalConfig<uint>("InviteMaxNum");
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        InitList();
    }
    protected override void OnHide()
    {
        base.OnHide();
    }

    void InitList() 
    {
        infos = DataManager.Manager<WelfareManager>().InviterInfos;
        if (infos.Count > 0)
        {
            m_ctor_PlayListScroll.CreateGrids(infos.Count);
        }
        //m_ctor_PlayListScroll.ResetPosition();
        m_trans_NullInvitedContent.gameObject.SetActive(infos.Count ==0);
        m_label_AlreadyNum.text = string.Format("{0}/{1}", infos.Count, InviteMaxNum);
       

    }
    void OnUpdateInviteList(UIGridBase data, int index)
    {
        if (null != infos && index < infos.Count)
        {
            data.SetGridData(infos[index]);
        }
    }

    void OnClickGrid(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIWelfareInviteFriendGrid)
                {
                    UIWelfareInviteFriendGrid sec = data as UIWelfareInviteFriendGrid;
                    DataManager.Instance.Sender.RequestPlayerInfoForOprate(sec.PlayID, PlayerOpreatePanel.ViewType.Normal);
                }
                break;
        }
    }
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }
}
