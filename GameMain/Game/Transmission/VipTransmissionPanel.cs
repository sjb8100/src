//*************************************************************************
//	创建日期:	2016-10-26 14:17
//	文件名称:	VipTransmissionPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:　  vip传送面板
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial　class VipTransmissionPanel : UIPanelBase
{
    GameObject m_goPrafab = null;
    List<Transform> m_lstTrans = new List<Transform>();
    Dictionary<BtnType, List<table.TransferDatabase>> m_dicTransmit = new Dictionary<BtnType, List<table.TransferDatabase>>();

    Dictionary<BtnType, UIToggle> m_dicToggle = new Dictionary<BtnType, UIToggle>();
    bool m_bInitToggle = false;

    protected override void OnLoading()
    {
        base.OnLoading();
        m_goPrafab = m_trans_UIVipTransmissionGrid.gameObject;
        InitList();
        InitData();

        UIToggle[] toggles = this.transform.Find("up").GetComponentsInChildren<UIToggle>();
        foreach (var item in toggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            m_dicToggle.Add(btntype, item);
            EventDelegate.Add(item.onChange, () =>
            {
                if (item.value)
                {
                    m_bInitToggle = true;
                    this.OnToggleChange(btntype);
                }
            });
        }
        m_goPrafab.gameObject.SetActive(false);
    }

    void InitList()
    {
        for (int i = 0; i < 12; i++)
        {
            CreatItem();
        }
    }

    GameObject CreatItem()
    {
        GameObject go = NGUITools.AddChild(m_trans_Root.gameObject, m_goPrafab);
        int x = m_lstTrans.Count % 4;
        int y = m_lstTrans.Count / 4;
        go.transform.localPosition = new UnityEngine.Vector3(x * 180, -90 * y, 0);
        m_lstTrans.Add(go.transform);
        UIEventListener.Get(go.transform.Find("btn").gameObject).onClick = OnTransMit;
        
        return go;
    }   
   
    void InitData()
    {
        List<table.TransferDatabase> lstTransmis = GameTableManager.Instance.GetTableList<table.TransferDatabase>();
        m_dicTransmit.Clear();
        for (int i = 0; i < lstTransmis.Count; ++i)
        {
            if (!m_dicTransmit.ContainsKey((BtnType)lstTransmis[i].type))
            {
                m_dicTransmit.Add((BtnType)lstTransmis[i].type, new List<table.TransferDatabase>());
            }
            m_dicTransmit[(BtnType)lstTransmis[i].type].Add(lstTransmis[i]);
        }
    }

    void OnToggleChange(BtnType btnType)
    {
        ShowList(btnType);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data is LangTalkData)
        {

        }
        else
        {

        }
        StartCoroutine(OnSetBtnState());
    }

    IEnumerator OnSetBtnState()
    {
        //等待所有toggle 都执行start后再设置enabled为false
        while (!m_bInitToggle)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        uint vipLevel = DataManager.Manager<Mall_HuangLingManager>().NobleID;

        foreach (var itor in m_dicToggle)
        {
            if ((int)itor.Key > vipLevel)
            {
                itor.Value.enabled = false;
                UIEventListener.Get(itor.Value.gameObject).onClick = delegate(GameObject go)
                {
                    if (itor.Value.enabled)
                    {
                        return;
                    }
                    if (itor.Key == BtnType.dixiacheng)
                    {
                        TipsManager.Instance.ShowTipsById(516);
                    }
                    else if (itor.Key == BtnType.Boss)
                    {
                        TipsManager.Instance.ShowTipsById(517);
                    }
                };
            }
            else
            {
                itor.Value.enabled = true;
            }
        }
    }

    void ShowList(BtnType btnType)
    {
        List<table.TransferDatabase> lstTransdata = null;
        if (m_dicTransmit.TryGetValue(btnType, out lstTransdata))
        {
            Transform trans = null;
            int i = 0;
            for (; i < lstTransdata.Count; ++i )
            {
                if (m_lstTrans.Count > i)
                {
                    trans = m_lstTrans[i];
                }
                else
                {
                    trans = CreatItem().transform;
                }

                if (trans != null)
                {
                    int index = lstTransdata[i].strTransmitMap.IndexOf("(");
                    if (index == -1)
                    {
                        index = lstTransdata[i].strTransmitMap.IndexOf("（");
                    }

                    string title = lstTransdata[i].strTransmitMap;
                    if (index != -1)
                    {
                        title = lstTransdata[i].strTransmitMap.Insert(index,"\n");
                    }
                    trans.Find("btn/Label").GetComponent<UILabel>().text = title;
                    trans.name = lstTransdata[i].type.ToString() + "_" + lstTransdata[i].mapid.ToString();
                    trans.gameObject.SetActive(true);
                }
            }
            for (int k = i; k < m_lstTrans.Count; ++k )
            {
                m_lstTrans[k].gameObject.SetActive(false);
            }
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void OnBtnsClick(BtnType btntype)
    {

    }
    void OnTransMit(GameObject go)
    {
        string strName = go.transform.parent.name;
        string strtype = strName.Substring(0, strName.IndexOf("_"));
        string strid = strName.Substring(strName.IndexOf("_") + 1);

        uint id = uint.Parse(strid);
        List<table.TransferDatabase> lstTransdata = null;
        if (m_dicTransmit.TryGetValue(((BtnType)int.Parse(strtype)), out lstTransdata))
        {
            for (int i = 0; i < lstTransdata.Count; ++i )
            {
                if (lstTransdata[i].mapid == id)
                {
                    string strMsg = string.Format("是否花费 [ff0000]{0}{1}[-] 传送到{2}", lstTransdata[i].costValue, ((ClientMoneyType)lstTransdata[i].costType).GetEnumDescription(), lstTransdata[i].strTransmitMap);
                    TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, strMsg, delegate()
                    {
                        ClientMoneyType montype = (ClientMoneyType)lstTransdata[i].costType;
                        if (MainPlayerHelper.IsHasEnoughMoney(montype, lstTransdata[i].costValue))
                        {
                            Client.IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
                            if (mapsys != null)
                            {
                                mapsys.RequestEnterMap(lstTransdata[i].mapid,1);
                            }
                        }
                    });
   
                    break;
                }
            }
        }
    }
}