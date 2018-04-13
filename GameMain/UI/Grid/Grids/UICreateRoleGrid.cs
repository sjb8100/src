/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UICreateRoleGrid
 * 版本号：  V1.0.0.0
 * 创建时间：7/15/2017 1:25:08 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UICreateRoleGrid : UIGridBase
{
    #region property
    private GameCmd.enumProfession m_emPro = GameCmd.enumProfession.Profession_None;
    public GameCmd.enumProfession Pro
    {
        get
        {
            return m_emPro;
        }
    }
    private UISpriteEx m_spIcon = null;
    private Transform m_tsSelectMask = null;
    private TweenScale m_ts = null;
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        m_spIcon = CacheTransform.Find("Content/Icon").GetComponent<UISpriteEx>();
        m_ts = CacheTransform.GetComponent<TweenScale>();
        m_tsSelectMask = CacheTransform.Find("Content/SelectMask");
        SetTriggerEffect(false);
    }

    public void SetGridInfo(GameCmd.enumProfession pro)
    {
        this.m_emPro = pro;
        if (null != m_spIcon)
        {
            if (m_spIcon.mSpriteCount == 2)
            {
                m_spIcon.mSpriteList = new string[2];

                string strIcon = ChooseRolePanel.GetSpriteName(pro);
                m_spIcon.mSpriteList[0] = strIcon;
                m_spIcon.mSpriteList[1] = strIcon + "_hui";
            }
        }
        SetSelect(false, false);
    }


    /// <summary>
    ///设置选中 
    /// </summary>
    /// <param name="select"></param>
    /// <param name="needAnim"></param>
    public void SetSelect(bool select,bool needAnim = true)
    {
        if (null != m_tsSelectMask && m_tsSelectMask.gameObject.activeSelf != select)
        {
            m_tsSelectMask.gameObject.SetActive(select);
        }

        if (null != m_ts)
        {
            if (needAnim)
            {
                m_ts.Play(select);
            }
            else
            {
                m_ts.gameObject.transform.localScale = (select) ? m_ts.to : m_ts.from;
            }
        }
        

        if (null != m_spIcon)
        {
            m_spIcon.ChangeSprite((select) ? 1: 2);
        }
    }
}