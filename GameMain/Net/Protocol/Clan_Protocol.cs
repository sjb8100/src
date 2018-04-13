/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Net.Protocol
 * 文件名：  Clan_Protocol
 * 版本号：  V1.0.0.0
 * 唯一标识：4bdcbff4-39be-4a3d-8221-4f75eee11694
 * 当前的用户域：USER-20160526UC
 * 电子邮箱：wenjunhua.zqgame
 * 创建时间：10/8/2016 11:50:58 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;

partial class Protocol
{
    /// <summary>
    /// 创建氏族
    /// </summary>
    /// <param name="clanName">氏族名称</param>
    /// <param name="msg">公告</param>
    public void CreateClanReq(string clanName, string msg)
    {
        stCreateClanUserCmd_C cmd = new stCreateClanUserCmd_C()
        {
            ClanName = clanName,
            BroadCastMsg = msg,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 创建临时氏族响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCreateClanRes(stCreateClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnCreateClan(msg.Ret, msg.info);
    }

    /// <summary>
    /// 加入氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void JoinClanReq(uint clanId)
    {
        stJoinClanUserCmd_C cmd = new stJoinClanUserCmd_C()
        {
            Clanid = clanId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 加入氏族响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnJoinClanRes(stJoinClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnJoinClan(msg.Ret, msg.info);
    }

    /// <summary>
    /// 申请加入氏族请求
    /// </summary>
    /// <param name="clanId"></param>
    public void RequestJoinClanReq(uint clanId)
    {
        stRequestJoinClanUserCmd_C cmd = new stRequestJoinClanUserCmd_C()
        {
            Clanid = clanId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 申请加入氏族请求响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRequestJoinClanRes(stRequestJoinClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnApplyJoinClan(msg.ret);
    }

    /// <summary>
    /// 服务器下发玩家申请氏族列表
    /// </summary>
    /// <param name="?"></param>
    [Execute]
    public void UserApplyClanListRes(stMyRequestListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnUserApplyClanList(msg.ClanID);
    }

    /// <summary>
    /// 零时氏族转正
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnFormalClan(stFormalClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnFormalClan(msg.Ret, msg.info);
    }

    /// <summary>
    /// 退出氏族
    /// </summary>
    public void QuitClanReq()
    {
        stLeaveClanUserCmd_C cmd = new stLeaveClanUserCmd_C()
        {
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 离开氏族响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnQuitClanRes(stLeaveClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnQuitClan(msg.Ret, msg.type, msg.Userid);
    }

    /// <summary>
    /// 踢出氏族
    /// </summary>
    /// <param name="playerId">踢出玩家id</param>
    public void KickClanReq(uint playerId)
    {
        stKickClanUserCmd_C cmd = new stKickClanUserCmd_C()
        {
            DesUserid = playerId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg">内容</param>
    /// <param name="type">类型</param>
    public void BroadCastClanNoticeReq(string msg, enumBroadCastType type)
    {
        stBroadCastClanUserCmd_C cmd = new stBroadCastClanUserCmd_C()
        {
            szMessage = msg,
            Type = type,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 广播公告响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnBroadCastClanNoticeRes(stBroadCastClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnBroadCastClanNotice(msg.Userid, msg.Type, msg.szMessage);
    }

    /// <summary>
    /// 邀请加入氏族
    /// </summary>
    /// <param name="invitedUserId"></param>
    public void InviteJoinClanReq(uint invitedUserId)
    {
        stInviteJoinClanUserCmd_C cmd = new stInviteJoinClanUserCmd_C()
        {
            UserInvitedID = invitedUserId,
        };
        SendCmd(cmd);
        TipsManager.Instance.ShowTips("邀请已发送");
    }

    /// <summary>
    /// 玩家响应邀请
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="clanId"></param>
    /// <param name="accept"></param>
    public void AnswerInvitJoinClanReq(uint playerId, uint clanId, bool accept)
    {
        stAnswerInviteClanUserCmd_C cmd = new stAnswerInviteClanUserCmd_C()
        {
            AnswerClanid = clanId,
            AnswerInviteID = playerId,
            Ret = (uint)((accept) ? 1 : 2),
        };
        SendCmd(cmd);
    }


    /// <summary>
    /// 服务器下发邀请加入氏族
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnInviteJoinClanRes(stInviteJoinClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().ServerInviteJoinClan(
            msg.UserInviteName, msg.UserInviteID, msg.InviteClanName, msg.InviteClanid);
    }


    #region Donate

    /// <summary>
    /// 捐献
    /// </summary>
    /// <param name="id"></param>
    public void ClanDonateReq(uint id)
    {
        stContributionClanUserCmd_C cmd = new stContributionClanUserCmd_C()
        {
            contri_id = id,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 捐献响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnClanDonateRes(stContributionClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnClanDonate((enumConRetType)msg.ret, msg.contri_id, msg.times);
    }

    /// <summary>
    /// 服务端同步捐献剩余次数
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void onClanDonateInfoChanged(stClanMemberInfoClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().onClanDonateInfoChanged(msg.vars);
    }
    #endregion

    /// <summary>
    /// 氏族解散响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void DissolveClanRes(stDissolveClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnDissolveClan(msg.Clanid, (msg.IsFormalClan == 1) ? true : false);
    }

    /// <summary>
    /// 请求快速加入
    /// </summary>
    public void QuickJoinClan()
    {
        stQuickJoinClanUserCmd_C cmd = new stQuickJoinClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 快速加速响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnQuickJoinClanRes(stQuickJoinClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnQuickJoinClan(msg.ret, msg.clannum);
    }

    /// <summary>
    /// 搜索氏族
    /// </summary>
    /// <param name="filterString">过滤字符串</param>
    public void SearchClan(eGetClanType type, string filterString, uint page)
    {
        stSearchClanUserCmd_C cmd = new stSearchClanUserCmd_C()
        {
            szKey = filterString,
            type = type,
            page = page,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 请求氏族信息
    /// </summary>
    /// <param name="clanId">氏族id</param>
    /// <param name="type">0：不带成员信息，1：带成员信息</param>
    public void GetClanInfoReq(uint clanId, uint type)
    {
        stGetClanInfoClanUserCmd_C cmd = new stGetClanInfoClanUserCmd_C()
        {
            Clanid = clanId,
            type = type,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 请求氏族信息响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetClanInfoRes(stGetClanInfoClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetClanInfo(msg.info, ((msg.type == 1) ? true : false));
    }

    /// <summary>
    /// 请求获取氏族列表
    /// </summary>
    /// <param name="clanId">氏族id</param>
    public void GetClanMemberListReq(uint clanId)
    {
        stGetMemberListClanUserCmd_C cmd = new stGetMemberListClanUserCmd_C()
        {
            Clanid = clanId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 请求获取氏族列表响应
    /// </summary>
    /// <param name="msg"></param>
    public void OnGetClanMemberListRes(stGetMemberListClanUserCmd_S msg)
    {

    }

    /// <summary>
    /// 氏族信息改变
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnClanInfoChangedRes(stBroadCastMemberInfoClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnClanInfoChanged(msg.type, msg.user);
    }

    /// <summary>
    /// 服务器下发氏族资金变更
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnClanMoneyGetRes(stSetMoneyClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnClanMoneyGet(msg.money);
    }

    /// <summary>
    /// 获取服务端氏族列表
    /// </summary>
    /// <param name="type">type = 0:查询临时氏族, type = 1:查询正式氏族, type = 2:两者一起查 </param>
    /// <param name="page">optional uint32 page = 2;	//页数，暂定10个为一页</param>
    public void GetClanInfoList(eGetClanType type, uint page)
    {
        stGetClanListClanUserCmd_C cmd = new stGetClanListClanUserCmd_C()
        {
            type = type,
            page = 0,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 获取服务端氏族列表响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetClanInfoListRes(stGetClanListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetClanInfoList(msg.type, msg.stClanInfoList);
    }

    #region Honor

    /// <summary>
    /// 获取氏族动态请求
    /// </summary>
    public void GetHonorInfoReq()
    {
        stGetHonorClanUserCmd_C cmd = new stGetHonorClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 获取氏族动态响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetHonorInfoRes(stGetHonorClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetHonorInfos(msg.type, msg.Honor);
    }
    #endregion

    #region Upgrade
    /// <summary>
    /// 升级请求
    /// </summary>
    public void ClanUpgradeReq()
    {
        stLevelUpClanUserCmd_C cmd = new stLevelUpClanUserCmd_C();
        SendCmd(cmd);
    }
    /// <summary>
    /// 升级响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnClanUpgradeRes(stLevelUpClanUserCmd_S msg)
    {
        bool success = (msg.ret == 1) ? true : false;
        DataManager.Manager<ClanManger>().OnClanUpgrade(success);
    }
    #endregion

    #region Member
    public void GetClanApplyListReq()
    {
        stRequestListClanUserCmd_C cmd = new stRequestListClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 获取氏族申请列表响应
    /// </summary>
    /// <param name="?"></param>
    [Execute]
    public void OnGetClanApplyListRes(stRequestListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetClanApplyList(msg.data, msg.op);
    }


    /// <summary>
    /// 清空请求加入氏族列表信息
    /// </summary>
    public void ClearClanApplyListReq()
    {
        stClearRequestClanUserCmd_C cmd = new stClearRequestClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器响应清空请求加入氏族列表信息
    /// </summary>
    [Execute]
    public void OnClearClanApplyList(stClearRequestClanUserCmd_S msg)
    {
        ////ret： 1成功, 0失败
        DataManager.Manager<ClanManger>().OnClearClanApplyList((msg.ret == 1) ? true : false);
    }

    /// <summary>
    /// 玩家请求处理
    /// </summary>
    /// <param name="userId">玩家id</param>
    /// <param name="ret">1：同意 2：拒绝</param>
    public void DealClanApplyReq(uint userId, bool ret)
    {
        stAgreeOrRefuseClanUserCmd_C cmd = new stAgreeOrRefuseClanUserCmd_C()
        {
            Desid = userId,
            Ret = (uint)(ret ? 1 : 2),
        };
        SendCmd(cmd);
    }
    /// <summary>
    /// 氏族申请处理响应
    /// </summary>
    [Execute]
    public void OnDealClanApplyRes(stAgreeOrRefuseClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnDealClanApply((msg.ret == 1) ? true : false, msg.desname);
    }

    /// <summary>
    /// 踢出成员
    /// </summary>
    /// <param name="desUserId"></param>
    public void ExpelClanMemberReq(uint desUserId)
    {
        stKickClanUserCmd_C cmd = new stKickClanUserCmd_C()
        {
            DesUserid = desUserId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 踢出成员响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnExpelClanMemberRes(stKickClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnExpelFromClan(msg.Userid, msg.DesUserid, ((msg.Ret == 1) ? true : false));
    }

    /// <summary>
    /// 设置职位
    /// </summary>
    /// <param name="desUserId"></param>
    /// <param name="duty"></param>
    public void SetClanDutyReq(uint desUserId, uint duty)
    {
        stSetDutyClanUserCmd_SC cmd = new stSetDutyClanUserCmd_SC()
        {
            DesUserID = desUserId,
            Duty = duty,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 设置氏族响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnSetClanDutyRes(stSetDutyClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnChangeDuty(msg.ret);
    }

    /// <summary>
    /// 转让氏族请求
    /// </summary>
    /// <param name="desUserId">目标氏族长</param>
    public void TransferClanReq(uint desUserId)
    {
        stTransferClanUserCmd_C cmd = new stTransferClanUserCmd_C()
        {
            DesUserID = desUserId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 转让氏族响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnTransferClanRes(stTransferClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnTransferClan((msg.ret == 1) ? true : false);
    }
    #endregion

    #region Skill
    /// <summary>
    /// 研发技能请求
    /// </summary>
    /// <param name="skillId"></param>
    public void DevClanSkillReq(uint skillId)
    {
        stDevelopSkillClanUserCmd_C cmd = new stDevelopSkillClanUserCmd_C()
        {
            skillid = skillId,
        };
        SendCmd(cmd);
    }


    /// <summary>
    /// 服务器下发研发响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnDevClanSkillRes(stDevelopSkillClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnDevClanSkill(msg.skillid);
    }
    #endregion

    #region Task
    /// <summary>
    /// 请求氏族任务列表
    /// </summary>
    public void RequestClanTaskInfosReq()
    {
        stRequestClanTaskScriptUserCmd_C cmd = new stRequestClanTaskScriptUserCmd_C();
        SendCmd(cmd);
    }

    public void ReqClanTaskStep()
    {
        stRequestClanTaskStepScriptUserCmd_C cmd = new stRequestClanTaskStepScriptUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 请求服务器下发氏族任务
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRequestClanTaskInfoRes(stRequestClanTaskScriptUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnRequestClanTaskInfos(msg.clantask);
    }

    /// <summary>
    /// 服务器下发刷新氏族任务
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRefreshClanTask(stRefreshClanTaskScriptUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnRefreshClanTaskInfo(msg.clantask);
    }

    /// <summary>
    /// 接受氏族任务
    /// </summary>
    /// <param name="taskId"></param>
    public void AcceptClanTaskReq(uint taskId)
    {
        stAcceptClanTaskScriptUserCmd_C cmd = new stAcceptClanTaskScriptUserCmd_C()
        {
            taskid = taskId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 完成氏族任务
    /// </summary>
    /// <param name="taskId"></param>
    public void FinishClanTaskReq(uint taskId)
    {
        stFinishClanTaskScriptUserCmd_C cmd = new stFinishClanTaskScriptUserCmd_C()
        {
            taskid = taskId,
        };
        SendCmd(cmd);
    }

    [Execute]
    public void OnClanTaskStep(stClanTaskStepScriptUserCmd_S cmd)
    {
        DataManager.Manager<ClanManger>().OnClanTaskStep(cmd);
    }
    #endregion

    #region ClanDeclareWar
    /// <summary>
    /// 请求氏族敌对势力
    /// </summary>
    public void SendGetClanRivalryInfosReq()
    {
        stWarEnemyListClanUserCmd_C cmd = new stWarEnemyListClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发敌对势力信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetClanRivalryInfosRes(stWarEnemyListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetClanRivalryInfos(msg.clan_list);
    }

    /// <summary>
    /// 服务器下发单个敌对势力变更
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnClanRivalryInfoChangedRes(stWarClanInfoClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnClanRivalryInfoChanged((int)msg.type, msg.clan_info);
    }

    /// <summary>
    /// 对氏族宣战
    /// </summary>
    /// <param name="clanId"></param>
    public void StartDeclareWarReq(uint clanId)
    {
        stWarStartClanUserCmd_C cmd = new stWarStartClanUserCmd_C()
        {
            clanid = clanId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 氏族宣战响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnStartDeclareWarRes(stWarStartClanUserCmd_S msg)
    {
        //收到
        //DataManager.Manager<ClanManger>().OnStartDeclareWar(1);
    }

    /// <summary>
    /// 获取宣战历史信息
    /// </summary>
    public void GetDeclareWarHistoryInfosReq()
    {
        stWarHistoryListClanUserCmd_C cmd = new stWarHistoryListClanUserCmd_C();
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发氏族宣战信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetDeclareWarHistoryInfosRes(stWarHistoryListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetDeclareWarHistoryInfos(msg.clan_list);
    }

    /// <summary>
    /// 请求查询氏族信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="page"></param>
    public void GetDeclareWarSerchInfosReq(string key, int page)
    {
        stWarSearchListClanUserCmd_C cmd = new stWarSearchListClanUserCmd_C()
        {
            szKey = key,
            page = (uint)page,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发氏族查询信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGetDeclareWarSerchInfosRes(stWarSearchListClanUserCmd_S msg)
    {
        DataManager.Manager<ClanManger>().OnGetDeclareWarSearchInfos(msg.clan_list, (int)msg.page);
    }
    #endregion


    #region ClanCityWar

    /// <summary>
    /// 报名成功
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarSignSuccess(stCityWarSignClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarSignSuccess(msg);
    }

    /// <summary>
    /// 报名信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarSignInfo(stCityWarSignInfoClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarSignInfo(msg);
    }

    /// <summary>
    /// 城战开始是 通知和回复
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarBeginNotice(stCityWarBeginClanUserCmd_CS msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarBeginNotice(msg);
    }

    /// <summary>
    /// 城战基本信息 进入副本时下发一次
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarBaseInfo(stCityWarBaseInfoClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarBaseInfo(msg);
    }

    /// <summary>
    /// 自己的击杀信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarSelfInfo(stCityWarSelfInfoClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarSelfInfo(msg);
    }



    /// <summary>
    /// 服务器下发城战面板信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarInfo(stCityWarInfoClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarInfo(msg);
    }

    /// <summary>
    /// 召集
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OCityWarCallClan(stCityWarCallClanClanUserCmd_CS msg)
    {
        DataManager.Manager<CityWarManager>().OCityWarCallClan(msg);
    }

    /// <summary>
    /// 召集
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarCallTeam(stCityWarCallTeamClanUserCmd_CS msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarCallTeam(msg);
    }

    /// <summary>
    /// 图腾改变
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarTowerChange(stCityWarTowerChangeClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarTowerChange(msg);
    }

    /// <summary>
    /// //城战胜利者通知 登录时 改变时
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarWinerClanId(stCityWarWinerClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarWinerClanId(msg);
    }

    /// <summary>
    /// 通知弹出结算界面  10秒后自动退出
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnCityWarFinish(stCityWarFinishClanUserCmd_S msg)
    {
        DataManager.Manager<CityWarManager>().OnCityWarFinish(msg);
    }

    //城池信息
    [Execute]
    public void OnCityWarFrameInfo(stCityWarFrameInfoClanUserCmd_CS cmd)
    {
        DataManager.Manager<CityWarManager>().OnCityWarFrameInfo(cmd);
    }

    //上缴雕像
    [Execute]
    public void OnCityWarSubmitStatue(stSubmitStatueClanUserCmd_CS cmd)
    {
        DataManager.Manager<CityWarManager>().OnCityWarSubmitStatue(cmd);
    }

     [Execute]
    public void OnOpenSubmitPanel(stOpenSubmitWindowClanUserCmd_CS cmd)
    {
        DataManager.Manager<CityWarManager>().OnOpenSubmitPanel(cmd);
    }

    #endregion



}