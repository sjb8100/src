using System;
using UnityEngine;

public class SingleFrendItem :MonoBehaviour
{
    UISprite m_headIcon;
    UILabel m_lableLevel;
    UILabel m_lableName;
    GameObject m_tipsGo;
    GameObject m_btnGo;
    GameObject m_marskGo;
    GameObject m_choose;
    FriendPanel m_parent;
    RoleRelation m_data;
    private bool m_issys;
    public bool IsSys
    {
        get { return m_issys; }
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
        m_headIcon = transform.Find("bg/icon").GetComponent<UISprite>();
        m_lableLevel = transform.Find("bg/level").GetComponent<UILabel>();
        m_lableName = transform.Find("bg/name").GetComponent<UILabel>();
        m_tipsGo = transform.Find("bg/warning").gameObject;
        m_btnGo = transform.Find("bg/btn_add").gameObject;
        m_btnGo.gameObject.SetActive(false);
        m_marskGo = transform.Find("bg/select").gameObject;
        m_choose = transform.Find("bg/btn_choose").gameObject;
        UIEventListener.Get(m_headIcon.gameObject).onClick = OnShowOpratePanel;

        UIEventListener.Get(m_btnGo).onClick = OnBtnAdd;
        UIEventListener.Get(transform.Find("bg").gameObject).onClick = OnClickMe;
    }

    void OnShowOpratePanel(GameObject go)
    {
        if (IsSys) return;

//         if (m_data.type == GameCmd.RelationType.Relation_Enemy)
// 	    {
//             data.strRemoveBtnName = "删除仇敌";
// 	    }



        DataManager.Instance.Sender.RequestPlayerInfoForOprate(m_data.uid, PlayerOpreatePanel.ViewType.Normal);
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
        if (m_parent  != null)
        {
            m_parent.DeActiveFrendItem();
            //m_parent.OnSelectFriend(IsSys, m_data,this);
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

    public void Init(FriendPanel parent,bool isSys,RoleRelation data)
    {
        m_parent = parent;
        m_issys = isSys;
        m_data = data;

        if (isSys)
        {
            m_lableName.text = "系统消息";
            m_lableLevel.text = "";
            m_headIcon.spriteName = "icon_system";
            SetTipsState(DataManager.Manager<ChatDataManager>().PrivateChatManager.GetNewMsgTipsWithId(0));
            m_btnGo.SetActive(false);
        }
        else
        {
            m_headIcon.spriteName = "icon_head";
            m_lableLevel.text = "Lv." + data.level.ToString();
            m_lableName.text = data.name;

            m_btnGo.SetActive(m_parent.CurrContent == FriendPanel.FriendPanelPageEnum.Page_添加);
            SetTipsState(DataManager.Manager<ChatDataManager>().PrivateChatManager.GetNewMsgTipsWithId(data.uid));
        }


        if (m_choose != null)
        {
            m_choose.SetActive(m_parent.CurrContent != FriendPanel.FriendPanelPageEnum.Page_添加);
        }
    }

    public void UpdateUI()
    {

    }

}