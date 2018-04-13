using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
using table;
using System.Collections;
using UnityEngine;


partial class Protocol
{
    /// <summary>
    /// 武斗场功能开启
    /// </summary>
    /// <param name="cmd"></param>
    //[Execute]
    //public void OnOpenLockArenaRes(stOpenLockArenaUserCmd_S cmd)
    //{
    //    string notice = cmd.notice_content;
    //    int firstOpen = cmd.first_open;
    //    Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---1 武斗场功能开启 >>> notice = {0}", cmd.notice_content);
    //}

    //武斗场主界面信息
    [Execute]
    public void OnMainArenaRes(stReturnMainArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaManager>().OnMainArenaRes(cmd);
    }


    // 返回前三名数据
     [Execute]
     public void OnArenaTopThree(stReturnTopUserArenaUserCmd_S cmd)
     {
         DataManager.Manager<ArenaManager>().OnArenaTopThree(cmd);
     }	
				
	// 返回挑战信息	
    [Execute]
    public void OnArenaRivalThree(stReturnOppUserArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaManager>().OnArenaRivalThree(cmd);
    }

    /// <summary>
    /// 刷新挑战CD响应
    /// </summary>
    [Execute]
    public void OnRefreshChallengeCDRes(stRefreshCDArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaManager>().OnRefreshChallengeCDRes(cmd);
    }


    /// <summary> 
    /// 刷新挑战次数响应
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRefreshChallengeTimesRes(stRefreshTimesArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaManager>().OnRefreshChallengeTimesRes(cmd);
    }


    /// <summary>
    /// 战报返回
    /// </summary>
    /// <param name="cmd"></param>

    [Execute]
    public void OnBattlelogRes(stReportArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaManager>().OnBattlelogRes(cmd);
    }

    /// <summary>
    /// 接到别人的邀请
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnChallengeInviteRes(stChallengeInviteArenaUserCmd_CS cmd)
    {

        DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
        {
            msgType = PushMsg.MsgType.Arena,
            senderId = cmd.offensive_id,
            //name = "",
            sendName = cmd.offensive_name,
            //groupId = cmd.o,
            sendTime = UnityEngine.Time.realtimeSinceStartup,
            cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("ArenaMsgCD"),
        });
    }

    /// <summary>
    /// 发起人收到对方同意或拒绝消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnInviteResultRes(stInviteResultArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaManager>().OnInviteResultRes(cmd);
    }

    /// <summary>
    /// 进入竞技场地图了
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnEnterArenaMapRes(stEnterMapArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaManager>().OnEnterArenaMapRes(cmd);
    }

    /// <summary>
    /// 通知双方开始战斗
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnStartBattleCDRes(stStartBattleArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaManager>().OnStartBattleCDRes(cmd);

        //uint CDTime = cmd.battle_time; //倒计时
        //object data = CDTime;

        //Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ARENASTARTBATTLECD, data);
    }

    /// <summary>
    /// 通知双方战斗结束
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnBattleEnd(stBattleFinalArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaManager>().OnBattleEnd(cmd);
    }

    /// <summary>
    /// 初始化技能设置面板
    /// </summary>
    /// <param name="cmd"></param>

    [Execute]
    public void OnGetArenaSetSkill(stGetSkillSettingArenaUserCmd_S cmd)
    {
        DataManager.Manager<ArenaSetSkillManager>().OnInitLocation(cmd);
    }

    [Execute]
    public void OnSetSkillState(stUseSkillStatusArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaSetSkillManager>().OnSetSkillState(cmd);
    }

    [Execute]
    public void OnSetLocation(stSkillSettingArenaUserCmd_CS cmd)
    {
        DataManager.Manager<ArenaSetSkillManager>().OnSetLocation(cmd);
    }

    [Execute]
    public void OnClean(stClearUsePosArenaUserCmd_CS cmd)
    {

    }


    [Execute]
    public void OnHttoDownLoadFinish(stDownLoadStatusDataUserCmd_S cmd)
    {
        KDownloadInstance.Instance().SetTakeReward(cmd.have_reward);
    }


    

    //[Execute]
    //public void OnExitArena(stExitSceneArenaUserCmd_CS cmd)
    //{
    //    DataManager.Manager<ArenaManager>().OnExitArena(cmd);
    //}

}

