using System;
using System.Collections;
using System.Collections.Generic;
using GameCmd;
using Common;

partial class Protocol
{

    RideManager RideMgr { get { return DataManager.Manager<RideManager>(); } }
    #region receive
    //[Execute]
    //public void Excute(stErrorRideUserCmd_S cmd )
    //{
    //    TipsManager.Instance.ShowTipsById((uint)cmd.error_no);
    //}

    [Execute]//坐骑列表
    public void Excute(stUserDataRideUserCmd_S cmd)
    {
        RideMgr.InitRideUserData(cmd.data);
    }
    [Execute]//坐骑移除
    public void Excute(stRemoveRideUserCmd_CS cmd)
    {
        RideMgr.RemoveRide(cmd.id);
    }
    [Execute] //坐骑添加
    public void Excute(stAddRideUserCmd_S cmd)
    {
        RideMgr.AddRide(cmd.obj, (GameCmd.AddRideAction)cmd.action);
        table.RideDataBase tabledata = GameTableManager.Instance.GetTableItem<table.RideDataBase>(cmd.obj.base_id);
        if (tabledata != null)
        {
            string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodezuoqi, tabledata.name);
            ChatDataManager.SendToChatSystem(txt);
        }

    }
    /*
    [Execute]//坐骑封印
    public void Excute(stSealRideUserCmd_CS cmd)
    {
        TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_fengyinchenggong);
        RideData  data = RideMgr.GetRideDataById(cmd.id);
        if (data != null)
        {
            string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_fengyinzuoqi, data.name);
            ChatDataManager.SendToChatSystem(txt);
        }
        RideMgr.RemoveRide(cmd.id);
    }
    [Execute]//坐骑喂食
    public void Excute(stFeedRideUserCmd_CS cmd)
    {
       
    }
    */
    [Execute]//坐骑出战
    public void Excute(stUserFightRideUserCmd_CS cmd)
    {
        RideMgr.SetFightRide(cmd.id);
    }
    [Execute]//坐骑回收
    public void Excute(stCallBackRideUserCmd_CS cmd)
    {
        RideMgr.CallBackRide(cmd.id);
    }
    /*
    public void Excute(stRemoveSealRideUserCmd_CS cmd)
    {
        TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_chenggongjiefeng);
    }
    [Execute]//技能领悟
    public void Excute(stUserGraspRideUserCmd_CS cmd)
    {
        if (cmd.skill != 0)
        {
            RideMgr.LearnSkill(cmd.id, cmd.skill);
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SawySkillPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.SawySkillPanel);
            }
            //如果正在上马 刷新技能icon
            if (cmd.id != RideMgr.UsingRide)
            {
                return;
            }
            Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
            if (player != null)
            {
                bool bRide = DataManager.Manager<RideManager>().IsRide;
                if (bRide)
	            {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel,UIMsgID.eSkillBtnRefresh,null);		 
	            }
            }
        }
        else
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SawySkillPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SawySkillPanel,UIMsgID.eRideUpdateLearnSkill,null);
            }
            TipsManager.Instance.ShowTips(LocalTextType.Ride_Skill_jinenglingwushibai);

        }
    }
    [Execute]//恢复饱食度
    public void Excute(stRefreshRepletionRideUserCmd_S cmd)
    {
        RideMgr.RefreshRepletion(cmd.id, cmd.repletion);
    }
    [Execute]//更新属性
    public void Excute(stRefreshAttrRideUserCmd_S cmd)
    {
        RideMgr.RefreshRideAttr(cmd.data);
    }
    [Execute]//坐骑升级
    public void Excute(stLevelUpRideUserCmd_CS cmd)
    {
        RideMgr.LevelUp(cmd.id, cmd.level, cmd.exp);
        //string exp = GameTableManager.Instance.GetGlobalConfig<string>("AddExpItemRide" + cmd.item.ToString());
        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Ride_Rank_shengjichenggong, cmd.level);
    }
    */
    [Execute]
    public void Execute(stAddMaxNumRideUserCmd_CS cmd)
    {
        RideMgr.ExpandeMaxNum(cmd.max_ride);
    }
    /*
    [Execute]
    public void Execute(stExpRideUserCmd_S cmd)
    {
        RideMgr.AddExp(cmd);

    }

    [Execute]
    public void Excute(stCanUseFeedRideUserCmd_S cmd)
    {
        RideMgr.bCheckFeed = cmd.can_use;
        RideMgr.nCheckFeedEndTime = cmd.end_time;
    }

    [Execute]
    public void Execute(stUsingSkillRideUserCmd_CS cmd)
    {
        
        //TipsManager.Instance.ShowTips("坐骑技能使用成功" + cmd.skill);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLSYSTEM_USESKILL, (uint)cmd.skill);
        Client.stSkillCDChange st = new Client.stSkillCDChange();
        st.skillid = (uint)cmd.skill;
        st.cd = cmd.cd;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)Client.GameEventID.SKILLCD_BEGIN , st );
    }
    */
    [Execute]
    public void Execute(stBroadCastRideUserCmd_S cmd)
    {//广播的发的是坐骑的baseid
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        Client.IEntity entity = es.FindPlayer(cmd.user_id);
        if (entity != null)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(entity))
            {//主角不处理 通过stStatusRideUserCmd_S 唯一决定
                return;
            }
            if (cmd.type == 0)//下马
            {
                bool bRide = (bool)entity.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
                if (bRide)
                {
                    entity.SendMessage(Client.EntityMessage.EntityCommond_UnRide, null);
                }
                entity.SetProp((int)Client.PlayerProp.RideBaseId, 0);
                #region 主角废弃
                //if (Client.ClientGlobal.Instance().IsMainPlayer(entity))
                //{
                //    RideMgr.UsingRide = 0;
                //    if (RideMgr.UnRideCallback != null)
                //    {
                //        RideMgr.UnRideCallback(RideMgr.UnRideCallbackParam);
                //        RideMgr.UnRideCallback = null;
                //        RideMgr.UnRideCallbackParam = null;
                //    }
                //}
                #endregion
            }
            else if (cmd.type == 1)//上马
            {
                bool bRide = (bool)entity.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
                if (bRide)
                {
                    return;
                }

                entity.SetProp((int)Client.PlayerProp.RideBaseId, (int)cmd.ride_id);
                entity.SendMessage(Client.EntityMessage.EntityCommond_Ride, (int)cmd.ride_id);

                #region 主角废弃
                //if (Client.ClientGlobal.Instance().IsMainPlayer(entity))
                //{
                //    RideMgr.UsingRide = RideMgr.Auto_Ride;

                //    Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                //    if (cs != null)
                //    {
                //        Client.ICombatRobot robot = cs.GetCombatRobot();
                //        if (robot != null && robot.Status != Client.CombatRobotStatus.STOP)
                //        {
                //            robot.Stop();
                //        }
                //    }
                //}
                #endregion

            }
        }

    }

    //[Execute]
    //public void Execute(stExpPassRideUserCmd_CS cmd)
    //{
    //    if (cmd.pass_result)
    //    {
    //        TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_chuanchengchenggong);
    //        RideMgr.TransExp();
    //    }
    //}

    [Execute]
    public void OnRideStatus(stStatusRideUserCmd_S cmd)
    {
        RideMgr.OnRideStatus(cmd);
    }
    #endregion

    #region send

    //public void RideAddExpRideUser(uint rid,uint itemid)
    //{
    //    NetService.Instance.Send(new stAddExpRideUserCmd_C() { 
    //        id = rid,
    //        item = itemid,
    //    });
    //}
    /// <summary>
    /// 坐骑升级
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="auto_buy">//是否消耗元宝补充</param>
    //public void RideLevelUpRideUser(uint rid,uint uitemid,bool bauto_buy,uint num)
    //{
    //    NetService.Instance.Send(new stLevelUpRideUserCmd_CS()
    //    {
    //        id = rid,
    //        item = uitemid,
    //        auto_buy = bauto_buy,
    //        num = num,
    //    });
    //}

    /// <summary>
    /// 技能领悟
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="skillid"></param>
    /// <param name="bauto_buy"></param>
    //public void RideGraspRideUser(uint rid,uint skillid, bool bauto_buy)
    //{
    //    NetService.Instance.Send(new stUserGraspRideUserCmd_CS()
    //    {
    //        id = rid,
    //        skill = (int)skillid,
    //        auto_buy = bauto_buy,
    //    });
    //}

    /// <summary>
    /// 坐骑回收
    /// </summary>
    /// <param name="rid"></param>
    public void RideCallBackRideUser(uint rid)
    {

        if (rid == RideMgr.UsingRide)
        {
            RideDownRide(null);
        }
        NetService.Instance.Send(new stCallBackRideUserCmd_CS()
        {
            id = rid,
        });
    }

    /// <summary>
    /// 坐骑出战
    /// </summary>
    /// <param name="rid"></param>
    public void RideFightRideUser(uint rid)
    {
        NetService.Instance.Send(new stUserFightRideUserCmd_CS()
        {
            id = rid,
        });
    }

    /// <summary>
    /// </summa坐骑喂食ry>
    /// <param name="rid"></param>
    /// <param name="itemid"></param>
    //public void RideFeedRideUser(uint rid,uint itemid,uint num)
    //{
    //    NetService.Instance.Send(new stFeedRideUserCmd_CS()
    //    {
    //        id = rid,
    //        feed_item = itemid,
    //        num = num,
    //    });
    //}

    /// <summary>
    /// 坐骑封印
    /// </summary>
    /// <param name="rid"></param>
    //public void RideSealRideUser(uint rid)
    //{
    //    NetService.Instance.Send(new stSealRideUserCmd_CS()
    //    {
    //        id = rid,
    //    });
    //}
    /// <summary>
    /// 坐骑移除
    /// </summary>
    /// <param name="rid"></param>
    public void RideRemoveRideUser(uint rid)
    {
        NetService.Instance.Send(new stRemoveRideUserCmd_CS()
        {
            id = rid,
        });
    }

    public void RideExpandMaxRide()
    {
        NetService.Instance.Send(new stAddMaxNumRideUserCmd_CS());
    }

    public void RideDownRide(Action<object> callback = null, object param = null)
    {
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            //    bool bRide = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
            bool bRide = DataManager.Manager<RideManager>().IsRide;
            if (!bRide)
            {
                if (callback != null)
                {
                    callback(param);
                }
                return;
            }
        }
        RideMgr.UnRideCallback = callback;
        RideMgr.UnRideCallbackParam = param;
        NetService.Instance.Send(new stDownRideUserCmd_C() { });
    }
    /// <summary>
    /// 上马
    /// </summary>
    public void RideUsingRide()
    {
        NetService.Instance.Send(new stUsingRideUserCmd_C());
    }

    public void RideUsingSkill(uint skillid)
    {
        //    NetService.Instance.Send(new stUsingSkillRideUserCmd_CS() { skill = (int)skillid });
    }

    public void RideRemoveSealRide(uint itemid)
    {
        //    NetService.Instance.Send(new stRemoveSealRideUserCmd_CS() { item_id = itemid });
    }

    //public void RideExpPass(uint leftRideid,uint rightRideid,GameCmd.PassType type)
    //{
    ////    NetService.Instance.Send(new stExpPassRideUserCmd_CS() { give_ride = leftRideid, get_ride = rightRideid, pass_type = type });
    //}

    #region 骑术相关
    [Execute]
    public void OnReceiveKnightExp(stSetKnightExpRideUserCmd_S cmd)
    {
        DataManager.Manager<RideManager>().OnReceiveKnightExp(cmd);
    }
    [Execute]
    public void OnReceiveKnightLevel(stSetKnightLevelRideUserCmd_S cmd)
    {
        DataManager.Manager<RideManager>().OnReceiveKnightLevel(cmd);
    }
    [Execute]
    public void OnReveiveKnightBreak(stSetKnightRankRideUserCmd_S cmd)
    {
        DataManager.Manager<RideManager>().OnReveiveKnightBreak(cmd);
    }
    [Execute]
    public void OnReveiveKnightTalent(stSetKnightTelantRideUserCmd_S cmd)
    {
        DataManager.Manager<RideManager>().OnReveiveKnightTalent(cmd);
    }
    [Execute]
    public void OnReveiveKnightPower(stSetKnightFightPowerRideUserCmd_S cmd)
    {
        DataManager.Manager<RideManager>().OnReveiveKnightPower(cmd);
    }
    #endregion
    #endregion
}
