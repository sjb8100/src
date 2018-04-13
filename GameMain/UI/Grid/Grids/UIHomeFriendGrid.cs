using Cmd;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIHomeFriendGrid : UIGridBase
{
    UISprite m_spIcon;
    UILabel m_lblName;
    UILabel m_lblJob;
    UILabel m_lblLv;
    GameObject m_btnAdd;    //加为好友
    GameObject m_btnZan;    //赞
    GameObject m_goNoOpen;    //未开放
    GameObject m_btnEnter;  //进入好友家园

    RoleRelation m_frdData;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_spIcon = this.transform.Find("friend_icon").GetComponent<UISprite>();
        m_lblName = this.transform.Find("friend_name").GetComponent<UILabel>();
        m_lblJob = this.transform.Find("friend_job").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("friend_lv").GetComponent<UILabel>();
        m_goNoOpen = this.transform.Find("NoOpen").gameObject;
        m_btnAdd = this.transform.Find("btn_add").gameObject;
        m_btnZan = this.transform.Find("btn_zan").gameObject;
        m_btnEnter = this.gameObject;

        UIEventListener.Get(m_btnAdd).onClick = OnClickAddFrd;
        UIEventListener.Get(m_btnZan).onClick = OnClickZan;
        UIEventListener.Get(m_btnEnter).onClick = onClickEnterFrdHome;

        EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, DoGameEvent);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_frdData = data as RoleRelation;


        m_spIcon.spriteName = "roleCard_huanshi";
        m_lblName.text = this.m_frdData.name;
        m_lblJob.text = ConstDefine.GetProfessionNameByType((enumProfession)this.m_frdData.profession);
        m_lblLv.text = this.m_frdData.level.ToString() + "级";

        List<RoleRelation> friendList = null;
        if (DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out friendList))
        {
            if (friendList.Exists((RoleRelation rr) => { return rr.uid == this.m_frdData.uid ? true : false; }))
            {
                if (this.m_frdData.help_tree)
                {
                    m_btnZan.SetActive(true);
                }
                else
                {
                    m_btnZan.SetActive(false);
                    if (this.m_frdData.level >= 20)  //暂时定20级
                    {
                        m_goNoOpen.gameObject.SetActive(false);
                    }
                    else
                    {
                        m_goNoOpen.gameObject.SetActive(true);//未开启家园
                    }
                }
                m_btnAdd.SetActive(false);
            }
            else   //搜索到的
            {
                m_btnAdd.SetActive(true);
                m_goNoOpen.SetActive(false);
                m_btnZan.SetActive(false);
            }
        }


    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.HOME_HELPSUCCESS)
        {
            if (this.m_frdData.help_tree)
            {
                m_btnZan.SetActive(true);
            }
            else
            {
                m_btnZan.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="go"></param>
    void OnClickAddFrd(GameObject go)
    {
        DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, this.m_frdData.uid);
        m_btnZan.gameObject.SetActive(false);
    }

    /// <summary>
    /// 给好有点赞
    /// </summary>
    /// <param name="go"></param>
    void OnClickZan(GameObject go)
    {
        DataManager.Manager<HomeDataManager>().ReqZanFriend(this.m_frdData.uid);
    }

    /// <summary>
    /// 点击好友，请求好友家园数据
    /// </summary>
    /// <param name="go"></param>
    void onClickEnterFrdHome(GameObject go)
    {
        DataManager.Manager<HomeDataManager>().ReqAllUserHomeData(this.m_frdData.uid);
    }

}

