using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



class UICommonUseItemPanelGrid : UIItemInfoGridBase
{

    #region define
    //ItemGrowCostStyle
    public enum ItemGrowCostStyle
    {
        NameNumRight,
        NameNumBottom,
    }
    #endregion

    #region property

    //樣式
    private ItemGrowCostStyle m_em_style = ItemGrowCostStyle.NameNumBottom;

    //消耗代币
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
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="baseId">消耗材料id</param>
    /// <param name="num">（useDq为true代表替代点券数量，反之消耗数量）</param>
    /// <param name="useDq">是否使用货币代替</param>
    ///  <param name="mType">useDq 为true 有效</param>
    public void SetGridData(uint baseId, uint num)
    {
        bool cansee = false;
        this.m_uint_baseId = baseId;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        //this.m_em_style = style;

        int holdNum = 0;
        holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
        //cansee = !useDq || (style != ItemGrowCostStyle.NameNumRight);

        //if (null != tempLabel)
        //{
        //    holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
        //    cansee = !useDq || (style != ItemGrowCostStyle.NameNumRight);
        //    if (tempLabel.gameObject.activeSelf != cansee)
        //        tempLabel.gameObject.SetActive(cansee);
        //    if (cansee)
        //    {
        //        tempLabel.text = ItemDefine.BuilderStringByHoldAndNeedNum(
        //            (uint)holdNum, num);
        //    }
        //}

        ResetInfoGrid();
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
        SetBindMask(baseItem.IsBind);
        cansee = holdNum < num;
        SetNotEnoughGet(cansee);

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

    }
    #endregion


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
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: BaseId);                              
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

}

