/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.CommonBg
 * 创建人：  wenjunhua.zqgame
 * 文件名：  CommonInputPanel
 * 版本号：  V1.0.0.0
 * 创建时间：10/28/2016 10:56:05 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class CommonInputPanel : UIPanelBase
{
    #region define
    /// <summary>
    /// 输入面板数据
    /// </summary>
    public class CommonInputPanelData
    {
        //输入汉字数目
        public int m_uint_maxWordsCount = 0;
        //title
        public string m_str_title;
        //开始内容
        public string m_str_starTxt;
        //确定按钮文本
        public string m_str_confirmText;
        //确定按钮回调
        public Action<string> confimAction;
        //关闭按钮回调
        public Action closeAction;
    }
    #endregion

    #region property
    //输入内容
    private string m_str_inputString = "";
    //输入字符数量
    public uint InputNum
    {
        get
        {
            if (string.IsNullOrEmpty(m_str_inputString))
            {
                return 0;
            }
            uint charNum = TextManager.GetCharNumByStrInUnicode(m_str_inputString);
            return TextManager.TransforCharNum2WordNum(charNum,true);
        }
    }

    //可输入最大数量
    public uint MaxNum
    {
        get
        {
            return (null != m_data) ? (uint)m_data.m_uint_maxWordsCount : 0;
        }
    }
    private CommonInputPanelData m_data;
    #endregion
    
    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        if (null != m_input_ContentArea)
        {
            m_input_ContentArea.onChange.Add(new EventDelegate(() =>
            {
                m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_ContentArea.value
                       , MaxNum);
                m_input_ContentArea.value = m_str_inputString;
                UpdateInputLimit();
            }));
            m_input_ContentArea.onSubmit.Add(new EventDelegate(() =>
            {
                m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_ContentArea.value
                       , MaxNum);
                m_input_ContentArea.value = m_str_inputString;
                UpdateInputLimit();
            }));
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is CommonInputPanelData)
        {
            m_data = data as CommonInputPanelData;
            InitInputPanel();
        }
    }


    protected override void OnHide()
    {
        base.OnHide();
        m_str_inputString = "";
    }
    #endregion

    #region Op
    /// <summary>
    /// 更新输入限制
    /// </summary>
    private void UpdateInputLimit()
    {
        if (null != m_label_InputTips)
        {
            m_label_InputTips.text 
                = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_InputLimit, MaxNum, MaxNum - InputNum);
        }
    }
    private void InitInputPanel()
    {
        //title
        if (null != m_label_TitleText)
        {
            m_label_TitleText.text = (null != m_data 
                && !string.IsNullOrEmpty(m_data.m_str_title)) ? m_data.m_str_title : "";
        }
        //确认文字
        if (null != m_label_ConfirmText)
        {
            m_label_ConfirmText.text = (null != m_data
                && !string.IsNullOrEmpty(m_data.m_str_confirmText)) ? m_data.m_str_confirmText : "";
        }
        //tips
        if (null != m_label_InputTips)
        {
            m_label_InputTips.text 
                = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_InputLimit, MaxNum, MaxNum - InputNum);
        }
        
        //输入限制
        if (null != m_input_ContentArea && null != m_data)
        {
            m_str_inputString = m_data.m_str_starTxt;
            m_input_ContentArea.value = m_data.m_str_starTxt;
        }
    }
    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        if (null != m_data && null != m_data.closeAction)
        {
            m_data.closeAction.Invoke();
        }
        HideSelf();
    }

    void onClick_BtnConfirm_Btn(GameObject caster)
    {
        string resultStr =   DataManager.Manager<TextManager>().ReplaceSensitiveWord(m_str_inputString, TextManager.MatchType.Max);

//         bool haveSensitive = DataManager.Manager<TextManager>().IsContainSensitiveWord(m_str_inputString, TextManager.MatchType.Max);
//         if (haveSensitive)
//         {
//             TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_Sensitive, "输入内容", "");
//             return;
//         }
        if (null != m_data && null != m_data.confimAction)
        {
            m_data.confimAction.Invoke(resultStr);
        }
    }

    void onClick_BtnEM_Btn(GameObject caster)
    {
        
    }
    #endregion
}