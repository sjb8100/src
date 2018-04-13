//*************************************************************************
//	创建日期:	2017-2-4 10:40
//	文件名称:	UIFriendGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	好友列表
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class UIFriendGrid : UIGridBase
{
    UISpriteEx m_headIcon;
    UILabel m_lableLevel;
    UILabel m_lableName;
    UILabel m_labelOnline;
    GameObject m_tipsGo;
    GameObject m_btnGo;
    GameObject m_marskGo;
    GameObject m_choose;
    FriendPanel m_parent;
    RoleRelation m_data;
    public RoleRelation Data { get { return m_data; } }
    public bool IsSys
    {
        get { return m_data.isSys; }
    }
    public uint PlayerId
    {
        get
        {
            if (m_data == null)
            {
                return 0;
            }
            return m_data.uid;
        }
    }
    void Awake()
    {
        m_headIcon = transform.Find("bg/icon").GetComponent<UISpriteEx>();
        m_lableLevel = transform.Find("bg/level").GetComponent<UILabel>();
        m_lableName = transform.Find("bg/name").GetComponent<UILabel>();
        m_labelOnline = transform.Find("bg/online").GetComponent<UILabel>();
        m_tipsGo = transform.Find("bg/warning").gameObject;
        m_btnGo = transform.Find("bg/btn_add").gameObject;
        m_btnGo.gameObject.SetActive(false);
        m_marskGo = transform.Find("bg/select").gameObject;
        m_choose = transform.Find("bg/btn_choose").gameObject;
        if (m_headIcon != null)
        {
            m_headIcon.MakePixelPerfect();
            UIEventListener.Get(m_headIcon.gameObject).onClick = OnShowOpratePanel;
        }
        UIEventListener.Get(m_btnGo).onClick = OnBtnAdd;
        //UIEventListener.Get(transform.Find("bg").gameObject).onClick = OnClickMe;
    }

    void OnShowOpratePanel(GameObject go)
    {
        if (IsSys) return;
        
        PlayerOpreatePanel.ViewType vt = PlayerOpreatePanel.ViewType.Normal;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FriendPanel))
        {
            FriendPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<FriendPanel>(PanelID.FriendPanel);
            if (panel != null)
            {
                switch (panel.CurrList)
                {
                    case FriendPanel.SecondTab.Contacts:
                        vt = PlayerOpreatePanel.ViewType.AddRemove_Contact;
                        break;
                    case FriendPanel.SecondTab.Interactive:
                        vt = PlayerOpreatePanel.ViewType.AddRemove_Interact;
                        break;
                    case FriendPanel.SecondTab.Enemy:
                        vt = PlayerOpreatePanel.ViewType.AddRemove_Enemy;
                        break;
                    case FriendPanel.SecondTab.BlackList:
                        vt = PlayerOpreatePanel.ViewType.AddRemove_Shield;
                        break;
                    default:
                        break;
                }
            }
        }
        DataManager.Instance.Sender.RequestPlayerInfoForOprate(m_data.uid, vt);

    }

    void OnBtnAdd(GameObject go)
    {
        if (m_data != null)
        {
            DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, m_data.uid);
        }
    }

    void OnClickMe(GameObject go)
    {
        if (m_parent != null)
        {
            m_parent.DeActiveFrendItem();
           // m_parent.OnSelectFriend(m_data, this);
        }

    }

    public void SetMaskState(bool active)
    {
        if (m_marskGo != null)
        {
            m_marskGo.SetActive(active);
        }
    }

    public void SetTipsState(bool active)
    {
        if (m_tipsGo != null)
        {
            m_tipsGo.SetActive(active);
        }
    }

    public void SetAddBtnState(bool active)
    {
        if (m_btnGo != null)
            m_btnGo.SetActive(active);
    }

    public void SetParent(FriendPanel  parent)
    {
        m_parent = parent;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is RoleRelation)
        {
            m_data = (RoleRelation)data;

            if (m_data.isSys)
            {
                m_lableName.text = "系统消息";
                m_lableLevel.text = "";
                m_headIcon.ChangeSprite(5);
                m_headIcon.MakePixelPerfect();

                SetTipsState(DataManager.Manager<ChatDataManager>().PrivateChatManager.GetNewMsgTipsWithId(0));
                m_btnGo.SetActive(false);
            }
            else
            {
                m_headIcon.ChangeSprite(m_data.profession);
                m_headIcon.MakePixelPerfect();

                m_lableLevel.text = string.Format("Lv.{0}", m_data.level);
                m_lableName.text = m_data.name;

                m_btnGo.SetActive(m_parent.CurrContent == FriendPanel.FriendPanelPageEnum.Page_添加);
                SetTipsState(DataManager.Manager<ChatDataManager>().PrivateChatManager.GetNewMsgTipsWithId(m_data.uid));
            }


            if (m_choose != null)
            {
                m_choose.SetActive(m_parent.CurrContent != FriendPanel.FriendPanelPageEnum.Page_添加);
            }

            RefreshLevel();

        }
    }

    public void RefreshLevel()
    {
        if (m_data == null && m_data.isSys)
        {
            return;
        }

        m_lableLevel.text = string.Format("Lv.{0}", m_data.level);
        m_lableName.text = m_data.name;
        m_labelOnline.text = m_data.online ? "在线" : "";
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_data != null)
        {
            m_data = null;
        }
    }
}