
//*************************************************************************
//	创建日期:	2017/8/14 星期一 16:54:58
//	文件名称:	PetDataManager_AddPointPlan
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;

enum PetAttrEnum
{/*1-智力   2-体质
3-力量   4-敏捷
5-精神*/
    None = 0,
    ZhiLi = 1,
    TiZhi = 2,
    Liliang = 3,
    MinJie = 4,
    JingShen = 5,

}
partial class PetDataManager
{
    int m_uPlanAllocLiliang = 0;
    public int PlanAllocLiliang
    {
        get
        {
            return m_uPlanAllocLiliang;
        }
        set
        {
            if (value >= 0)
            {
                m_uPlanAllocLiliang = value;
                int key = (int)PetAttrEnum.Liliang;
                if (PlanAllocPointDic.ContainsKey(key))
                {
                    PlanAllocPointDic[key] = value;
                }
                else
                {
                    PlanAllocPointDic.Add(key, value);
                }
            }
        }
    }
    int m_uPlanAllocMinjie = 0;
    public int PlanAllocMinjie
    {
        get
        {
            return m_uPlanAllocMinjie;
        }
        set
        {
            if (value >= 0)
            {
                m_uPlanAllocMinjie = value;
                int key = (int)PetAttrEnum.MinJie;
                if (PlanAllocPointDic.ContainsKey(key))
                {
                    PlanAllocPointDic[key] = value;
                }
                else
                {
                    PlanAllocPointDic.Add(key, value);
                }
            }
        }
    }
    int m_uPlanAllocZhili = 0;
    public int PlanAllocZhili
    {
        get
        {
            return m_uPlanAllocZhili;
        }
        set
        {
            if (value >= 0)
            {
                m_uPlanAllocZhili = value;
                int key = (int)PetAttrEnum.ZhiLi;
                if (PlanAllocPointDic.ContainsKey(key))
                {
                    PlanAllocPointDic[key] = value;
                }
                else
                {
                    PlanAllocPointDic.Add(key, value);
                }
            }
        }
    }
    int m_uPlanAllocTizhi = 0;
    public int PlanAlloTizhi
    {
        get
        {
            return m_uPlanAllocTizhi;
        }
        set
        {
            if (value >= 0)
            {
                m_uPlanAllocTizhi = value;
                int key = (int)PetAttrEnum.TiZhi;
                if (PlanAllocPointDic.ContainsKey(key))
                {
                    PlanAllocPointDic[key] = value;
                }
                else
                {
                    PlanAllocPointDic.Add(key, value);
                }
            }
        }
    }
    int m_uPlanAllocJingshen = 0;

    public int PlanAllocJingshen
    {
        get
        {
            return m_uPlanAllocJingshen;
        }
        set
        {
            if (value >= 0)
            {
                m_uPlanAllocJingshen = value;
                int key = (int)PetAttrEnum.JingShen;
                if (PlanAllocPointDic.ContainsKey(key))
                {
                    PlanAllocPointDic[key] = value;
                }
                else
                {
                    PlanAllocPointDic.Add(key, value);
                }
            }
        }
    }
    public int PointType = 0;
    public Dictionary<int, int> PlanAllocPointDic = new Dictionary<int, int>();
   public void InitPlanPoint()
    {
        if(CurPet != null)
        {
            PetTalent plan = CurPet.GetAttrPlan();
            PlanAllocJingshen =(int) plan.jingshen;
            PlanAllocLiliang = (int)plan.liliang;
            PlanAllocMinjie = (int)plan.minjie;
            PlanAllocZhili = (int)plan.zhili;
            PlanAlloTizhi = (int)plan.tizhi;
            PointType = CurPet.GetExPlan();

        }
    }
    public int GetPlanAllocPoint()
    {
        return m_uPlanAllocJingshen + m_uPlanAllocLiliang + m_uPlanAllocMinjie + m_uPlanAllocTizhi + m_uPlanAllocZhili;
    }
    public bool CheckPlanCanAlloc()
    {
        uint num = GameTableManager.Instance.GetGlobalConfig<uint>("LevelArg");
        if (GetPlanAllocPoint() >= num)
        {
            return false;
        }
        return true;
    }

    public void OnPlanSucess(stAttrPlanPetUserCmd_C cmd)
    {
        TipsManager.Instance.ShowTips(LocalTextType.Pet_Commond_jihuabaocunchenggong);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PetAddPointPlanPanel);
    }
}