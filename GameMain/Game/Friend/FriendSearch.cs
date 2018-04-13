using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FriendSearch : MonoBehaviour
{
    UILabel m_lableName;
    UILabel m_lableLv;
    UISpriteEx m_headIcon;
    GameObject m_btnAdd;

    RoleRelation m_data;
    void Awake()
    {
        m_lableName = transform.Find("name").GetComponent<UILabel>();
        m_lableLv = transform.Find("level").GetComponent<UILabel>();
        m_headIcon = transform.Find("icon").GetComponent<UISpriteEx>();
        m_btnAdd = transform.Find("btn_add").gameObject;

        if (m_btnAdd != null)
        {
            UIEventListener.Get(m_btnAdd).onClick = OnBtnAdd;
        }
        if (m_headIcon != null)
        {
            UIEventListener.Get(m_headIcon.gameObject).onClick = OnBtnHead;
        }
    }

    void OnBtnHead(GameObject go)
    {
        if (null != m_data)
        {
            DataManager.Instance.Sender.RequestPlayerInfoForOprate(m_data.uid, PlayerOpreatePanel.ViewType.Normal);
        }
    }

    void OnBtnAdd(GameObject go)
    {
        if (m_data != null)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(m_data.uid))
            {
                TipsManager.Instance.ShowTipsById(510);
                return;
            }

            List<RoleRelation> list;
            if (DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out list))
            {
                uint maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("MAX_FRIEND_SIZE");

                if (list.Count >= maxNum)
                {
                    TipsManager.Instance.ShowTipsById(503);
                    return;
                }
            }
            DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, m_data.uid);
        }
    }

    public void SetInfo(RoleRelation data,string strSearch)
    {
        m_data = data;
        m_lableLv.text = "Lv." + data.level.ToString();
        m_lableLv.depth = m_headIcon.depth + 1;
        m_headIcon.ChangeSprite(data.profession);
        string strname = data.name;
        int index = strname.IndexOf(strSearch);
        if (index != -1)
        {
            strname = strname.Insert(index + strSearch.Length, "[-]");
            strname = strname.Insert(index, "[00ff00]");

            //strname = strname.Substring(0, index) + "[00ff00]" + strname.Substring(index, strSearch.Length) + "[-]" + strname.Substring(index + strSearch.Length);
        }
        m_lableName.text = strname;
    }
}
