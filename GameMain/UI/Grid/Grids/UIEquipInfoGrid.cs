using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIEquipInfoGrid : UIItemInfoGridBase
{
    #region Property
    //装备名称
    private UILabel name;
    private UILabel power;
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
    private Transform mtsLock = null;
    private bool mbGrowEnable = false;
    private bool mbCondiEnable = false;
    private Transform mtsPowerContent = null;
    private Transform mtsPowerUp = null;
    private Transform mtsPowerDown = null;
    #endregion
    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        power = CacheTransform.Find("Content/FightPower").GetComponent<UILabel>();
        mtsPowerContent = CacheTransform.Find("Content/PowerMaskContent");
        mtsPowerUp = CacheTransform.Find("Content/PowerMaskContent/PowerUp");
        mtsPowerDown = CacheTransform.Find("Content/PowerMaskContent/PowerDown");
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        mtsDisableBg = CacheTransform.Find("Content/Disable");
        mtsLock = CacheTransform.Find("Content/Lock");
        mtsHightlight = CacheTransform.Find("Content/Hightlight");
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="check"></param>
    /// <param name="enable"></param>
    public void SetGridViewData(uint qwThisId,bool check,bool foringGrowEnable,bool condiEnable)
    {
 	    this.qwThisId = qwThisId;
        Equip baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
        mbGrowEnable = foringGrowEnable;
        mbCondiEnable = condiEnable;
        SetHightLight(check);
        if (null != baseItem)
        {
            ResetInfoGrid();
            if (null != name)
            {
                name.text = baseItem.Name;
            }

            if (null != power)
            {
                power.text = string.Format("战斗力 {0}", baseItem.Power);
            }
            bool isMatchJob = DataManager.IsMatchPalyerJob((int)baseItem.BaseData.useRole);
            bool fightPowerUp = false;
            bool needShowPower = false;
            if (DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.QWThisID, out fightPowerUp))
            {
                needShowPower = true;
            }
            if (null != mtsPowerContent && mtsPowerContent.gameObject.activeSelf != needShowPower)
            {
                mtsPowerContent.gameObject.SetActive(needShowPower);
            }
            if (needShowPower)
            {
                if (mtsPowerUp.gameObject.activeSelf != fightPowerUp)
                {
                    mtsPowerUp.gameObject.SetActive(fightPowerUp);
                }
                if (mtsPowerDown.gameObject.activeSelf == fightPowerUp)
                {
                    mtsPowerDown.gameObject.SetActive(!fightPowerUp);
                }
            }

            if (mbGrowEnable && (!DataManager.IsMatchPalyerLv((int)baseItem.BaseData.useLevel)
                || !isMatchJob))
            {
                SetWarningMask(true);
            }

            SetDurableMask(!baseItem.HaveDurable);    

            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            SetBindMask(baseItem.IsBind);
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        bool enable = mbGrowEnable && mbCondiEnable && hightLight;
        if (null != m_toggle)
        {
            if (m_toggle.value != enable)
            {
                m_toggle.value = enable;
            }
        }
        enable = mbGrowEnable && !mbCondiEnable;
        if (null != mtsDisableBg && mtsDisableBg.gameObject.activeSelf != enable)
        {
            mtsDisableBg.gameObject.SetActive(enable);
        }

        enable = !mbGrowEnable;
        if (null != mtsLock && mtsLock.gameObject.activeSelf != enable)
        {
            mtsLock.gameObject.SetActive(enable);
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