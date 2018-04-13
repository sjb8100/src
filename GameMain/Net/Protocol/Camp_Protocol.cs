using System;
using System.Collections.Generic;
using GameCmd;
using Common;
partial class Protocol
{
    #region Execute
    
    /// <summary>
    /// 请求阵营战信息
    /// </summary>
    /// <param name="index"></param>
    public void SignCampInfoReq(uint index)
    {
        stSignInfoCampUserCmd_C cmd = new stSignInfoCampUserCmd_C()
        {
            index = index,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器返回阵营战信息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSignCampInfoRes(stSignInfoCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().OnGetSignCampInfo(cmd);
        
    }

    /// <summary>
    /// 报名阵营战
    /// </summary>
    /// <param name="index"></param>
    public void SignCampReq(uint index)
    {
        stSignCampUserCmd_C cmd = new stSignCampUserCmd_C()
        {
            index = index,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 报名成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSignCampRes(stSignCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().OnDosignCamp(cmd.index, cmd.num);
    }

    /// <summary>
    /// 取消报名请求
    /// </summary>
    /// <param name="index"></param>
    public void CancelCampSignReq(uint index)
    {
        stCancelCampUserCmd_C cmd = new stCancelCampUserCmd_C()
        {
            index = index,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 取消报名
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnCancelCampSignRes(stCancelCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().OnDoCancelSignCamp(cmd.index);
    }

    [Execute]//还有15秒就要开始了
    public void OnSignCamp(stReadyCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().FightingIndex = cmd.index;
    /*    TipsManager.Instance.ShowTips("还有15秒就要开始了");*/
        int time = GameTableManager.Instance.GetGlobalConfig<int>("CF_EnterSceneLeftTime");
        TipsManager.Instance.ShowTipWindow((uint)time, 0, Client.TipWindowType.CancelOk, string.Format("还有{0}秒就要开始了阵营战斗",time),
            delegate() { NetService.Instance.Send(new stReadyCampUserCmd_CS() { index = cmd.index, join = 1 }); },
            delegate() { NetService.Instance.Send(new stReadyCampUserCmd_CS() { index = cmd.index, join = 0 }); }, okstr: "确定", cancleStr: "取消");
    }

    /// <summary>
    /// 服务器返回是否参加（join 1表示确定参加，0表示不参加）
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnReadyCamp(stReadyCampUserCmd_CS cmd)
    {
        if (cmd.join == 1)
        {
            DataManager.Manager<CampCombatManager>().FightingIndex = cmd.index;
        }
    }

    /// <summary>
    /// 报名了哪些场次   camps = [1,3,4]报名了第1，3，4场
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSignCamp(stMyInfoCampUserCmd_S cmd)
    {
        CampCombatManager mgr = DataManager.Manager<CampCombatManager>();
        mgr.UpdateCampFightLeftTimes((null != cmd.indexs) ? cmd.indexs.Count : 0);
        mgr.UpdateIfSignUp(cmd.indexs);
    }

    /// <summary>
    /// 服务器下发副本关闭
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnCloseCamp(stCloseCampUserCmd_S cmd)
    {
        if (cmd.type == 1)
        {
            //人数不足
        }else if (cmd.type == 2)
        {
            //时间到了
        }
        DataManager.Manager<CampCombatManager>().OnQuitCamp();
    }

    /// <summary>
    /// 服务器下发阵营战某一方信息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnGetCampInfo(stCampInfoCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().UpdateCampUserBattleData(cmd);
    }

    /// <summary>
    /// 战斗结结束服务器下发结果
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnEndCamp(stEndCampUserCmd_S cmd)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CampFightingPanel,false);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CampWarBattleListPanel,false);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampWarResultPanel,data:cmd);
    }

    /// <summary>
    /// 服务器下发某个成员信息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnMemberInfoCamp(stMemberInfoCampUserCmd_S cmd)
    {
        
    }

    /// <summary>
    /// 发送所有场次的状态
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAllCFInfoCamp(stCFInfoCampUserCmd_S cmd)
    {
        CampCombatManager mgr = DataManager.Manager<CampCombatManager>();

//         for (int i = 0; i < cmd.state.Count; i++)
//         {
//             mgr.UpdateSignUpStatus(i + 1,(int)cmd.state[i]);
//         }
        
    }

    /// <summary>
    /// 进入阵营战
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnOnEnterCamp(stOnEnterCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().OnEnterCamp(cmd);
    }

    /// <summary>
    /// 服务器下发战斗数据
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnCampKillInfo(stCampKillInfoCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().RefreshFighingInfo(cmd);
    }

    /// <summary>
    /// 设置阵营
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSetCampUser(GameCmd.stSetCampUserCmd_S cmd)
    {

        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }
        //0是玩家,1是npc
         Client.IEntity entity  = null;
        if (cmd.entry_type == 0)
        {
           entity = es.FindPlayer(cmd.entry_id);
        }
        else if(cmd.entry_type == 1)
        {
            entity = es.FindNPC(cmd.entry_id);
        }

        if (entity != null)
        {
            Client.stPropUpdate prop = new Client.stPropUpdate();
            prop.uid = entity.GetUID();
            prop.nPropIndex = (int)Client.CreatureProp.Camp;
            prop.oldValue = entity.GetProp((int)Client.CreatureProp.Camp);
            prop.newValue = (int)cmd.camp;
            entity.SetProp((int)Client.CreatureProp.Camp, (int)cmd.camp);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
        }

        

        if (Client.ClientGlobal.Instance().IsMainPlayer(cmd.entry_id)) 
        {
            if (cmd.camp == eCamp.CF_Green || cmd.camp == eCamp.CF_Red)
            {
                DataManager.Manager<CampCombatManager>().OnSetCamp();
            }
            //进去就请求刷行数据
            DataManager.Instance.Sender.RequestCampInfoCamp(0, 0, DataManager.Manager<CampCombatManager>().FightingIndex);
        } 


    }

    /// <summary>
    /// 成员战斗数据刷新消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void RefreshMemberInfo(stRefreshMemberInfoCampUserCmd_S cmd) 
    {
        DataManager.Manager<CampCombatManager>().RefreshMemberInfo(cmd);
    }

    /// <summary>
    /// 本日今日阵营战次数
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnCampTimes(stEnterCampUserCmd_S cmd)
    {
        DataManager.Manager<CampCombatManager>().OnCampTimes(cmd);
    }

    #endregion

    #region send


    ///// <summary>
    ///// 告诉服务器是否确定要参加,//1表示确定参加，0表示不参加，返还一次参加次数
    ///// </summary>
    ///// <param name="isJion"></param>
    ///// <param name="nIndex"></param>
    //public void RequestReadyCamp(bool isJion,uint nIndex)
    //{

    //    //NetService.Instance.Send(new stReadyCampUserCmd_C() { index = nIndex, join = isJion ? (uint)1 : (uint)0 });
    //}

    ///// <summary>
    ///// 请求参赛信息
    ///// </summary>
    //public void RequestMyInfoCamp()
    //{
    //    NetService.Instance.Send(new stMyInfoCampUserCmd_C());
    //}

    /// <summary>
    /// //0:要求返回信息里包含所有成员信息,1:要求返回信息里不包含成员信息
    /// </summary>
    /// <param name="nType">0:要求返回信息里包含所有成员信息,1:要求返回信息里不包含成员信息</param>
    /// <param name="nCamp">0:双方信息，1:请求我方信息，2请求敌方信息</param>
    /// <param name="nIndex"></param>
    public void RequestCampInfoCamp(uint nType,uint nCamp,uint nIndex)
    {
        NetService.Instance.Send(new stCampInfoCampUserCmd_C() { type = nType, camp = nCamp, index = nIndex });
    }

    /// <summary>
    /// 请求某个成员的信息
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="nIndex"></param>
    public void RequestMemberInfoCamp(uint uid,uint nIndex)
    {
        NetService.Instance.Send(new stMemberInfoCampUserCmd_C() { userid = uid, index = nIndex });
    }

    /// <summary>
    /// 请求报名状态信息
    /// </summary>
    public void RequestCFInfoCamp()
    {
        NetService.Instance.Send(new stCFInfoCampUserCmd_C());
    }
    #endregion

}
