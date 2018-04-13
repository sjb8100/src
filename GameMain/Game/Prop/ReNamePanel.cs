using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ReNamePanel : UIPanelBase
{

    uint m_reNameItemId;

    UIItemInfoGrid m_baseGrid;

     uint m_MaxNum = 6;

    int itemCount;

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
            return TextManager.TransforCharNum2WordNum(charNum, true);
        }
    }

    #region override


    protected override void OnLoading()
    {
        base.OnLoading();

        m_MaxNum = TextManager.CONST_NAME_MAX_WORDS;//最大名字数

        //改名道具ID
        m_reNameItemId = (uint)GameTableManager.Instance.GetGlobalConfig<int>("ChangeNameItemID");

        if (m_trans_ItemGridRoot.childCount == 0)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiiteminfogrid) as GameObject;
            GameObject cloneObj = NGUITools.AddChild(m_trans_ItemGridRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_baseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_baseGrid)
                {
                    m_baseGrid = cloneObj.AddComponent<UIItemInfoGrid>();
                }
            }
        }

        m_input_Input.onChange.Add(new EventDelegate(OnChangeDelgate));

        m_input_Input.onSubmit.Add(new EventDelegate(OnSubmitDelgate));


        //this.m_defaultText = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Team_Recruit_hanhua);
        m_input_Input.defaultText = "点击输入";

        m_input_Input.characterLimit = (int)m_MaxNum;
        //m_label_wordnumber.text = string.Format("最多可以输入{0}个字", MaxNum);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        InitItem();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        if (m_baseGrid != null)
        {
            m_baseGrid.Release(true);
            UIManager.OnObjsRelease(m_baseGrid.CacheTransform,(uint)GridID.Uiiteminfogrid);
            m_baseGrid = null;
        }
    }
    #endregion

    #region method

    void InitItem()
    {
        if (m_baseGrid != null)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_reNameItemId);
            itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.m_reNameItemId);//道具存量

            m_baseGrid.Reset();
            m_baseGrid.SetBorder(true, baseItem.BorderIcon);
            m_baseGrid.SetIcon(true, baseItem.Icon);
            m_baseGrid.SetNum(false);

            string itemCountStr = string.Empty;

            //获取途径
            if (itemCount < 1)
            {
                m_baseGrid.SetNotEnoughGet(true);
                m_baseGrid.RegisterUIEventDelegate(UIItemInfoEventDelegate);

                itemCountStr = ColorManager.GetColorString(ColorType.Red, itemCount.ToString());
            }
            else
            {
                m_baseGrid.SetNotEnoughGet(false);
                m_baseGrid.UnRegisterUIEventDelegate();

                itemCountStr = ColorManager.GetColorString(ColorType.White, itemCount.ToString());
            }

            //数量
            m_label_ItemCount.text = string.Format("{0}/1", itemCountStr);

            //名字
            m_label_ItemName.text = baseItem.LocalName;
        }
    }

    void OnChangeDelgate()
    {
        m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_Input.value
                      , m_MaxNum);

        m_str_inputString = m_str_inputString.Replace(" ", "");
        m_str_inputString = DataManager.Manager<TextManager>().ReplaceSensitiveWord(m_str_inputString, TextManager.MatchType.Max);

        m_input_Input.value = m_str_inputString;

       

    }

    void OnSubmitDelgate()
    {
        m_str_inputString = TextManager.GetTextByWordsCountLimitInUnicode(m_input_Input.value
                     , m_MaxNum);

        m_str_inputString = m_str_inputString.Replace(" ", "");
        m_str_inputString = DataManager.Manager<TextManager>().ReplaceSensitiveWord(m_str_inputString, TextManager.MatchType.Max);

        m_input_Input.value = m_str_inputString;

      
    }

    /// <summary>
    /// 点击弹出获取item面板
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void UIItemInfoEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: this.m_reNameItemId);
        }
    }

    void SendReName()
    {
        GameCmd.stChangeCharNameDataUserCmd_CS cmd = new GameCmd.stChangeCharNameDataUserCmd_CS();
        cmd.newname = m_str_inputString;
        NetService.Instance.Send(cmd);
    }

    #endregion


    #region click


    void onClick_Btn_right_Btn(GameObject caster)
    {

        if (itemCount > 0)
        {
            SendReName();
        }
        else
        {
            TipsManager.Instance.ShowTips("道具不足");
        }


        HideSelf();
    }

    void onClick_Btn_left_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion
}

