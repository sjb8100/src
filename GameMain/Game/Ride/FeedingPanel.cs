//*************************************************************************
//	创建日期:	2016-12-30 11:26
//	文件名称:	FeedingPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	坐骑自动喂食选择界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;


partial class FeedingPanel : UIPanelBase
{

    List<RideFeeditem> m_lstRideFeeditem = new List<RideFeeditem>(5);

    RideFeeditem m_preRideFeeditem = null;
    RideFeeditem m_currRideFeeditem = null;
    uint m_rideid;
    protected override void OnLoading()
    {
        base.OnLoading();
        for (int i = 1; i <= m_lstRideFeeditem.Capacity; i++)
        {
            AddFeedItem();
        }
        m_widget_item01.gameObject.SetActive(false);
    }

    private RideFeeditem AddFeedItem()
    {
        GameObject go = NGUITools.AddChild(m_trans_root.gameObject, m_widget_item01.gameObject);
        if (go != null)
        {
            go.SetActive(true);
            RideFeeditem item = go.AddComponent<RideFeeditem>();
            m_lstRideFeeditem.Add(item);
            return item;
        }
        return null;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data != null && data is uint)
        {
            m_rideid = (uint)data;
        }

        m_trans_root.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null) return;
        uint feedItemId = (uint)option.GetInt("MedicalSetting", "RideFeedId", 0);
        // 
        List<string> keys = GameTableManager.Instance.GetGlobalConfigKeyList("FeedItemRide");
        keys.Sort();
        RideFeeditem feeditem = null;
        int i = 0;
        for (; i < keys.Count; i++)
        {
            if (i >= m_lstRideFeeditem.Count)
            {
                feeditem = AddFeedItem();
            }
            else
            {
                feeditem = m_lstRideFeeditem[i];
            }
            uint itemid = uint.Parse(keys[i]);
            feeditem.gameObject.SetActive(true);
            feeditem.transform.localPosition = new UnityEngine.Vector3(0, -i * 105, 0);
            feeditem.InitUI(itemid, OnSelectItem);
            if (itemid == feedItemId)
            {
                OnSelectItem(feeditem);
            }
        }

        for (int k = i; k < m_lstRideFeeditem.Count; k++)
        {
            m_lstRideFeeditem[k].gameObject.SetActive(false);
        }
    }

    void OnSelectItem(RideFeeditem item)
    {
        m_preRideFeeditem = m_currRideFeeditem;
        if (m_preRideFeeditem != null)
        {
            m_preRideFeeditem.SetState(false);
        }

        m_currRideFeeditem = item;
        m_currRideFeeditem.SetState(true);

        m_rideid = m_currRideFeeditem.ItemId;
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_queding_Btn(GameObject caster)
    {
        DataManager.Manager<RideManager>().SetFeedConfig(m_rideid);
        //TipsManager.Instance.ShowTipsById(113510);
        this.HideSelf();
    }

}
