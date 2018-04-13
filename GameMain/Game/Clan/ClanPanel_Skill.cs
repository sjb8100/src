/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanPanel_Skill
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:32:54 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanPanel
{
    #region define
    public enum ClanSkillMode
    {
        None,
        Learn,
        Dev,
    }
    #endregion

    #region property
    //当前技能面板模式
    private ClanSkillMode m_em_skillMode = ClanSkillMode.None;
    //氏族技能id本地数据
    private List<uint> m_list_clanSkillIds = new List<uint>();
    //技能tab缓存
    private Dictionary<ClanSkillMode, UITabGrid> m_dic_skillTabs = new Dictionary<ClanSkillMode, UITabGrid>();
    //当前选中技能id
    private uint m_uint_SelectSkillId = 0;
    #endregion

    #region init
    private void InitSkill()
    {
        if (IsInitMode(ClanPanelMode.Skill))
        {
            return;
        }
        SetInitMode(ClanPanelMode.Skill);
        if (null != m_ctor_SkillScrollView)
        {
            m_ctor_SkillScrollView.RefreshCheck();
            m_ctor_SkillScrollView.Initialize<UIClanSkillGrid>(m_trans_UIClanSkillGrid.gameObject, OnUpdateUIGrid, OnUIGridEventDlg);
        }

        UITabGrid tabGrid = null;
        if (null != m_trans_TabSkillLearn)
        {
            tabGrid = m_trans_TabSkillLearn.GetComponent<UITabGrid>();
            if (null == tabGrid)
            {
                tabGrid = m_trans_TabSkillLearn.gameObject.AddComponent<UITabGrid>();
                tabGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        SetSkillMode(ClanSkillMode.Learn);
                    }
                });
                m_dic_skillTabs.Add(ClanSkillMode.Learn, tabGrid);
            }
        }
        if (null != m_trans_TabSkillDev)
        {
            tabGrid = m_trans_TabSkillDev.GetComponent<UITabGrid>();
            if (null == tabGrid)
            {
                tabGrid = m_trans_TabSkillDev.gameObject.AddComponent<UITabGrid>();
                tabGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        SetSkillMode(ClanSkillMode.Dev);
                    }
                });
                m_dic_skillTabs.Add(ClanSkillMode.Dev, tabGrid);
            }
        }

        UICurrencyGrid cGrid = null;
        if (null != m_trans_SkillCost1)
        {
            cGrid = m_trans_SkillCost1.GetComponent<UICurrencyGrid>();
            if (null == cGrid)
            {
                m_trans_SkillCost1.gameObject.AddComponent<UICurrencyGrid>();
            }
        }

        if (null != m_trans_SkillCost2)
        {
            cGrid = m_trans_SkillCost2.GetComponent<UICurrencyGrid>();
            if (null == cGrid)
            {
                m_trans_SkillCost2.gameObject.AddComponent<UICurrencyGrid>();
            }
        }
    }

    private void BuildSkill()
    {
        if (!IsInitMode(ClanPanelMode.Skill))
        {
            return;
        }
        SetSkillMode(ClanSkillMode.Learn);
    }

    #endregion

    #region Op
    
    /// <summary>
    /// 当前技能模式是否为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool IsSkillMode(ClanSkillMode mode)
    {
        return m_em_skillMode == mode;
    }
    /// <summary>
    /// 技能改变
    /// </summary>
    /// <param name="skillId"></param>
    private void OnSkillChanged(uint skillId)
    {
        if (null != m_ctor_SkillScrollView
            && null != m_list_clanSkillIds
            && m_list_clanSkillIds.Contains(skillId))
        {
            m_ctor_SkillScrollView.UpdateData(m_list_clanSkillIds.IndexOf(skillId));
            if (m_uint_SelectSkillId == skillId)
            {
                UpdateSkill();
            }
        }
    }

    /// <summary>
    /// 构造氏族技能数据
    /// </summary>
    private void StructClanSkillsData()
    {
        m_list_clanSkillIds.Clear();
        m_list_clanSkillIds.AddRange(m_mgr.GetClanSkillDatas());
    }

    /// <summary>
    /// 构造氏族技能
    /// </summary>
    private void BuildClanSkillList()
    {
        if (null != m_ctor_SkillScrollView)
        {
            m_ctor_SkillScrollView.CreateGrids(m_list_clanSkillIds.Count);
            if (m_list_clanSkillIds.Count > 0)
            {
                SetSelectSkillId(m_list_clanSkillIds[0]);
            }else
            {
                m_uint_SelectSkillId = 0;
            }
        }
    }

    /// <summary>
    /// 设置选中技能id
    /// </summary>
    /// <param name="id"></param>
    private void SetSelectSkillId(uint id)
    {
        if (m_uint_SelectSkillId != id && null != m_ctor_SkillScrollView)
        {
            UIClanSkillGrid csGrid = m_list_clanSkillIds.Contains(m_uint_SelectSkillId)
                    ? m_ctor_SkillScrollView.GetGrid<UIClanSkillGrid>(m_list_clanSkillIds.IndexOf(m_uint_SelectSkillId)) : null;
            if (null != csGrid)
            {
                csGrid.SetHightLight(false);
            }
            m_uint_SelectSkillId = id;
            csGrid = m_list_clanSkillIds.Contains(m_uint_SelectSkillId)
                ? m_ctor_SkillScrollView.GetGrid<UIClanSkillGrid>(m_list_clanSkillIds.IndexOf(m_uint_SelectSkillId)) : null;
            if (null != csGrid)
            {
                csGrid.SetHightLight(true);
            }
        }
        UpdateSkill();
    }

    /// <summary>
    /// 设置模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetSkillMode(ClanSkillMode mode)
    {
        if (mode != m_em_skillMode)
        {
            UITabGrid tab = (m_dic_skillTabs.ContainsKey(m_em_skillMode)
                ? m_dic_skillTabs[m_em_skillMode] : null);
            if (null != tab)
            {
                tab.SetHightLight(false);
            }
            m_em_skillMode = mode;
            tab = (m_dic_skillTabs.ContainsKey(m_em_skillMode)
                ? m_dic_skillTabs[m_em_skillMode] : null);
            if (null != tab)
            {
                tab.SetHightLight(true);
            }
            StructClanSkillsData();
            BuildClanSkillList();
            UpdateSkill();
        }
    }
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed = null;
    /// <summary>
    /// 更新技能信息
    /// </summary>
    private void UpdateSkill()
    {
        if (!IsInitMode(ClanPanelMode.Skill))
        {
            return;
        }

        TextManager tmgr = DataManager.Manager<TextManager>();
        if (null != m_ctor_SkillScrollView)
        {
            m_ctor_SkillScrollView.UpdateActiveGridData();
        }
        bool isLearn = IsSkillMode(ClanSkillMode.Learn);
       
        //当前等级
        uint curLv = isLearn ? m_mgr.GetClanSkillLearnLv(m_uint_SelectSkillId)
            : m_mgr.GetClanSkillDevLv(m_uint_SelectSkillId);

        table.SkillDatabase skDBCur
            = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(m_uint_SelectSkillId, (int)curLv);
        //技能最大等级
        uint maxLv = (null != skDBCur) ? skDBCur.dwMaxLevel : 1;
        //最大等级
        uint limitLv = isLearn ? m_mgr.GetClanSkillDevLv(m_uint_SelectSkillId)
            : ((null != skDBCur) ? skDBCur.dwMaxLevel : 0);
        bool isMaxLv = (curLv == maxLv);

        table.SkillDatabase skDBNext
            = (!isMaxLv) ? GameTableManager.Instance.GetTableItem<table.SkillDatabase>(m_uint_SelectSkillId, (int)curLv + 1) : null;
        //名称
        if (null != m_label_SkillName)
        {
            m_label_SkillName.text = (null != skDBCur) ? skDBCur.strName : "";
        }

        //等级
        if (null != m_label_SkillLv)
        {

            m_label_SkillLv.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_FM_Lv, "", curLv);
        }
        
        //技能效果
        if (null != m_label_SkillEffectCur)
        {
            m_label_SkillEffectCur.text = (null != skDBCur) ? skDBCur.strDesc : "";
        }
        //技能下一级效果
        if (null != m_label_SkillEffectNext)
        {
            if (m_label_SkillEffectNext.gameObject.activeSelf == isMaxLv)
            {
                m_label_SkillEffectNext.gameObject.SetActive(!isMaxLv);
            }
            if (!isMaxLv)
            {
                m_label_SkillEffectNext.text = ColorManager.GetColorString(ColorType.Green
                    , (null != skDBNext) ? skDBNext.strDesc : "");
            }
        }
        //tips
        if (null != m_label_SkillTips)
        {
            string tips = "";
            if (isMaxLv)
            {
                tips = ColorManager.GetColorString(ColorType.Red, tmgr.GetLocalText(LocalTextType.Local_TXT_UpgradeMax));
            }
            else
            {
                string name = (isLearn) ? tmgr.GetLocalText(LocalTextType.Local_TXT_Learn) 
                    : tmgr.GetLocalText(LocalTextType.Local_TXT_Dev);
                string who = (isLearn) ? tmgr.GetLocalText(LocalTextType.Local_TXT_Person)
                    : tmgr.GetLocalText(LocalTextType.Local_TXT_Clan);
                table.ClanSkillDataBase sdbNext =
                        GameTableManager.Instance.GetTableItem<table.ClanSkillDataBase>(m_uint_SelectSkillId, (int)curLv + 1);
                ColorType cType = ColorType.Green;
                if ((isLearn && null != skDBNext && DataManager.Instance.PlayerLv < skDBNext.dwNeedLevel)
                    || (!isLearn && null != sdbNext && null != ClanInfo && ClanInfo.Lv < sdbNext.needClanLv))
                {
                    cType = ColorType.Red;
                }
                uint lvuint = 0;
                if (isLearn)
                {
                    lvuint = (null != skDBNext) ? skDBNext.dwNeedLevel : 0;
                }
                else
                {
                    lvuint = (null != sdbNext) ? sdbNext.needClanLv : 0;
                }
                string lvStr = ColorManager.GetColorString(cType, tmgr.GetLocalFormatText(LocalTextType.Local_TXT_FM_Lv, who, lvuint));
                
                tips = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_NextLvNeed, name, lvStr);
            }
            m_label_SkillTips.text = tips;
        }

        if(null != m_trans_Cost && m_trans_Cost.gameObject.activeSelf == isMaxLv)
        {
            m_trans_Cost.gameObject.SetActive(!isMaxLv);
        }
        if (!isMaxLv)
        {
            //消耗
            if (null != m_trans_SkillCost1 && null != m_trans_SkillCost2)
            {
                UICurrencyGrid cGrid1 = null;
                UICurrencyGrid cGrid2 = null;
                cGrid1 = m_trans_SkillCost1.GetComponent<UICurrencyGrid>();
                cGrid2 = m_trans_SkillCost2.GetComponent<UICurrencyGrid>();
                UICurrencyGrid.UICurrencyGridData d1 =
                    new UICurrencyGrid.UICurrencyGridData(ClanDefine.CONST_ICON_ZG_NAME, 0);
                UICurrencyGrid.UICurrencyGridData d2 =
                    new UICurrencyGrid.UICurrencyGridData(ClanDefine.CONST_ICON_ZG_NAME, 0);
                if (isLearn)
                {
                    d1 = new UICurrencyGrid.UICurrencyGridData(
                        MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold), skDBNext.dwMoney);
                    d2 = new UICurrencyGrid.UICurrencyGridData(
                        MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Reputation), skDBNext.dwNeedSW);

                    m_trans_SkillCost2.gameObject.SetActive(true);

                    m_label_SkillRemainNum.text = UserData.Reputation.ToString();
                    UIManager.GetAtlasAsyn(MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Reputation), ref m_priceAsynSeed, () =>
                    {
                        if (null != m_sprite_SkillRemainIcon)
                        {
                            m_sprite_SkillRemainIcon.atlas = null;
                        }
                    }, m_sprite_SkillRemainIcon);     
                }
                else
                {
                    table.ClanSkillDataBase cskdbNext
                        = GameTableManager.Instance.GetTableItem<table.ClanSkillDataBase>(m_uint_SelectSkillId, (int)curLv + 1);
                    if (null != cskdbNext)
                    {
                        d1 = new UICurrencyGrid.UICurrencyGridData(ClanDefine.CONST_ICON_ZJ_NAME, cskdbNext.coseZJ);
                        //d2 = new UICurrencyGrid.UICurrencyGridData(ClanDefine.CONST_ICON_ZJ_NAME, cskdbNext.coseZJ);
                    }
                    m_trans_SkillCost2.gameObject.SetActive(false);
                    m_label_SkillRemainNum.text = DataManager.Manager<ClanManger>().ClanInfo.Money.ToString();

                    UIManager.GetAtlasAsyn(ClanDefine.CONST_ICON_ZJ_NAME, ref m_priceAsynSeed, () =>
                    {
                        if (null != m_sprite_SkillRemainIcon)
                        {
                            m_sprite_SkillRemainIcon.atlas = null;
                        }
                    }, m_sprite_SkillRemainIcon);
                }
                if (null != cGrid1)
                {
                    cGrid1.SetGridData(d1);
                }
                if (null != cGrid2)
                {
                    cGrid2.SetGridData(d2);
                }

                bool canSee = IsSkillMode(ClanSkillMode.Learn);
                if (null != m_btn_BtnLearn && m_btn_BtnLearn.gameObject.activeSelf != canSee)
                {
                    m_btn_BtnLearn.gameObject.SetActive(canSee);
                }
                canSee = !canSee;
                if (null != m_btn_BtnDev && m_btn_BtnDev.gameObject.activeSelf != canSee)
                {
                    m_btn_BtnDev.gameObject.SetActive(canSee);
                }
            }
        }
    }
    #endregion

    #region UIEvent
    void onClick_BtnLearn_Btn(GameObject caster)
    {
        m_mgr.LearnClanSkill(m_uint_SelectSkillId);
    }

    void onClick_BtnDev_Btn(GameObject caster)
    {
        m_mgr.DevClanSkill(m_uint_SelectSkillId);
    }
    #endregion
}