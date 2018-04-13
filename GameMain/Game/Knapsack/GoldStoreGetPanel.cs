/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GoldStoreGetPanel
 * 版本号：  V1.0.0.0
 * 创建时间：5/26/2017 2:35:48 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class GoldStoreGetPanel
{

    #region property
    //仓库存取铜币数量
    private uint wareHouseStoreTakeOutCopperNum = 0;
    #endregion

    #region overridemethod
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
        wareHouseStoreTakeOutCopperNum = 0;
        UpdateWareHouseStoreCopperNum(0);
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
    }

    #endregion


    #region Op

    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED, OnGlobalUIEventHandler);
        }
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED:
                UpdateWareHouseStoreCopperNum((uint)data);
                Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHINPUTMAXNUM, OnGlobalUIEventHandler);
                break;
        }
    }
    /// <summary>
    /// 更新仓库存储铜币数量
    /// </summary>
    /// <param name="num"></param>
    public void UpdateWareHouseStoreCopperNum(uint num)
    {
        if (null != m_label_KnapsackOwnCopperStoreNum)
            m_label_KnapsackOwnCopperStoreNum.text = "" + (uint)UserData.Coupon;
        if (null != m_label_WareHouseCurStoreCopperNum)
            m_label_WareHouseCurStoreCopperNum.text = "" + DataManager.Manager<KnapsackManager>().WareHouseStoreCopperNum;
        if (null != m_label_WareHouseStoreTakeCopperNum)
        {
            m_label_WareHouseStoreTakeCopperNum.text = wareHouseStoreTakeOutCopperNum.ToString();
        }
    }

    private void OnWareHouseStoreTakeOutCopperNumInput(int num)
    {
        UpdateWareHouseStoreTakeOutCopperNum(num);
    }

    private void UpdateWareHouseStoreTakeOutCopperNum(int num)
    {
        num = Math.Max(1, num);
        wareHouseStoreTakeOutCopperNum = (uint)num;
        if (null != m_label_WareHouseStoreTakeCopperNum)
            m_label_WareHouseStoreTakeCopperNum.text = "" + wareHouseStoreTakeOutCopperNum;
    }

    private uint GetRefreshStoreCopperMaxNum()
    {
        return Math.Max((uint)UserData.Coupon, (uint)DataManager.Manager<KnapsackManager>().WareHouseStoreCopperNum);
    }
    #endregion

    #region UIEvent

    void onClick_WareHouseStoreCopperBtnClose_Btn(GameObject caster)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.HandInputPanel))
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.HandInputPanel);
        HideSelf();
    }

    void onClick_WareHouseStoreCopperBtnStore_Btn(GameObject caster)
    {
        DataManager.Manager<KnapsackManager>().StoreCopperInWareHouse(wareHouseStoreTakeOutCopperNum, (success) =>
        {
            if (success)
                onClick_WareHouseStoreCopperBtnClose_Btn(null);
        });
    }

    void onClick_WareHouseStoreCopperBtnTakeOut_Btn(GameObject caster)
    {
        DataManager.Manager<KnapsackManager>().TakeOutCopperFromWareHouse(wareHouseStoreTakeOutCopperNum, (success) =>
        {
            if (success)
                onClick_WareHouseStoreCopperBtnClose_Btn(null);
        });
    }

    void onClick_WareHouseStoreCopperAdd_Btn(GameObject caster)
    {
        UpdateWareHouseStoreTakeOutCopperNum((int)wareHouseStoreTakeOutCopperNum + 1);
    }

    void onClick_WareHouseStoreCopperSub_Btn(GameObject caster)
    {
        if (wareHouseStoreTakeOutCopperNum > 1)
            UpdateWareHouseStoreTakeOutCopperNum((int)wareHouseStoreTakeOutCopperNum - 1);
    }

    void onClick_WareHouseStoreCopperHandInput_Btn(GameObject caster)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                onInputValue = OnWareHouseStoreTakeOutCopperNumInput,
                showLocalOffsetPosition = new Vector3(299, -63f, 0),
                maxInputNum = Math.Max((uint)UserData.Coupon, (uint)DataManager.Manager<KnapsackManager>().WareHouseStoreCopperNum),
                onGetInputMaxDlg = GetRefreshStoreCopperMaxNum,
            });
        }
    }
    #endregion
}