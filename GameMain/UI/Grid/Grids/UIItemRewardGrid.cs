using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
class UIItemRewardGrid : UIItemInfoGridBase
{
    #region define
    #endregion

    #region property
    private uint m_uint_id = 0;
    private uint ID
    {
        get
        {
            return m_uint_id;
        }
    }
    private BaseItem baseItem;
    private BaseItem BaseData
    {
        get
        {
            return baseItem;
        }
    }
    private Transform m_ts_none;
    private Transform mtsInfoRoot;
    private UILabel Name;
    UISprite Block; 
    //    private UILabel Num;
    Transform infoGrid;
    private BoxCollider boc;
    bool IsGetGodWeapen = false;
    uint WeapenDataID = 0;

    UISprite Mark;

    Transform Particle;

    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
       
        infoGrid = CacheTransform.Find("Content/InfoGridRoot/InfoGrid");
        m_ts_none = CacheTransform.Find("Content/None");
        mtsInfoRoot = CacheTransform.Find("Content/InfoGridRoot");
        Name = CacheTransform.Find("Content/InfoGridRoot/Name").GetComponent<UILabel>();
        Block = CacheTransform.Find("Content/InfoGridRoot/Block").GetComponent<UISprite>();
        Mark = CacheTransform.Find("Content/InfoGridRoot/Mark").GetComponent<UISprite>();
        boc = CacheTransform.Find("Content/InfoGridRoot/InfoGrid").GetComponent<BoxCollider>();
        Particle = CacheTransform.Find("Content/InfoGridRoot/Particle");

        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    public void SetGridData(uint id, uint num, bool nameVisible = true, bool circleStyle = false, bool numVisible = true, ItemSerialize data = null, bool blockVisible = false,bool hasGot =false, bool GodWeapenType =false, uint GodWeapenDataID = 0)
    {
        this.m_uint_id = id;
        bool empty = (id == 0);
        ResetInfoGrid();
        IsGetGodWeapen = !blockVisible && !hasGot && GodWeapenType;
        WeapenDataID = GodWeapenDataID;
        if (null != m_ts_none && m_ts_none.gameObject.activeSelf != empty)
        {
            m_ts_none.gameObject.SetActive(empty);
        }

        if (null != mtsInfoRoot && mtsInfoRoot.gameObject.activeSelf == empty)
        {
            mtsInfoRoot.gameObject.SetActive(!empty);
        }
        if (!empty)
        {
            baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(m_uint_id);
            if (baseItem == null)
            {
                if (data != null)
                {
                    baseItem = new BaseItem(m_uint_id, data);
                }
                else
                {
                    baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_id);
                }

            }


         
            SetBg(!circleStyle);
            bool isMatchJob = false;
            if (baseItem.BaseData != null)
            {
                isMatchJob = DataManager.IsMatchPalyerJob((int)baseItem.BaseData.useRole);
                if (!DataManager.IsMatchPalyerLv((int)baseItem.BaseData.useLevel)
                   || !isMatchJob)
                {
                    SetWarningMask(true);
                }
            }
            else
            {
                Engine.Utility.Log.Error("ID为{0}的奖励道具不存在,检查配置表格", m_uint_id);
            }

            bool fightPowerUp = false;
            if (isMatchJob
                && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.QWThisID, out fightPowerUp))
            {
                SetFightPower(true, fightPowerUp);
            }


            SetIcon(true, baseItem.Icon, circleStyle: circleStyle);
            SetBorder(true, baseItem.BorderIcon, circleStyle: circleStyle);
            if (baseItem.IsMuhon)
            {
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }

            SetBindMask(baseItem.IsBind);
            SetName(nameVisible, baseItem.Name);
            SetNum(numVisible, TextManager.GetFormatNumText(num));
            SetBlock(blockVisible);
            SetMark(hasGot);
            if (boc != null)
            {
                boc.enabled = true;
            }
            

            AddEffect(IsGetGodWeapen);
        }
    }
    public void SetBgAndBorder(bool show) 
    {
        if(m_baseGrid != null)
        {
            m_baseGrid.SetBg(show);
            m_baseGrid.SetLockMask(show);
            m_baseGrid.SetBorder(show);
            m_baseGrid.SetWarningMask(show);
        }
    }
    public void SetName(bool enable, string value)
    {
        if (Name != null)
        {
            Name.gameObject.SetActive(enable);
            Name.text = value;
        }
    }
    public void AddDrag()
    {
        if (infoGrid != null)
        {
            if (infoGrid.GetComponent<UIDragCacheScrollView>() == null)
            {
                infoGrid.gameObject.AddComponent<UIDragCacheScrollView>();
            }
        }
    }

    void SetBlock(bool value) 
    {
        if(Block !=  null)
        {
            Block.gameObject.SetActive(value);
        }
    }
    void SetMark(bool value)
    {
        if (Mark != null)
        {
            Mark.gameObject.SetActive(value);
        }
    }

    public void HideBoxCollider(bool active)
    {
        if (boc != null)
        {
            boc.enabled = !active;
        }
    }
    UIFrameAnimation p;
    public void AddEffect(bool showEffect, uint effectId = 52018) 
    {
        if (Particle != null)
        {
            Particle.gameObject.SetActive(showEffect);
            if (showEffect)
            {
                p = Particle.GetComponent<UIFrameAnimation>();
                if (p == null)
                {
                    p = Particle.gameObject.AddComponent<UIFrameAnimation>();
                    p.depth = 100;
                }
                if (p != null)
                {
                    p.SetDimensions(1, 1);
                    p.ReleaseParticle();
                    p.AddFrameParticle(effectId);
                }
            }
            else 
            {
                if (p != null)
                {
                    p.ReleaseParticle();
                }
            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        IsGetGodWeapen = false;
        WeapenDataID = 0;
        if (p != null)
        {
            p.ReleaseParticle();
        }
    }
    #endregion

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                        if (ID != 0)
                        {
                            if (infoGrid.NotEnough)
                            {
                                InvokeUIDlg(eventType, data, ID);
                            }
                            else
                            {
                                if (IsGetGodWeapen)
                                {
                                    NetService.Instance.Send(new stGetArticfactRewardDataUserCmd_CS() { id = WeapenDataID });
                                }
                                else
                                {
                                    TipsManager.Instance.ShowItemTips(BaseData);
                                }

                            }
                        }
                      }
                    }              
                break;
        }
    }
    #endregion
}