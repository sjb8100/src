/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemAssistSelectGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/13/2017 11:04:47 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIItemAssistSelectGrid : UIItemInfoGridBase
{
    #region property
    private UIToggle m_tg_select;
    private UILabel m_lab_name;
    private Transform m_ts_costDQ;
    private UILabel m_lab_des;
    private UILabel m_lab_num;
    private uint m_uint_baseId;
    public uint BaseId
    {
        get
        {
            return m_uint_baseId;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        m_tg_select = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        if (null != m_tg_select)
        {
            m_tg_select.onChange.Add(new EventDelegate(() =>
            {
                InvokeUIDlg(UIEventType.Click,this,m_tg_select.value);
            }));
        }
        m_ts_costDQ = CacheTransform.Find("Content/CostDQ");
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    private void OnToggleValueChanged(bool value)
    {

    }
    /// <summary>
    /// 设置辅助道具数据
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="select"></param>
    /// <param name="num"></param>
    /// <param name="useDQ"></param>
    /// <param name="costNum"></param>
    /// <param name="mType"></param>
    public void SetGridData(uint baseId,bool select
        ,uint num = 1,bool useDQ = false
        ,uint costNum = 0,GameCmd.MoneyType mType = GameCmd.MoneyType.MoneyType_Gold)
    {
        bool cansee = false;
        this.m_uint_baseId = baseId;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (null != m_lab_name)
        {
            m_lab_name.text = baseItem.Name;
        }

        if (null != m_lab_des)
        {
            m_lab_des.text = DataManager.Manager<EquipManager>().GetEquipAssistMaterialDes(baseItem.BaseId);
        }
        int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
        if (null != m_lab_num)
        {
            cansee = !useDQ || !select;
            if (m_lab_num.gameObject.activeSelf != cansee)
                m_lab_num.gameObject.SetActive(cansee);
            if (cansee)
            {
                m_lab_num.text = ItemDefine.BuilderStringByHoldAndNeedNum((uint)holdNum, num);
            }
        }
        ResetInfoGrid();
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);

        bool fightPowerUp = false;
        if (baseItem.IsEquip
            && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.BaseId, out fightPowerUp))
        {
            SetFightPower(true, fightPowerUp);
        }

        if (baseItem.IsMuhon)
        {
            SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
            SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
        }
        else if (baseItem.IsRuneStone)
        {
            SetRuneStoneMask(true, (uint)baseItem.Grade);
        }
        cansee = num > holdNum;
        SetNotEnoughGet(cansee);

        if (null != m_tg_select && m_tg_select.value != select)
        {
            m_tg_select.value = select;
        }

        if (null != m_ts_costDQ)
        {
            cansee = useDQ && select;
            if (m_ts_costDQ.gameObject.activeSelf != cansee)
                m_ts_costDQ.gameObject.SetActive(cansee);
            if (cansee)
            {
                UICurrencyGrid costGrid = m_ts_costDQ.GetComponent<UICurrencyGrid>();
                if (null == costGrid)
                {
                    costGrid = m_ts_costDQ.gameObject.AddComponent<UICurrencyGrid>();
                }
                costGrid.SetGridData(
                    new UICurrencyGrid.UICurrencyGridData(MallDefine.GetCurrencyIconNameByType(mType), costNum));
            }
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
                    if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                        if (BaseId != 0)
                        {
                            if (infoGrid.NotEnough)
                            {
                                InvokeUIDlg(eventType, data, BaseId);
                            }
                            else
                            {
                                TipsManager.Instance.ShowItemTips(BaseId);
                            }
                        }

                    }
                }
                break;
        }

    }
    #endregion
}