/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanDeclareWarCostPanel
 * 版本号：  V1.0.0.0
 * 创建时间：12/23/2016 1:57:16 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ClanDeclareWarCostPanel
{
    #region define
    public class ClanDeclareWarCostData
    {
        public uint ClanId = 0;
        public string ClanName = "";
    }
    #endregion

    #region property
    private ClanDeclareWarCostData m_data;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is ClanDeclareWarCostData)
        {
            this.m_data = data as ClanDeclareWarCostData;
            if (null != m_label_DeclareWarTarget)
            {
                
                m_label_DeclareWarTarget.text = string.Format("确定对{0}氏族宣战？"
                    , ColorManager.GetColorString(ColorType.Red, m_data.ClanName));
            }
            if (null != m_label_DeclareWarDur)
            {
                m_label_DeclareWarDur.text = string.Format("宣战将会持续{0}小时且不可取消"
                    , ClanManger.ClanDeclareWarDur);
            }
            if (null != m_label_ZijinCost)
            {
                m_label_ZijinCost.text = ClanManger.ClanDeclareWarNeedZJ.ToString();
            }
            if (null != m_label_ZugongCost)
            {
                m_label_ZugongCost.text = ClanManger.ClanDeclareWarNeedZG.ToString();
            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
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

    void onClick_BtnDeclareWar_Btn(GameObject caster)
    {
        if (null == m_data)
        {
            TipsManager.Instance.ShowTips("数据错误");
            return;
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANSTARTDECLAREWAR);
        DataManager.Manager<ClanManger>().StartDeclareWar(m_data.ClanId);

        HideSelf();
    }
    #endregion
}