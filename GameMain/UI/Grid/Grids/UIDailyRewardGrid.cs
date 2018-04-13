using System;
using table;
using System.Collections;
using UnityEngine;
using GameCmd;
public class UIDailyRewardGrid : UIGridBase
{

    public uint id { set; get; }

    CMResAsynSeedData<CMAtlas> m_priceAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed_2 = null;

    UISprite m_sprite_Bgback;
    UISprite m_sprite_Icon;
    Transform m_trans_Particle;
    UILabel m_label_Num;

    UISprite m_sprite_Mark;

    bool hasGot = false;
    bool canGet = false;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_sprite_Bgback = CacheTransform.Find("bg_back").GetComponent<UISprite>();
        m_sprite_Icon = CacheTransform.Find("icon").GetComponent<UISprite>();
        m_trans_Particle = CacheTransform.Find("Particle");
        m_label_Num = CacheTransform.Find("Active_Num").GetComponent<UILabel>();
        m_sprite_Mark = CacheTransform.Find("Mark").GetComponent<UISprite>();
        UIEventListener.Get(gameObject).onClick = OnClickRewardBox;
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null && data is uint)
        {
            id = (uint)data;
            DailyAwardDataBase tab = GameTableManager.Instance.GetTableItem<DailyAwardDataBase>(id);
            if (tab != null)
            {
               
                m_label_Num.text = tab.liveness.ToString();
                hasGot = DataManager.Manager<DailyManager>().RewardBoxList.Contains(id);
                canGet = tab.liveness <= DataManager.Manager<DailyManager>().ActiveTotalValue;
                SetIcon(tab);
                AddEffect(canGet && !hasGot);
            }
        }       
    }

    void SetIcon(DailyAwardDataBase msg) 
    {
        string icon = hasGot ? msg.openicon : msg.closeicon;

        UIManager.GetAtlasAsyn(icon, ref m_priceAsynSeed, () =>
        {
            if (null != m_sprite_Icon)
            {
                m_sprite_Icon.atlas = null;
            }
        }, m_sprite_Icon);

        UIManager.GetAtlasAsyn(msg.BgIcon, ref m_priceAsynSeed_2, () =>
        {
            if (null != m_sprite_Bgback)
            {
                m_sprite_Bgback.atlas = null;
            }
        }, m_sprite_Bgback);

        m_sprite_Mark.gameObject.SetActive(hasGot);
    }

    void AddEffect(bool show) 
    {
        UIParticleWidget p = m_trans_Particle.GetComponent<UIParticleWidget>();
        if (p == null)
        {
            p = m_trans_Particle.gameObject.AddComponent<UIParticleWidget>();
            p.depth = 20;
        }
        if (p != null)
        {
            if (show)
            {
                p.SetDimensions(80, 80);
                p.ReleaseParticle();
                p.AddParticle(50013);
            }
            else
            {
                p.ReleaseParticle();
            }
        }
       
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_priceAsynSeed_2 != null)
        {
            m_priceAsynSeed_2.Release(depthRelease);
        }
        if (m_priceAsynSeed != null)
        {
            m_priceAsynSeed.Release(depthRelease);
        }
 
    }
    void OnClickRewardBox(GameObject caster) 
    {
        uint state = 0;
        if (hasGot)
        {
            state = 2;
        }
        else 
        {
            if (canGet)
            {
                state = 1;
            }
            else 
            {
                state = 0;
            }
        
        }
        ActiveTakeParam par = new ActiveTakeParam();
        par.type = ActiveTakeType.Daily;
        par.boxID = id;
        par.canGetState = state;
        par.ids = DataManager.Manager<DailyManager>().RewardBoxList;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ActiveTakePanel, data: par);
    }
}