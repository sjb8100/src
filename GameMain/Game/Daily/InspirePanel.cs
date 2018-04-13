/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Daily
 * 创建人：  wenjunhua.zqgame
 * 文件名：  InspirePanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/26/2018 11:50:45 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class InspirePanel
{
    #region property
    private JvBaoBossWorldManager m_mgr = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RefreshInspireData(GameCmd.InspireType.InspireType_Coin);
        RefreshInspireData(GameCmd.InspireType.InspireType_Money);
        RegisterGlobalEvent(true);
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (null != m_goldAsynSeed)
        {
            m_goldAsynSeed.Release();
            m_goldAsynSeed = null;
        }

        if (null != m_BYuanAsynSeed)
        {
            m_BYuanAsynSeed.Release();
            m_BYuanAsynSeed = null;
        }
        RegisterGlobalEvent(false);
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    #endregion

    #region OP
    private void InitWidgets()
    {
        m_mgr = DataManager.Manager<JvBaoBossWorldManager>();
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventId, object obj)
    {
        switch (eventId)
        {
            case (int)Client.GameEventID.UIEVENT_WORLDBOSSINSPIREREFRESH:
                {
                    if (null != obj && obj is GameCmd.InspireType)
                    {
                        RefreshInspireData((GameCmd.InspireType)obj);
                    }
                }
                break;
        }
    }
    private CMResAsynSeedData<CMAtlas> m_goldAsynSeed = null;
    private CMResAsynSeedData<CMAtlas> m_BYuanAsynSeed = null;
    /// <summary>
    /// 刷新
    /// </summary>
    /// <param name="inspireType"></param>
    private void RefreshInspireData(GameCmd.InspireType inspireType)
    {
        string tempText = "";
        string costTypeIcon = "";
        int count = 0;
        JvBaoBossWorldManager.LocalInspireData localData = null;
        string leftText = "";
        bool inpireEnable = false;
        if (m_mgr.TryGetInspirePlayerData(inspireType, out localData))
        {
            if (localData.LeftTimes > 0)
            {
                leftText = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_InspireLeftTimesEnough
                    , localData.LeftTimes, localData.MaxTimes);
            }
            else
            {
                leftText = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_InspireLeftTimesNotEnough
                    , localData.LeftTimes, localData.MaxTimes);
            }
            count = (int)localData.MaxTimes - (int)localData.LeftTimes;
            if (localData.LeftTimes != 0)
            {
                inpireEnable = true;
            }
            if (count == 0)
            {
                count = 1;
            }
        }
        
        table.InspireDataBase db = null;
        if (m_mgr.TryGetInspireDB(inspireType,count, out db))
        {
            tempText = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HuntingBoss_InspireFormatTxt
                    , db.addBufferDes);
            costTypeIcon = MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)db.costType);
        }

        if (inspireType == GameCmd.InspireType.InspireType_Coin)
        {
            if (null != m_label_GoldInspEffect)
            {
                m_label_GoldInspEffect.text = tempText;
            }

            if (null != m_label_GoldInspLeft)
            {
                m_label_GoldInspLeft.text = leftText;
            }
            if (null != db)
            {
                if (null != m_label_GoldInspireCostNum)
                {
                    m_label_GoldInspireCostNum.text = db.costNum.ToString();
                }

                if (null != m_sprite_GoldInspireCostIcon && !string.IsNullOrEmpty(costTypeIcon))
                {
                    UIManager.GetAtlasAsyn(costTypeIcon
                        , ref m_goldAsynSeed, () =>
                        {
                            if (null != m_sprite_GoldInspireCostIcon)
                            {
                                m_sprite_GoldInspireCostIcon.atlas = null;
                            }
                        }, m_sprite_GoldInspireCostIcon);
                }
            }

            if (null != m_btn_BtnGoldInspire && m_btn_BtnGoldInspire.isEnabled != inpireEnable)
                m_btn_BtnGoldInspire.isEnabled = inpireEnable;
            
        }else if (inspireType == GameCmd.InspireType.InspireType_Money)
        {
            if (null != m_label_BYuanInspEffect)
            {
                m_label_BYuanInspEffect.text = tempText;
            }

            if (null != m_label_BYuanInspLeft)
            {
                m_label_BYuanInspLeft.text = leftText;
            }

            if (null != db && null != m_label_BYuanInspireCostNum)
            {
                m_label_BYuanInspireCostNum.text = db.costNum.ToString();
            }

            if (null != m_sprite_BYuanInspireCostIcon && !string.IsNullOrEmpty(costTypeIcon))
            {
                UIManager.GetAtlasAsyn(costTypeIcon, ref m_BYuanAsynSeed, () =>
                {
                    if (null != m_sprite_BYuanInspireCostIcon)
                    {
                        m_sprite_BYuanInspireCostIcon.atlas = null;
                    }
                }, m_sprite_BYuanInspireCostIcon);
            }

            if (null != m_btn_BtnBYuanInspire && m_btn_BtnBYuanInspire.isEnabled != inpireEnable)
                m_btn_BtnBYuanInspire.isEnabled = inpireEnable;
        }
    }
    #endregion

    #region nguibtns
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnGoldInspire_Btn(GameObject caster)
    {
        m_mgr.CoinInspire();
    }

    void onClick_BtnBYuanInspire_Btn(GameObject caster)
    {
        m_mgr.YuanBaoInspire();
    }
    #endregion
}