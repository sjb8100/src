using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIEquipGrid : UIItemInfoGridBase
{
    #region Property
    //部位名称
    //private UILabel name;
    private UISprite m_sp_preview = null;

    private UILabel m_labStrengthenLv = null;
    private Transform m_tsStrengthen = null;
    private Transform m_tsParticle = null;
    private BaseItem data;
    public BaseItem Data
    {
        get
        {
            return data;
        }
    }
    private ClientSuitData m_suitData;
    public ClientSuitData SuitData
    {
        get
        {
            return m_suitData;
        }
    }

    private GameCmd.EquipPos m_emEPos = GameCmd.EquipPos.EquipPos_None;
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        m_labStrengthenLv = gameObject.transform.Find("Content/Strengthen/Label").GetComponent<UILabel>();
        m_tsStrengthen = gameObject.transform.Find("Content/Strengthen");
        m_sp_preview = gameObject.transform.Find("Content/Preview").GetComponent<UISprite>();
        m_tsParticle = gameObject.transform.Find("Content/Particle");
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        bool empty = (null == data);
        ResetInfoGrid(!empty);
        if (empty)
        {
            this.data = null;
            if (particle != null)
            {
                particle.ReleaseParticle();
            }
            return;
        }
        this.data = data as BaseEquip;
        SetIcon(true, this.data.Icon);
        SetBorder(true, this.data.BorderIcon);
        SetBindMask(this.data.IsBind);
        SetTimeLimitMask(false);
        if (this.data.IsMuhon)
        {
            SetMuhonMask(true, Muhon.GetMuhonStarLevel(this.data.BaseId));
            SetMuhonLv(true, Muhon.GetMuhonLv(this.data));
        }
        if (this.data.IsEquip)
        {
            Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(this.data.QWThisID);
            if (equip != null)
            {
                bool HaveDurable = equip.HaveDurable;
                bool warningMaskEnable = !HaveDurable;
                SetDurableMask(!HaveDurable);
           
            } 
        }
    }
    UIFrameAnimation particle;
    void AddParticle(uint effectid) 
    {
        if (m_tsParticle != null)
        {
            bool showEffect = effectid != 0;
            m_tsParticle.gameObject.SetActive(showEffect);
            particle = m_tsParticle.GetComponent<UIFrameAnimation>();
            if (showEffect)
            {             
                if (particle == null)
                {
                    particle = m_tsParticle.gameObject.AddComponent<UIFrameAnimation>();
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
    public void UpdateStrengthenInfo(uint stLv,bool haveData)
    {
        if (m_emEPos == GameCmd.EquipPos.EquipPos_SoulOne 
            || m_emEPos == GameCmd.EquipPos.EquipPos_SoulTwo 
            || m_emEPos == GameCmd.EquipPos.EquipPos_None)
        {
            return;
        }
        if (null != m_labStrengthenLv)
        {
            m_labStrengthenLv.text = stLv.ToString();
        }
        if (haveData)
        {
            uint particleID = DataManager.Manager<EquipManager>().GetEquipParticleIDByStrengthenLevel(stLv);
            AddParticle(particleID);
        }    
    }

    /// <summary>
    /// 初始化装备格子
    /// </summary>
    /// <param name="pos"></param>
    public void InitEquipGrid(GameCmd.EquipPos pos)
    {
        m_emEPos = pos;
        bool visible = (m_emEPos != GameCmd.EquipPos.EquipPos_SoulOne
            && m_emEPos != GameCmd.EquipPos.EquipPos_SoulTwo
            && m_emEPos != GameCmd.EquipPos.EquipPos_None);
        
        if (null != m_tsStrengthen && m_tsStrengthen.gameObject.activeSelf != visible)
        {
            m_tsStrengthen.gameObject.SetActive(visible);
        }
        SetPreView(EquipDefine.GetEquipPartIcon(pos));
    }

    /// <summary>
    /// 初始化时装格子
    /// </summary>
    /// <param name="suitType"></param>
    public void InitSuitGrid(GameCmd.EquipSuitType suitType)
    {
        SetPreView(EquipDefine.GetSuitIcon(suitType));
    }

    public void SetSuitData(ClientSuitData data)
    {
        m_suitData = data;
        ResetInfoGrid(m_suitData.suitBaseID != 0);
        table.SuitDataBase database = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(m_suitData.suitBaseID, 1);
        if (database != null)
        {
            SetIcon(true, database.icon);
            SetBindMask(false);
            SetTimeLimitMask(false);
        }
    }

    CMResAsynSeedData<CMAtlas> previewAtlas = null;
    /// <summary>
    /// 设置部位图标
    /// </summary>
    /// <param name="iconName"></param>
    public void SetPreView(string iconName)
    {
        if (null != this.m_sp_preview)
        {
            UIManager.GetAtlasAsyn(iconName, ref previewAtlas, () =>
            {
                if (null != this.m_sp_preview)
                {
                    this.m_sp_preview.atlas = null;
                }
            }, m_sp_preview);
        }
    }

    #region Release
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        if (null != previewAtlas)
        {
            previewAtlas.Release(true);
        }
        data = null;
        if (particle != null)
        {
            particle.ReleaseParticle();
        }
    }
    #endregion
}