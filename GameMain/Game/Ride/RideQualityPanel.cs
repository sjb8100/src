using System;
using System.Collections.Generic;
using UnityEngine;
using table;
using GameCmd;
using Engine;
using Client;
using Engine.Utility;

partial class RideQualityPanel: UIPanelBase
{
   RideManager rmg
    {
       get
        {
            return DataManager.Manager<RideManager>();
        }
    }
    Dictionary<int, Transform> m_dicTrans = new Dictionary<int, Transform>();
    Dictionary<int, uint> m_totalTalent = new Dictionary<int, uint>();
    protected override void OnLoading()
    {
        base.OnLoading();
        m_dicTrans.Clear();
        m_dicTrans.Add((int)RideManager.RideTalentEnum.liliang, m_trans_liliangzizhi);
        m_dicTrans.Add((int)RideManager.RideTalentEnum.minjie, m_trans_mingjie);

        m_dicTrans.Add((int)RideManager.RideTalentEnum.zhili, m_trans_zhili);

        m_dicTrans.Add((int)RideManager.RideTalentEnum.tizhi, m_trans_tili);

        m_dicTrans.Add((int)RideManager.RideTalentEnum.jingshen, m_trans_jingshen);

        m_totalTalent.Clear();
        List<uint> totalList = GameTableManager.Instance.GetGlobalConfigList<uint>("KnightTelantItemAddMax");
        if(totalList != null)
        {
            if(totalList.Count == 5)
            {
                Array ar = Enum.GetValues(typeof(RideManager.RideTalentEnum));
               for(int i = 0;i<ar.Length;i++)
               {
                   if(i<totalList.Count)
                   {
                       m_totalTalent.Add((int)ar.GetValue(i), totalList[i]);
                   }
               }
            }
            else
            {
                Log.Error("全局 KnightTelantItemAddMax is invalid");
            }
        }
        else
        {
            Log.Error("KnightTelantItemAddMax is not exist");
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RefreshTalent();
        rmg.ValueUpdateEvent += rmg_ValueUpdateEvent;
    }

    void rmg_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if(e != null)
        {
            if(e.key == RideManager.RideDispatchEnum.RefreshKnightTalent.ToString())
            {
                RefreshTalent();
            }
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        rmg.ValueUpdateEvent -= rmg_ValueUpdateEvent;
    }
    void RefreshTalent()
    {
        KnightTelant rt = DataManager.Manager<RideManager>().RideTalent;
        foreach(var dic in m_dicTrans)
        {
            Transform t = dic.Value;
            uint curTal = 0;
            if(dic.Key == (int)RideManager.RideTalentEnum.jingshen)
            {
                curTal = rt.jingshen;
            }
            else if(dic.Key == (int)RideManager.RideTalentEnum.liliang)
            {
                curTal = rt.liliang;
            }
            else if (dic.Key == (int)RideManager.RideTalentEnum.minjie)
            {
                curTal = rt.minjie;
            }
            else if (dic.Key == (int)RideManager.RideTalentEnum.tizhi)
            {
                curTal = rt.tizhi;
            }
            else if (dic.Key == (int)RideManager.RideTalentEnum.zhili)
            {
                curTal = rt.zhili;
            }
            uint totalTalet = m_totalTalent[dic.Key];
            if(totalTalet != 0)
            {
                float fv = curTal * 1.0f / totalTalet;
                Transform slider = t.Find("AchievementSlider");
                if(slider != null)
                {
                    UISlider s = slider.GetComponent<UISlider>();
                    if(s != null)
                    {
                        s.value = fv;
                    }
                    Transform num = slider.Find("QualityNum");
                    if(num != null)
                    {
                        UILabel label = num.GetComponent<UILabel>();
                        if(label != null)
                        {
                            label.text = string.Format("{0}/{1}", curTal, totalTalet);
                        }
                    }
                }
            }

        }
    }
    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    void OnClickAdd(RideManager.RideTalentEnum talent)
    {
        UseItemCommonPanel.UseItemParam data = new UseItemCommonPanel.UseItemParam();
        data.type = UseItemCommonPanel.UseItemEnum.RideTalent;
        data.customParam = talent;
        //data.userId = CurPet.GetID();
        //data.tabs = new int[] { (int)TabMode.ShuXing };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UseItemCommonPanel,data:data);
    }
    void onClick_Btn_Go1_Btn(GameObject caster)
    {
        OnClickAdd(RideManager.RideTalentEnum.liliang);
    }

    void onClick_Btn_Go2_Btn(GameObject caster)
    {
        OnClickAdd(RideManager.RideTalentEnum.minjie);

    }

    void onClick_Btn_Go3_Btn(GameObject caster)
    {
        OnClickAdd(RideManager.RideTalentEnum.zhili);
    }

    void onClick_Btn_Go4_Btn(GameObject caster)
    {
        OnClickAdd(RideManager.RideTalentEnum.tizhi);
    }

    void onClick_Btn_Go5_Btn(GameObject caster)
    {
        OnClickAdd(RideManager.RideTalentEnum.jingshen);
    }


}
