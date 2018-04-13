/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GridStrengthenSuitPanel
 * 版本号：  V1.0.0.0
 * 创建时间：6/7/2017 5:41:28 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using GameCmd;
public class SuitPanelParam
{
    public enum SuitType 
    { 
        None,
        Strengthen,
        Color,
        Stone,
    }
    public SuitType m_type;
    public Vector3 vec;
    public bool isOther;
}
partial class GridStrengthenSuitPanel
{
    #region property
    SuitPanelParam.SuitType m_CurType = SuitPanelParam.SuitType.None;
    List<ColorSuitDataBase> colors = null;
    List<GemSuitDataBase> gems = null;
    bool isOtherPlayer = false;

    uint ActiveStrengthenSuitLv = 0;
    uint ActiveColorSuitLv = 0;
    uint ActiveStoneSuitLv = 0;
    private List<uint> m_lstSuitLv = null;
    List<string> StrengthenDataDes = null;
    #endregion



    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        colors = GameTableManager.Instance.GetTableList<ColorSuitDataBase>();
        gems = GameTableManager.Instance.GetTableList<GemSuitDataBase>();
        StrengthenDataDes = new List<string>();
        m_lstSuitLv = new List<uint>();
        if (null != m_ctor_ScrollView)
        {
            GameObject resObj = UIManager.GetResGameObj(GridID.Uistrengthensuitpropgrid) as GameObject;
            m_ctor_ScrollView.RefreshCheck();
            m_ctor_ScrollView.Initialize<UIStrengthenSuitPropGrid>(resObj, OnUpdateGridData, null);
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
       
        Vector3 pos = Vector3.zero;
        if (null != data && data is SuitPanelParam)
        {
            SuitPanelParam par = (SuitPanelParam)data;
            pos = (Vector3)par.vec;
            m_CurType = par.m_type;
            isOtherPlayer = par.isOther;
        }
        if (null != m_trans_Content)
        {
            m_trans_Content.transform.localPosition = pos;
        }
        StrengthenDataDes.Clear();
        m_lstSuitLv.Clear();
        if (isOtherPlayer)
        {
            ViewPlayerData retData = ViewPlayerData.GetViewPlayerData();
            if (retData == null)
            {
                return;
            }
            List<GemInlayList> gem_data = retData.gem_data;
            List<StrengthList> strengthList = retData.strengthList;
            List<ItemSerialize> itemList = retData.itemList;
            List<uint> tempStrength = new List<uint>();
            #region 宝石等级
            ActiveStoneSuitLv = 0;
            for (int i = 0; i < gem_data.Count; i ++ )
            {
                uint itemID = gem_data[i].base_id;
                table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
                if (item != null)
                {
                    ActiveStoneSuitLv += item.grade;
                }
            }
            #endregion
            #region 颜色等级
            ActiveColorSuitLv = 0;
            List<BaseItem> items = new List<BaseItem>();
            for (int i = 0; i < itemList.Count; i++)
            {
                BaseItem baseItem = new BaseItem(itemList[i].dwObjectID, itemList[i]);
                if (baseItem != null)
                {
                    if (baseItem.EquipColor >= EquipDefine.EquipColor.Orange)
                    {
                        items.Add(baseItem);
                    }
                }
            }
            for (int j = 0; j < colors.Count; j++)
            {
                uint count = 0;
                for (int x = 0; x < items.Count; x++)
                {
                    if (items[x].Grade >= colors[j].grade)
                    {
                        count++;
                    }
                }
                if (count >= colors[j].equip_num)
                {
                    ActiveColorSuitLv = colors[j].level;
                }
            }
            #endregion
            #region 套装强化等级
            ActiveStrengthenSuitLv = 0;
            List<GridStrengthenSuitDataBase> StrengthenDataBase = GameTableManager.Instance.GetTableList<GridStrengthenSuitDataBase>();
            
            for (int i = 0; i < StrengthenDataBase.Count; i++)
            {
                if (StrengthenDataBase[i].job == retData.job)
                {
                    uint matchNum = 0;
                    m_lstSuitLv.Add(StrengthenDataBase[i].suitlv);
                    StrengthenDataDes.Add(StrengthenDataBase[i].des);
                    for (int j = 0; j < strengthList.Count; j++)
                    {
                        if (strengthList[j].level >= StrengthenDataBase[i].triggerStrengLv)
                        {
                            matchNum++;
                        }
                    }
                    if (matchNum >= StrengthenDataBase[i].triggerPosNum)
                    {
                        ActiveStrengthenSuitLv = StrengthenDataBase[i].suitlv;
                    }
                  
                }
            }
           
            #endregion
        }
        else 
        {
            ActiveStrengthenSuitLv = DataManager.Manager<EquipManager>().ActiveStrengthenSuitLv;
            ActiveColorSuitLv = DataManager.Manager<EquipManager>().ActiveColorSuitLv;
            ActiveStoneSuitLv = DataManager.Manager<EquipManager>().ActiveStoneSuitLv;
            m_lstSuitLv.AddRange(DataManager.Manager<EquipManager>().CurGroupSuitData.GetStrengthenLvDatas());  
            for(int i = 0 ; i <m_lstSuitLv.Count ;i++ )
            {
               string des =  DataManager.Manager<EquipManager>().CurGroupSuitData.GetLocalStrengthenData(m_lstSuitLv[i]).Des;
               StrengthenDataDes.Add(des);
            }
        }
        CreateData();
       
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    #endregion

    #region OP

    /// <summary>
    /// 注册UI全局事件
    /// </summary>
    /// <param name="register"></param>
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            //装备格强化
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
        }
        else
        {
            //装备格强化
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            //强化套装属性变更
            case (int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED:
                {
                    if (null != m_ctor_ScrollView)
                    {
                        m_ctor_ScrollView.UpdateActiveGridData();
                    }
                }
                break;
        }
    }
  
    private void CreateData()
    {
        m_label_TextLabel.text = "";
        if (m_CurType == SuitPanelParam.SuitType.Strengthen)
        {
            m_trans_Offset.gameObject.SetActive(true);
            m_scrollview_ColorScroll.gameObject.SetActive(false);
            m_label_StrengthenTxt.text = "强化套装属性:";
            if (null != m_ctor_ScrollView)
            {
                m_ctor_ScrollView.CreateGrids(m_lstSuitLv.Count);
            }
        }
        else if (m_CurType == SuitPanelParam.SuitType.Color)
        {
            m_trans_Offset.gameObject.SetActive(false);
            m_scrollview_ColorScroll.gameObject.SetActive(true);     
            m_label_StrengthenTxt.text = "颜色套装属性:";
            StringBuilder builder = new StringBuilder();
            uint level = ActiveColorSuitLv;
            for (int i = 0; i < colors.Count; i++)
            {
                bool active = level>= colors[i].level;
                ColorType colorTyp = (active) ? ColorType.JZRY_Tips_Green : ColorType.JZRY_Tips_White;
                string colorText = ColorManager.GetNGUIColorOfType(colorTyp) + colors[i].Des;
                string des = i > 0 ? "\n" + colorText : colorText;
                builder.Append(des);
            }
            m_label_TextLabel.text = builder.ToString();
        }
        else if (m_CurType == SuitPanelParam.SuitType.Stone)
        {
            m_trans_Offset.gameObject.SetActive(false);
            m_scrollview_ColorScroll.gameObject.SetActive(true);
            m_label_StrengthenTxt.text = "宝石套装属性:";
            StringBuilder builder = new StringBuilder();
            bool active =false;           
            uint level = ActiveStoneSuitLv;
            for (int j = 0; j < gems.Count; j++ )
            {
                if (isOtherPlayer)
                 {
                     active = level >= gems[j].gem_all_level;
                 }
                 else 
                 {                   
                     active = level >= gems[j].level;
                 }
                ColorType colorTyp = (active) ? ColorType.JZRY_Tips_Green : ColorType.JZRY_Tips_White;
                string colorText = ColorManager.GetNGUIColorOfType(colorTyp) + gems[j].Des;
                string des = j > 0 ? "\n" + colorText : colorText;
                builder.Append(des);
            }
            m_label_TextLabel.text = builder.ToString();
        }
    }

  
    private void OnUpdateGridData(UIGridBase grid,int index)
    {
        if (index < m_lstSuitLv.Count)
        {
            UIStrengthenSuitPropGrid sGrid = grid as UIStrengthenSuitPropGrid;
            if (m_CurType == SuitPanelParam.SuitType.Strengthen)
            {
                bool active = ActiveStrengthenSuitLv >= m_lstSuitLv[index];
                sGrid.SetDes(active, StrengthenDataDes[index]);
            }          
        }
    }
    #endregion
}