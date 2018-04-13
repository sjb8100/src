/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  InvitePanel
 * 版本号：  V1.0.0.0
 * 创建时间：10/31/2016 2:51:42 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class InvitePanel
{
    #region define
    public class InvitePanelData
    {
        //title
        public string title;
        //邀请列表
        public List<InviteData> inviteDatas;
        //点击邀请响应
        public Action<InviteData> inviteBtnClickAction;

        /// <summary>
        /// 根据用户id获取邀请数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InviteData GetInviteData(uint userId)
        {
            if (null != inviteDatas && inviteDatas.Count > 0)
            {
                foreach(InviteData data in inviteDatas)
                {
                    if (data.userId == userId)
                    {
                        return data;
                    }
                }
            }
            return null;
        }
        public bool isClanInvite = false;
    }

    public class InviteData
    {
        public string name;
        public string icon;
        public int lv;
        public uint userId;
    }
    #endregion
    #region property
    private UIGridCreatorBase m_inviteCreator = null;
    private UIGridCreatorBase m_clanInviteCreator = null;
    private InvitePanelData m_inviteData = null;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
       
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is InvitePanelData)
        {
            m_inviteData = data as InvitePanelData;
        }else
        {
            m_inviteData = null;
        }
        if (null != m_label_TitleText)
        {
            m_label_TitleText.text = ((null != m_inviteData) ? m_inviteData.title : "");
        }
        InitWidgets(m_inviteData.isClanInvite);
        if (m_inviteData.isClanInvite)
        {
            CreateClanInviteList();   
        }
        else
        {
            CreateList();
        }
     
    }

    #endregion

    #region init
    /// <summary>
    /// 邀请界面  氏族和组队用的预制不一样  只能在这个地方通过类型来加载不同的预制   
    /// </summary>
    /// <param name="type"></param>
    private void InitWidgets(bool type)
    {
        if (!type)
        {
            if (null == m_inviteCreator)
            {
                if (null != m_trans_ScrollView)
                {
                    m_inviteCreator = m_trans_ScrollView.GetComponent<UIGridCreatorBase>();
                    if (null == m_inviteCreator)
                    {
                        m_inviteCreator = m_trans_ScrollView.gameObject.AddComponent<UIGridCreatorBase>();
                        GameObject obj = UIManager.GetResGameObj(GridID.Uiteaminvitegrid) as GameObject;
                        m_inviteCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                        m_inviteCreator.gridContentOffset = new Vector2(0, 78);
                        m_inviteCreator.gridWidth = 410;
                        m_inviteCreator.gridHeight = 108;
                        m_inviteCreator.RefreshCheck();
                        m_inviteCreator.Initialize<UIInviteGrid>(obj, OnUpdateUIGrid, OnUIGridEventDlg);
                    }
                }

            }
        }
        else 
        { 
            //氏族格子用特殊的预制
            if (null == m_clanInviteCreator)
            {
                if (null != m_trans_ScrollView)
                {
                    m_clanInviteCreator = m_trans_ScrollView.GetComponent<UIGridCreatorBase>();
                    if (null == m_clanInviteCreator)
               {
                        m_clanInviteCreator = m_trans_ScrollView.gameObject.AddComponent<UIGridCreatorBase>();
                        GameObject obj = UIManager.GetResGameObj(GridID.Uiinviteclangrid) as GameObject;
                        m_clanInviteCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
                        m_clanInviteCreator.gridContentOffset = new Vector2(0, 78);
                        m_clanInviteCreator.gridWidth = 410;
                        m_clanInviteCreator.gridHeight = 108;
                        m_clanInviteCreator.RefreshCheck();
                        m_clanInviteCreator.Initialize<UIInviteGrid>(obj, OnUpdateUIGrid, OnUIGridEventDlg);
                    }
                }

            }
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    #endregion

    #region Op
    private void CreateList()
    {
        if (null != m_inviteCreator)
        {
            m_inviteCreator.CreateGrids(
                (null != m_inviteData && null != m_inviteData.inviteDatas) ? m_inviteData.inviteDatas.Count : 0);
        }
    }
    private void CreateClanInviteList()
    {
        if (null != m_clanInviteCreator)
        {
            m_clanInviteCreator.CreateGrids(
                (null != m_inviteData && null != m_inviteData.inviteDatas) ? m_inviteData.inviteDatas.Count : 0);
        }
    }
    #endregion

    #region UIEvent

    private void OnUpdateUIGrid(UIGridBase gridBase,int index)
    {
        if (null == m_inviteData 
            || null == m_inviteData.inviteDatas
            || m_inviteData.inviteDatas.Count <= index)
        {
            return;
        }
        UIInviteGrid grid = gridBase as UIInviteGrid;
        InviteData data = m_inviteData.inviteDatas[index];
        if (null != grid && null != data)
        {
            grid.SetData(data.userId, data.name, data.icon, (uint)data.lv);
        }
    }

    private void OnUIGridEventDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    UIInviteGrid grid = data as UIInviteGrid;
                    if (null != m_inviteData && null != m_inviteData.inviteBtnClickAction)
                    {
                        InviteData inviteData = m_inviteData.GetInviteData(grid.UserId);
                        if (null != data && null != param)
                        {
                            m_inviteData.inviteBtnClickAction.Invoke(inviteData);
                        }
                    }
                }
                break;
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion
}
