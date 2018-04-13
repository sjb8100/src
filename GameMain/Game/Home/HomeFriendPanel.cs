using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;


partial class HomeFriendPanel : UIPanelBase
{
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    List<RoleRelation> friendList = null;
    List<RoleRelation> myFriendList = null;
    List<RoleRelation> searchList = null;


    UIGridCreatorBase homeFriendGridCreator;

    enum FriendType
    {
        MyFriend,
        SearchFriend
    }

    FriendType m_eFriendType = FriendType.MyFriend;

    #region override

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {

    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        DataManager.Manager<RelationManager>().ValueUpdateEvent += OnValueUpdateEventArgs;

        InitHomeFriend();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eHomeFriendUpdate)
        {
            InitHomeFriend();
        }
        return true;
    }

    #endregion

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if ("SEARCH" == v.key)
        {
            List<RoleRelation> datas = (List<RoleRelation>)v.newValue;
            searchList = datas;
        }

        if (RelationType.Relation_Friend.ToString() == v.key)
        {
            List<RoleRelation> datas = (List<RoleRelation>)v.newValue;
            if (searchList != null)
            {
                if (searchList.Exists((RoleRelation rr) => {return rr.uid == datas[0].uid ? true:false;}))
                    {
                        searchList = null;
                    }
            }
        }

        InitHomeFriend();
    }

    void InitHomeFriend()
    {
        if (m_eFriendType == FriendType.MyFriend)
        {
            if (DataManager.Manager<RelationManager>().GetRelationListByType(RelationType.Relation_Friend, out myFriendList))
            {
                friendList = myFriendList;
            }
        }
        else
        {
            friendList = searchList;
        }

        if (friendList == null || friendList.Count == 0)
        {
            m_trans_listContent.gameObject.SetActive(false);
        }
        else 
        {
            m_trans_listContent.gameObject.SetActive(true);
        }

        if (friendList != null && friendList.Count > 0)
        {
            UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uihomefriendgrid) as UnityEngine.GameObject;

            homeFriendGridCreator = m_trans_listScrollView.GetComponent<UIGridCreatorBase>();
            if (homeFriendGridCreator == null)
            {
                homeFriendGridCreator = m_trans_listScrollView.gameObject.AddComponent<UIGridCreatorBase>();
            }

            homeFriendGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            homeFriendGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            homeFriendGridCreator.gridWidth = 320;
            homeFriendGridCreator.gridHeight = 100;
            //homeFriendGridCreator.rowcolumLimit = 5;
            homeFriendGridCreator.RefreshCheck();
            homeFriendGridCreator.Initialize<UIHomeFriendGrid>(obj, OnHomeFriendGridDataUpdate, OnHomeFriendgGridUIEvent);
            homeFriendGridCreator.CreateGrids((null != friendList) ? friendList.Count : 0);
        }

    }

    private void OnHomeFriendGridDataUpdate(UIGridBase data, int index)
    {
        if (null != friendList && index < friendList.Count)
        {
            data.SetGridData(friendList[index]);
        }
    }

    private void OnHomeFriendgGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                //进入别人的家园


                break;
        }
    }

    void SetReflashBtn() 
    {
        m_btn_btn_refresh.enabled = true;
    }


    #region click




    void onClick_Btn_close_Btn(GameObject caster)
    {
        //m_trans_searchContent.GetChild(0);
    }

    void onClick_FriendList_Btn(GameObject caster)
    {

        m_eFriendType = FriendType.MyFriend;

        InitHomeFriend();
    }

    void onClick_SearchList_Btn(GameObject caster)
    {
        m_eFriendType = FriendType.SearchFriend;
        InitHomeFriend();
    }

    void onClick_Btn_refresh_Btn(GameObject caster)
    {
        DataManager.Manager<HomeDataManager>().ReqHomeFriendStatus();
        //m_btn_btn_refresh.enabled = false;
        //Invoke("SetReflashBtn", 300);
    }

    void onClick_Btn_search_Btn(GameObject caster)
    {
        myFriendList = null;
        string name = m_input_Level_Input.value;
        //还缺搜索好友

        if (!string.IsNullOrEmpty(m_input_Level_Input.value))
        {
            uint uid = 0;
            if (uint.TryParse(m_input_Level_Input.value, out uid))
            {
                DataManager.Instance.Sender.RequestSearchUserRelation(uid, "");
            }
            else
            {
                DataManager.Instance.Sender.RequestSearchUserRelation(0, m_input_Level_Input.value);
            }
        }
    }

    #endregion
}
