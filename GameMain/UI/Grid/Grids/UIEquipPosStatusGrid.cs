using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIEquipPosStatusGrid : UIItemInfoGridBase
{
    #region Property
    //装备名称
    private UILabel m_labName;
    //强化描述
    private UILabel m_labStrengthenDes;
    //装备图标
    private UISprite icon;
    private UISprite bg;
    //选中标示
    private GameObject selectMask;
    //已装备标示
    private GameObject equipMask;
    //宝石孔Content
    private GameObject gemHoleContent;
    //宝石孔图标1
    private UITexture gemHoleIcon1;
    //宝石孔图标2
    private UITexture gemHoleIcon2;
    //宝石孔图标3
    private UITexture gemHoleIcon3;
    //宝石孔Lock1
    private UISprite m_sp_lock1;
    //宝石孔lock2
    private UISprite m_sp_lock2;
    //宝石孔lock3
    private UISprite m_sp_lock3;

    private UISprite m_spr_redPoint;

    private Transform m_trans_Particle;
    //显示宝石孔时，装备名称位置
    public float gemShowNameYPos = 16;

    private GameCmd.EquipPos data = GameCmd.EquipPos.EquipPos_None;
    public GameCmd.EquipPos Data
    {
        get
        {
            return data;
        }
    }
    private UIToggle m_tg_hightlight = null;
    #endregion
    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labName = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_labStrengthenDes = CacheTransform.Find("Content/StrengthenDes").GetComponent<UILabel>();
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        icon = CacheTransform.Find("Content/Icon").GetComponent<UISprite>();
        gemHoleContent = CacheTransform.Find("Content/GemHoleContent").gameObject;
        gemHoleIcon1 = CacheTransform.Find("Content/GemHoleContent/GemHoleIcon_1").GetComponent<UITexture>();
        gemHoleIcon2 = CacheTransform.Find("Content/GemHoleContent/GemHoleIcon_2").GetComponent<UITexture>();
        gemHoleIcon3 = CacheTransform.Find("Content/GemHoleContent/GemHoleIcon_3").GetComponent<UITexture>();
        m_sp_lock1 = CacheTransform.Find("Content/GemHoleContent/GemLock_1").GetComponent<UISprite>();
        m_sp_lock2 = CacheTransform.Find("Content/GemHoleContent/GemLock_2").GetComponent<UISprite>();
        m_sp_lock3 = CacheTransform.Find("Content/GemHoleContent/GemLock_3").GetComponent<UISprite>();
        m_spr_redPoint = CacheTransform.Find("Content/RedPoint").GetComponent<UISprite>();
        m_trans_Particle = CacheTransform.Find("Content/Particle");
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }
    public override void SetGridData(object data)
    {
        
    }

    public void SetGridViewData(GameCmd.EquipPos pos,bool inlay = false)
     {
         this.data = pos;
         if (null != m_labName)
         {
             m_labName.text = string.Format("{0}.部位", EquipDefine.GetEquipPosName(this.data));
         }

        if (null != gemHoleContent && gemHoleContent.activeSelf != inlay)
        {
            gemHoleContent.gameObject.SetActive(inlay);
        }

        if (null != m_labStrengthenDes && m_labStrengthenDes.gameObject.activeSelf == inlay)
        {
            m_labStrengthenDes.gameObject.SetActive(!inlay);
        }
        uint StrengthenLv = 0;
        if (!inlay)
        {
            StrengthenLv =  DataManager.Manager<EquipManager>().GetGridStrengthenLvByPos(this.data);
            m_labStrengthenDes.text = string.Format("强化 {0}级"
                , StrengthenLv);
        }

         uint equipId = 0;
         bool equip = DataManager.Manager<EquipManager>().IsEquipPos(this.data, out equipId);
         ResetInfoGrid(equip);
         if (equip)
         {
             Equip itemData = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(equipId);
             SetIcon(true, itemData.Icon);
             SetBorder(true, itemData.BorderIcon);
             SetBindMask(itemData.IsBind);

             uint particleID = DataManager.Manager<EquipManager>().GetEquipParticleIDByStrengthenLevel(StrengthenLv);
             AddParticle(particleID);
         }
         else 
         {
             if(particle != null)
             {
                 particle.ReleaseParticle();
             }
         }
         if (null != icon)
         {
             if (icon.gameObject.activeSelf == equip)
                 icon.gameObject.SetActive(equip);
             if (!equip)
             {
                 string iconName = EquipDefine.GetEquipPartIcon(this.data);
                 UIManager.GetAtlasAsyn(iconName, ref m_bgIcon, () =>
                     {
                         if (null != icon)
                         {
                             icon.atlas = null;
                         }
                     }, icon);
             }
         }
        bool match  =false;
         if (inlay)
         {
             for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
             {
                 if (DataManager.Manager<ForgingManager>().EquipPosCanInlay(this.data, i))
                 {
                     match = true;
                 }
             }
         }
         else 
         {
             match = DataManager.Manager<ForgingManager>().JudgeEquipPosCanStrengthen(this.data);
         }
         SetRedPointStatus(match);
     }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }

        if (null != m_bgIcon)
        {
            m_bgIcon.Release(true);
            m_bgIcon = null;
        }

        if (null != m_inlayIcon1)
        {
            m_inlayIcon1.Release(true);
            m_inlayIcon1 = null;
        }

        if (null != m_inlayIcon2)
        {
            m_inlayIcon2.Release(true);
            m_inlayIcon2 = null;
        }

        if (null != m_inlayIcon3)
        {
            m_inlayIcon3.Release(true);
            m_inlayIcon3 = null;
        }
        if (particle != null)
        {
            particle.ReleaseParticle();
        }
    }

    public void SetRedPointStatus(bool enable)
    {
        if (m_spr_redPoint != null)
        {
            m_spr_redPoint.gameObject.SetActive(enable);
        }
    }
    #endregion

    UIFrameAnimation particle = null;
    void AddParticle(uint effectid)
    {
        if (m_trans_Particle != null)
        {
            bool showEffect = effectid != 0;
            m_trans_Particle.gameObject.SetActive(showEffect);
            particle = m_trans_Particle.GetComponent<UIFrameAnimation>();
            if (showEffect)
            {              
                if (particle == null)
                {
                    particle = m_trans_Particle.gameObject.AddComponent<UIFrameAnimation>();
                    particle.depth = 100;
                }
                if (particle != null)
                {
                    particle.SetDimensions(1, 1);
                    particle.ReleaseParticle();
                    particle.AddFrameParticle(effectid);
                }
            }
            else
            {
                if (particle != null)
                {
                    particle.ReleaseParticle();
                }
            }
        }
    }
    #region Set
    private CMResAsynSeedData<CMAtlas> m_bgIcon = null;
    private CMResAsynSeedData<CMTexture> m_inlayIcon1 = null;
    private CMResAsynSeedData<CMTexture> m_inlayIcon2 = null;
    private CMResAsynSeedData<CMTexture> m_inlayIcon3 = null;
    /// <summary>
    /// 設置鑲嵌寶石
    /// </summary>
    /// <param name="index"></param>
    /// <param name="iconName"></param>
    public void SetInlayIcon(EquipManager.EquipGridIndexType index,bool unlock = true,bool inlay = false,string iconName = "")
    {
        UITexture icon = null;
        UISprite lockIcon = null;
        CMResAsynSeedData<CMTexture> m_inlayIcon = null;
        Action action = null;
        switch (index)
        {
            case EquipManager.EquipGridIndexType.First:
                icon = gemHoleIcon1;
                lockIcon = m_sp_lock1;
                m_inlayIcon = m_inlayIcon1;
                action = ()=>
                    {
                        if (null != gemHoleIcon1)
                        {
                            gemHoleIcon1.mainTexture = null;
                        }
                    };
                
                break;
            case EquipManager.EquipGridIndexType.Second:
                icon = gemHoleIcon2;
                lockIcon = m_sp_lock2;
                m_inlayIcon = m_inlayIcon2;
                action = ()=>
                    {
                        if (null != gemHoleIcon2)
                        {
                            gemHoleIcon2.mainTexture = null;
                        }
                    };
                break;
            case EquipManager.EquipGridIndexType.Third:
                icon = gemHoleIcon3;
                lockIcon = m_sp_lock3;
                m_inlayIcon = m_inlayIcon3;
                action = ()=>
                    {
                        if (null != gemHoleIcon3)
                        {
                            gemHoleIcon3.mainTexture = null;
                        }
                    };
                break;
        }
        if (null != lockIcon && lockIcon.gameObject.activeSelf == unlock)
        {
            lockIcon.gameObject.SetActive(!unlock);
        }

        if (null != icon)
        {
            if (icon.gameObject.activeSelf != inlay)
            {
                icon.gameObject.SetActive(inlay);
            }
            if (inlay && !string.IsNullOrEmpty(iconName))
            {
                UIManager.GetTextureAsyn(iconName, ref m_inlayIcon, action, icon,false);
            }
        }
    }

    //public void EnableNotice(bool enable)
    //{
    //    if (null != noticePoint && noticePoint.enabled != enable)
    //        noticePoint.enabled = enable;
    //}

    #endregion
}