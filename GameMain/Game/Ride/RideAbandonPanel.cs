using System;
using System.Collections.Generic;
using UnityEngine;

partial class RideAbandonPanel : UIPanelBase
{

    RideData m_ridedata;
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data is RideData)
        {
            m_ridedata = data as RideData;
            m_label_rarity.text = DataManager.Manager<RideManager>().GetRideQualityStr(m_ridedata.quality);
            m_label_level.text = m_ridedata.level.ToString();

            table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(m_ridedata.baseid, (int)m_ridedata.level);
            if(feeddata != null){
                float value =  (feeddata.speed / 10000.0f) * 100;
                m_label_speed.text = value.ToString() + "%";
            }

            m_slider_satiationscorllbar.value = m_ridedata.repletion * 1.0f / m_ridedata.maxRepletion;
            m_slider_satiationscorllbar.transform.Find("Percent").GetComponent<UILabel>().text = string.Format("{0}/{1}", m_ridedata.repletion, m_ridedata.maxRepletion);
            m_label_life.text = m_ridedata.life.ToString();
            m_sprite_icon.spriteName = m_ridedata.icon;
           

            if (m_ridedata.life > 0)//封印
            {
                m_widget_fengyin.gameObject.SetActive(true);
                m_widget_qiuqi.gameObject.SetActive(false);
                m_label_name.text = string.Format("封印{0}", m_ridedata.name);
                m_btn_btn_diuqi.GetComponentInChildren<UILabel>().text = "封印";

                m_label_change_life.text = (m_ridedata.life - m_ridedata.subLife).ToString();
                m_label_life.text = m_ridedata.life.ToString();
            }
            else
            {
                m_widget_fengyin.gameObject.SetActive(false);
                m_widget_qiuqi.gameObject.SetActive(true);
                m_label_name.text = string.Format("丢弃{0}", m_ridedata.name);
                m_btn_btn_diuqi.GetComponentInChildren<UILabel>().text = "丢弃";

            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        m_ridedata = null;
    }
    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_cancel_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_diuqi_Btn(GameObject caster)
    {
        if (m_ridedata != null)
        {
            if (m_ridedata.life > 0)
            {
               // DataManager.Instance.Sender.RideSealRideUser(m_ridedata.id);
            }
            else
            {
                DataManager.Instance.Sender.RideRemoveRideUser(m_ridedata.id);
            }
        }
        this.HideSelf();
    }


}
