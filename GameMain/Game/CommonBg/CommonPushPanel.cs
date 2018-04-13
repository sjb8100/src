
using UnityEngine;
using table;
using Client;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;
partial class CommonPushPanel : UIPanelBase
{
    PushType m_pushType;
    int m_nCountDown;
    uint m_uThisID;

    protected override void OnLoading()
    {
        base.OnLoading();
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        return base.OnMsg(msgid, param);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }
    public void InitPushItem(PushItem item)
    {

        if (item == null)
        {
            return;
        }
        m_pushType = item.pushType;
        m_uThisID = item.thisID;
        //是不是成就推送
        bool isAchievementPush = m_pushType == PushType.Achievement;
        m_trans_NormalRoot.gameObject.SetActive(!isAchievementPush);
        m_trans_AchievementRoot.gameObject.SetActive(isAchievementPush);
        if (isAchievementPush)
        {
            m_nCountDown = GameTableManager.Instance.GetGlobalConfig<int>("AchievementCountDown");
            m_label_AchievementName.text = item.strName;
            UIEventListener.Get(m_label_ClickLabel.gameObject).onClick = OnClickLabel;
        }
        else 
        {
            if (item.pushType == PushType.Equip)
            {
                m_trans_EffectContent.gameObject.SetActive(true);
                BaseEquip be = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseEquip>(item.thisID);
                if (be != null)
                {
                    m_label_power.text = be.Power.ToString();
                }
                m_nCountDown = GameTableManager.Instance.GetGlobalConfig<int>("EquipUseCountDown");
            }
            else if (item.pushType == PushType.Consum)
            {
                m_trans_EffectContent.gameObject.SetActive(false);
                m_nCountDown = GameTableManager.Instance.GetGlobalConfig<int>("DanyaoUseCountDown");
            }
            BaseItem bi = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(item.thisID);
            if (bi != null)
            {
                ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(bi.BaseId);

                if (db != null)
                {
                    m_label_Name.text = db.itemName;

                    UIManager.GetTextureAsyn(db.itemIcon, ref m_curIconAsynSeed, () =>
                    {
                        if (null != m__Icon)
                        {
                            m__Icon.mainTexture = null;
                        }
                    }, m__Icon);
                    UIManager.GetAtlasAsyn(bi.BorderIcon, ref m_curQualityAsynSeed, () =>
                    {
                        if (null != m_sprite_qualityIcon)
                        {
                            m_sprite_qualityIcon.atlas = null;
                        }
                    }, m_sprite_qualityIcon);
                }
            }
        }      
    }
    void OnClickLabel(GameObject go) 
    {
        if (m_pushType == PushType.Achievement && m_uThisID !=0)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AchievementPanel, data: m_uThisID);
            HideSelf();
        }
    }

    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    void ShowUPArrow(bool bShow)
    {
        m_sprite_UpArrpw.gameObject.SetActive(bShow);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release(depthRelease);
            m_curQualityAsynSeed = null;
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    void onClick_BtnUse_Btn(GameObject caster)
    {
        if (m_pushType == PushType.Equip)
        {
            DataManager.Manager<EquipManager>().EquipItem(m_uThisID, true);
        }
        else if (m_pushType == PushType.Consum)
        {
            DataManager.Manager<ItemManager>().Use(m_uThisID, 1);
        }
        PushNext();
    }

    void PushNext()
    {
        HideSelf();
        DataManager.Manager<CommonPushDataManager>().PopPushPanel();

    }
    void onClick_BtnClose_Btn(GameObject caster)
    {
        PushNext();
    }
    void onClick_AchievementClose_Btn(GameObject caster)
    {
        PushNext();
    }


    public void RefreshLabel()
    {

        if (m_nCountDown <= 0)
        {
            PushNext();
            return;
        }
        m_nCountDown -= 1;
        string showStr = "";
        if (m_pushType == PushType.Equip)
        {
            showStr = string.Format("{0}({1})", CommonData.GetLocalString("装备"), m_nCountDown);
        }
        else if (m_pushType == PushType.Consum)
        {
            showStr = string.Format("{0}({1})", CommonData.GetLocalString("使用"), m_nCountDown);
        }
        m_label_btnLable.text = showStr;

    }
}
