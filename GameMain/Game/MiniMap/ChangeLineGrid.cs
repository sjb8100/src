
//*************************************************************************
//	创建日期:	2017/5/12 星期五 14:41:31
//	文件名称:	ChangeLineGrid
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ChangeLineGrid : UIGridBase
{
    UILabel m_labelLineNum;
    UILabel m_labelPlayerNum;

    Transform m_transRed;
    Transform m_transYellow;
    Transform m_transGreen;
    Transform m_transHigh;
    public uint LineNum = 0;
    protected override void OnAwake()
    {
        base.OnAwake();

        Transform line = transform.Find("Line_Label");
        if (line != null)
        {
            m_labelLineNum = line.GetComponent<UILabel>();
        }
        Transform player = transform.Find("Num_Label");
        if (player != null)
        {
            m_labelPlayerNum = player.GetComponent<UILabel>();
        }
        Transform sign = transform.Find("Sigh");
        if (sign != null)
        {
            m_transRed = sign.Find("R");
            m_transGreen = sign.Find("G");
            m_transYellow = sign.Find("Y");
        }
        m_transHigh = transform.Find("Bg/ChooseMark");
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if(m_transHigh != null)
        {
            m_transHigh.gameObject.SetActive(hightLight);
        }
    }
    public void ShowInfo(ChangeLineInfo info)
    {
        LineNum = info.lineNum;
        if (m_labelPlayerNum != null)
        {
            m_labelPlayerNum.text = info.playerNum.ToString() + CommonData.GetLocalString("人");
        }
        if (m_labelLineNum != null)
        {
            IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
            if (ms != null)
            {
                string str = ms.GetMapName() + "(" + info.lineNum + CommonData.GetLocalString("线")+")";
                m_labelLineNum.text = str;
            }

        }
        ShowSign(info.playerNum);

    }
    void ShowSign(uint playerNum)
    {
        uint red = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeLine", "red");
        uint yellow = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeLine", "yellow");
        uint green = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeLine", "green");
        if(m_transGreen != null && m_transRed != null&& m_transYellow != null)
        {
            if (playerNum <= green)
            {
                m_transGreen.gameObject.SetActive(true);
                m_transRed.gameObject.SetActive(false);
                m_transYellow.gameObject.SetActive(false);
            }
            else if(playerNum>green && playerNum <= yellow)
            {
                m_transGreen.gameObject.SetActive(false);
                m_transRed.gameObject.SetActive(false);
                m_transYellow.gameObject.SetActive(true);
            }
            else if(playerNum > yellow)
            {
                m_transGreen.gameObject.SetActive(false);
                m_transRed.gameObject.SetActive(true);
                m_transYellow.gameObject.SetActive(false);
            }
        }
      
    }
}
