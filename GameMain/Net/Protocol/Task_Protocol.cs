using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Engine.Utility;
using GameCmd;
using UnityEngine;
partial class Protocol
{
    /// <summary>
    /// 发送任务
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stAddRoleTaskScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().AddTask(cmd.task.id, cmd.task.state, cmd.task.operate);
    }

    /// <summary>
    /// 批量发送任务 登陆后发送
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stAddRoleTaskListScriptUserCmd_S cmd)
    {
        Log.Trace("TASK:批量发送任务");
        DataManager.Manager<TaskDataManager>().InitTasks(cmd.dataList);
    }


    /// <summary>
    /// 删除任务
    /// </summary>
    [Execute]
    public void Execute(GameCmd.stDelRoleTaskScriptUserCmd_CS cmd)
    {
        Engine.Utility.Log.LogGroup("LCY", "删除的  taskId = {0}", cmd.dwTaskID);
        DataManager.Manager<TaskDataManager>().DeleteTaskByTaskID(cmd.dwTaskID);
    }

    /// <summary>
    /// 通知客户端批量任务发送完毕
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stNotifyFinishSendTaskScriptUserCmd_S cmd)
    {
        //设置所有需要更新NPC任务状态
        Log.Info("通知客户端批量任务发送完毕");

    }



    [Execute]//stRequestTaskIntroScriptUserCmd_C
    public void Excute(stReturnTaskIntroScriptUserCmd_S cmd)
    {
        Debug.LogError("回复任务简介：" + cmd.taskid + cmd.data);

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionPanel, UIMsgID.eRefreshTaskDesc, cmd);
        }
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionMessagePanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionMessagePanel, UIMsgID.eRefreshTaskDesc, cmd);
        }

    }

    /// <summary>
    /// 任务奖励返回
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stTaskRewardScriptUserCmd_S cmd)
    {
        QuestTranceManager.Instance.SetTaskTrackReward(cmd.task_id, cmd.item_base_id
            , cmd.item_num, cmd.gold, cmd.money, cmd.exp
            , cmd.clanmoney, cmd.clancon, cmd.clanrep);
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionPanel, UIMsgID.eRefreshTaskDesc, cmd);
        }
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionMessagePanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionMessagePanel, UIMsgID.eRefreshTaskDesc, cmd);
        }
    }
    /// <summary>
    /// 刷新任务状态
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stRefreshStateRoleTaskScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().UpdateTaskState(cmd.dwTaskID, cmd.dwState);
    }

    /// <summary>
    /// 刷新任务operate
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stRefreshOperateRoleTaskScriptUserCmd_S cmd)
    {
        Log.Info("刷新任务operate {0}  {1}", cmd.dwTaskID, cmd.dwOperate);
        DataManager.Manager<TaskDataManager>().UpdateTaskOperate(cmd.dwTaskID, cmd.dwOperate);
    }

    /// <summary>
    /// 任务追踪回复
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stTaskTraceScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().UpdateTaskTrace(cmd.task_id, cmd.txt_id, cmd.@params);
    }

    [Execute]
    public void Excute(stTalkDataScriptUserCmd_S cmd)
    {
        LangTalkData langtalkdata = LangTalkData.GetDataByCmd(cmd);
        if (langtalkdata == null)
        {
            Engine.Utility.Log.Error("对话id{0} 有问题", cmd.txt_id);
        }
        DataManager.Manager<TaskDataManager>().OnTalkData(langtalkdata, cmd.step);
    }

    [Execute]
    public void Execute(stRequestSubmitListScriptUserCmd_CS cmd)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SubmitPanel, data: cmd);
    }
    #region 悬赏任务

    [Execute]//发布令牌任务
    public void Excute(stPublicTokenTaskScriptUserCmd_CS cmd)
    {
        TaskDataManager taskMgr = DataManager.Manager<TaskDataManager>();

        RewardMisssionInfo missioninfo = new RewardMisssionInfo();
        table.PublicTokenDataBase reward = GameTableManager.Instance.GetTableItem<table.PublicTokenDataBase>(cmd.tokentaskid, 1);
        if (reward != null)
        {
            missioninfo.strIcon = reward.smallicon;
            missioninfo.strName = reward.title;
            missioninfo.nType = 1;
            missioninfo.ntaskid = reward.taskid;
            missioninfo.id = reward.id;
            table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(reward.taskid);
            if (quest != null)
            {
                missioninfo.nExp = quest.dwRewardExp;
                missioninfo.nleftTime = quest.dwLimitTime * 60 * 60;
            }
            missioninfo.nstate = (int)TokenTaskState.TOKEN_STATE_PUBLISH;
            taskMgr.RewardMisssionData.ReleaseRewardList.Add(missioninfo);
        }
    }

    [Execute]//接受令牌任务
    public void Excute(stAcceptTokenTaskScriptUserCmd_CS cmd)
    {
        DataManager.Manager<TaskDataManager>().OnReceiveTokenTask(cmd.tokentaskid);
    }

    [Execute]//获取令牌任务数量
    public void Excute(stGetTokenTaskNumScriptUserCmd_CS cmd)
    {
        DataManager.Manager<TaskDataManager>().OnGetTokenTaskNum(cmd.tasknum);
    }

    [Execute]
    public void Excute(stReturnSelfTokenTAskScriptUserCmd_S cmd)
    {
        RewardMisssionMgr taskMgr = DataManager.Manager<TaskDataManager>().RewardMisssionData;
        if (taskMgr != null)
        {
            taskMgr.InitTask(cmd.data, cmd.acceptTaskRemain, cmd.publicTaskRemain);
        }
    }
    #endregion

    #region 刷星 stRefreshStarScriptUserCmd_C 客户端请求刷星
    [Execute]////服务器返回星数据 通知客户端打开刷星界面
    public void Execute(GameCmd.stRefreshStarScriptUserCmd_S cmd)
    {

        TaskDataManager mgr = DataManager.Manager<TaskDataManager>();
        mgr.AddStarTask(new StarTaskData()
        {
            id = cmd.star.id,
            star = cmd.star.star,
            all_refresh = cmd.star.all_refresh,
            gold_refresh = cmd.star.gold_refresh,
        });
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionMessagePanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionMessagePanel, UIMsgID.eRefreshStarTask, cmd.star.id);
        }
    }

    [Execute]////登录时服务器下发玩家所有刷星数据
    public void Execute(GameCmd.stUserStarDataScriptUserCmd_S cmd)
    {
        TaskDataManager mgr = DataManager.Manager<TaskDataManager>();
        for (int i = 0; i < cmd.all_star.Count; i++)
        {
            mgr.AddStarTask(new StarTaskData()
            {
                id = cmd.all_star[i].id,
                star = cmd.all_star[i].star,
                all_refresh = cmd.all_star[i].all_refresh,
                gold_refresh = cmd.all_star[i].gold_refresh,
            });
        }
    }

    [Execute]///服务器删除星数据 
    public void Execute(GameCmd.stRemoveStarScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().RemoveStarTask(cmd.id);
    }

    [Execute]///服务器清空星数据  
    public void Execute(GameCmd.stClearAllStarScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().RemoveAllStarTask();
    }
    #endregion


    /// <summary>
    /// 功能开放
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stSystemForecastListPropertyUserCmd_S cmd)
    {
        DataManager.Manager<FunctionPushManager>().InitOpenList(cmd.data);
    }
    [Execute]
    public void Execute(stRequestSystemForecastPropertyUserCmd_CS cmd)
    {
        if (cmd.state != 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Trailer_Commond_1);
            DataManager.Manager<FunctionPushManager>().AddOpenSys(cmd.id);
        }
        else 
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHFUNCTIONPUSHOPEN, null);
        }
    }


    [Execute]
    public void Execute(stInviteGoMapRequestUserCmd_CS cmd)
    {
        DataManager.Manager<FunctionPushManager>().AddTransmitMsg(cmd);
    }

    [Execute]
    public void Excute(stOpenWindowScriptUserCmd_S cmd)
    {
        //跳转
        ItemManager.DoJump(cmd.win_id);
    }

    /// <summary>
    /// 环任务  0表示没开始 -1表示第一轮已经结束 第二轮还没开始 其他表示正在做某一环	
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stRefreshRingTaskScriptUserCmd_S cmd)
    {
        DataManager.Manager<TaskDataManager>().OnRefreshRingTask(cmd.ring);
    }

    //-----------------------------------------------------------------------------------------------
    //SENDER
    public void RequestFinishTask(uint nTaskID)
    {
        NetService.Instance.Send(new GameCmd.stClientFinishTaskScriptUserCmd_C() { task_id = nTaskID });

    }

    public void RequestDelTask(uint nTaskID)
    {
        NetService.Instance.Send(new GameCmd.stDelRoleTaskScriptUserCmd_CS() { dwTaskID = nTaskID });

    }

    public void RequestDialogSelect(uint nChose, string strStep)
    {
        NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
        {
            step = strStep,
            dwChose = nChose,
        });
    }

    public void RequestTaskTip(uint nTaskID)
    {
        NetService.Instance.Send(new GameCmd.stRequestTaskTipScriptUserCmd_C() { taskid = nTaskID });
    }

    public void RequestTaskReward(uint nTaskID)
    {
        NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = nTaskID });
    }

    public void RequestAcceptTask(uint nTaskID)
    {
        NetService.Instance.Send(new GameCmd.stClientAcceptTaskScriptUserCmd_C() { task_id = nTaskID });

    }
    /// <summary>
    /// 任务使用道具
    /// </summary>
    public void RequestUseItem(uint nItemId)
    {
        GameCmd.stUsePropertyScriptUserCmd_C cmd = new GameCmd.stUsePropertyScriptUserCmd_C();
        cmd.userid = Client.ClientGlobal.Instance().MainPlayer.GetID();
        cmd.itemid = nItemId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 请求系统预告奖励
    /// </summary>
    /// <param name="nID"></param>
    public void RequestSystemForecastReward(uint nID)
    {
        NetService.Instance.Send(new stRequestSystemForecastPropertyUserCmd_CS() { id = nID });
    }

}

