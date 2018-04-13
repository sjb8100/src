/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIWeaponSoulInfoSelectGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/20/2017 3:08:00 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIWeaponSoulInfoSelectGrid : UIItemInfoGridBase
{
    #region Property
    //装备名称
    private UILabel m_lab_name;
    private UILabel Lv;
    private UIToggle m_toggle;

    private Transform mtsHightlight = null;

    private uint qwThisId = 0;
    public uint QWThisId
    {
        get
        {
            return qwThisId;
        }
    }
    //不可用背景
    private Transform mtsDisableBg = null;
    private bool mbEnable = false;

    #endregion
    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        Lv = CacheTransform.Find("Content/Level").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        mtsDisableBg = CacheTransform.Find("Content/Disable");
        mtsHightlight = CacheTransform.Find("Content/Hightlight");
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="check"></param>
    /// <param name="enable"></param>
    public void SetGridViewData(uint qwThisId, bool check, bool enable,bool blendStyle = false)
    {
        this.qwThisId = qwThisId;
        Muhon baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(qwThisId);
        mbEnable = enable;
        SetHightLight(check);
        if (null != baseItem)
        {
            if (null != m_lab_name)
            {
                m_lab_name.text = baseItem.Name;
            }

            if (null != Lv)
            {
                Lv.text = ColorManager.GetColorString(ColorType.JZRY_Txt_Black
                    , DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_Txt_Set_4, baseItem.Level));
            }
            bool isMatchJob = DataManager.IsMatchPalyerJob((int)baseItem.BaseData.useRole);
            bool fightPowerUp = false;
            if (isMatchJob
                && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.QWThisID, out fightPowerUp))
            {
                SetFightPower(true, fightPowerUp);
            }
            if (!DataManager.IsMatchPalyerLv((int)baseItem.BaseData.useLevel)
                || !isMatchJob)
            {
                SetWarningMask(true);
            }
            SetMuhonMask(true, baseItem.StartLevel);
            SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            SetBindMask(baseItem.IsBind);


            SetNum(false);
            SetTimeLimitMask(false);
            SetWarningMask(false);
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        bool enableHightLight = mbEnable && hightLight;
        if (null != m_toggle)
        {
            if (m_toggle.value != enableHightLight)
            {
                m_toggle.value = enableHightLight;
            }
        }

        if (null != mtsDisableBg && mtsDisableBg.gameObject.activeSelf == mbEnable)
        {
            mtsDisableBg.gameObject.SetActive(!mbEnable);
        }

        if (null != mtsHightlight && mtsHightlight.gameObject.activeSelf != hightLight)
        {
            mtsHightlight.gameObject.SetActive(hightLight);
        }
    }
    public override void Reset()
    {
        base.Reset();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
    }
    #endregion

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    base.InfoGridUIEventDlg(eventType, this, param);
                    if (data is UIItemInfoGrid && qwThisId != 0)
                    {
                        TipsManager.Instance.ShowItemTips(qwThisId);
                    }
                }
                break;
        }

    }
    #endregion
}