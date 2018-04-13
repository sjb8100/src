//*************************************************************************
//	创建日期:	2016-11-18 17:55
//	文件名称:	UIWelfareTypeGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	福利按钮
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class UIWelfareTypeGrid : UIGridBase
{
    UISprite m_spriteSelect = null;
   // UISprite m_spriteIcon = null;
    UILabel m_lableName;
    UISprite redPoint = null;
    WelfareType m_welfareType = WelfareType.None;

    public WelfareType Welfare
    {
        get { return m_welfareType; }
    }
    void Awake()
    {
        m_lableName = transform.Find("Name").GetComponent<UILabel>();
        //m_spriteIcon = transform.Find("IconContent/Icon").GetComponent<UISprite>();
        m_spriteSelect = transform.Find("Select").GetComponent<UISprite>();
        redPoint = transform.Find("Warrning").GetComponent<UISprite>();

        m_spriteSelect.gameObject.SetActive(false);  
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is WelfareType)
        {
            m_welfareType = (WelfareType)data;

            if (m_lableName != null)
            {
                m_lableName.text = GetTitle();
            }
            bool value =false;
            if (m_welfareType == WelfareType.RewardFind)
            {
                value = DataManager.Manager<WelfareManager>().HasRewardCanReBack();             
            }
            else 
            {
                value = DataManager.Manager<WelfareManager>().HasRewardInType(1, (uint)m_welfareType);
            }
            redPoint.gameObject.SetActive(value);
        }
    }
    private uint sevenDayIndex;
    public uint SevenDayIndex 
    {
        get 
        {
            return sevenDayIndex;
        }
        set 
        {
            sevenDayIndex = value;
        }
    }
    public void SetSevenDayGridData(int index) 
    {
        if (m_lableName != null)
        {
            sevenDayIndex =(uint)index+1;
            OnSevenDaySelect(false);
            m_lableName.text = GetSevenDayTitle(index);
            bool value = DataManager.Manager<WelfareManager>().HasRewardInType(2, sevenDayIndex) && sevenDayIndex <= DataManager.Manager<WelfareManager>().ServerOpenCurrDay;
            redPoint.gameObject.SetActive(value);
        }
    }
    string GetSevenDayTitle(int index) 
    {
        string value = "";
        switch (index) 
        {
            case 0:
                value = "第一天";
                break;
            case 1:
                value = "第二天";
                break;
            case 2:
                value = "第三天";
                break;
            case 3:
                value = "第四天";
                break;
            case 4:
                value = "第五天";
                break;
            case 5:
                value = "第六天";
                break;
            case 6:
                value = "第七天";
                break;
        }
        return value;
    }
    string GetTitle() 
    {
        LocalTextType type = LocalTextType.LocalText_None;
        switch (m_welfareType)
        {
            case WelfareType.None:
                break;
            case WelfareType.Month:
                type = LocalTextType.Local_TXT_WelfareMonth;
                break;
            case WelfareType.OnLine:
                type = LocalTextType.Local_TXT_WelfareOnline;
                break;
            case WelfareType.SevenDay:
                type = LocalTextType.Local_TXT_Welfare7Day;
                break;
            case WelfareType.RoleLevel:
                type = LocalTextType.Local_TXT_WelfareLeve;
                break;
            case WelfareType.OpenSever:
                type = LocalTextType.Local_TXT_WelfareSever;
                break;
            case WelfareType.BindGift:
                type = LocalTextType.Local_TXT_BindGift;
                break;
            case WelfareType.RewardFind:
                type = LocalTextType.Local_TXT_RewardFind;
                break;
            case WelfareType.FriendInvite:
                type = LocalTextType.Local_TXT_FriendInvite;
                break;
            case WelfareType.RushLevel:
                type = LocalTextType.Local_TXT_RushLevel;
                break;
            case WelfareType.CDKey:
                type = LocalTextType.Local_TXT_CDKey;
                    break;
            case WelfareType.CollectWord:
                    type = LocalTextType.Local_TXT_CollectWord;
                    break;
            case WelfareType.End:
                break;
            default:
                break;
        }
        return DataManager.Manager<TextManager>().GetLocalText(type);
    }

    public void OnWelfareSelect(WelfareType type )
    {
        if (m_spriteSelect == null)
        {
            return;
        }
        m_spriteSelect.gameObject.SetActive(type == m_welfareType);             
    }
    public void OnSevenDaySelect(bool value)
    {
        if (m_spriteSelect == null)
        {
            return;
        }
        m_spriteSelect.enabled = value;        
    }
}
