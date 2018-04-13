/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  EquipCompoundChoosePanel
 * 版本号：  V1.0.0.0
 * 创建时间：8/8/2017 4:22:11 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class EquipCompoundChoosePanel:IGlobalEvent
{
    #region property
    private UIItemShowGrid m_growCost = null;
    private UICompoundSelectAttrGrid[] cardArray = null;
    private EquipManager emgr = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        emgr = DataManager.Manager<EquipManager>();
        Transform tempTrans = null;
        if (null != m_trans_CostRoot && null == m_growCost)
        {
            tempTrans = UIManager.GetObj(GridID.Uiitemshowgrid);
            if (null != tempTrans)
            {
                Util.AddChildToTarget(m_trans_CostRoot,tempTrans);
                m_growCost = tempTrans.GetComponent<UIItemShowGrid>();
                if (null == m_growCost)
                {
                    m_growCost = tempTrans.gameObject.AddComponent<UIItemShowGrid>();
                }
            }
        }

        if (null == cardArray && null != m_trans_ResultRoot)
        {
            StringBuilder rootBuilder = new StringBuilder();
            cardArray = new UICompoundSelectAttrGrid[3];
            UICompoundSelectAttrGrid tempGrid = null;
            Transform parentTs = null;
            for(int i= 0,max = cardArray.Length;i < max;i++)
            {
                rootBuilder.Remove(0,rootBuilder.Length);
                rootBuilder.Append("ResultRoot");
                rootBuilder.Append(i + 1);
                parentTs = m_trans_ResultRoot.Find(rootBuilder.ToString());
                if (null == parentTs)
                    continue;

                tempTrans = UIManager.GetObj(GridID.Uicompoundattrselectgrid);
                if (null != tempTrans)
                {
                    Util.AddChildToTarget(parentTs, tempTrans);
                    tempGrid = tempTrans.GetComponent<UICompoundSelectAttrGrid>();
                    if (null == tempGrid)
                    {
                        tempGrid = tempTrans.gameObject.AddComponent<UICompoundSelectAttrGrid>();
                    }
                    if (null != tempGrid)
                    {
                        tempGrid.RegisterUIEventDelegate(OnGridEventDlg);
                    }
                }
                cardArray[i] = tempGrid;
            }
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        ResetData();
        if (null == emgr.CurGroupSuitData || null == cardArray)
        {
            Engine.Utility.Log.Error("Show EquipCompoundCompletePanel failed,data error!");
            return;
        }
        RegisterGlobalEvent(true);
        ShowResult();
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        Release(false);
    }
    #endregion

    #region event
    //注册/反注册全局事件
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDOPENRESULT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDOPENRESULT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
        }
    }

    //全局事件处理
    public void GlobalEventHandler(int eventid, object data)
    {
        switch(eventid)
        {
            case (int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDOPENRESULT:
                {
                    if (null != data && data is uint)
                    {
                        uint openIndex = (uint)data;
                        if (openIndex >= 1 && openIndex <= 3
                            && null != cardArray && cardArray.Length == 3)
                        {
                            bool isOpen = false;
                            UICompoundSelectAttrGrid tempGrid = null;
                            for (int i = 0, max = cardArray.Length; i < max; i++)
                            {
                                tempGrid = cardArray[i];
                                if (null == tempGrid)
                                {
                                    continue;
                                }
                                isOpen = emgr.IsEquipCompoundResultOpen(tempGrid.Index);
                                if (tempGrid.Index == openIndex)
                                {
                                    if (emgr.CompoundSelectIndex == tempGrid.Index)
                                    {
                                        tempGrid.SetSelect(true);
                                    }
                                    tempGrid.OpenCard();
                                }else if (!isOpen)
                                {
                                    tempGrid.UpdateCloseCostStatus(false);
                                }
                            }
                        }
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    if (null != data && data is ItemDefine.UpdateItemPassData)
                    {
                        ItemDefine.UpdateItemPassData passData = (ItemDefine.UpdateItemPassData)data;
                        if (passData.BaseId == m_assistID
                            && null != cardArray )
                        {
                            UpdateAssistData();
                            bool freeGet = emgr.CompoundSelectIndex == 0;
                            if (!freeGet)
                            {
                                UICompoundSelectAttrGrid tempGrid = null;
                                bool isOpen = false;
                                for (int i = 0, max = cardArray.Length; i < max; i++)
                                {
                                    tempGrid = cardArray[i];
                                    if (null == tempGrid)
                                    {
                                        continue;
                                    }
                                    isOpen = emgr.IsEquipCompoundResultOpen(tempGrid.Index);
                                    if (isOpen)
                                    {
                                        continue;
                                    }
                                    tempGrid.UpdateCloseCostStatus(freeGet);
                                }
                            }
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region OP
    private void ResetData()
    {
        if (null != m_growCost)
        {
            m_growCost.SetGridData(0);
        }

        if (null != cardArray)
        {
            UICompoundSelectAttrGrid tempGrid = null;
            for (int i = 0, max = cardArray.Length; i < max; i++)
            {
                if (null == tempGrid)
                    continue;
                tempGrid.SetData(false);
            }
        }
    }

    private void ShowResult()
    {
        GameCmd.stEquipComposeItemWorkUserCmd_CS compoundData = emgr.CompoundData;
        if (null == compoundData.index_info || compoundData.index_info.Count != cardArray.Length)
        {
            return;
        }

        table.EquipComposeDataBase equipComposeDB = DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipComposeDataBase>(compoundData.new_item);
        if (null == equipComposeDB)
        {
            return;
        }
        m_assistID = equipComposeDB.assistId;
        m_assistNeedNum = equipComposeDB.assistNum;

        List<EquipManager.CompoudSelectAttrData> cardDatas = new List<EquipManager.CompoudSelectAttrData>();
        int replaceCoinNum = (int)DataManager.Manager<MallManager>().GetDQPriceByItem(m_assistID);

        List<GameCmd.PairNumber> attrs = null;
        bool isBind = false;
        GameCmd.PropIndexInfo tempIndexInfo = null;
        EquipManager.CompoudSelectAttrData tempSelectAttrData = null; ;
        GameCmd.PairNumber tempPair = null;
        GameCmd.PropIndexInfo.PropPair tempPropPair = null;
        
        for (int i = 0, max = compoundData.index_info.Count; i < max; i++)
        {
            isBind = false;
            tempIndexInfo = compoundData.index_info[i];
            tempSelectAttrData = new EquipManager.CompoudSelectAttrData();
            tempSelectAttrData.Index = tempIndexInfo.index;
            tempSelectAttrData.BaseID = compoundData.new_item;
            attrs = null;
            if (null != tempIndexInfo.value)
            {
                attrs = new List<GameCmd.PairNumber>();
                for (int j = 0, maxJ = tempIndexInfo.value.Count; j < maxJ; j++)
                {
                    tempPropPair = tempIndexInfo.value[j];
                    if (tempPropPair.key == (uint)GameCmd.eItemAttribute.Item_Attribute_FightPower)
                    {
                        continue;
                    }
                    if (tempPropPair.key == (uint)GameCmd.eItemAttribute.Item_Attribute_Bind)
                    {
                        if (ItemDefine.IsBind(tempPropPair.value))
                        {
                            isBind = true;
                        }
                        continue;
                    }
                    tempPair = new GameCmd.PairNumber()
                    {
                        id = tempPropPair.key,
                        value = tempPropPair.value,
                    };
                    attrs.Add(tempPair);
                }
            }
            tempSelectAttrData.CostItemID = equipComposeDB.assistId;
            tempSelectAttrData.CostItemNum = (int)equipComposeDB.assistNum;
            tempSelectAttrData.ReplaceCostMoneyType = GameCmd.MoneyType.MoneyType_Coin;
            tempSelectAttrData.ReplaceCostMoneyNum = replaceCoinNum;
            tempSelectAttrData.Attrs = attrs;
            tempSelectAttrData.IsBind = isBind;
            cardDatas.Add(tempSelectAttrData);
        }
        bool isOpen = false;
        bool freeGet = (emgr.CompoundSelectIndex == 0);
        bool select = false;
        UICompoundSelectAttrGrid tempGrid = null;
        for (int i = 0, max = cardArray.Length; i < max; i++)
        {
            select = false;
            isOpen = false;
            tempGrid = cardArray[i];
            if (null == tempGrid)
                continue;
            tempSelectAttrData = cardDatas[i];
            if (!freeGet)
            {
                isOpen = emgr.IsEquipCompoundResultOpen(tempSelectAttrData.Index);
                if (isOpen)
                {
                    select = (emgr.CompoundSelectIndex == tempSelectAttrData.Index); 
                }
            }

            tempGrid.SetData(isOpen, data: tempSelectAttrData, select: select, freeGet: freeGet);
        }
        UpdateAssistData();
    }

    private UICompoundSelectAttrGrid GetGridByIndex(uint index)
    {
        UICompoundSelectAttrGrid grid = null;
        if (index >= 1 && index <=3 && null != cardArray && cardArray.Length >= index)
        {
            grid = cardArray[index - 1];
        }
        return grid;
    }

    private uint m_assistID = 0;
    private uint m_assistNeedNum = 0;
    private void UpdateAssistData()
    {
        if (null != m_growCost)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_assistID);
            m_growCost.SetGridData(m_assistID);
            if (null != m_label_AssistName)
            {
                m_label_AssistName.text = baseItem.Name;
            }
            if (null != m_label_AssistNum)
            {
                int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_assistID);
                m_label_AssistNum.text = ItemDefine.BuilderStringByHoldAndNeedNum((uint)holdNum, m_assistNeedNum);;
            }
        }
    }

    private bool SelectEquipCompoundIndex()
    {
        if (DataManager.Manager<EquipManager>().CompoundSelectIndex != 0)
        {
            return true;
        }else
        {
            TipsManager.Instance.ShowTips(LocalTextType.forging_compose_ChooseTips);
        }
        return false;
    }

    private void AttrCheckStatusChanged(UICompoundSelectAttrGrid grid)
    {
        uint index = grid.Index;
        if (emgr.IsEquipCompoundResultOpen(index))
        {
            if (emgr.CompoundSelectIndex != index)
            {
                if (emgr.CompoundSelectIndex != 0)
                {
                    UICompoundSelectAttrGrid tempGrid = GetGridByIndex(emgr.CompoundSelectIndex);
                    if (null != tempGrid)
                    {
                        tempGrid.SetSelect(false); 
                    }
                }
                emgr.SetSelectCompoundResult(index);
            }
            grid.SetSelect(true);
        }else
        {
            emgr.OpenEquipCompoundResult(index);
        }
            
    }

    private void OnGridEventDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UICompoundSelectAttrGrid)
                    {
                        UICompoundSelectAttrGrid cGrid = data as UICompoundSelectAttrGrid;
                        AttrCheckStatusChanged(cGrid);
                    }
                }
                break;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        uint resID = 0;
        if (null != m_growCost)
        {
            m_growCost.Release(depthRelease);
            if (depthRelease)
            {
                resID = (uint)GridID.Uiitemgrowcostgrid;
                UIManager.OnObjsRelease(m_growCost.CacheTransform, resID);
            }
        }

        if (null != cardArray)
        {
            resID = (uint)GridID.Uicompoundattrselectgrid;
            UICompoundSelectAttrGrid tempGrid = null;
            for (int i = 0, max = cardArray.Length; i < max;i++ )
            {
                if (null == tempGrid)
                    continue;
                tempGrid.Release(depthRelease);
                if (depthRelease)
                {
                    UIManager.OnObjsRelease(tempGrid.CacheTransform, resID);
                }
            }

            if (depthRelease)
            {
                cardArray = null;
            }
                
        }
    }

    #endregion

    #region UIEvent
    private void onClick_Confirm_Btn(GameObject obj)
    {
        bool success = SelectEquipCompoundIndex();
        if (success)
        {
            HideSelf();
            //选中结果
            emgr.DoEquipCompoundSelectResult();
        }
    }
    #endregion
}