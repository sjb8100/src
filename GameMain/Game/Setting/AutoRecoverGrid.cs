using System;
using System.Collections.Generic;
using UnityEngine;
using table;
public class AutoRecoverGrid : MonoBehaviour
{
    public enum MedicalType{
        Hp = 0,
        Mp = 1,
        HpAtOnce = 2,
        PetHp = 3,
        Max = 4,
    }

//     UIToggle toggle;
//     UILabel labledes;
//     UISlider slider;
//     UILabel lablePersen;
//     UISprite sprite;
//     Transform transIcon;
    MedicalType mtype;
    UIItem uiitem;
    uint m_itemid;
//     void Awake()
//     {
//         toggle = transform.Find("BaseHp").GetComponent<UIToggle>();
//         labledes = transform.Find("Label").GetComponent<UILabel>();
//         slider = transform.Find("BaseHp_slider").GetComponent<UISlider>();
//         sprite = slider.transform.Find("Sprite").GetComponent<UISprite>();
//         transIcon = transform.Find("BaseHp_icon");
//     }
    public MedicalType GetMedicalType()
    {
        return mtype;
    }

    public void Init(MedicalType type, GameObject targetTrans)
    {
        mtype = type;
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null)
        {
            return;
        }
        int itemid  = 0;
        if (type != MedicalType.HpAtOnce)
        {
            List<uint> medicalIDs = new List<uint>();
            List<ItemDataBase> items = GameTableManager.Instance.GetTableList<ItemDataBase>();
            uint level = (uint)((null != DataManager.Instance.MainPlayer)
               ? DataManager.Instance.MainPlayer.GetProp((int)Client.CreatureProp.Level) : 0);
            for (int i = 0; i < items.Count;i++ )
            {
                if (items[i].useLevel <= level)
                {
                    if (items[i].baseType == 2 && items[i].subType == (uint)type + 1)
                    {
                        medicalIDs.Add(items[i].itemID);
                    }
                }
            }
            if (medicalIDs.Count>0)
            {
                int exeTimes = 0;
                for (int m = 0; m < medicalIDs.Count;m++ )
                {
                    int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(medicalIDs[m]);
                    if (num != 0)
                    {
                        itemid = (int)medicalIDs[m];
                    }
                    else 
                    {
                        exeTimes++;
                    }
                }
                if (exeTimes == medicalIDs.Count)
                {
                    itemid = (int)medicalIDs[medicalIDs.Count-1]; 
                }
            }
        }
        else
        {
            itemid = option.GetInt("MedicalSetting", type.ToString() + "itemid", 0);
        }
        UpdateItem((uint)itemid, targetTrans);
    }

    public void UpdateItem(uint itemid, GameObject targetTrans)
    {
        if (uiitem != null)
        {
            uiitem.Release();
            uiitem = null;
        }
        m_itemid = itemid;
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
//        bool showGetWay = mtype == MedicalType.Hp || mtype == MedicalType.Mp; 
        uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemid, (uint)num,0,null,null);
        if (uiitem != null)
        {
            uiitem.Attach(targetTrans.transform);
            targetTrans.transform.GetComponent<UISprite>().enabled = false;
        }
        else
        {
            targetTrans.transform.GetComponent<UISprite>().enabled = true;
        }
    }

    public void Save()
    {
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null)
        {
            return;
        }
        if (uiitem != null)
	    {
            option.WriteInt("MedicalSetting", mtype.ToString() + "itemid", (int)m_itemid);
    	}
    }
}