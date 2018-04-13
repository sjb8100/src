
//*************************************************************************
//	创建日期:	2017/2/21 星期二 15:49:24
//	文件名称:	FashionTips
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using GameCmd;
using Engine;
using Engine.Utility;

enum FashionTipsType
{
    None = 0,
    ShowBuy = 1,//显示购买
    ShowEquip = 2,//显示装备
    ShowUnLoad = 3,//显示卸下
    ShowAddTime = 4,//显示续费
}
partial class FashionTips
{
    SuitDataBase m_suitDataBase = null;
    uint m_suitID = 0;
    protected override void OnLoading()
    {

        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null && data is SuitDataBase)
        {
            SuitDataBase db = data as SuitDataBase;
            InitDataBase(db);
        }
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.PropPanel))
        {
            Transform trans = this.transform.Find("ColliderMask");
            if (trans != null)
            {
                trans.gameObject.SetActive(false);
            }
        }
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();


        this.HideSelf();
    }

    void ClosePanel(GameObject go)
    {
        HideSelf();
    }
    public void InitDataBase(SuitDataBase db)
    {
        m_suitDataBase = db;
        if (db != null)
        {
            m_suitID = db.base_id;
            m_label_tipssuitname.text = db.name;
            m_label_tipsdes.text = db.desc;
            m_label_getwaydes.text = db.activeWay;
            ShowAttr();
            SuitDataManager dm = DataManager.Manager<SuitDataManager>();
            SuitState st = dm.GetSuitState(m_suitID);
            FashionTipsType ft = FashionTipsType.ShowBuy;
            if (st == SuitState.Show)
            {
                ft = FashionTipsType.None;
            }
            else if (st == SuitState.Equip)
            {
                ft = FashionTipsType.ShowUnLoad;
            }
            else if (st == SuitState.HasBuy)
            {
                ft = FashionTipsType.ShowEquip;
            }
            else if (st == SuitState.NoEffect)
            {
                ft = FashionTipsType.ShowAddTime;
            }
            else if (st == SuitState.Active)
            {
                ft = FashionTipsType.None;
            }
            else
            {
                ft = FashionTipsType.ShowBuy;
            }
            ShowBtn(ft);
        }
    }
    void ShowBtn(FashionTipsType type)
    {
        if (type == FashionTipsType.ShowBuy)
        {
            m_btn_AddTime.gameObject.SetActive(false);
            m_btn_unload.gameObject.SetActive(false);
            m_btn_Equip.gameObject.SetActive(false);
            m_btn_buy.gameObject.SetActive(true);
        }
        else if (type == FashionTipsType.ShowEquip)
        {
            m_btn_AddTime.gameObject.SetActive(true);
            m_btn_unload.gameObject.SetActive(false);
            m_btn_Equip.gameObject.SetActive(true);
            m_btn_buy.gameObject.SetActive(false);
        }
        else if (type == FashionTipsType.ShowUnLoad)
        {
            m_btn_AddTime.gameObject.SetActive(true);
            m_btn_unload.gameObject.SetActive(true);
            m_btn_Equip.gameObject.SetActive(false);
            m_btn_buy.gameObject.SetActive(false);
        }
        else if (type == FashionTipsType.None)
        {
            m_btn_AddTime.gameObject.SetActive(false);
            m_btn_unload.gameObject.SetActive(false);
            m_btn_Equip.gameObject.SetActive(false);
            m_btn_buy.gameObject.SetActive(false);
        }
        else if (type == FashionTipsType.ShowAddTime)
        {
            m_btn_AddTime.gameObject.SetActive(true);
            m_btn_unload.gameObject.SetActive(false);
            m_btn_Equip.gameObject.SetActive(false);
            m_btn_buy.gameObject.SetActive(false);
        }
    }
    void ShowAttr()
    {
        if (m_suitDataBase.typeMask == 1)
        {
            m_trans_AttrRoot.gameObject.SetActive(true);
            RefreshAttr();
            m_trans_getwaycontent.gameObject.SetActive(false);
        }
        else
        {
            m_trans_AttrRoot.gameObject.SetActive(false);
            m_trans_getwaycontent.gameObject.SetActive(true);
        }
    }
    void RefreshAttr()
    {
        if (m_suitDataBase == null)
        {
            return;
        }
        string addAttrStr = m_suitDataBase.effect_id;
        List<uint> effectList = StringUtil.GetSplitStringList<uint>(addAttrStr, '_');
        int count = m_trans_AttrRoot.childCount;
        int attrCount = effectList.Count;
        for (int i = 0; i < count; i++)
        {
            string name = (i + 1).ToString();
            Transform attrTrans = m_trans_AttrRoot.Find(name);
            if (attrTrans != null)
            {
                if (i < attrCount)
                {
                    attrTrans.gameObject.SetActive(true);
                    uint effectID = effectList[i];
                    StateDataBase edb = GameTableManager.Instance.GetTableItem<StateDataBase>(effectID);
                    if (edb != null)
                    {
                        UILabel nameLable = attrTrans.Find("attrname").GetComponent<UILabel>();
                        if (nameLable != null)
                        {
                            nameLable.text = edb.des;
                        }
                    }
                    else
                    {
                        UILabel nameLable = attrTrans.Find("attrname").GetComponent<UILabel>();
                        if (nameLable != null)
                        {
                            nameLable.text = "";
                        }
                    }
                }
                else
                {
                    attrTrans.gameObject.SetActive(false);
                }

            }
        }
    }
    void onClick_Equip_Btn(GameObject caster)
    {
        if (m_suitDataBase == null)
        {
            return;
        }
        stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
        cmd.type = SuitOPType.SuitOPType_Equip;
        //固定3当装备
        cmd.suit_id = (m_suitDataBase.base_id << 16) + 3;
        NetService.Instance.Send(cmd);
    }

    void onClick_AddTime_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BuyDressPanel, data: SuitOPType.SuitOPType_Renew);
    }

    void onClick_Buy_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BuyDressPanel, data: SuitOPType.SuitOPType_Buy);
    }

    void onClick_Unload_Btn(GameObject caster)
    {
        if (m_suitDataBase == null)
        {
            return;
        }
        stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
        cmd.type = SuitOPType.SuitOPType_Unequip;
        //固定3当装备
        cmd.suit_id = (m_suitDataBase.base_id << 16) + 3;
        NetService.Instance.Send(cmd);
    }
}