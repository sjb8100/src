/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.FB
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UICampSignupGrid
 * 版本号：  V1.0.0.0
 * 创建时间：6/30/2017 10:03:05 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class UICampSignupGrid
{
    #region property
    GameCmd.eCFState m_SignStatus = GameCmd.eCFState.CFS_Close;
    bool m_bsign;
    uint m_nIndex = 0;
    public uint Index { get { return m_nIndex; } }
    #endregion

    #region overridemethod
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        LocalCampSignInfo info = data as LocalCampSignInfo;
        m_nIndex = info.Index;
        m_label_WarSequence.text = info.Index.ToString();
        m_label_ApplyTime.text = info.SignTime;
        m_label_StartTime.text = info.StartTime;
        m_label_EndTime.text = info.EndTime;
        RefreshNum(info.GetSignNumByCampSection(DataManager.Manager<CampCombatManager>().CampSectionIndex));
        RefreshStatus(info.State, info.Sign);
    }
    #endregion

    public void RefreshNum(uint num)
    {
        m_label_PeopleNum.text = num.ToString();
    }

    public void RefreshStatus(GameCmd.eCFState status, bool bSign)
    {
        m_SignStatus = status;
        m_bsign = bSign;
        //未报名 
        bool visible = (status == GameCmd.eCFState.CFS_Sign && !bSign);
        if (m_btn_BtnSign.gameObject.activeSelf != visible)
        {
            m_btn_BtnSign.gameObject.SetActive(visible);
        }
        //已报名 
        visible = (status == GameCmd.eCFState.CFS_Sign && bSign);
        if (m_btn_BtnCancleSign.gameObject.activeSelf != visible)
        {
            m_btn_BtnCancleSign.gameObject.SetActive(visible);
        }
        //准备开始 
        visible = (status == GameCmd.eCFState.CFS_Ready);
        if (m_btn_BtnReady.gameObject.activeSelf != visible)
        {
            m_btn_BtnReady.gameObject.SetActive(visible);
        }
        //正在进行
        visible = (status == GameCmd.eCFState.CFS_Fighting);
        if (m_btn_BtnStart.gameObject.activeSelf != visible)
        {
            m_btn_BtnStart.gameObject.SetActive(visible);
        }
        //未开始 
        visible = (status == GameCmd.eCFState.CFS_Close);
        if (m_btn_BtnNotStart.gameObject.activeSelf != visible)
        {
            m_btn_BtnNotStart.gameObject.SetActive(visible);
        }
        //结束
        visible = (status == GameCmd.eCFState.CFS_End);
        if (m_btn_BtnOver.gameObject.activeSelf != visible)
        {
            m_btn_BtnOver.gameObject.SetActive(visible);
        }
    }

    void onClick_BtnOver_Btn(GameObject caster)
    {
        
    }

    void onClick_BtnStart_Btn(GameObject caster)
    {
        
    }

    void onClick_BtnSign_Btn(GameObject caster)
    {
        DataManager.Manager<CampCombatManager>().DoCampSignOp((uint)m_nIndex, m_bsign);
    }

    void onClick_BtnNotStart_Btn(GameObject caster)
    {
        
    }

    void onClick_BtnCancleSign_Btn(GameObject caster)
    {
        DataManager.Manager<CampCombatManager>().DoCampSignOp((uint)m_nIndex, m_bsign);
    }

    void onClick_BtnReady_Btn(GameObject caster)
    {
        
    }
}