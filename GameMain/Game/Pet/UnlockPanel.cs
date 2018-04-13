//-----------------------------------------
//此文件自动生成，请勿手动修改
//生成日期: 6/25/2016 2:13:41 PM
//-----------------------------------------
using UnityEngine;

using GameCmd;
partial class UnlockPanel
{

    uint m_nSkillNeedItemID = 0;
    uint m_nNeedNum = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow( data );
        uint itemID = GameTableManager.Instance.GetGlobalConfig<uint>( "MaxPetItem" );

        m_nNeedNum =(uint) DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        m_nSkillNeedItemID = itemID;
        UIItem.AttachParent(m_sprite_unlock_item_icon.transform, itemID, (uint)m_nNeedNum, ShowGetWayCallBack);
        table.ItemDataBase itemData = GameTableManager.Instance.GetTableItem<table.ItemDataBase>( itemID );
        if(itemData != null)
        {
            m_label_unlock_item_name.text = itemData.itemName;
        }
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId( (uint)itemID );
      //  m_label_unlock_item_number.text = num.ToString();
    }
    void ShowGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nSkillNeedItemID);
        if (  itemCount == 0)
        {
            TipsManager.Instance.ShowItemTips(m_nSkillNeedItemID);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(m_nSkillNeedItemID);
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    void onClick_Unlock_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Unlock_quxiao_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Unlock_queding_Btn(GameObject caster)
    {
        stAddMaxNumPetUserCmd_CS cmd = new stAddMaxNumPetUserCmd_CS();
        NetService.Instance.Send( cmd );
        HideSelf();
    }


}
