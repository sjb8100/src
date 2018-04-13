//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;
using table;
using Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;

partial class PetAddPointPlanPanel : UIPanelBase
{
    PetDataManager m_dataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    Dictionary<int, Transform> m_attrGouDic = new Dictionary<int, Transform>();
    protected override void OnLoading()
    {
        base.OnLoading();


        m_attrGouDic.Add((int)PetAttrEnum.ZhiLi, m_trans_zhiligou);
        m_attrGouDic.Add((int)PetAttrEnum.TiZhi, m_trans_tizhigou);
        m_attrGouDic.Add((int)PetAttrEnum.MinJie, m_trans_minjiegou);
        m_attrGouDic.Add((int)PetAttrEnum.Liliang, m_trans_lilianggou);
        m_attrGouDic.Add((int)PetAttrEnum.JingShen, m_trans_jingshengou);
        var iter = m_attrGouDic.GetEnumerator();


        while (iter.MoveNext())
        {
            var item = iter.Current;
            Transform gou = item.Value.Find("check/gou");
            Transform box = item.Value.Find("check/box");

            if (item.Key == (int)PetAttrEnum.JingShen)
            {
                UIEventListener.Get(box.gameObject).onClick = OnJingshenCheckClick;
            }
            else if (item.Key == (int)PetAttrEnum.Liliang)
            {
                UIEventListener.Get(box.gameObject).onClick = OnLiliangCheckClick;
            }
            else if (item.Key == (int)PetAttrEnum.MinJie)
            {
                UIEventListener.Get(box.gameObject).onClick = OnMinJieCheckClick;
            }
            else if (item.Key == (int)PetAttrEnum.TiZhi)
            {
                UIEventListener.Get(box.gameObject).onClick = OnTizhiCheckClick;
            }
            else if (item.Key == (int)PetAttrEnum.ZhiLi)
            {
                UIEventListener.Get(box.gameObject).onClick = OnZhiLiCheckClick;
            }
        }


    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        PetAddPointPlanItem item = m_trans_jingshenPoint.gameObject.GetComponent<PetAddPointPlanItem>();
        if(item == null)
        {
            item = m_trans_jingshenPoint.gameObject.AddComponent<PetAddPointPlanItem>();
        }
        item.InitPetAddPlan(PetAttrEnum.JingShen);

        item = m_trans_liliangPoint.gameObject.GetComponent<PetAddPointPlanItem>();
        if (item == null)
        {
            item = m_trans_liliangPoint.gameObject.AddComponent<PetAddPointPlanItem>();
        }
        item.InitPetAddPlan(PetAttrEnum.Liliang);

        item = m_trans_minjiePoint.gameObject.GetComponent<PetAddPointPlanItem>();
        if (item == null)
        {
            item = m_trans_minjiePoint.gameObject.AddComponent<PetAddPointPlanItem>();
        }
        item.InitPetAddPlan(PetAttrEnum.MinJie);

        item = m_trans_zhiliPoint.gameObject.GetComponent<PetAddPointPlanItem>();
        if (item == null)
        {
            item = m_trans_zhiliPoint.gameObject.AddComponent<PetAddPointPlanItem>();
        }
        item.InitPetAddPlan(PetAttrEnum.ZhiLi);

        item = m_trans_tizhiPoint.gameObject.GetComponent<PetAddPointPlanItem>();
        if (item == null)
        {
            item = m_trans_tizhiPoint.gameObject.AddComponent<PetAddPointPlanItem>();
        }
        item.InitPetAddPlan(PetAttrEnum.TiZhi); 


        m_label_totalnum.text = GameTableManager.Instance.GetGlobalConfig<uint>("LevelArg").ToString();
        int curgou = 0;
        var iter = m_attrGouDic.GetEnumerator();
        if (m_dataManager.CurPet != null)
        {
            curgou = m_dataManager.CurPet.GetExPlan();
            while (iter.MoveNext())
            {
                var dicitem = iter.Current;
                Transform gou = dicitem.Value.Find("check/gou");
                Transform box = dicitem.Value.Find("check/box");
                if (curgou == (int)dicitem.Key)
                {
                    gou.gameObject.SetActive(true);
                }
                else
                {
                    gou.gameObject.SetActive(false);
                }
                if (dicitem.Key == (int)PetAttrEnum.JingShen)
                {
                    UIEventListener.Get(box.gameObject).onClick = OnJingshenCheckClick;
                }
                else if (dicitem.Key == (int)PetAttrEnum.Liliang)
                {
                    UIEventListener.Get(box.gameObject).onClick = OnLiliangCheckClick;
                }
                else if (dicitem.Key == (int)PetAttrEnum.MinJie)
                {
                    UIEventListener.Get(box.gameObject).onClick = OnMinJieCheckClick;
                }
                else if (dicitem.Key == (int)PetAttrEnum.TiZhi)
                {
                    UIEventListener.Get(box.gameObject).onClick = OnTizhiCheckClick;
                }
                else if (dicitem.Key == (int)PetAttrEnum.ZhiLi)
                {
                    UIEventListener.Get(box.gameObject).onClick = OnZhiLiCheckClick;
                }
            }
        }

    }
    void SetGou(int goupos)
    {
        int curgou = goupos;
        var iter = m_attrGouDic.GetEnumerator();
        if (m_dataManager.CurPet != null)
        {
     
            while (iter.MoveNext())
            {
                var item = iter.Current;
                Transform gou = item.Value.Find("check/gou");
                Transform box = item.Value.Find("check/box");
                if (curgou == (int)item.Key)
                {
                    gou.gameObject.SetActive(true);
                }
                else
                {
                    gou.gameObject.SetActive(false);
                }
            }
        }
    }
    void OnLiliangCheckClick(GameObject go)
    {
        m_dataManager.PointType = (int)PetAttrEnum.Liliang;
        SetGou(m_dataManager.PointType);
    }
    void OnMinJieCheckClick(GameObject go)
    {
        m_dataManager.PointType = (int)PetAttrEnum.MinJie;
        SetGou(m_dataManager.PointType);
    }
    void OnZhiLiCheckClick(GameObject go)
    {
        m_dataManager.PointType = (int)PetAttrEnum.ZhiLi;
        SetGou(m_dataManager.PointType);
    }
    void OnTizhiCheckClick(GameObject go)
    {
        m_dataManager.PointType = (int)PetAttrEnum.TiZhi;
        SetGou(m_dataManager.PointType);
    }
    void OnJingshenCheckClick(GameObject go)
    {
        m_dataManager.PointType = (int)PetAttrEnum.JingShen;
        SetGou(m_dataManager.PointType);
    }
    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_right_Btn(GameObject caster)
    {
        if(m_dataManager.CurPet == null)
        {
            return;
        }
        GameCmd.stAttrPlanPetUserCmd_C cmd = new GameCmd.stAttrPlanPetUserCmd_C();
        GameCmd.PetTalent plan = new GameCmd.PetTalent();
        plan.jingshen = (uint)m_dataManager.PlanAllocJingshen;
        plan.liliang = (uint)m_dataManager.PlanAllocLiliang;
        plan.minjie = (uint)m_dataManager.PlanAllocMinjie;
        plan.tizhi = (uint)m_dataManager.PlanAlloTizhi;
        plan.zhili = (uint)m_dataManager.PlanAllocZhili;
        cmd.attr_plan = plan;
        cmd.id = m_dataManager.CurPet.GetID();
        cmd.ex_plan = m_dataManager.PointType;
        NetService.Instance.Send(cmd);
    }


}
