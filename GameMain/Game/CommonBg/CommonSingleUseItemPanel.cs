//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;
using System;
using table;
using System.Collections.Generic;
using Client;
using GameCmd;
public struct CommonSingleParam
{
   public string titletips;//标题文字
   public string contentdes;//内容文字
   public string autobuydes;//自动购买描述
   public uint itemID;//消耗道具id
   public uint consumNum;//消耗道具数量
   public bool bShowAutoBuy;//是否显示自动够买
   public string oktext;//ok按钮上的文字
   public string canceltext;//
   public Action cancleAction;
   public Action<bool> okAction;
}
partial class CommonSingleUseItemPanel: UIPanelBase
{
    bool bAutoBuy = false;
    Action _cancleAction;
    Action<bool> _okAction;
    bool bCanOk = false;
 
    uint m_uNeedMoney = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_sprite_biankuang.gameObject).onClick = OnAutoBuyClick;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if(data is CommonSingleParam)
        {
            CommonSingleParam sp = (CommonSingleParam)data ;
            m_label_title.text = sp.titletips;
            m_label_contentdes.text = sp.contentdes;
            m_label_autobuydes.text = sp.autobuydes;
            m_label_oktext.text = sp.oktext;
            m_label_canceltext.text = sp.canceltext;
            m_sprite_autoFlag.gameObject.SetActive(sp.bShowAutoBuy);
            bAutoBuy = sp.bShowAutoBuy;
            uint itemID = sp.itemID;
            ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(sp.itemID);
            if(db != null)
            {
                int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                UIItem.AttachParent(m_sprite_unlock_item_icon.transform, itemID, (uint)itemCount);
                if(itemCount >= sp.consumNum)
                {
                    bCanOk = true;
                }
                else
                {
                    bCanOk = false;
                }
                m_label_unlock_item_name.text = db.itemName;
                m_label_neednum.text = StringUtil.GetNumNeedString(itemCount, sp.consumNum);
               
            }
            PointConsumeDataBase pdb = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>(itemID);
            if(pdb != null)
            {
                m_label_moneynum.text = pdb.buyPrice.ToString();
                m_uNeedMoney = pdb.buyPrice;
            }
            _cancleAction = sp.cancleAction;
            _okAction = sp.okAction;
            ShowItemNum(!bAutoBuy);
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    void OnAutoBuyClick(GameObject go)
    {
        bAutoBuy = !bAutoBuy;

        m_sprite_autoFlag.gameObject.SetActive(bAutoBuy);
        ShowItemNum(!bAutoBuy);
    }
    void ShowItemNum(bool bShow)
    {

        m_trans_moneycontent.gameObject.SetActive(!bShow);
        m_label_neednum.gameObject.SetActive(bShow);
    }
    void onClick_Unlock_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Cancel_Btn(GameObject caster)
    {
        if(_cancleAction != null)
        {
            _cancleAction();

        }
        HideSelf();
    }

    void onClick_Ok_Btn(GameObject caster)
    {
        if(!bAutoBuy)
        {
            if (!bCanOk)
            {
                TipsManager.Instance.ShowTipsById(6);
                return;
            }
        }
        else
        {
            bool bfull = MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, m_uNeedMoney);
            if(!bfull)
            {
                return;
            }
        }
   
        if(_okAction !=null)
        {
            _okAction(bAutoBuy);
        }
        HideSelf();
    }


}
