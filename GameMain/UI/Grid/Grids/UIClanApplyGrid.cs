/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanApplyGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/24/2016 4:22:21 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIClanApplyGrid : UIGridBase
{
    #region property
    //职业
    private UISprite m_sp_prof;
    //名称
    private UILabel m_lab_name;
    //等级
    private UILabel m_lab_lv;
    //战斗力
    private UILabel m_lab_fight;
    //玩家id
    private uint m_uint_userId;
    //同意
    private GameObject m_obj_agree;
    //拒绝
    private GameObject m_obj_refuse;
    public uint UserId
    {
        get
        {
            return m_uint_userId;
        }
    }

    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_sp_prof = CacheTransform.Find("Content/Prof").GetComponent<UISprite>();
        m_lab_lv = CacheTransform.Find("Content/Lv").GetComponent<UILabel>();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_fight = CacheTransform.Find("Content/Fight").GetComponent<UILabel>();

        m_obj_agree = CacheTransform.Find("Content/BtnAgree").gameObject;
        if (null != m_obj_agree)
        {
            UIEventListener.Get(m_obj_agree).onClick = (obj) =>
            {
                OnDealApply(true);
            };
        }
        m_obj_refuse = CacheTransform.Find("Content/BtnRefuse").gameObject;
        if (null != m_obj_refuse)
        {
            UIEventListener.Get(m_obj_refuse).onClick = (obj) =>
            {
                OnDealApply(false);
            };
        }
        
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is GameCmd.stRequestListClanUserCmd_S.Data))
        {
            Engine.Utility.Log.Error("UIClanApplyGrid SetGridData failed,data error");
            return;
        }
        GameCmd.stRequestListClanUserCmd_S.Data tempData = data as GameCmd.stRequestListClanUserCmd_S.Data;
        m_uint_userId = tempData.id;
        if (null != m_sp_prof)
        {
            m_sp_prof.spriteName = DataManager.GetIconByProfession(tempData.jov);
        }
        if (null != m_lab_name)
        {
            m_lab_name.text = tempData.name + "";
        }
        if (null != m_lab_lv)
        {
            m_lab_lv.text = tempData.level + "";
        }
        if (null != m_lab_fight)
        {
            m_lab_fight.text =tempData.fight+ "";
        }

    }
    #endregion

    #region event
    public void OnDealApply(bool agree)
    {
        InvokeUIDlg(UIEventType.Click, this, agree);
    }
    #endregion
}