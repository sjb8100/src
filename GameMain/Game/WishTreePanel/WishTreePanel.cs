using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Common;
using GameCmd;
using table;
using Client;

partial class WishTreePanel : UIPanelBase
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
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        homeDM.GetTreeData();
//         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_BUYTREE, EventCallBack);
//         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_TREEBEGIN, EventCallBack);
//         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_TREEGAIN, EventCallBack);
//         Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);
        homeDM.ValueUpdateEvent += OnValueUpdateEventArgs;
    }
    void EventCallBack(int nEventID, object param) 
    {
//         if (nEventID == (int)Client.GameEventID.HOME_BUYTREE)
//         {
//             RefreshTreeState();
// 
//         }
//         if (nEventID == (int)Client.GameEventID.HOME_TREEBEGIN)
//         {
// 
//             RefreshTreeTime();
//         }
//         if (nEventID == (int)Client.GameEventID.HOME_TREEGAIN)
//         {
//             RefreshTreeGain();
// 
//         }
//         if (nEventID == (int)Client.GameEventID.HOME_HELPSUCCESS)
//         {
// 
// 
//         }
    }


    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
//         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_BUYTREE, EventCallBack);
//         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_TREEBEGIN, EventCallBack);
//         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_TREEGAIN, EventCallBack);
//         Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);
        homeDM.ValueUpdateEvent += OnValueUpdateEventArgs;
        homeDM.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnValueUpdateEventArgs(object obj,ValueUpdateEventArgs v)
    { 
    if(v.key == HomeDispatchEvent.ChangeTreeState.ToString())
    {
       RefreshTreeState();   
    }
    }

    void OnDisable()
    {
        currUID = 0;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eSetPropRoleID)
        {
            currUID = (uint)param;
            ShowByUID(currUID);
        }

        return true;
    }
    protected override void OnShow(object data)
    {
        ShowByUID(ClientGlobal.Instance().MainPlayer.GetID());
        ShowTreeBtn();
        base.OnShow(data);
    }

    public void ShowByUID(uint id)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(id);
        if (player == null)
        {
            return;
        }      
        int level = player.GetProp((int)CreatureProp.Level);

        #region 基础经验显示
        for (int i = 1; i < 5;i++ )
        {
            uint treeID = uint.Parse("50" + i);  
            if(treeID ==null)
            {
                return;
            }
            table.WishingTreeDataBase data = GameTableManager.Instance.GetTableItem<table.WishingTreeDataBase>(treeID);
            uint level_min2 = data.level_min2;
            GameObject tree = m_widget_tree.transform.Find("tree_" + i).gameObject;
            UILabel exp = tree.transform.Find("exp").GetComponent<UILabel>();
            if (level < level_min2)
            {
                exp.text = (level * data.master_exp_mul1).ToString();
            }
            else 
            {
                exp.text = (level * data.master_exp_mul2).ToString();
            }
            Transform status = tree.transform.Find("status").GetComponent<Transform>();
            UISprite btn = status.transform.Find("btn_buy_" + i).GetComponent<UISprite>();
            UILabel price = btn.GetComponentInChildren<UILabel>();
            price.text = data.price.ToString();

        }

        #region 元宝显示

        #endregion
        #endregion


    }


    void ShowTreeBtn() 
    {
    //根据玩家已有的树级别进行判断显示
        if (homeDM.MaxTreeID == 501) 
        {
            //默认UI显示就行

        }
        if (homeDM.MaxTreeID == 502)
        {
            //1改成"已购买"
            SetAllUsingText(1,"已购买");
            //2改成"使用中"
            m_btn_btn_buy_2.gameObject.SetActive(false);
            SetAllUsingText(2,"使用中");
        }
        if (homeDM.MaxTreeID == 503)
        {
            SetAllUsingText(1, "已购买");
            m_btn_btn_buy_3.gameObject.SetActive(false);
            SetAllUsingText(3, "使用中");
            if (homeDM.TreeIDs.Contains(502))
            {
                m_btn_btn_buy_2.gameObject.SetActive(false);
                SetAllUsingText(2, "已购买");
            }
        }
        if (homeDM.MaxTreeID == 504)
        {

            SetAllUsingText(1, "已购买");
            m_btn_btn_buy_4.gameObject.SetActive(false);
            SetAllUsingText(4, "使用中");

            if (homeDM.TreeIDs.Contains(503) && homeDM.TreeIDs.Contains(502))
            {
                m_btn_btn_buy_2.gameObject.SetActive(false);
                SetAllUsingText(2, "已购买");
                m_btn_btn_buy_3.gameObject.SetActive(false);
                SetAllUsingText(3, "已购买");
            }
            else if (homeDM.TreeIDs.Contains(502))
            {
                m_btn_btn_buy_2.gameObject.SetActive(false);
                SetAllUsingText(2, "已购买");
            }
            else if (homeDM.TreeIDs.Contains(503))
            {
                m_btn_btn_buy_3.gameObject.SetActive(false);
                SetAllUsingText(3, "已购买");
            }       
        }

    }

    public void RefreshTreeState() 
    {
        m_sprite_reminder.gameObject.SetActive(false);
        //购买新树刷新UI+使用其他等级的树
        ShowTreeBtn();       
    }

    /// <summary>
    /// 刷新树的时间
    /// </summary>
    public void RefreshTreeTime()
    { 
    }

    /// <summary>
    /// 树收获
    /// </summary>
    public void RefreshTreeGain()
    {
 
        
        
    }

   
    void onClick_Close_Btn(GameObject caster) 
    {
        m_sprite_reminder.gameObject.SetActive(false);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.WishTreePanel);
    }




    #region  买树
    int buyID = 0;
    void SetBuyBtnAllStatus(GameObject obj) 
    {

        string[] name = obj.name.Split("_".ToCharArray());
        buyID = int.Parse("50" + name[2]);
        if (buyID > treeData.max_tree)
        {
            Buy();
        }
        else 
        {
        //弹出框"已购更高级别的树，是否继续购买".
            m_sprite_reminder.gameObject.SetActive(true);
        }
    }

    //买树用的按键
    void onClick_Btn_buy_1_Btn(GameObject caster)
    {
        SetBuyBtnAllStatus(caster);
    }
    void onClick_Btn_buy_2_Btn(GameObject caster)
    {
        SetBuyBtnAllStatus(caster);
    }
    void onClick_Btn_buy_3_Btn(GameObject caster)
    {
        SetBuyBtnAllStatus(caster);
    }
    void onClick_Btn_buy_4_Btn(GameObject caster)
    {
        SetBuyBtnAllStatus(caster);
    }



    void onClick_Cancel_Btn(GameObject caster)
    {
        m_sprite_reminder.gameObject.SetActive(false);

    }
    void onClick_Confirm_Btn(GameObject caster)
    {
     /*元宝是否足够，若否，则出现通用充值界面
     上述判断全部通过，则完成购买，出现提示：成功购买XXX（114527）。并使用ID最大的许愿树数据，同时标识为“使用中”，其他非可购买list标识为“已购买”*/
        Buy();
       
    }
    void Buy()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            uint cold = (uint)player.GetProp((int)PlayerProp.Cold);
            table.WishingTreeDataBase data = GameTableManager.Instance.GetTableItem<table.WishingTreeDataBase>(treeData.max_tree);
            uint price = data.price;
            if (cold >= price)
            {
                DataManager.Manager<HomeDataManager>().BuyTree(buyID);
            }
            else
            {
                //充值页面
            }
        }
    }
    void SetAllUsingText(int arg, string text)
    {
        GameObject targetTree = m_widget_tree.transform.Find("tree_" + arg).gameObject;
        GameObject targetStatus = targetTree.transform.Find("status").gameObject;
        GameObject useBtn = targetStatus.transform.Find("using").gameObject;
        UILabel usingLabel = useBtn.GetComponent<UILabel>();
        usingLabel.text = text;
        useBtn.SetActive(true);
    }
    #endregion
}
