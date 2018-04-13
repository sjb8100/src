using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIItemGrid : UIItemInfoGridBase, Engine.Utility.ITimer
{
    #region Property 
    private readonly uint ITEMCD_TIMERID = 1000;
    private readonly uint ITEMCD_GAP_TIME = 30;
    //锁
    private GameObject m_obj_lockmask = null;
    //勾选框
    private UIToggle checkBox;
    //格子数据
    private BaseItem data;
    public BaseItem Data
    {
        get
        {
            return data;
        }
    }
    //背包位置
    private uint location = 0;
    public uint Location
    {
        get
        {
            return (location == 0) ? ((null != data) ? data.ServerLocaltion : 0) : location;
        }
    }
    
    //是否为空
    public bool Empty
    {
        get
        {
            return (null == data || data.Num == 0) ? true : false;
        }
    }
    //是否解锁
    private bool isLock = false;
    public bool IsLock
    {
        get
        {
            return isLock;
        }
    }

    private Transform m_tsCD = null;
    private UILabel m_labCDTime = null;
    private UISprite m_spCDIcon = null;
    private UISprite m_treasureIcon = null;
    #endregion

    #region override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        m_obj_lockmask = CacheTransform.Find("Content/LockMask").gameObject;
        checkBox = CacheTransform.Find("Content/Select").GetComponent<UIToggle>();
        checkBox.instantTween = true;


        m_tsCD = CacheTransform.Find("Content/CD");
        m_spCDIcon = CacheTransform.Find("Content/CD/CDIcon").GetComponent<UISprite>();
        m_labCDTime = CacheTransform.Find("Content/CD/CDTime").GetComponent<UILabel>();
        m_treasureIcon = CacheTransform.Find("Content/treasureIcon").GetComponent<UISprite>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"),false);
        //EnableCheckBox(false);
        //SetLock(false);
        //ResetStatus();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_USEITEMCD, DoGameEvent); // 创建实体
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_USEITEMCD, DoGameEvent); // 创建实体
    }

    //public override void Reset()
    //{
    //    base.Reset();
    //    ResetStatus();
    //}

    //public void ResetStatus()
    //{
    //    data = null;
    //    location = 0;
    //    isLock = false;
    //    SetCDStatus(false);
    //    EnableCheckBox(false);
    //    SetNum(false);      
    //}

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
    }

    public void SetTreasureVisible(bool visible) 
    {
        if (m_treasureIcon != null)
        {
            m_treasureIcon.gameObject.SetActive(visible);
        }
    }
    #endregion
    
    #region Set

    private void SetCDStatus(bool enable)
    {
        if (null != m_tsCD && m_tsCD.gameObject.activeSelf != enable)
        {
            m_tsCD.gameObject.SetActive(enable);
        }
    }

    public void SetGridData(UIItemInfoGridBase.InfoGridType infoGridType,BaseItem baseItem)
    {
        bool empty = (null == baseItem);
        ResetInfoGrid(!empty);
        SetTreasureVisible(false);
        if (empty)
        {
            this.data = null;
            return;
        }
        this.data = baseItem as BaseItem;
        SetIcon(true, this.data.Icon);
        SetBorder(true, this.data.BorderIcon);
        bool enable = this.data.Num > 1;
        SetNum(enable, TextManager.GetFormatNumText(this.data.Num));
        SetBindMask(this.data.IsBind);
        //SetTimeLimitMask(false);
        bool isMatchJob = DataManager.IsMatchPalyerJob((int)this.data.BaseData.useRole);
        bool haveDurable = true;
        if (this.data.IsEquip)
        {
            bool fightPowerUp = false;
            if (isMatchJob
                && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(this.data.QWThisID, out fightPowerUp))
            {
                SetFightPower(true, fightPowerUp);
            }
            Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(this.data.QWThisID);
            if (equip != null)
            {
                haveDurable = equip.HaveDurable; 
            }        
        }
        bool isMatchLv = DataManager.IsMatchPalyerLv((int)this.data.BaseData.useLevel);
        bool warningMaskEnable = !(isMatchJob && isMatchLv);
        if (infoGridType == InfoGridType.Knapsack)
        {
            if (!empty)
            {
                //warningMaskEnable = !(isMatchJob && isMatchLv);
                DoGameEvent((int)Client.GameEventID.UIEVENT_USEITEMCD, new Client.stUseItemCD()
                {
                    itemBaseId = data.BaseId,
                });
            }
            
        }
        else if (infoGridType == InfoGridType.KnapsackSell)
        {
            //warningMaskEnable = !this.data.CanSell2NPC;
        }
        else if (infoGridType == InfoGridType.KnapsackSplit && baseItem.IsEquip)
        {
            //BaseEquip baseEquip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(baseItem.QWThisID);
            //if (null != baseEquip && !baseEquip.CanSplit)
            //    warningMaskEnable = true;

        }else if (infoGridType == InfoGridType.KnapsackWareHouse)
        {
            //warningMaskEnable = !baseItem.CanStore2WareHouse;
        }
        if (warningMaskEnable)
        {
            SetWarningMask(true);
        }

        SetDurableMask(!haveDurable);

        if (this.data.IsMuhon)
        {
            SetMuhonMask(true, Muhon.GetMuhonStarLevel(this.data.BaseId));
            SetMuhonLv(true, Muhon.GetMuhonLv(this.data));
        }
        else if (this.data.IsRuneStone)
        {
            SetRuneStoneMask(true, (uint)this.data.Grade);
        }else if (this.data.IsChips)
        {
            SetChips(true);
        }
        SetTreasureVisible(data.IsTreasure && infoGridType == InfoGridType.Consignment);

    }

    /// <summary>
    /// 设置上锁
    /// </summary>
    public void SetLock(bool isLock)
    {
        this.isLock = isLock;
        if (null != m_obj_lockmask && m_obj_lockmask.gameObject.activeSelf != isLock)
        {
            m_obj_lockmask.gameObject.SetActive(isLock);
        }
    }
    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="location"></param>
    public void SetLocation(uint location)
    {
        this.location = location;
        bool isLock = !DataManager.Manager<KnapsackManager>().IsGridUnlock(location);
        SetLock(isLock);

    }

    public void ChangeItemNum(int num)
    {
        bool enable = num >= 1;
        SetNum(enable, num.ToString());
    }
    #endregion

    #region CheckBoxt
    public void EnableCheckBox(bool enable = true)
    {
        if (null != checkBox )
        {
            if (checkBox.gameObject.activeSelf != enable)
                checkBox.gameObject.SetActive(enable);
            if (checkBox.value != enable)
            {
                checkBox.value = enable;
            }
        }
    }
    #endregion

    #region ITimer
    void DoGameEvent(int EventId, object param)
    {
        if ((int)Client.GameEventID.UIEVENT_USEITEMCD == EventId && !Empty)
        {
            Client.stUseItemCD useItemCD = (Client.stUseItemCD)param;
            ItemManager.CDData cdData;
            if (DataManager.Manager<ItemManager>().TryGetItemCDByBaseId(data.BaseId, out cdData))
            {
                Engine.Utility.TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
                Engine.Utility.TimerAxis.Instance().SetTimer(ITEMCD_TIMERID, ITEMCD_GAP_TIME, this);
            }
        }
        else
        {
            Engine.Utility.TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
            SetCDStatus(false);
        }
    }

    public void OnTimer(uint uTimerID)
    {
        ItemManager.CDData cdData;
        if (!Empty && uTimerID == ITEMCD_TIMERID
            && DataManager.Manager<ItemManager>().TryGetItemCDByBaseId(this.data.BaseId, out cdData))
        {
            SetCDStatus(true);
            if (null != m_spCDIcon)
            {
                m_spCDIcon.fillAmount = cdData.remainCD / cdData.totalCD;
            }

            if(null != m_labCDTime)
            {
                m_labCDTime.text = ((int)cdData.remainCD + 1).ToString();
            }
        }
        else
        {
            SetCDStatus(false);
            Engine.Utility.TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
        }
    }
    #endregion

    #region Release

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_USEITEMCD, DoGameEvent);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        data = null;
    }
    #endregion
}