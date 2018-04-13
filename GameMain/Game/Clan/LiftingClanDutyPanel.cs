/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  LiftingClanDutyPanel
 * 版本号：  V1.0.0.0
 * 创建时间：10/31/2016 11:45:09 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class LiftingClanDutyPanel
{

    #region LiftClanDutyDat
    public class LiftClanDutyData
    {
        //玩家id
        public uint desUserId;
        //剩余副族长位置
        public uint leftFZZ;
    }
    #endregion

    #region property
    private LiftClanDutyData m_data;
    //氏族信息
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null == data || !(data is LiftClanDutyData))
        {
            return;
        }
        m_data = data as LiftClanDutyData;
        InitPanelUI();
    }

    #endregion

    #region initpanel
    private void InitPanelUI()
    {
        if (null != m_btn_BtnAppointFZZ)
        {
            m_btn_BtnAppointFZZ.GetComponentInChildren<UILabel>().text = string.Format("副氏族长（剩余{0}）", ((null != m_data) ? m_data.leftFZZ : 0));
        }
        if (null != m_btn_BtnAppointNormal)
        {
            m_btn_BtnAppointNormal.GetComponentInChildren<UILabel>().text = "普通成员";
        }
    }
    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnCancel_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnTransferZZ_Btn(GameObject caster)
    {

        if (null != m_data)
        {
            DataManager.Manager<ClanManger>().TransferClan(m_data.desUserId);
        }
        HideSelf();
    }

    void onClick_BtnAppointFZZ_Btn(GameObject caster)
    {

        if (null != m_data)
        {
            DataManager.Manager<ClanManger>().ChangeDuty(m_data.desUserId,GameCmd.enumClanDuty.CD_Deputy);
        }

        HideSelf();
    }

    void onClick_BtnAppointNormal_Btn(GameObject caster)
    {
       
        if (null != m_data)
        {
            DataManager.Manager<ClanManger>().ChangeDuty(m_data.desUserId, GameCmd.enumClanDuty.CD_Member);
        }
        HideSelf();
    }

    #endregion
}