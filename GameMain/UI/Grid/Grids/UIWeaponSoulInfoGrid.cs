using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIWeaponSoulInfoGrid : UIItemInfoGridBase
{
    #region Property
    //名称
    private UILabel m_lab_name;
    //已装备标示
    private GameObject equipMask;
    //等级
    private UILabel m_lab_lv;
    //选中toggle
    private UIToggle m_tg_hightlight;
    private uint qwThisId = 0;
    private bool m_bool_needCheck = false;


    private Transform mtsHightlight = null;
    //不可用背景
    private Transform mtsDisableBg = null;
    private bool mbEnable = false;

    public uint QWThisId
    {
        get
        {
            return qwThisId;
        }
    }
    #endregion

    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        mtsDisableBg = CacheTransform.Find("Content/Disable");
        mtsHightlight = CacheTransform.Find("Content/Hightlight");

        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        if (null != CacheTransform.Find("Content/EquipMask"))
            equipMask = CacheTransform.Find("Content/EquipMask").gameObject;
        if (null != CacheTransform.Find("Content/Level"))
            m_lab_lv = CacheTransform.Find("Content/Level").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    public override void SetHightLight(bool hightLight)
    {
        bool enableHightLight = mbEnable && hightLight;
        if (null != m_tg_hightlight)
        {
            if (m_tg_hightlight.value != enableHightLight)
            {
                m_tg_hightlight.value = enableHightLight;
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

    #region Set
    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="check"></param>
    /// <param name="enable"></param>
    public void SetGridViewData(uint qwThisId, bool check, bool enable)
    {
        this.qwThisId = qwThisId;
        mbEnable = enable;
        SetHightLight(check);
        Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(qwThisId);
        ResetInfoGrid();
        if (null == muhon)
        {
            return;
        }
        //等级
        if (null != m_lab_lv )
        {
            m_lab_lv.text = string.Format("{0}级",muhon.Level);
        }
        //名称
        if (null != m_lab_name )
        {
            m_lab_name.text = muhon.Name;
        }
        bool isWear = DataManager.Manager<EquipManager>().IsWearEquip(qwThisId);
        //装备表示
        if (null != equipMask && equipMask.gameObject.activeSelf != isWear)
        {
            equipMask.gameObject.SetActive(isWear);
        }

        bool isMatchJob = DataManager.IsMatchPalyerJob((int)muhon.BaseData.useRole);
        bool fightPowerUp = false;
        if (isMatchJob
            && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(muhon.QWThisID, out fightPowerUp))
        {
            SetFightPower(true, fightPowerUp);
        }
        if (!DataManager.IsMatchPalyerLv((int)muhon.BaseData.useLevel)
            || !isMatchJob)
        {
            SetWarningMask(true);
        }
        SetMuhonMask(true, muhon.StartLevel);
        SetMuhonLv(true, Muhon.GetMuhonLv(muhon));
        SetIcon(true, muhon.Icon);
        SetBorder(true, muhon.BorderIcon);
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