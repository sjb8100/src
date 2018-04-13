/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanCreatePanel_Support
 * 版本号：  V1.0.0.0
 * 创建时间：2/22/2017 3:38:17 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanCreatePanel
{
    #region property
    #endregion

    #region init
    private void InitSupport()
    {
        if (IsInitMode(ClanCreateMode.Support))
        {
            return;
        }
        SetInitMode(ClanCreateMode.Support);
        if (null == m_lst_clanTempInfos)
        {
            m_lst_clanTempInfos = new List<ClanDefine.LocalClanInfo>();
        }
        if (null != m_ctor_ClanSupportScrollView)
        {
            m_ctor_ClanSupportScrollView.Initialize<UIClanSupportGrid>(m_trans_UIClanSupportGrid.gameObject, OnUIGridUpdate, OnUIGridEventDlg);
        }

        if (null != m_input_SupportSearchInput)
        {
//            m_input_SupportSearchInput.defaultColor = Color.gray;
            m_input_SupportSearchInput.onChange.Add(new EventDelegate(() =>
            {
                m_str_inputSeachClanInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_SupportSearchInput.value
                    , TextManager.CONST_NAME_MAX_WORDS);
                m_input_SupportSearchInput.value = m_str_inputSeachClanInfo;
            }));
            m_input_ClanNameInput.onSubmit.Add(new EventDelegate(() =>
            {
                m_str_inputSeachClanInfo = TextManager.GetTextByWordsCountLimitInUnicode(m_input_SupportSearchInput.value
                     , TextManager.CONST_NAME_MAX_WORDS);
                m_input_SupportSearchInput.value = m_str_inputSeachClanInfo;
            }));
        }
    }
    /// <summary>
    /// 设置当前选中的氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void SetSelectSupportClan(ClanDefine.LocalClanInfo selectInfo)
    {
        if (selectInfo.Id == m_uint_selectId || null == m_ctor_ClanSupportScrollView)
        {
            return;
        }
        //刷新数据
        UIClanSupportGrid grid = m_ctor_ClanSupportScrollView.GetGrid<UIClanSupportGrid>(m_lst_clanTempInfos.IndexOf(ClanData));
        if (null != grid)
        {
            grid.SetHightLight(false);
        }
        grid = m_ctor_ClanSupportScrollView.GetGrid<UIClanSupportGrid>(m_lst_clanTempInfos.IndexOf(selectInfo));
        if (null != grid)
        {
            grid.SetHightLight(true);
        }
        m_uint_selectId = selectInfo.Id;
        UpdateSupport();
    }
    private void UpdateSupport()
    {
        //是否自己是族长   如果是  隐藏支持氏族和联系组长的按钮
        bool value = DataManager.Manager<ClanManger>().IsClanCreatorSelf;
        m_btn_BtnSupport.transform.parent.gameObject.SetActive(!value);      
        bool isSupport = !m_mgr.IsJoinClan || (null != ClanData && ClanData.Id != m_mgr.ClanId);
       
        if (null != m_btn_BtnSupport && m_btn_BtnSupport.gameObject.activeSelf != isSupport)
        {
            m_btn_BtnSupport.gameObject.SetActive(isSupport);
        }

        if (null != m_btn_BtnSupportCancel && m_btn_BtnSupportCancel.gameObject.activeSelf == isSupport)
        {
            m_btn_BtnSupportCancel.gameObject.SetActive(!isSupport);
        }
        UpdateSupportGG();
    }

    private void UpdateSupportGG()
    {
        if (null != m_label_SupportGGInfo)
        {
            m_label_SupportGGInfo.text = (null != ClanData) ? ClanData.GG : "";
        }
    }



    #endregion

    #region UIEvent
    void onClick_BtnSupportSearch_Btn(GameObject caster)
    {
        SearchClan();
    }

    void onClick_BtnSupportContactShaikh_Btn(GameObject caster)
    {
        ContactShaikh();
    }

    void onClick_BtnSupport_Btn(GameObject caster)
    {
        SupportClan();
    }

    void onClick_BtnSupportCancel_Btn(GameObject caster)
    {
        CancelSupportClan();
    }
    #endregion
}

