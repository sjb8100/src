
//*************************************************************************
//	创建日期:	2017/12/11 星期一 10:57:16
//	文件名称:	UIRedEnvelopeDetailGrid
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
using Engine.Utility;
using UnityEngine;


class UIRedEnvelopeDetailGrid : UIGridBase
{
    UISprite m_goldSpr;//金币icon
    UILabel m_moneyNumLabel;
    UILabel m_nameLalel;
    Transform m_transBestIcon;
    Transform m_transBestLabel;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_goldSpr = transform.Find("goldicon2").GetComponent<UISprite>();
        m_moneyNumLabel = transform.Find("totalNum_label").GetComponent<UILabel>();
        m_nameLalel = transform.Find("total_label").GetComponent<UILabel>();
        m_transBestIcon = transform.Find("bestIcon");
        m_transBestLabel = transform.Find("best_label");
    }
    public void InitDetailGrid(string name,uint goldNum,int index,uint moneyType)
    {
        m_goldSpr.spriteName = MainPlayerHelper.GetMoneyIconByType((int)moneyType);
        m_moneyNumLabel.text = goldNum.ToString();
        m_nameLalel.text = name;
        if(index == 0)
        {
            m_transBestIcon.gameObject.SetActive(true);
            m_transBestLabel.gameObject.SetActive(true);
        }
        else
        {
            m_transBestIcon.gameObject.SetActive(false);
            m_transBestLabel.gameObject.SetActive(false);
        }
    }
}
