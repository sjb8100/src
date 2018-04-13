/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/19/2016 11:44:32 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;

class UIClanGrid : UIGridBase
{
    #region property
    //id
    private UILabel idLabel;
    //氏族名称
    private UILabel m_lab_ClanName;
    //族长
    private UILabel m_label_ClanShaikh;
    //氏族数量
    private UILabel m_label_ClanNum;
    private UILabel m_lab_lv;
    private UIToggle m_tg_hightlight;
    //标记
    private Transform m_ts_tagContent;
    //背景图
    private UISprite m_spr_bgContent;
    //id
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
        idLabel = CacheTransform.Find("Content/ClanID").GetComponent<UILabel>();
        m_lab_ClanName = CacheTransform.Find("Content/ClanName").GetComponent<UILabel>();
        m_label_ClanShaikh = CacheTransform.Find("Content/ClanShaikh").GetComponent<UILabel>();
        m_label_ClanNum = CacheTransform.Find("Content/ClanNum").GetComponent<UILabel>();
        m_lab_lv = CacheTransform.Find("Content/ClanLv").GetComponent<UILabel>();
        m_ts_tagContent = CacheTransform.Find("Content/Tag");
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        m_spr_bgContent = CacheTransform.Find("Content/Toggle/Background").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
        {
            return;
        }
        m_data = (ClanDefine.LocalClanInfo)data;
        if (null != idLabel)
        {
            idLabel.text = m_data.Id.ToString();
        }
        if (null != m_lab_ClanName)
        {
            m_lab_ClanName.text = m_data.Name;
        }
        if (null != m_lab_lv)
        {
            m_lab_lv.text = m_data.Lv.ToString();
        }
        if (null != m_label_ClanNum)
        {
            uint max = 0;
            if (m_data.IsFormal)
            {
                ClanMemberDataBase tab = GameTableManager.Instance.GetTableItem<ClanMemberDataBase>(m_data.Lv);
                if (tab != null)
                {
                    max = tab.memberNum;
                }
            }
            else
            {
                max = ClanManger.TempClanSupporter;
            }
            m_label_ClanNum.text = m_data.MemberCount + "/" + max;         
        }
        if (null != m_label_ClanShaikh)
        {
            GameCmd.stClanMemberInfo member = m_data.GetMemberInfo(m_data.ShaikhId);
            m_label_ClanShaikh.text = (null != member) ? member.name : "";
        }
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
}