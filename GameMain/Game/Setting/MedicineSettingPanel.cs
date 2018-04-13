using System;
using System.Collections.Generic;
using UnityEngine;


public partial class MedicineSettingPanel : UIPanelBase
{
    public class MedicineSettingParam
    {
        public SettingPanel.MedicalType type;
        public int tab;
    }
    GameObject itemPreafb;
    List<MedicalReplace> m_lstMedicalReplace = new List<MedicalReplace>();

    MedicineSettingParam m_MedicineSettingParam = null;

    protected override void OnLoading()
    {
        base.OnLoading();
        itemPreafb = m_widget_itemprefab.gameObject;
        itemPreafb.SetActive(false);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if(data == null)
        {
            return;
        }

        m_MedicineSettingParam = (MedicineSettingParam)data;
        AutoRecoverGrid.MedicalType type = (AutoRecoverGrid.MedicalType)m_MedicineSettingParam.type; 
        OnShowItems(type);
    }

    public override void OnColliderMaskClicked() 
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    void OnShowItems(AutoRecoverGrid.MedicalType type)
    {
        m_trans_root.DestroyChildren();
        m_lstMedicalReplace.Clear();

        ItemManager itemMgr = DataManager.Manager<ItemManager>();
        List<table.ItemDataBase> items = null;
        switch (type)
        {
            case AutoRecoverGrid.MedicalType.Hp:
                items = GetItemListByType(GameCmd.ItemBaseType.ItemBaseType_Consumption, 1);
                break;
            case AutoRecoverGrid.MedicalType.Mp:
                items = GetItemListByType(GameCmd.ItemBaseType.ItemBaseType_Consumption, 2);
                break;
            case AutoRecoverGrid.MedicalType.HpAtOnce:
                items = GetItemListByType(GameCmd.ItemBaseType.ItemBaseType_Consumption, 3);
                break;
            case AutoRecoverGrid.MedicalType.PetHp:
                items = GetItemListByType(GameCmd.ItemBaseType.ItemBaseType_Consumption, 4);
                break;
            default:
                break;
        }

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject go = NGUITools.AddChild(m_trans_root.gameObject, m_widget_itemprefab.gameObject);
                go.transform.localPosition = new Vector3(0,-i * 100,0);
                MedicalReplace mr = go.AddComponent<MedicalReplace>();
                go.SetActive(true);
                mr.Init(this,items[i],type);
                m_lstMedicalReplace.Add(mr);
            }
        }
        m_trans_root.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    public void OnClickItem(uint itemBaseId)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: itemBaseId);
    }
    List<table.ItemDataBase> GetItemListByType(GameCmd.ItemBaseType itype, uint subtype)
    {
        List<table.ItemDataBase> items = new List<table.ItemDataBase>();
        List<table.ItemDataBase> itemtables = GameTableManager.Instance.GetTableList<table.ItemDataBase>();
        uint level = (uint)((null != DataManager.Instance.MainPlayer)
               ? DataManager.Instance.MainPlayer.GetProp((int)Client.CreatureProp.Level) : 0);
        for (int i = 0,imax = itemtables.Count; i < imax; i++)
        {
            if (itemtables[i].baseType == (uint)itype && itemtables[i].subType == subtype && itemtables[i].useLevel <= level)
            {
                items.Add(itemtables[i]);
            }
        }
        return items;
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            OnShow((MedicineSettingParam)param);
        }
        return base.OnMsg(msgid, param);
    }
}