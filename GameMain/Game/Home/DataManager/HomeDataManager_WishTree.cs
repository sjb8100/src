using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Engine;
using Engine.Utility;
using Client;
using table;


partial class HomeDataManager
{
    //许愿树数据
    TreeData treeData;
    WishTreePanel treeUI
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.WishTreePanel) as WishTreePanel;
        }
    }
    HomeControlPanel treeModel
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.HomeControlPanel) as HomeControlPanel;
        }
    }
    public enum TreeState
    {
        Iron = 1,
        Copper,
        Sliver,
        Gold,
    }
    /// <summary>
    /// 已经买的树的ID
    /// </summary>
    List<uint> treeIDs = null;
    public List<uint> TreeIDs
    {
        get
        {
            return treeIDs;
        }
        set
        {
            treeIDs = value;
        }
    }

    bool helpSelf = false;
    public bool HelpSelf
    {
        get
        {
            return helpSelf;
        }
        set
        {
            helpSelf = value;
        }
    }
    /// <summary>
    /// 最大树的ID
    /// </summary>
    uint maxTreeID = 0;
    public uint MaxTreeID
    {
        get
        {
            return maxTreeID;
        }
        set
        {
            maxTreeID = value;
        }
    }

    /// <summary>
    /// 这棵树集赞的数量
    /// </summary>
    uint helpNum = 0;
    public uint HelpNum
    {
        get
        {
            return helpNum;
        }
        set
        {
            helpNum = value;
        }
    }

    /// <summary>
    /// 剩余时间（秒）
    /// </summary>
    uint treeLeftTime = 0;
    public uint TreeLeftTime
    {
        get
        {
            return treeLeftTime;
        }
        set
        {
            treeLeftTime = value;
        }
    }
    List<uint> helpList = null;
    public List<uint> HelpList
    {
        get
        {
            return helpList;
        }
        set
        {
            helpList = value;
        }
    }
    void OnClickTreeEntity(IEntity et)
    {
        if (et == null)
        {
            Log.Error("et is null");
            return;
        }
        HomeEntityInfo info = EntityStateDic[et.GetUID()];
        selectLandIndex = info.index;
        uint selfID = ClientGlobal.Instance().MainPlayer.GetID();
        if (!IsMyHome())
        {
            selfID = m_uHomeUserID;
        }
        ClickTree(selfID);

    }
    void RefreshTreeUI()
    {
        // HomeSceneUIRoot.Instance.ShowTreeSceneUI();
    }

    /// <summary>
    /// 点击实体  传人物ID进去
    /// </summary>
    /// <param name="selfId"></param>
    public void ClickTree(uint selfId)
    {
        //获取进入的是谁的家园这个目前还没有 以后换成进入家园的角色ID就行了   根据这个角色Id获取他的许愿树等级
        if (DataManager.Manager<RelationManager>().IsMyFriend(selfId) || selfId == ClientGlobal.Instance().MainPlayer.GetID())
        {
            //树不在集赞状态  可以收获
            stReqTreeDataHomeUserCmd_C cmd = new stReqTreeDataHomeUserCmd_C();
            NetService.Instance.Send(cmd);
            table.WishingTreeDataBase data = GameTableManager.Instance.GetTableItem<table.WishingTreeDataBase>(MaxTreeID);
            uint max_help_num = data.loveMaxNum;
            if (HelpNum < max_help_num)
            {
                //园主自己的点赞
                if (selfId == ClientGlobal.Instance().MainPlayer.GetID())
                {
                    if (HelpSelf)
                    {
                        if (TreeLeftTime > 0 && TreeIDs.Count < 4)
                        {
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.WishTreePanel);
                        }
                    }
                    else
                    {
                        stHelpTreeHomeUserCmd_CS c = new stHelpTreeHomeUserCmd_CS() { help_who = selfId };
                        NetService.Instance.Send(c);
                    }
                }
                //好友的点赞
                else 
                {
                    stHelpTreeHomeUserCmd_CS c = new stHelpTreeHomeUserCmd_CS() { help_who = selfId };
                    NetService.Instance.Send(c);
                }
            }
            else 
            {
                if (TreeIDs.Count < 4)
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.WishTreePanel);
                }
                else 
                {
                    TipsManager.Instance.ShowTipsById(114122);
                }
            }
        }
        else
        {
            //显示添加好友的TipsManager
            Action AddFriend = delegate
            {
                DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, selfId);
            };
            TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, "您和他不是好友,是否添加好友?", AddFriend, null,null, "提示", "确定", "取消");
        }
    }



    /// <summary>
    /// 刷新树的数据
    /// </summary>
    /// <param name="data"></param>
    void RefreshTreeData(TreeData data)
    {
        treeData = data;
        MaxTreeID = data.max_tree;
        TreeIDs = data.have_trees;
        HelpNum = data.help_num;
        TreeLeftTime = data.cost_time;
        HelpSelf = data.help_self;
        HelpList = data.help_list;

    }

    public TreeData GetTreeData()
    {
        if (treeData == null)
        {
            Log.Error("tree data is null");
        }
        return treeData;
    }

    public void OnRecieveTreeMsg(TreeData data)
    {
        RefreshTreeData(data);
        if ( TreeLeftTime == 0)
        {
            stTreeGainHomeUserCmd_CS cmd = new stTreeGainHomeUserCmd_CS();
            NetService.Instance.Send(cmd);
        }
        //----------------为了GM命令设置剩余时间为0才派发下面这个事件，后期需要删除----------------------------------------------------
        /*********************************************************************************
                *********************************************************************************
                    *********************************************************************************
                        *********************************************************************************/
        TreeLeftTime = data.cost_time;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HOME_TREEBEGIN, null);
    }

    public void BuyTree(int treeId)
    {
        if (treeId != null)
        {
            stBuyTreeHomeUserCmd_CS cmd = new stBuyTreeHomeUserCmd_CS() { tree = treeId };
            NetService.Instance.Send(cmd);
        }

    }


    /// <summary>
    /// 买树处理返回的消息
    /// </summary>
    public void OnBuyTree(stBuyTreeHomeUserCmd_CS cmd)
    {
        uint treeid = (uint)cmd.tree;
        string msg = "";
        if (treeUI != null)
        {
            if (treeid < MaxTreeID)
            {
                TreeIDs.Add(treeid);
            }
            else
            {
                MaxTreeID = treeid;
                TreeIDs.Add(treeid);
            }
        }
        //TipsManager.Instance.ShowTips("成功购买许愿树");
        if (treeid == 502)
        {
            msg = "铜·许愿树";
        }
        if (treeid == 503)
        {
            msg = "银·许愿树";
        }
        if (treeid == 504)
        {
            msg = "金·许愿树";
        }
        TipsManager.Instance.ShowTipsById(114527, msg);
        treeUI.RefreshTreeState();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HOME_BUYTREE, null);
    }

    /// <summary>
    /// 树开始集赞  传过来的是剩余时间
    /// </summary>
    /// <param name="costtime"></param>
    public void OnTreeBegin(uint costtime)
    {
        TreeLeftTime = costtime;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HOME_TREEBEGIN, null);
    }

    public void OnTreeGain(int tree, int help_num, int exp)
    {

        MaxTreeID = (uint)tree;
        HelpNum = 0;
        TipsManager.Instance.ShowTipsById(114528, "经验x", exp);
        stTreeBeginHomeUserCmd_CS cmd = new stTreeBeginHomeUserCmd_CS();
        NetService.Instance.Send(cmd);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HOME_TREEGAIN, null);
    }

    public void OnTreeHelp(uint help_who, int tree)
    {
        HelpNum += 1;
        if (help_who == ClientGlobal.Instance().MainPlayer.GetID())
        {
            HelpSelf = true;
        }

        if (help_who != ClientGlobal.Instance().MainPlayer.GetID())//给别人点赞
        {
            List<RoleRelation> friendList = null;
            DataManager.Manager<RelationManager>().GetRelationListByType(RelationType.Relation_Friend, out friendList);
            if (friendList != null)
            {
                RoleRelation data = friendList.Find((RoleRelation rr) => { return rr.uid == help_who; });
                data.help_tree = false;
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HOME_HELPSUCCESS, null);
    }




    void OnTreeProcess()
    {
        if (TreeLeftTime > 0)
        {
            TreeLeftTime -= 1;
        }

    }
}
