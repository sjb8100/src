using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UISevenDayRewardGrid : UIGridBase
{

     Transform rewardRoot;
     UIGridCreatorBase m_ctor;
     Transform m_trans_UIItemRewardGrid;
     UILabel Active_Num;
     Transform Particle;
     Transform m_none;
     Transform m_get;
     Transform m_got;
     List<UIItemRewardData> m_lst_data;
     public UIItemRewardData _UIItemRewardDatas
     {
         set;
         get;
     }
    #region overridemethod

    protected override void OnAwake()
    {
        base.OnAwake();
        Active_Num = CacheTransform.Find("Active_Num").GetComponent<UILabel>();
        Particle = CacheTransform.Find("Particle");
        m_none = CacheTransform.Find("None");
        m_get = CacheTransform.Find("Get");
        m_got = CacheTransform.Find("Got");
        rewardRoot = CacheTransform.Find("root");
        m_trans_UIItemRewardGrid = CacheTransform.Find("UIItemRewardGrid");
        AddCreator(rewardRoot);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        _UIItemRewardDatas = (UIItemRewardData)data;
        if (_UIItemRewardDatas != null)
        {
            if (m_ctor != null)
            {
                m_ctor.CreateGrids(1);
            }
        }
       
    }

    void AddCreator(Transform parent) 
    {
        if (parent != null)
       {
           m_ctor = parent.GetComponent<UIGridCreatorBase>();
           if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
           m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
           m_ctor.gridWidth = 90;
           m_ctor.gridHeight = 90;
           m_ctor.RefreshCheck();
           m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData,null);
       }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (_UIItemRewardDatas != null)
                {
                    uint itemID = _UIItemRewardDatas.itemID;
                    itemShow.SetGridData(itemID, 1, false, false, false);
                    Active_Num.text = _UIItemRewardDatas.num.ToString();
                    bool isNone = _UIItemRewardDatas.blockVisible;
                    bool isGot = _UIItemRewardDatas.hasGot;
                    m_none.gameObject.SetActive(isNone);
                    m_get.gameObject.SetActive(!isNone && !isGot);
                    m_got.gameObject.SetActive(isGot);
                    AddParticle(!isNone && !isGot);
                    if (itemShow != null)
                    {
                        itemShow.HideBoxCollider(!isNone && !isGot);
                    }
                }
            }
        }
    }
    void AddParticle(bool add) 
    {
        UIParticleWidget p = Particle.GetComponent<UIParticleWidget>();
        if (p == null)
        {
            p = Particle.gameObject.AddComponent<UIParticleWidget>();
            p.depth = 100;
        }
        if (add)
        {
            if (p != null)
            {
                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                p.AddParticle(50025);
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

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    #endregion
}