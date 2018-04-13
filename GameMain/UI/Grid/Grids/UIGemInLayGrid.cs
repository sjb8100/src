using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIGemInLayGrid : UIGridBase
{
    #region Property
    //图标
    private UITexture m_spIcon;
    //图标
    private UISprite m_spBorder;
    //名称
    private UILabel m_labLv;
    //名称
    private UILabel m_labLockLv;

    private UISprite m_spr_redPoint;
    private Transform m_tsLock = null;
    private Transform m_tsAdd = null;
    private EquipManager.EquipGridIndexType data = EquipManager.EquipGridIndexType.None;
    public EquipManager.EquipGridIndexType Data
    {
        get
        {
            return data;
        }
    }
    #endregion

    #region override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labLv = CacheTransform.Find("Content/Des/Lv").GetComponent<UILabel>();
        
        m_spIcon = CacheTransform.Find("Content/Icon").GetComponent<UITexture>();
        m_spBorder = CacheTransform.Find("Content/Border").GetComponent<UISprite>();
        m_tsAdd = CacheTransform.Find("Content/Add");
        m_tsLock = CacheTransform.Find("Content/LockContent");
        m_labLockLv = CacheTransform.Find("Content/LockContent/LockDes").GetComponent<UILabel>();
        m_spr_redPoint = CacheTransform.Find("Content/RedPoint").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.data = (EquipManager.EquipGridIndexType)data;
        Reset();
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
    }

    public override void Reset()
    {
        base.Reset();
        if (null != m_labLv)
        {
            m_labLv.enabled = false;
        }
        if (null != m_spIcon && m_spIcon.gameObject.activeSelf)
        {
            m_spIcon.gameObject.SetActive(false);
        }
        if (null != m_spBorder && m_spBorder.gameObject.activeSelf)
        {
            m_spBorder.gameObject.SetActive(false);
        }
        if (null != m_tsAdd && m_tsAdd.gameObject.activeSelf)
        {
            m_tsAdd.gameObject.SetActive(false);
        }
        if (null != m_tsLock && m_tsLock.gameObject.activeSelf)
        {
            m_tsLock.gameObject.SetActive(false);
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_inlayCASD)
        {
            m_inlayCASD.Release(true);
            m_inlayCASD = null;
        }

        if (null != m_borderCASD)
        {
            m_borderCASD.Release(true);
            m_borderCASD = null;
        }
    }
    #endregion
    public void SetRedPointStatus(bool enable) 
    {
        if (m_spr_redPoint != null)
        {
            m_spr_redPoint.gameObject.SetActive(enable);
        }
    }

    #region Op

    CMResAsynSeedData<CMTexture> m_inlayCASD = null;
    CMResAsynSeedData<CMAtlas> m_borderCASD = null;
    /// <summary>
    /// 刷新格子数据
    /// </summary>
    public void UpdateGridData(GameCmd.EquipPos selectPos)
    {
        Reset();
        EquipManager emgr = DataManager.Manager<EquipManager>();
        uint inlayBaseId = 0;
        if (emgr.TryGetEquipGridInlayGem(selectPos, data, out inlayBaseId))
        {
            Gem gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(inlayBaseId, ItemDefine.ItemDataType.Gem);
            if (null != m_spIcon)
            {
                if (!m_spIcon.gameObject.activeSelf)
                    m_spIcon.gameObject.SetActive(true);
                UIManager.GetTextureAsyn(gem.Icon, ref m_inlayCASD, () =>
                    {
                        if (null != m_spIcon)
                        {
                            m_spIcon.mainTexture = null;
                        }
                    }, m_spIcon);
            }

            if (null != m_spBorder)
            {
                if (!m_spBorder.gameObject.activeSelf)
                    m_spBorder.gameObject.SetActive(true);
                string iconName = UIManager.GetIconName(gem.BorderIcon, true);
                UIManager.GetAtlasAsyn(iconName, ref m_borderCASD, () =>
                {
                    if (null != m_spBorder)
                    {
                        m_spBorder.atlas = null;
                    }
                }, m_spBorder);
            }

            if (null != m_labLv)
            {
                if (!m_labLv.enabled)
                    m_labLv.enabled = true;
                m_labLv.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_Txt_Set_4, gem.BaseData.grade) ;
            }
        }
        else if (emgr.IsUnlockEquipGridIndex(data))
        {
            if (null != m_tsAdd)
            {
                m_tsAdd.gameObject.SetActive(true);
            }          
        }
        else
        {
            if (null != m_tsLock)
            {
                m_tsLock.gameObject.SetActive(true);
                m_labLockLv.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_Txt_Set_4
                    , emgr.GetEquipGridUnlockLevel(data)) ;
            }      
        }
        bool showRed = DataManager.Manager<ForgingManager>().EquipPosCanInlay(selectPos, this.data);
        SetRedPointStatus(showRed);
    }
    #endregion
}