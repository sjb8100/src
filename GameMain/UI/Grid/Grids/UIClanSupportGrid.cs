/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanSupportGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/23/2017 9:41:27 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UIClanSupportGrid : UIGridBase
{
    #region property
    //氏族名称
    private UILabel m_lab_ClanName;
    //族长
    private UILabel m_label_ClanShaikh;
    //剩余时间
    public UILabel m_lab_ClanLeftTime;
    //氏族数量
    private UILabel m_label_ClanNum;
    private UIToggle m_tg_hightlight;
    //标记
    private Transform m_ts_tagContent;
    //背景图
    private UISprite m_spr_bgContent;
    //id
    private UILabel m_label_idContent;
    //数据
    private ClanDefine.LocalClanInfo m_data;
    public ClanDefine.LocalClanInfo Data
    {
        get
        {
            return m_data;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_ClanName = CacheTransform.Find("Content/ClanName").GetComponent<UILabel>();
        m_label_ClanShaikh = CacheTransform.Find("Content/ClanShaikh").GetComponent<UILabel>();
        m_label_ClanNum = CacheTransform.Find("Content/ClanNum").GetComponent<UILabel>();
        m_lab_ClanLeftTime = CacheTransform.Find("Content/ClanLeftTime").GetComponent<UILabel>();
        m_ts_tagContent = CacheTransform.Find("Content/Tag");
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        m_spr_bgContent = CacheTransform.Find("Content/Toggle/Background").GetComponent<UISprite>();
        m_label_idContent = CacheTransform.Find("Content/ClanID").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
        {
            return;
        }
        m_data = (ClanDefine.LocalClanInfo)data;
        if (null != m_label_idContent)
        {
            m_label_idContent.text = m_data.Id.ToString();
        }
        if (null != m_lab_ClanName)
        {
            m_lab_ClanName.text = m_data.Name;
        }
        if (null != m_label_ClanNum)
        {
             uint max = 0;
            if(m_data.IsFormal)
            {
                 ClanMemberDataBase tab = GameTableManager.Instance.GetTableItem<ClanMemberDataBase>(m_data.Lv);
                if (tab != null)
                {
                    max =tab.memberNum;
                }
            }
            else
            {
                max =  ClanManger.TempClanSupporter;
            }
            m_label_ClanNum.text = m_data.MemberCount + "/" + max;
        }
        if (null != m_label_ClanShaikh)
        {
            GameCmd.stClanMemberInfo member = m_data.GetMemberInfo(m_data.Creator);
            m_label_ClanShaikh.text = (null != member) ? member.name : "";
        }
        m_bool_updateTimeLeft = true;
    }

    public void SetBgSprite(int index) 
    {
        if (m_spr_bgContent != null)
        {
            m_spr_bgContent.spriteName = index % 2 == 0 ? "biankuang_bantou_baikuai" : "biankuang_bantou_danlan";
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }

    public void SetTagEnable(bool tag)
    {
        if (null != m_ts_tagContent && m_ts_tagContent.gameObject.activeSelf != tag)
        {
            m_ts_tagContent.gameObject.SetActive(tag);
        }
    }

    #endregion
    #region Update
    //更新刷新间隔
    public const float REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新剩余的时间
    private float next_refresh_left_time = 0;

    private bool m_bool_updateTimeLeft = false;
    void Update()
    {
        if (m_bool_updateTimeLeft)
        {
            next_refresh_left_time -= Time.deltaTime;
            if (next_refresh_left_time <= Mathf.Epsilon)
            {
                next_refresh_left_time = REFRESH_LEFTTIME_GAP;
                if (!GetClanLeftTimeStr(out m_str_tempText))
                {
                    m_lab_ClanLeftTime.text = m_str_tempText;
                }
                else
                {
                    m_bool_updateTimeLeft = false;
                }
            }
        }

    }
    //剩余时间
    private long m_l_leftSeconds = 0;
    //temptext
    private string m_str_tempText = "";
    /// <summary>
    ///获取氏族剩余时间
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private bool GetClanLeftTimeStr(out string text)
    {
        bool isTimeOut = false;
        text = "00:00:00";
        if (null == m_data)
        {
            return true;
        }

        long nn = DateTimeHelper.Instance.Now;
        if (ScheduleDefine.ScheduleUnit.IsInCycleDateTme(
            nn
            , (m_data.CreateTime + (uint)(ClanManger.TempClanLastMinute * 60))
            , nn, out m_l_leftSeconds))
        {
            text = DateTimeHelper.ParseTimeSeconds((int)m_l_leftSeconds);
            return false;
        }
        return true;
    }
    #endregion
}
