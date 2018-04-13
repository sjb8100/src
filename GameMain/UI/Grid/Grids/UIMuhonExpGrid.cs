/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIMuhonExpGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/18/2017 2:33:30 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
class UIMuhonExpGrid : UIItemInfoGridBase, ITimer
{
    #region property
    //名称
    private UILabel mlabName;
    //数量
    private UILabel mlabDes;
    //消耗代币
    private Transform m_ts_costDQ;
    private uint m_uint_baseId;
    public uint BaseId
    {
        get
        {
            return m_uint_baseId;
        }
    }
    //长按定时器
    private const int LONGPRESS_TIMER_ID = 2000;
    private Transform infoRoot = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        mlabName = CacheTransform.Find("Content/Name").GetComponent<UILabel>(); ;
        mlabDes = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        infoRoot = CacheTransform.Find("Content/InfoGridRoot/InfoGrid");
        InitItemInfoGrid(infoRoot);
        RegisterGlobalUIEvent(true);
        SetTriggerEffect(true, infoRoot, (int)UITriggerEffectType.Scale);
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="num"></param>
    public void SetGridData(uint baseId, uint num)
    {
        this.m_uint_baseId = baseId;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (null != mlabName)
        {
            mlabName.text = baseItem.Name;
        }

        if (null != mlabDes)
        {
            mlabDes.text = string.Format("经验+{0}", DataManager.Manager<EquipManager>().GetMuhonExpOfId(baseId));
        }

        int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
        ResetInfoGrid();
        SetNum(true, ItemDefine.BuilderStringByHoldAndNeedNum((uint)holdNum, 1));
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
        SetBindMask(baseItem.IsBind);
        SetNotEnoughGet((holdNum < num));
    }

    public override void OnPress(bool isDown)
    {
        base.OnPress(isDown);
        if (isDown)
        {
            TimerAxis.Instance().SetTimer(LONGPRESS_TIMER_ID, 200, this);
        }
        else
        {
            TimerAxis.Instance().KillTimer(LONGPRESS_TIMER_ID, this);
        }
    }
    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        TimerAxis.Instance().KillTimer(LONGPRESS_TIMER_ID, this);
        RegisterGlobalUIEvent(false);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(depthRelease);
        }
    }
    #endregion

    #region ITimer
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == LONGPRESS_TIMER_ID)
        {
            InvokeUIDlg(UIEventType.LongPressing, this, BaseId);
        }
    }
    #endregion

    #region UIEventCallBack

    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }

     /// <param name="data">物品id</param>
    public void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    ItemDefine.UpdateItemPassData passData = (ItemDefine.UpdateItemPassData)data;
                    if (passData.BaseId == BaseId)
                    {
                        SetGridData(BaseId, 1);
                    }
                }
                break;
        }
    }
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    InvokeUIDlg(eventType, this, BaseId);
                    //if (data is UIItemInfoGrid)
                    //{
                    //    UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                    //    if (BaseId != 0)
                    //    {
                    //        if (infoGrid.NotEnough)
                    //        {
                    //            InvokeUIDlg(eventType, data, m_uint_baseId);
                    //        }
                    //        else
                    //        {
                    //            TipsManager.Instance.ShowItemTips(BaseId);
                    //        }
                    //    }

                    //}
                }
                break;
        }

    }
    #endregion
}