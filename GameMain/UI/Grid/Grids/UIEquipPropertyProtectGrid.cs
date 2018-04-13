using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;

class UIEquipPropertyProtectGrid : UIItemInfoGridBase
{
    #region Property
    //属性名称
    private UILabel attrDes;
    //添加保护的符石数量
    private UILabel m_labNum;
    //
    private UILabel m_labPortectDes;
    //添加移除动作回调
    private Action<uint> changeCallback;

    private UIToggle m_toggle;
    public uint AttrId = 0;
    private uint m_uint_rsId = 0;
    #endregion

    #region OverrideMethod
    protected override void OnAwake()
    {
        base.OnAwake();
        attrDes = CacheTransform.Find("Content/AttrDes").GetComponent<UILabel>();
        m_labNum = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        m_labNum.color = Color.white;
        m_labPortectDes = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        m_toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        if (null != m_toggle)
        {
            UIEventListener.Get(m_toggle.gameObject).onClick = (obj) =>
                {
                    if (null != changeCallback)
                    {
                        changeCallback.Invoke(AttrId);
                    }
                };
        }
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
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
    /// 设置格子
    /// </summary>
    /// <param name="attr">属性</param>
    /// <param name="totalProtectAttrNum">保护属性总条数</param>
    /// <param name="selectNum">选择数量</param>
    /// <param name="callback"></param>
    public void SetGridView(GameCmd.PairNumber attr, bool select,int fillNum = 0,int needNum = 0
        , Action<uint> callback = null)
    {
        ResetInfoGrid();
        RuneStone rs = null;
        uint runeStoneBaseId = 0;
        AttrId = attr.id;
        EquipManager emgr = DataManager.Manager<EquipManager>();
        if (emgr.TryGetRuneStoneIdByEffectId(emgr.TransformAttrToEffectId(attr), out runeStoneBaseId))
        {
            rs = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(runeStoneBaseId,ItemDefine.ItemDataType.RuneStone);
        }
        else
        {
            Engine.Utility.Log.Error("TryGetRuneStoneIdByEffectId failed attr pair = [id,{0}]-[value,{1}]", attr.id, attr.value);
        }
        m_uint_rsId = rs.BaseId;
        if (null != this.attrDes)
        {
            this.attrDes.text = emgr.GetAttrDes(attr);
        }

        if (null != this.m_labPortectDes)
        {
            this.m_labPortectDes.text = string.Format("消耗符石档次：{0}档", rs.Grade);
        }
        if (null != m_toggle)
        {
            m_toggle.value = select;
        }
        SetIcon(true, rs.Icon);
        SetBorder(true, rs.BorderIcon);
        int holdNum = (int)DataManager.Manager<ItemManager>().GetItemNumByBaseId(runeStoneBaseId);
        SetRuneStoneMask(true, (uint)rs.Grade);
        if (select && fillNum < needNum)
        {
            SetNotEnoughGet(true);
        }
        if (null != m_labNum)
        {
            if (select)
            {
                this.m_labNum.text = ItemDefine.BuilderStringByHoldAndNeedNum((uint)fillNum, (uint)needNum);
            }else
            {
                this.m_labNum.text = holdNum.ToString();
            }
            
        }

        if (this.changeCallback != callback)
        {
            this.changeCallback = callback;
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
                        if (infoGrid.NotEnough)
                        {
                            TipsManager.Instance.ShowItemTips(m_uint_rsId);
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_uint_rsId);                       
                        }
                        else
                        {
                            TipsManager.Instance.ShowItemTips(m_uint_rsId);
                        }
                    }
                }
                break;
        }

    }
    #endregion
}