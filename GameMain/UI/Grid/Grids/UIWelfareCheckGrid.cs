//*************************************************************************
//	创建日期:	2016-11-18 17:54
//	文件名称:	UIWelfareCheckGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	月卡签到
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;

class UIWelfareCheckGrid :UIGridBase
{
    UILabel m_lableIndex = null;
    Transform m_spriteIcon = null;
    GameObject m_goCheck = null;
    GameObject m_goSelect = null;
    WelfareData m_WelfareData = null;

    Transform m_trans_UIItemRewardGrid; 
    UIGridCreatorBase m_ctor;
    public QuickLevState State
    {
        get
        {
            if (m_WelfareData != null)
            {
                return m_WelfareData.state;
            }
            return QuickLevState.QuickLevState_None;
        }
    }

    public uint WelfareId
    {
        get
        {
            if (m_WelfareData != null)
            {
                return m_WelfareData.id;
            }
            return 0;
        }
    }

    public uint ItemId
    {
        get { return m_WelfareData.lstWelfareItems[0].itemid; }
    }
    
    void Awake()
    {
        m_lableIndex = transform.Find("Index").GetComponent<UILabel>();
        m_spriteIcon = transform.Find("Icon").GetComponent<Transform>();
        m_goCheck = transform.Find("Checked").gameObject;
        m_goSelect = transform.Find("Select").gameObject;
        m_trans_UIItemRewardGrid = transform.Find("UIItemRewardGrid");
        AddCreator(m_spriteIcon);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is WelfareData)
        {
            m_WelfareData = (WelfareData)data;
            RefreshSigne();
             list.Clear();
            if (m_WelfareData.lstWelfareItems.Count > 0)
            {
             
                list.Add(new UIItemRewardData() 
                {
                    itemID = m_WelfareData.lstWelfareItems[0].itemid,
                    num = m_WelfareData.lstWelfareItems[0].itemNum,
                });
                if (m_lableIndex != null)
                {
                    m_lableIndex.text = m_WelfareData.param.ToString();
                }
            }
              m_ctor.CreateGrids(list.Count);
        

        }
    }

    public void RefreshSigne()
    {
        if (m_goCheck != null)
        {
            bool addparticle = m_WelfareData.state == QuickLevState.QuickLevState_CanGet;
            m_goCheck.SetActive(m_WelfareData.state == QuickLevState.QuickLevState_HaveGet);
            m_goSelect.SetActive(addparticle);
            UIParticleWidget p = m_goSelect.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = m_goSelect.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {
                p.SetDimensions(144, 120);
                p.ReleaseParticle();
                if (addparticle)
                {
                    p.AddRoundParticle();
                }
               
            }
        }
    }

    List<UIItemRewardData> list = new List<UIItemRewardData>();
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
                if(index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false);
                    BoxCollider[] coll = itemShow.transform.GetComponentsInChildren<BoxCollider>();
                    if (coll != null)
                    {
                        foreach (var i in coll)
                        {
                            i.enabled = false;
                        }

                    }
                }                            
            }
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_goSelect != null)
        {
            UIParticleWidget p = m_goSelect.GetComponent<UIParticleWidget>();
            if (p != null)
            {
                p.ReleaseParticle();
            }
        }
       
    }
}
