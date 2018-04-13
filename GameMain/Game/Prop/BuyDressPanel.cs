
//*************************************************************************
//	创建日期:	2017/2/22 星期三 10:35:13
//	文件名称:	BuyDressPanel
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using GameCmd;
using Engine;
using Engine.Utility;


partial class BuyDressPanel
{
    SuitDataManager m_SuitData
    {
        get
        {
            return DataManager.Manager<SuitDataManager>();
        }
    }
    SuitDataBase CurDataBase
    {
        get
        {
            return m_SuitData.CurSuitDataBase;
        }
    }
    GameCmd.SuitOPType m_opType = SuitOPType.SuitOPType_Buy;
    uint m_nDayCount = 3;
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null && data is SuitOPType)
        {
            m_opType = (SuitOPType)data;
        }
        if (CurDataBase != null)
        {
            m_label_suitName.text = CurDataBase.name;
            UIManager.GetTextureAsyn(CurDataBase.icon, ref m_iconCASD, () =>
            {
                if (null != m__DressIcon)
                {
                    m__DressIcon.mainTexture = null;
                }
            }, m__DressIcon);
            m_label_suitdes.text = CurDataBase.desc;
            int count = m_trans_BtnGroup.childCount;
            for (int i = 0; i < count; i++)
            {
                string name = "Btn_" + (i + 1);
                Transform btnTrans = m_trans_BtnGroup.Find(name);
                if (btnTrans != null)
                {
                    UIEventListener.Get(btnTrans.gameObject).onClick += OnDayClick;
                    if (i == 0)
                    {
                        UIToggle tog = btnTrans.GetComponent<UIToggle>();
                        if (tog != null)
                        {
                            tog.value = true;
                        }
                    }
                }

            }
            InitCost();

        }

    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }
    }
    void InitCost()
    {
        int count = m_trans_CostGroup.childCount;
        for (int i = 0; i < count; i++)
        {
            name = "Cost_" + (i + 1);
            Transform costTrans = m_trans_CostGroup.Find(name);
            int lv = 3 + i;
            SuitDataBase needDb = GameTableManager.Instance.GetTableItem<SuitDataBase>(CurDataBase.base_id, lv);
            if (needDb != null)
            {
                string moneyStr = needDb.buyPrice;
                List<uint> typeMoney = StringUtil.GetSplitStringList<uint>(moneyStr, '_');
                if (typeMoney.Count == 2)
                {
                    int type = (int)typeMoney[0];
                    uint cost = typeMoney[1];
                    if (costTrans != null)
                    {
                        Transform iconTrans = costTrans.Find("moneySpr");
                        if (iconTrans != null)
                        {
                            UISprite spr = iconTrans.GetComponent<UISprite>();
                            if (spr != null)
                            {
                                spr.spriteName = MainPlayerHelper.GetMoneyIconByType(type);
                            }
                        }
                        Transform labelTrans = costTrans.Find("moneyCost");
                        if (labelTrans != null)
                        {
                            Transform lineTrans = costTrans.Find("RedLine");
                            if(lineTrans != null)
                            {
                                UILabel label = labelTrans.GetComponent<UILabel>();
                                if (label != null)
                                {
                                    label.text = cost.ToString();
                                    if (m_opType == SuitOPType.SuitOPType_Renew)
                                    {
                                        lineTrans.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        lineTrans.gameObject.SetActive(false);
                                    }
                                }
                            }
                          
                        }
                        Transform renewlabelTrans = costTrans.Find("renewMoney");
                        if (renewlabelTrans != null)
                        {
                            if (m_opType == SuitOPType.SuitOPType_Renew)
                            {
                                renewlabelTrans.gameObject.SetActive(true);
                            }
                            else
                            {
                                renewlabelTrans.gameObject.SetActive(false);
                            }
                            UILabel label = renewlabelTrans.GetComponent<UILabel>();
                            if (label != null)
                            {
                                label.text = needDb.renewalPrice;
                            }
                        }
                        Transform dayTrans = costTrans.Find("Day_7");
                        if (dayTrans != null)
                        {
                            UILabel dayLabel = dayTrans.GetComponent<UILabel>();
                            if (dayLabel != null)
                            {
                                dayLabel.text = m_SuitData.GetLeftTimeStringMin((int)needDb.time );
                            }
                        }
                    }
                }
            }
        }
    }

    void OnDayClick(GameObject go)
    {
        if (CurDataBase == null)
        {
            Log.Error("suit database is null");
            return;
        }
        string name = go.name;
        string[] strArray = name.Split('_');
        if (strArray.Length == 2)
        {
            string indexStr = strArray[1];
            int index = 0;
            if (int.TryParse(indexStr, out index))
            {
                m_nDayCount = 2 + (uint)index;
            }
        }
    }
    void onClick_Unlock_close_Btn(GameObject caster)
    {
        HideSelf();

    }

    void onClick_Unlock_queding_Btn(GameObject caster)
    {
        if (CurDataBase != null)
        {
            ClientSuitData sst = DataManager.Manager<SuitDataManager>().GetSuitData(CurDataBase.base_id);
            if(sst.suitState == SuitState.HasBuy)
            {
               if(sst.leftTime == 0)
               {
                   string tips = string.Format("{0}{1}{2}", CommonData.GetLocalString("您已经拥有永久时装"), CurDataBase.name, CommonData.GetLocalString(",是否仍确定使用？"));
                   TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () => {
                       stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
                       cmd.type = m_opType;

                       uint suitID = (CurDataBase.base_id << 16);
                       suitID += m_nDayCount;
                       cmd.suit_id = suitID;
                       NetService.Instance.Send(cmd);
                   });
               }
               else
               {
                   stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
                   cmd.type = m_opType;

                   uint suitID = (CurDataBase.base_id << 16);
                   suitID += m_nDayCount;
                   cmd.suit_id = suitID;
                   NetService.Instance.Send(cmd);
               }
            }
            else
            {
                stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
                cmd.type = m_opType;

                uint suitID = (CurDataBase.base_id << 16);
                suitID += m_nDayCount;
                cmd.suit_id = suitID;
                NetService.Instance.Send(cmd);
            }
        
        }

        HideSelf();
    }


}
