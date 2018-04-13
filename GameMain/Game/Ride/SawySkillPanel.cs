using System;
using System.Collections.Generic;
using UnityEngine;

partial class SawySkillPanel : UIPanelBase
{
    public class LearnSkillInfo
    {
        public uint rideid;
        public uint skillid;
    }

    LearnSkillInfo m_LearnSkillInfo = null;

    UIItem m_uiitem;
    uint m_nUseItemId = 0;
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();

    }

    protected override void OnLoading()
    {
        base.OnLoading();
        m_nUseItemId = GameTableManager.Instance.GetGlobalConfig<uint>("Ride_LearnSkill");
        m_sprite_xiaohao_icon.gameObject.SetActive(true);

        EventDelegate.Add(m_toggle_xiaohao_Sprite.onChange, () =>
        {
           
            OnSelectCoin();
        });
    }

    private void OnSelectCoin()
    {
        m_label_xiaohao_dianjuan.gameObject.SetActive(m_toggle_xiaohao_Sprite.value);
        if (m_toggle_xiaohao_Sprite.value)
        {
            uint itemid = GameTableManager.Instance.GetGlobalConfig<uint>("Ride_LearnSkill");
            int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemid);
            if (num > 0)
            {
                m_label_xiaohao_dianjuan.gameObject.SetActive(false);
                m_label_xiaohao_number.gameObject.SetActive(true);
            }
            else
            {
                table.PointConsumeDataBase database = GameTableManager.Instance.GetTableItem<table.PointConsumeDataBase>(itemid);
                if (database != null)
                {
                    int coin = MainPlayerHelper.GetMainPlayer().GetProp((int)Client.PlayerProp.Cold);
                    if (coin > database.buyPrice)
                    {
                        m_label_xiaohao_dianjuan.text = string.Format("{0}/{1}", coin, database.buyPrice);
                    }
                    else
                    {
                        m_label_xiaohao_dianjuan.text = string.Format("[ff0000]{0}[-]/{1}", coin, database.buyPrice);
                    }
                }

                m_label_xiaohao_number.gameObject.SetActive(false);
            }
        }
        else
        {
            m_label_xiaohao_number.gameObject.SetActive(true);
        }
    }
    private CMResAsynSeedData<CMAtlas> m_qCASD = null;
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is LearnSkillInfo)
        {
            m_LearnSkillInfo = data as LearnSkillInfo;
        }
        if (m_LearnSkillInfo == null)
        {
            return;
        }
        table.RideSkillDes skilldata = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(m_LearnSkillInfo.skillid);
        if (skilldata != null)
        {
            m_label_skill_effect_Label.text = skilldata.skillDesc;
            m_label_name.text = string.Format("领悟{0}", skilldata.skillName);
        }

        table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(m_nUseItemId);
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nUseItemId);
        if (itemdata != null)
        {
            if (m_uiitem != null)
            {
                m_uiitem.Release();
                m_uiitem = null;
            }
            if (num > 0)
            {
                m_uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemdata.itemID, (uint)num);
            }
            else
            {
                m_uiitem = DataManager.Manager<UIManager>().GetUICommonItem(itemdata.itemID, (uint)num, 0, OnGetItem);
            }
            if (m_uiitem != null)
            {
                m_uiitem.Attach(m_sprite_xiaohao_icon.cachedTransform);
                UIManager.GetQualityAtlasAsyn(itemdata.quality, ref m_qCASD, () =>
                {
                    if (null != m_sprite_itemqua)
                    {
                        m_sprite_itemqua.atlas = null;
                    }
                }, m_sprite_itemqua);
            }
            m_label_xiaohao_name.text = itemdata.itemName;
        }

        if (num >= 1)
        {
            m_label_xiaohao_number.text = string.Format("{0}/{1}", num, 1);
        }
        else
        {
            m_label_xiaohao_number.text = string.Format("[ff0000]{0}[-]/{1}", num, 1);
        }
        OnSelectCoin();
    }

    void OnGetItem(UIItemCommonGrid grid)
    {
         DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_nUseItemId);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eRideUpdateLearnSkill)
        {
            if (m_LearnSkillInfo != null)
            {
                OnShow(m_LearnSkillInfo);
            }
        }else if (msgid == UIMsgID.eShowUI)
        {
            LearnSkillInfo info = (LearnSkillInfo)param;
            OnShow(info);
        }
        return base.OnMsg(msgid, param);
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        m_LearnSkillInfo = null;
        if (null != m_qCASD)
        {
            m_qCASD.Release(true);
            m_qCASD = null;
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_cancel_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_lingwu_Btn(GameObject caster)
    {
        if (m_LearnSkillInfo != null)
        {
           // DataManager.Instance.Sender.RideGraspRideUser(m_LearnSkillInfo.rideid, m_LearnSkillInfo.skillid, m_toggle_xiaohao_Sprite.value);
        }
    }
}
