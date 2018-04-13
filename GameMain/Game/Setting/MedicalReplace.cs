using System;
using System.Collections.Generic;
using UnityEngine;

public class MedicalReplace : MonoBehaviour
{
    public class MedicalReplaceInfo
    {
        public AutoRecoverGrid.MedicalType type;
        public uint itemid;
    }

    UILabel lableName;
    UILabel lableDes;
    Transform iconTrans;
    AutoRecoverGrid.MedicalType mtype;
    UIItem uiitem;
    uint m_itemid;
    MedicineSettingPanel m_parent = null;
    void Awake()
    {
        lableName = transform.Find("name").GetComponent<UILabel>();
        lableDes = transform.Find("result").GetComponent<UILabel>();
        iconTrans = transform.Find("icon");
        Transform btn = transform.Find("btn_switch");
        if (btn != null)
        {
            UIEventListener.Get(btn.gameObject).onClick = OnReplace;
        }
    }

    void OnReplace(GameObject go)
    {
        if (uiitem != null)
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SettingPanel, UIMsgID.eReplaceMedical, new MedicalReplaceInfo()
            {
                type = mtype,
                itemid = m_itemid,
            });
            
        }

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MedicineSettingPanel);
    }

    public void Init(MedicineSettingPanel parent,table.ItemDataBase itemdatabase, AutoRecoverGrid.MedicalType type)
    {
        m_parent = parent;
        if (itemdatabase == null) return;
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemdatabase.itemID);
        mtype = type;
        m_itemid = itemdatabase.itemID;
        if (uiitem != null)
        {
            uiitem.Release();
            uiitem = null;
        }
        bool showGetWay = mtype != AutoRecoverGrid.MedicalType.HpAtOnce;
        if (num > 0)
        {
            uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemdatabase.itemID, (uint)num,0,null,null ,showGetWay);
        }
        else
        {
            uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemdatabase.itemID, (uint)num,0, (grid) => {
                m_parent.OnClickItem(grid.Data.DwObjectId);
            }, null, showGetWay);
        }
        if (uiitem != null && iconTrans != null)
        {
            uiitem.Attach(iconTrans);
            //uiitem.SetPosition(true, new Vector3(0, -iconTrans.GetComponent<UIWidget>().height * 0.5f, 0));
        }

        if (lableDes != null) lableDes.text = itemdatabase.description;
        if (lableName != null) lableName.text = itemdatabase.itemName;
    }
}