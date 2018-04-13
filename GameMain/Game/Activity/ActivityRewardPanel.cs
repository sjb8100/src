using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;
partial class ActivityRewardPanel
{
    RecRetRewardData cur_rewData;
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        DataManager.Manager<ActivityManager>().ValueUpdateEvent += OnUpdateList;
        if (data != null)
        {
            if (data is RecRetRewardData)
           {
               cur_rewData = (RecRetRewardData)data;
               InitPanel();
           }
        }
    }
    void InitPanel() 
    {
        if (m_ctor_itemRoot != null)
        {
            m_ctor_itemRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnActivityListUpdate,null);
        }
        CreateItem();
        m_label_text.text = cur_rewData.rewarddesc;
        m_label_title.text = cur_rewData.rewardtitle;
        bool canGet = cur_rewData.state == 0;
        m_btn_lingquBtn.gameObject.SetActive(canGet);
        m_sprite_Status_Received.gameObject.SetActive(!canGet);
    }
    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnGetRechargeRewRet"))
        {
            ulong id = (ulong)value.oldValue;
            
            if (cur_rewData.rewardid == id)
            {
                uint ret = (uint)value.newValue;
                cur_rewData.state = ret;
                bool canGet = cur_rewData.state == 0;
                m_btn_lingquBtn.gameObject.SetActive(canGet);
                m_sprite_Status_Received.gameObject.SetActive(!canGet);
            }

//             if (value.newValue != null)
//             {
//                 uint id = (uint)value.newValue;
//                 if (m_lst_dailyGiftIDs.Contains(id))
//                 {
//                     m_ctor_DailyGiftRoot.UpdateData(m_lst_dailyGiftIDs.IndexOf(id));
//                 }
//             }
        }
    }


    void OnActivityListUpdate(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            if (items != null)
            {
                if (index < items.Count)
                {
                    UIItemRewardGrid data = grid as UIItemRewardGrid;
                    data.SetGridData(items[index].itemid, items[index].itemnum,false);
                }
            }
         
        }
    }
    List<RecRetRewardData.ItemInfo> items = null;
    void CreateItem() 
    {
        if (cur_rewData != null)
        {
            if (items == null)
            {
                items = new List<RecRetRewardData.ItemInfo>();
            }
            items.Clear();
            items.AddRange(cur_rewData.data);
            m_ctor_itemRoot.CreateGrids(cur_rewData.data.Count);
        }
    }

    void onClick_LingquBtn_Btn(GameObject caster)
    {
        if(cur_rewData != null)
        {
            Action agree = delegate
            {
                NetService.Instance.Send(new stGetRechargeRewRetPropertyUserCmd_CS() { rewardid = cur_rewData.rewardid, ret = cur_rewData.state });
            };
            string str = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_Rebate_4);
            TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, str, agree, okstr: "确定", cancleStr: "取消", title: "提示");
        }
      
    }

    protected override void OnHide()
    {
        base.OnHide();
        DataManager.Manager<ActivityManager>().ValueUpdateEvent -= OnUpdateList;
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
}

