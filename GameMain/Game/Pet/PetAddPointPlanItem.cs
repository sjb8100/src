
//*************************************************************************
//	创建日期:	2017/8/14 星期一 17:01:31
//	文件名称:	PetAddPointPlanItem
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PetAddPointPlanItem : MonoBehaviour
{
    PetDataManager m_dataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    UILabel m_label_numpoint;
    PetAttrEnum m_petAttr;
    void Awake()
    {
        m_label_numpoint = transform.Find("num_bg/numPoint").GetComponent<UILabel>();
        Transform add = transform.Find("btn_add");
        if (add != null)
        {
            UIEventListener.Get(add.gameObject).onClick = OnAddClick;
        }
        Transform minus = transform.Find("btn_cut");
        if (minus != null)
        {
            UIEventListener.Get(minus.gameObject).onClick = OnMinusClick;
        }

    }
    public void InitPetAddPlan(PetAttrEnum attr)
    {
        m_petAttr = attr;
        SetPointNum();
    }
    void SetPointNum()
    {
        int key = (int)m_petAttr;
        if (m_dataManager.PlanAllocPointDic.ContainsKey(key))
        {
            m_label_numpoint.text = m_dataManager.PlanAllocPointDic[key].ToString();
        }
    }
    void OnAddClick(GameObject go)
    {
        if (m_dataManager.CheckPlanCanAlloc())
        {
          if(m_petAttr == PetAttrEnum.JingShen)
          {
              m_dataManager.PlanAllocJingshen += 1;
          }
          else if(m_petAttr == PetAttrEnum.Liliang)
          {
              m_dataManager.PlanAllocLiliang += 1;
          }
          else if(m_petAttr == PetAttrEnum.MinJie)
          {
              m_dataManager.PlanAllocMinjie += 1;
          }
          else if(m_petAttr == PetAttrEnum.TiZhi)
          {
              m_dataManager.PlanAlloTizhi += 1;
          }
          else if(m_petAttr == PetAttrEnum.ZhiLi)
          {
              m_dataManager.PlanAllocZhili += 1;
          }
          SetPointNum();
        }
        else
        {
            TipsManager.Instance.ShowTips("没有可分配点");
        }
    }
    void OnMinusClick(GameObject go)
    {
        if (m_petAttr == PetAttrEnum.JingShen)
        {
            m_dataManager.PlanAllocJingshen -= 1;
        }
        else if (m_petAttr == PetAttrEnum.Liliang)
        {
            m_dataManager.PlanAllocLiliang -= 1;
        }
        else if (m_petAttr == PetAttrEnum.MinJie)
        {
            m_dataManager.PlanAllocMinjie -= 1;
        }
        else if (m_petAttr == PetAttrEnum.TiZhi)
        {
            m_dataManager.PlanAlloTizhi -= 1;
        }
        else if (m_petAttr == PetAttrEnum.ZhiLi)
        {
            m_dataManager.PlanAllocZhili -= 1;
        }
        SetPointNum();
    }
}