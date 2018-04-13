using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using Common;

partial class HomeControlPanel: UIPanelBase
{
      public uint currUID = 0;
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    TreeData treeData
    {
        get
        {
            return homeDM.GetTreeData();
        }
    }

    void EventCallBack(int nEventID, object param) 
    {
        TreeHelpSuccess();
    }

    
    #region  点赞
    /// <summary>
    /// 点赞
    /// </summary>
    /// <param name="caster"></param> 
    void onClick_Btn_tree_Btn(GameObject caster)
    {
        //获取进入的是谁的家园这个目前还没有 以后换成进入家园的角色ID就行了   根据这个角色Id获取他的许愿树等级
        uint selfId = ClientGlobal.Instance().MainPlayer.GetID();
        if(!homeDM.IsMyHome())
        {
            selfId = homeDM.m_uHomeUserID;
        }
        //判断对方是不是自己的好友
        DataManager.Manager<HomeDataManager>().ClickTree(selfId);
    }
    public void TreeHelpSuccess()
    {
        //显示+1的tips
//         TipsManager.Instance.ShowTips("点赞成功");
//         UISprite sp = m_btn_btn_tree.GetComponentInChildren<UISprite>();
//         sp.gameObject.SetActive(false);
    }
    #endregion

    
}
