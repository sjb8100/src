//*************************************************************************
//	创建日期:	2016-10-10 10:44
//	文件名称:	RewardCard.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	悬赏任务令牌
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class RewardCard : UIGridBase
{
    const int COSTNUM = 1;
    UILabel m_labelName = null;
    UITexture m_textureIcon = null;
    UILabel m_labelCost = null;
    UILabel m_labelBtn = null;
    GameObject m_goOpen = null;
    UISprite m_spriteCostIcon = null;
    UILabel m_labelOpenLevel = null;
    UILabel m_labelMissionNum = null;
    UILabel m_lblExp = null;
    UIAtlas m_btnAtlas = null;
    RewardPanel m_parent = null;
    int m_nIndex;
    private CMResAsynSeedData<CMTexture> m_iuiIconSeed = null;

    public int Index { get { return m_nIndex; } }
    table.PublicTokenDataBase m_PublicTokenDataBase = null;
    table.AcceptTokenDataBase m_AcceptTokenDataBase = null;
    public uint TaskId
    {
        get
        {
            if (m_PublicTokenDataBase != null)
            {
                return m_PublicTokenDataBase.taskid;
            }
            if (m_AcceptTokenDataBase != null)
            {
                return m_AcceptTokenDataBase.taskid;
            }
            return 0;
        }
    }

    public uint CostMoney
    {
        get
        {
            if (m_AcceptTokenDataBase != null)
            {
                return m_AcceptTokenDataBase.costMoney;
            }
            return 0;
        }
    }
    public uint TaskType
    {
        get
        {
            if (m_PublicTokenDataBase != null)
            {
                return 1;
            }
            if (m_AcceptTokenDataBase != null)
            {
                return 2;
            }
            return 0;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iuiIconSeed)
        {
            m_iuiIconSeed.Release(true);
            m_iuiIconSeed = null;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_labelName = transform.Find("top/Label").GetComponent<UILabel>();
        m_textureIcon = transform.Find("icon").GetComponent<UITexture>();
        m_goOpen = transform.Find("open").gameObject;
        m_labelCost = m_goOpen.transform.Find("gold").GetComponent<UILabel>();

        m_labelMissionNum = m_goOpen.transform.Find("missionNum").GetComponent<UILabel>();
        GameObject btn = m_goOpen.transform.Find("btn").gameObject;
        if (btn != null)
        {
            UIEventListener.Get(btn).onClick = OnBtnClick;
            m_btnAtlas = btn.GetComponent<UISprite>().atlas;

            m_labelBtn = btn.transform.Find("Label").GetComponent<UILabel>();
        }
        m_spriteCostIcon = m_goOpen.transform.Find("smallIcon").GetComponent<UISprite>();


        m_labelOpenLevel = transform.Find("lock/openLevel").GetComponent<UILabel>();
        m_lblExp = this.transform.Find("reward/acceptExp").GetComponent<UILabel>();

        //去掉点击事件（策划要求）
        //UIEventListener.Get(gameObject).onClick = OnClickMe;
    }

    void OnEnable()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
    }

    void OnDisable()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
    }

    public void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                ItemDefine.UpdateItemPassData itemdata = (ItemDefine.UpdateItemPassData)data;
                if (TaskType == 1)
                {
                    BaseItem item = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(itemdata.QWThisId);
                    int itemNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_PublicTokenDataBase.tokenItemid);
                    if (item != null)
                    {
                        if (item.BaseId == m_PublicTokenDataBase.tokenItemid)
                        {
                            m_labelCost.text = string.Format("{0}{1}[-]/{2}", itemNum >= 1 ? "[ffffff]" : "[ff0000]", itemNum, 1);
                        }
                    }
                    else
                    {
                        m_labelCost.text = string.Format("{0}{1}[-]/{2}", itemNum >= 1 ? "[ffffff]" : "[ff0000]", itemNum, 1);
                    }
                    if (m_labelBtn != null)
                    {
                        if (itemNum >= COSTNUM)
                        {
                            m_labelBtn.text = "发 布";
                        }
                        else
                        {
                            m_labelBtn.text = "获 取";
                        }
                    }
                }
                break;
        }
    }

    void OnBtnClick(GameObject go)
    {
        int nlevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);

        if (TaskType == 1)
        {
            int itemNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_PublicTokenDataBase.tokenItemid);
            if (itemNum >= COSTNUM || m_parent.IsUseMoney())
            {
                //发布
                NetService.Instance.Send(new GameCmd.stPublicTokenTaskScriptUserCmd_CS()
                {
                    tokentaskid = m_PublicTokenDataBase.id,
                    userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                    isusemoney = m_parent.IsUseMoney() ? (uint)1 : 0,
                });
            }
            else
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_PublicTokenDataBase.tokenItemid);
            }
        }
        else if (TaskType == 2)
        {
            RewardMisssionInfo receiveReward = DataManager.Manager<TaskDataManager>().RewardMisssionData.receiveReward;
            if (receiveReward != null)
            {
                if (receiveReward.id == m_AcceptTokenDataBase.id)
                {
                    if (receiveReward.nstate == (uint)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT)
                    {
                        /* DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RewardPanel);
                         DataManager.Manager<UIPanelManager>().HidePanel(PanelID.DailyPanel);

                         Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                         new Client.stDoingTask() { taskid = receiveReward.ntaskid });*/

                        //去做任务
                        GoToDoTask(receiveReward.ntaskid);
                    }
                    else if (receiveReward.nstate == (uint)GameCmd.TokenTaskState.TOKEN_STATE_FINISH)
                    {
                        NetService.Instance.Send(new GameCmd.stAcceptTokenTaskFinishScriptUserCmd_C()
                        {
                            tokentaskid = receiveReward.id,
                            //userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                        });
                        NetService.Instance.Send(new GameCmd.stGetTokenTaskNumScriptUserCmd_CS());
                    }
                }
                else
                {
                    //接取
                    AcceptTokenTask();
                }
            }
            else
            {
                AcceptTokenTask();
            }
        }

    }

    private void AcceptTokenTask()
    {
        NetService.Instance.Send(new GameCmd.stAcceptTokenTaskScriptUserCmd_CS()
        {
            tokentaskid = m_AcceptTokenDataBase.id,
            userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
        });

        NetService.Instance.Send(new GameCmd.stGetTokenTaskNumScriptUserCmd_CS());
    }
    void OnClickMe(GameObject go)
    {
        if (m_parent != null)
        {
            m_parent.OnClickCard(this);
        }
    }

    public void Init(table.PublicTokenDataBase database, int index, RewardPanel panel)
    {
        m_AcceptTokenDataBase = null;
        m_PublicTokenDataBase = database;
        m_parent = panel;
        m_nIndex = index;

        if (m_labelName != null)
        {
            m_labelName.text = database.title;
        }

        if (m_textureIcon != null)
        {
           // m_textureIcon.spriteName = database.icon;

            UIManager.GetTextureAsyn(database.bgId, ref m_iuiIconSeed, () =>
            {
                if (m_textureIcon != null)
                {
                    m_textureIcon.mainTexture = null;
                }
            }, m_textureIcon);
        }

        int level = MainPlayerHelper.GetPlayerLevel();

        if (m_goOpen != null)
        {
            m_goOpen.gameObject.SetActive(level >= database.openLvele);
        }
        if (level >= database.openLvele)
        {
            m_labelOpenLevel.text = "";
            SetBottomUI();
        }
        else
        {
            m_labelOpenLevel.text = string.Format("{0}级开启", database.openLvele);
        }

        //exp
        if (m_lblExp != null)
        {
            table.QuestDataBase taskdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(database.taskid);
            if (taskdb != null)
            {
                m_lblExp.text = taskdb.dwRewardExp.ToString();
            }
        }
    }

    public void Init(table.AcceptTokenDataBase database, int index, RewardPanel panel)
    {
        m_PublicTokenDataBase = null;
        m_AcceptTokenDataBase = database;
        m_parent = panel;
        m_nIndex = index;
        if (m_labelName != null)
        {
            m_labelName.text = database.title;
        }

        if (m_textureIcon != null)
        {
            //m_spriteIcon.spriteName = database.icon;
            UIManager.GetTextureAsyn(database.bgId, ref m_iuiIconSeed, () =>
            {
                if (m_textureIcon != null)
                {
                    m_textureIcon.mainTexture = null;
                }
            }, m_textureIcon);
        }

        int level = MainPlayerHelper.GetPlayerLevel();

        if (m_goOpen != null)
        {
            m_goOpen.gameObject.SetActive(level >= database.openLvele);
        }
        if (level >= database.openLvele)
        {
            m_labelOpenLevel.text = "";
            SetBottomUI();
        }
        else
        {
            m_labelOpenLevel.text = string.Format("{0}级开启", database.openLvele);
        }

        //exp
        if (m_lblExp != null)
        {
            table.QuestDataBase taskdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(database.taskid);
            if (taskdb != null)
            {
                List<table.RandomTargetDataBase> randomlist = GameTableManager.Instance.GetTableList<table.RandomTargetDataBase>();
                for (int i = 0, imax = randomlist.Count; i < imax; i++)
                {
                    if (randomlist[i].activity_type == 1 && taskdb.target_group == randomlist[i].group && randomlist[i].level == MainPlayerHelper.GetPlayerLevel())
                    {
                        m_lblExp.text = string.Format("{0}", randomlist[i].level * randomlist[i].exp_ratio);
                        break;
                    }
                }
            }
        }
    }
    public void SetBottomUI()
    {
        if (m_labelCost != null)
        {
            if (TaskType == 1)
            {
                UpdateItemNum();
            }
            else if (TaskType == 2)
            {
                if (m_spriteCostIcon != null)
                {
                    m_spriteCostIcon.gameObject.SetActive(false);
                }
                if (m_labelCost != null)
                {
                    m_labelCost.gameObject.SetActive(false);
                }
                RewardMisssionInfo receiveReward = DataManager.Manager<TaskDataManager>().RewardMisssionData.receiveReward;
                if (receiveReward != null)
                {
                    if (receiveReward.id == m_AcceptTokenDataBase.id)
                    {
                        if (receiveReward.nstate == (uint)GameCmd.TokenTaskState.TOKEN_STATE_FINISH)
                        {
                            if (m_labelMissionNum != null)
                            {
                                m_labelMissionNum.gameObject.SetActive(true);
                                m_labelMissionNum.text = "已完成";
                            }

                            m_labelBtn.text = "领取奖励";
                        }
                        else if (receiveReward.nstate == (uint)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT)
                        {
                            if (m_labelMissionNum != null)
                            {
                                m_labelMissionNum.gameObject.SetActive(true);
                                m_labelMissionNum.text = "进行中";
                            }
                            m_labelBtn.text = "前 往";
                        }
                    }
                    else
                    {
                        SetReceiveUI();
                    }
                }
                else
                {
                    SetReceiveUI();
                }
            }
        }
    }

    public void UpdateItemNum()
    {
        int itemNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_PublicTokenDataBase.tokenItemid);

        if (m_labelMissionNum != null)
        {
            m_labelMissionNum.gameObject.SetActive(false);
        }

        if (m_spriteCostIcon != null && m_labelBtn != null && m_labelCost != null)
        {
            m_spriteCostIcon.gameObject.SetActive(true);
            m_labelCost.gameObject.SetActive(true);
            bool select = m_parent.IsUseMoney();
            if (itemNum < COSTNUM)
            {
                if (!select)
                {
                    //m_spriteCostIcon.atlas = m_spriteIcon.atlas;
                    m_spriteCostIcon.spriteName = m_PublicTokenDataBase.smallicon;
                    m_labelBtn.text = "获 取";
                    m_labelCost.text = string.Format("{0}{1}[-]/{2}", itemNum >= COSTNUM ? "[ffffff]" : "[ff0000]", itemNum, COSTNUM);
                }
                else
                {
                    m_spriteCostIcon.atlas = m_btnAtlas;
                    m_spriteCostIcon.spriteName = "tubiao_huobi_jin";
                    m_labelBtn.text = "发 布";

                    uint needMoney = 0;
                    uint currMoney = 0;
                    bool enough = EnoughMoney(out needMoney, out currMoney);
                    m_labelCost.text = string.Format("{0}{1}[-]/{2}", enough ? "[ffffff]" : "[ff0000]", currMoney, needMoney);
                }
                //m_labelBtn.text = "发 布";
            }
            else
            {
                //m_spriteCostIcon.atlas = m_spriteIcon.atlas;
                m_spriteCostIcon.spriteName = m_PublicTokenDataBase.smallicon;
                m_labelBtn.text = "发 布";
                m_labelCost.text = string.Format("{0}{1}[-]/{2}", itemNum >= COSTNUM ? "[ffffff]" : "[ff0000]", itemNum, COSTNUM);
            }
        }
    }
    bool EnoughMoney(out uint price, out uint coin)
    {
        coin = 0;
        price = 0;
        if (m_PublicTokenDataBase == null)
        {
            return false;
        }
        coin = (uint)MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);
        table.PointConsumeDataBase pointCons = GameTableManager.Instance.GetTableItem<table.PointConsumeDataBase>(m_PublicTokenDataBase.tokenItemid);
        if (pointCons != null)
        {
            price = pointCons.buyPrice;
            return (coin >= pointCons.buyPrice);
        }
        return false;
    }

    private void SetReceiveUI()
    {
        Dictionary<uint, uint> dict = DataManager.Manager<TaskDataManager>().RewardMisssionData.RewardTaskLeftNum;
        uint num = 0;
        if (dict.ContainsKey(m_AcceptTokenDataBase.id))
        {
            num = dict[m_AcceptTokenDataBase.id];
        }
        m_labelMissionNum.gameObject.SetActive(true);
        m_labelMissionNum.text = string.Format("任务：{0}", num);
        m_labelBtn.text = "接 取";
    }

    /// <summary>
    /// 去做任务
    /// </summary>
    /// <param name="receiveTaskId"></param>
    void GoToDoTask(uint receiveTaskId)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RewardPanel);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.DailyPanel);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
        new Client.stDoingTask() { taskid = receiveTaskId });
    }

}
