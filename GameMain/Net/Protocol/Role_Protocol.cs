using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;
using Controller;
partial class Protocol
{
    #region 网络消息 角色创建添加
    /// <summary>
    /// 刷新战斗力协议
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stRefreshFightPowerDataUserCmd_S cmd)
    {
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        int preFightPower = 0;
        if (null != player)
        {
            preFightPower = player.GetProp((int)Client.FightCreatureProp.Power);
            player.SetProp((int)Client.FightCreatureProp.Power, (int)cmd.power);
        }
        //发送战斗力变更事件
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.PLAYER_FIGHTPOWER_REFRESH, new Client.stRefreshPowerParams()
        {
            PreFightPower = preFightPower,
            CurFightPower = (int)cmd.power,
        });
    }
    /// <summary>
    /// 刷新主角金钱
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stRefreshMoneyPropertyUserCmd_S cmd)
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        GameCmd.MoneyType e = (GameCmd.MoneyType)cmd.type;
        //为第一次加载 || InitType 服务器通知是否是游戏运行中刷新货币
        bool firstLoad = (null == player) || cmd.inittype == 1;
        ItemDefine.UpdateCurrecyPassData passData = null;
        int changeNum = 0;
        switch (e)
        {
            case GameCmd.MoneyType.MoneyType_MoneyTicket: // (文钱)
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.Money, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - UserData.Money;
                    UserData.Money = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_Gold: // (金币)
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.Coupon, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.Coupon;
                    UserData.Coupon = (int)cmd.dwNum;

                    break;
                }
            case GameCmd.MoneyType.MoneyType_Coin: // (元宝)
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.Cold, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.Cold;
                    UserData.Cold = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_Score: //积分
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.Score, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.Score;
                    UserData.Score = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_Reputation: //声望
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.Reputation, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.Reputation;
                    UserData.Reputation = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_AchievePoint: //声望
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.AchievePoint, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.AchievePoint;
                    UserData.AchievePoint = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_CampCoin: //阵营积分
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.CampCoin, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.CampCoin;
                    UserData.CampCoin = (int)cmd.dwNum;
                    break;
                }

            case GameCmd.MoneyType.MoneyType_HuntingCoin: //狩猎积分
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.ShouLieScore, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.ShouLieScore;
                    UserData.ShouLieScore = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_FishingMoney: //鱼币
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.FishingMoney, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.FishingMoney;
                    UserData.FishingMoney = (int)cmd.dwNum;
                    break;
                }
            case GameCmd.MoneyType.MoneyType_TradeGold: //银两
                {
                    if (player != null)
                    {
                        player.SetProp((int)PlayerProp.YinLiang, (int)cmd.dwNum);
                    }
                    changeNum = (int)cmd.dwNum - (int)UserData.YinLiang;
                    UserData.YinLiang = (int)cmd.dwNum;
                    break;
                }
            default:
                {
                    break;
                }
        }

        if (!firstLoad)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, new ItemDefine.UpdateCurrecyPassData()
                {
                    MoneyType = e,
                    ChangeNum = changeNum,
                });
        }

    }

    /// <summary>
    /// 登录成功后主角信息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stMainUserDataUserCmd_S cmd)
    {
        //MainRole.Instance.MainData = cmd.data;

        // TODO: 后面再来处理
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        EntityCreateData data = new EntityCreateData();
        data.PropList = new EntityAttr[(int)PlayerProp.End - (int)EntityProp.Begin];
        data.ID = cmd.data.dwUserID;
        RoleUtil.BuildPlayerPropList(cmd.data, ref data.PropList);
        IPlayer player = es.FindPlayer(cmd.data.dwUserID);
        if (player != null)
        {
            player.UpdateProp(data);
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSetRoleProperty, player);
        }
    }

    /// <summary>
    /// 地图上增加或者刷新人物
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stAddUserMapScreenUserCmd_S cmd)
    {
        //AddPlayer(cmd.data);
        EntityCreator.Instance().AddPlayer(cmd.data, Vector3.zero, 0, null, false);
    }

    /// <summary>
    /// 地图上增加人物和坐标
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stAddUserAndPosMapScreenUserCmd_S cmd)
    {
        //IPlayer player = AddPlayer(cmd.data.mapuserdata);
        //if (player != null)

        //MapVector2 mapPos = MapVector2.FromCoordinate(cmd.data.x, cmd.data.y);
        Vector3 pos = new Vector3(cmd.data.cur_pos.x * 0.01f, 0, -cmd.data.cur_pos.y * 0.01f); // 服务器到客户端坐标转换
        //player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
        //Vector3 rot = GameUtil.S2CDirection(cmd.data.byDir);
        //player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
        //Engine.Utility.Log.Error("add player pos is ({0},{1})", pos.x, pos.z);
        EntityCreator.Instance().AddPlayer(cmd.data.mapuserdata, pos, cmd.data.byDir, cmd.data.poslist, true);
    }

    /// <summary>
    /// 请求玩家名字
    /// </summary>
    /// <param name="id"></param>
    public void RequestName(uint id)
    {
        var cmd = new stRequestUserNameRequestUserCmd_C();
        cmd.data.Add(id);
        NetService.Instance.Send(cmd);


    }
    /// <summary>
    /// 返回玩家姓名
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stReturnUserNameRequestUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        foreach (var item in cmd.data)
        {
            IPlayer player = es.FindPlayer(item.id);
            if (player != null)
            {
                player.SendMessage(EntityMessage.EntityCommond_SetName, item.name);

                //玩家自己
               if (Client.ClientGlobal.Instance().IsMainPlayer(player))
               {
                   stNewName name = new stNewName();
                   name.newName = item.name;
                   Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_NEWNAME, name);
               }
            }
        }
    }

    /// <summary>
    /// 地图上面删除人物
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stRemoveUserMapScreenUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        {
            EntityCreator.Instance().RemovePlayer(cmd.dwUserTempID);
            IPlayer player = es.FindPlayer(cmd.dwUserTempID);
            if (player != null)
            {
                if (!ClientGlobal.Instance().IsMainPlayer(player))
                {
                    Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                    removeEntiy.uid = player.GetUID();
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);

                    es.RemoveEntity(player);
                }
            }

        }
    }

    /// <summary>
    /// 批量删角色
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stBatchRemoveUserMapScreenUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }
        for (int i = 0; i < cmd.id.Count; ++i)
        {
            EntityCreator.Instance().RemovePlayer(cmd.id[i]);

            IPlayer player = es.FindPlayer(cmd.id[i]);
            if (player != null)
            {
                if (!ClientGlobal.Instance().IsMainPlayer(player))
                {
                    Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                    removeEntiy.uid = player.GetUID();
                    Engine.Utility.Log.Info("remove :" + cmd.id[i]);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);

                    es.RemoveEntity(player);
                }
            }
        }
    }

    #endregion


    #region 网络消息 角色移动

    [Execute]
    public void GetDesPos(stGetDstPosDataUserCmd_CS cmd)
    {
        IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_Player, cmd.userid);

        //Move move = new Move();
        //move.m_speed = player.GetProp( (int)CreatureProp.MoveSpeed ) * 0.001f; // 速度为测试速度
        ////move.m_target = scenePos;
        //move.strRunAct = Client.EntityAction.Run;
        //move.path = new List<Vector3>();
        //move.path.Add( new Vector3( cmd.x , 0 , -cmd.y ) );

        if (player != null)
        {
            ISkillPart skillPart = player.GetPart(EntityPart.Skill) as ISkillPart;
            skillPart.GetDesPos(cmd);
        }
        //IEntity npc = EntitySystem.EntityHelper.GetEntity( SceneEntryType.SceneEntry_NPC , cmd.npcid );
        //if(npc != null)
        //{
        //    npc.SendMessage( EntityMessage.EntityCommand_MovePath , (object)move );
        //}

    }

    // 刷新服务器位置
    [Execute]
    public void RefrushServerPos(GameCmd.stSendUserCurPosMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.char_id);
        if (player == null)
        {
            return;
        }

        player.RefrushServerPos(new Vector3(cmd.pos_info.x * 0.01f, 0, -cmd.pos_info.y * 0.01f));
    }

    /*
    /// <summary>
    /// 用户移动下行消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stUserMoveDownMoveUserCmd cmd)
    {
        //IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        //if (es == null)
        //{
        //    Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
        //    return;
        //}


        //MapVector2 mapPos = MapVector2.FromCoordinate(cmd.dst_x, cmd.dst_y);
        //Vector3 pos = new Vector3(mapPos.x, 0, -mapPos.y); // 服务器到客户端坐标转换
        //IPlayer player = es.FindPlayer(cmd.dwUserTempID);
        //if (player != null && !ClientGlobal.Instance().IsMainPlayer(player))
        //{
        //    Move move = new Move();
        //    //move.m_dir = Global.S2CDirection(cmd.dir);
        //    move.strRunAct = "Ani_Run"; // 动作名需要统一处理
        //    move.m_target = pos;
        //    move.m_speed = player.GetProp((int)CreatureProp.CreatureProp_MoveSpeed)*0.01f;

        //    player.SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);
        //}
    }

    [Execute]
    public void Execute(GameCmd.stUserMoveDownPosListMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.charid);
        if (player == null)
        {
            return;
        }

        if(ClientGlobal.Instance().IsMainPlayer(player))
        {
            return;
        }

        Move move = new Move();
        move.m_speed = player.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE; // 速度为测试速度
        //move.m_target = scenePos;
        move.strRunAct = Client.EntityAction.Run;
        move.path = new List<Vector3>();
        for (int i = cmd.poslist.Count-1; i >=0; --i)
        {
            if (i > 0)
            {
                move.path.Add(new Vector3(cmd.poslist[i].x, 0, -cmd.poslist[i].y));
            }
            else
            {
                move.path.Add(new Vector3(cmd.poslist[i].x * 0.01f, 0, -cmd.poslist[i].y * 0.01f));
            }
        }
        player.SendMessage(EntityMessage.EntityCommand_MovePath, (object)move);
    }
    */
    /*
    //-------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 玩家按方向移动
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stUserMoveDownDirMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.charid);
        if (player == null)
        {
            return;
        }

        if (ClientGlobal.Instance().IsMainPlayer(player))
        {
            return;
        }

        Move move = new Move();
        move.m_speed = player.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE; // 速度为测试速度
        move.strRunAct = Client.EntityAction.Run;
        move.m_dir = cmd.dir * 10;
        move.path = new List<Vector3>();
        player.SendMessage(EntityMessage.EntityCommand_MoveDir, (object)move);
    }

    [Execute]
    public void Execute(GameCmd.stUserMoveStopMoveUserCmd_CS cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.charid);
        if (player == null)
        {
            return;
        }

        if(ClientGlobal.Instance().IsMainPlayer(player))
        {
            return;
        }

        Vector3 pos = new Vector3(cmd.pos.x*0.01f, 0, -cmd.pos.y*0.01f);

        Engine.Utility.Log.LogGroup("ZDY","stop move ==============================!" );
        player.SendMessage(EntityMessage.EntityCommand_StopMove, pos);
    }
    */
    #endregion


    #region ActorMove新版本消息

    [Execute]
    public void Execute(GameCmd.stTimeTickMoveUserCmd_S cmd)
    {
        uint time = cmd.server_time + ((uint)(Time.realtimeSinceStartup * 1000) - cmd.client_time);
        Engine.Utility.Log.LogGroup("XXF", "ServerTime:{0} {1}", cmd.server_time, time);
        EntitySystem.EntityConfig.serverTime = time;// cmd.server_time + ((uint)Time.realtimeSinceStartup * 1000 - cmd.client_time);
        EntitySystem.EntityConfig.m_bForceMove = false;

        //注意：先抛事件，在置BCheckingTime状态
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHECKINGTIME, false);    

        //对时结束
        NetService.Instance.BCheckingTime = false;
    }

    [Execute]
    public void Execute(GameCmd.stUserMoveMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.charid);
        if (player == null)
        {
            return;
        }

        if (ClientGlobal.Instance().IsMainPlayer(player))
        {
            return;
        }

        Move move = new Move();
        move.m_speed = player.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE; // 速度为测试速度
        move.strRunAct = Client.EntityAction.Run;
        move.path = new List<Vector3>();
        // 添加起点
        move.path.Add(new Vector3(cmd.begin_pos_x * 0.01f, 0, -cmd.begin_pos_y * 0.01f));
        for (int i = 0; i < cmd.poslist.Count; ++i)
        {
            //if (i > 0)
            //{
            //    move.path.Add(new Vector3(cmd.poslist[i].x, 0, -cmd.poslist[i].y));
            //}
            //else
            {
                move.path.Add(new Vector3(cmd.poslist[i].x * 0.01f, 0, -cmd.poslist[i].y * 0.01f));
            }
        }
        player.SendMessage(EntityMessage.EntityCommand_MovePath, (object)move);
    }

    [Execute]
    public void Execute(GameCmd.stUserStopMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(cmd.charid);
        if (player == null)
        {
            return;
        }

        if (ClientGlobal.Instance().IsMainPlayer(player))
        {
            return;
        }

        Vector3 pos = new Vector3(cmd.stop_pos_x * 0.01f, 0, -cmd.stop_pos_y * 0.01f);

        Engine.Utility.Log.LogGroup("ZDY", "stop move ==============================!");
        player.SendMessage(EntityMessage.EntityCommand_ForceStopMove, pos);
    }

    [Execute]
    public void Execute(GameCmd.stForceStopUserMoveUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return;
        }

        Vector3 pos = new Vector3(cmd.stop_pos_x * 0.01f, 0, -cmd.stop_pos_y * 0.01f);
        Engine.Utility.Log.LogGroup("ZDY", "stop move ==============================!");
        player.SendMessage(EntityMessage.EntityCommand_ForceStopMove, pos);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);

        // 请求服务器同步时间
        GameCmd.stTimeTickMoveUserCmd_C cmd_send = new GameCmd.stTimeTickMoveUserCmd_C
        {
            client_time = (uint)(Time.realtimeSinceStartup * 1000)
        };
        if (cmd_send != null && ClientGlobal.Instance().netService != null)
        {
            ClientGlobal.Instance().netService.SendCheckTime(cmd_send);
        }

        // 同步下服务器时间
        EntitySystem.EntityConfig.m_bForceMove = true;
        //EntitySystem.EntityConfig.serverTime = cmd.server_time;
        //Engine.Utility.Log.LogGroup("XXF", "ForceStop {0}", EntitySystem.EntityConfig.serverTime);
    }


    #endregion

    #region 网络消息 角色升级
    [Execute]
    public void Execute(GameCmd.stLevelUpMagicUserCmd_S cmd)
    {
        Client.IEntity en = EntitySystem.EntityHelper.GetEntity(GameCmd.SceneEntryType.SceneEntry_Player, cmd.dwUserTempID);
        if (en == null)
        {
            return;
        }

        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.Level;
        prop.oldValue = en.GetProp((int)CreatureProp.Level);
        prop.newValue = (int)cmd.dwLevel;

        stEntityLevelUp levelup = new stEntityLevelUp();
        levelup.uid = en.GetUID();
        levelup.nLastLevel = en.GetProp((int)CreatureProp.Level);
        levelup.nLevel = (int)cmd.dwLevel;

        en.SetProp((int)CreatureProp.Level, (int)cmd.dwLevel);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);

        // 等级提升
        if (levelup.nLevel > levelup.nLastLevel)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_LEVELUP, levelup);

            //IEntity mainPlayer = EntitySystem.EntityHelper.GetEntity(MainPlayerHelper.GetPlayerUID());


            if (ClientGlobal.Instance().IsMainPlayer(en))
            {
                //主角实体加特效

                EntitySystem.EntityHelper.AddEffect(en, 9001);
                //统计
                DataManager.Manager<LoginDataManager>().UserUpLevel(levelup.nLevel);
            }


        }


        // 飘字提示 升级成功
        //         if(ClientGlobal.Instance().IsMainPlayer(en))
        //         {
        //             string strTips = string.Format("角色等级提升");
        //             TipsManager.Instance.ShowTips(strTips);
        //         }
    }


    #endregion

    #region 属性更新
    [Execute]
    public void SetEntityState(stStatesChangeDataUserCmd_S cmd)
    {

        if (cmd.type == (int)GameCmd.SceneEntryType.SceneEntry_Player)
        {
            IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_Player, cmd.id);
            if (player != null)
            {
                player.SetProp((int)EntityProp.EntityState, (int)cmd.states);
            }
        }
        else if (cmd.type == (int)GameCmd.SceneEntryType.SceneEntry_NPC)
        {
            IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_NPC, cmd.id);

            if (player != null)
            {
                player.SetProp((int)EntityProp.EntityState, (int)cmd.states);
            }
        }

    }

    [Execute]
    public void SetFlag(stFlagsChangeDataUserCmd_S cmd)
    {
        if (cmd.type == (int)GameCmd.SceneEntryType.SceneEntry_Player)
        {
            IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_Player, cmd.id);
            if (player == null)
            {
                return;
            }
            if (player is IPlayer)
            {
                player.SetProp((int)PlayerProp.StateBit, (int)cmd.flags);
            }
        }
    }

    [Execute]

    public void SetRoleHP(stHPChangeDataUserCmd_S cmd)
    {//
        stBuffDamage st = new stBuffDamage();
        st.changeValue = cmd.value;
        st.damagetype = cmd.ctype;
        st.curHp = cmd.cur_hp;
        if (cmd.obj_type == (int)GameCmd.SceneEntryType.SceneEntry_Player)
        {
            IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_Player, cmd.obj_id);
            if (player != null)
            {
                st.uid = player.GetUID();
                st.entityType = (int)EntityType.EntityType_Player;

                if (cmd.max_hp != 0)
                {
                    player.SetProp((int)CreatureProp.MaxHp, (int)cmd.max_hp);
                }
                if (cmd.cur_hp != 0)
                {
                    player.SetProp((int)CreatureProp.Hp, (int)cmd.cur_hp);
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_SHOWBUFFDAMAGE, (object)st);
            }
        }
        else if (cmd.obj_type == (int)GameCmd.SceneEntryType.SceneEntry_NPC)
        {
            IEntity player = EntitySystem.EntityHelper.GetEntity(SceneEntryType.SceneEntry_NPC, cmd.obj_id);
            if (player != null)
            {
                st.uid = player.GetUID();
                st.entityType = (int)EntityType.EntityType_NPC;
                if (cmd.max_hp != 0)
                {
                    player.SetProp((int)CreatureProp.MaxHp, (int)cmd.max_hp);
                }
                if (cmd.cur_hp != 0)
                {
                    player.SetProp((int)CreatureProp.Hp, (int)cmd.cur_hp);
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_SHOWBUFFDAMAGE, (object)st);
            }
        }

    }
    // 服务器通知血量更新
    [Execute]
    public void Excute(GameCmd.stSetHPDataUserCmd_S cmd)
    {//只有npc用 角色用stHPChangeDataUserCmd_S
        Client.IEntity en = EntitySystem.EntityHelper.GetEntity((GameCmd.SceneEntryType)cmd.type, cmd.id);
        if (en == null)
        {
            return;
        }

        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.Hp;
        prop.oldValue = en.GetProp((int)CreatureProp.Hp);
        prop.newValue = (int)cmd.curhp;

        en.SetProp((int)CreatureProp.Hp, (int)cmd.curhp);
        if (cmd.curhp == 0)
        {
            Engine.Utility.Log.LogGroup("ZDY", "cmd.curhp----------------" + cmd.curhp);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
    }

    // 服务器通知魔法更新
    [Execute]
    public void Excute(GameCmd.stSetSPDataUserCmd_S cmd)
    {
        Client.IEntity en = EntitySystem.EntityHelper.GetEntity(GameCmd.SceneEntryType.SceneEntry_Player, cmd.id);
        if (en == null)
        {
            return;
        }

        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.Mp;
        prop.oldValue = en.GetProp((int)CreatureProp.Mp);
        prop.newValue = (int)cmd.sp;

        en.SetProp((int)CreatureProp.Mp, (int)cmd.sp);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_MPUPDATE, prop);

        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.MaxMp;
        prop.oldValue = en.GetProp((int)CreatureProp.MaxMp);
        prop.newValue = (int)cmd.maxsp;

        en.SetProp((int)CreatureProp.MaxMp, (int)cmd.maxsp);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_MPUPDATE, prop);
    }

    // 服务器通知更新生命和魔法
    [Execute]
    public void Excute(GameCmd.stSetHpSpDataUserCmd_CS cmd)
    {
        Client.IEntity en = ClientGlobal.Instance().MainPlayer;
        if (en == null)
        {
            return;
        }

        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.Mp;
        prop.oldValue = en.GetProp((int)CreatureProp.Mp);
        prop.newValue = (int)cmd.sp;

        en.SetProp((int)CreatureProp.Mp, (int)cmd.sp);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_MPUPDATE, prop);

        if (en.GetProp((int)CreatureProp.MaxMp) != (int)cmd.maxsp)
        {
            prop.nPropIndex = (int)CreatureProp.MaxMp;
            prop.oldValue = en.GetProp((int)CreatureProp.MaxMp);
            prop.newValue = (int)cmd.maxsp;

            en.SetProp((int)CreatureProp.MaxMp, (int)cmd.maxsp);

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_MPUPDATE, prop);
        }

        prop.nPropIndex = (int)CreatureProp.Hp;
        prop.oldValue = en.GetProp((int)CreatureProp.Hp);
        prop.newValue = (int)cmd.hp;

        en.SetProp((int)CreatureProp.Hp, (int)cmd.hp);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);

        if (en.GetProp((int)CreatureProp.MaxHp) != (int)cmd.maxhp)
        {
            prop.nPropIndex = (int)CreatureProp.MaxHp;
            prop.oldValue = en.GetProp((int)CreatureProp.MaxHp);
            prop.newValue = (int)cmd.maxhp;
            en.SetProp((int)CreatureProp.MaxHp, (int)cmd.maxsp);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
        }

    }
    /// <summary>
    /// 经验更新
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stObtainExpPropertyUserCmd_S cmd)
    {
        Client.IEntity en = ClientGlobal.Instance().MainPlayer;
        if (en == null)
        {
            return;
        }
        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)PlayerProp.Exp;
        prop.oldValue = en.GetProp((int)PlayerProp.Exp);
        prop.newValue = (int)cmd.dwUserExp;

        en.SetProp((int)PlayerProp.Exp, (int)cmd.dwUserExp);

        if (cmd.dwExp > 0)  //增加的经验
        {
            TipsManager.Instance.ShowTips("获得经验X" + cmd.dwExp);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
    }

    /// <summary>
    /// 改名
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(stChangeCharNameDataUserCmd_CS cmd)
    {
        string newName = cmd.newname;
        string oldName = cmd.oldname;

        //IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        //if (mainPlayer != null)
        //{
        //    mainPlayer.SendMessage(EntityMessage.EntityCommond_SetName, cmd.newname);

        //    stNewName name = new stNewName();
        //    name.newName = cmd.newname;

        //    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_NEWNAME, name);
        //}
    }

    #endregion

    #region 查看玩家 stViewRoleReturnPropertyUserCmd_S 返回
    [Execute]
    public void Excute(GameCmd.stViewRoleReturnPropertyUserCmd_S cmd)
    {
        //         IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        //         if (es == null)
        //         {
        //             Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
        //             return;
        //         }
        // 
        //         EntityCreateData data = new EntityCreateData();
        //         data.PropList = new EntityAttr[(int)PlayerProp.End - (int)EntityProp.Begin];
        //         data.ID = cmd.userdata.dwUserID;
        //         RoleUtil.BuildPlayerPropList(cmd.userdata, ref data.PropList);
        //         IPlayer player = es.FindPlayer(cmd.userdata.dwUserID);
        //         if (player != null)
        //         {
        //             player.UpdateProp(data);
        //             DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PropPanel, data: cmd.userdata.dwUserID);
        //         }

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ViewPlayerPanel, data: ViewPlayerData.BuildViewData(cmd));
    }


    [Execute]
    public void OnResViewPkValueRemainTime(stRequestPkRemainTimePropertyUserCmd_CS cmd) 
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.REFRESHPKVALUEREMAINTIME, cmd.time);
    }
    #endregion

    #region 复活
    /// <summary>
    /// 复活 cmd stOKReliveUserCmd 
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(GameCmd.stMainUserRelivePropertyUserCmd_S cmd)
    {
        //  GameCmd.stReliveColdMagicUserCmd
        Engine.Utility.Log.Info("复活玩家 id: " + cmd.dwUserTempID);

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();

        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }
        IPlayer player = es.FindPlayer(cmd.dwUserTempID);
        if (player == null) return;
        //  MapVector2 mapPos = MapVector2.FromCoordinate(cmd.x, cmd.y);
        Vector3 pos = new Vector3(cmd.x, 0, -cmd.y); // 服务器到客户端坐标转换
        player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
        player.ChangeState(CreatureState.Normal);

        stEntityRelive stRelive = new stEntityRelive();
        stRelive.uid = player.GetUID();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_RELIVE, stRelive);
    }

    [Execute]
    public void Excute(GameCmd.stReliveColdMagicUserCmd_S cmd)
    {
        //TODO : 显示复活CD 
        if (ClientGlobal.Instance().MainPlayer != null)
        {
            DataManager.Manager<ReLiveDataManager>().AddReLiveColdInfo(cmd);

            DoDie();
            return;
        }
        //防止进入游戏就死亡，但是MainPlayer 为空
        StartCoroutine(WaitForMainPlayer());
    }

    void DoDie()
    {
        Engine.Utility.Log.Info("主角死亡............");
        //客户端自己设置血量为0
        Client.IEntity en = ClientGlobal.Instance().MainPlayer;
        if (en == null)
        {
            return;
        }
        stPropUpdate prop = new stPropUpdate();
        prop.uid = en.GetUID();
        prop.nPropIndex = (int)CreatureProp.Hp;
        prop.oldValue = en.GetProp((int)CreatureProp.Hp);
        prop.newValue = 0;
        en.SetProp((int)CreatureProp.Hp, prop.newValue);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
        //清空选中目标
        ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().UpdateTarget(null);

        if (Client.ClientGlobal.Instance().MainPlayer.IsDead())
        {
            if (DataManager.Manager<NvWaManager>().EnterNvWa == false)//女娲无复活弹框
            {
                float waitTime = 1f;
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReLivePanel,data : waitTime);
            }
        }
    }


    public IEnumerator WaitForMainPlayer()
    {
        while (ClientGlobal.Instance().MainPlayer == null)
        {
            yield return null;
        }
        IMapSystem ms = ClientGlobal.Instance().GetMapSystem();
        if (ms.Process >= 1.0f)
        {
            DoDie();
        }
        yield return null;
    }
    #endregion



    #region pk model
    /// <summary>
    /// Pk模式
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stSetPKModePropertyUserCmd_CS cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IEntity en = es.FindPlayer(cmd.userid);
        if (en != null)
        {
            en.SetProp((int)PlayerProp.PkMode, (int)cmd.pkmode);
            stPropUpdate prop = new stPropUpdate();
            prop.uid = en.GetUID();
            prop.nPropIndex = (int)PlayerProp.PkMode;
            prop.oldValue = en.GetProp((int)PlayerProp.PkMode);
            prop.newValue = (int)cmd.pkmode;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
            //切换模式后刷新一下场景内的的血条
            RoleStateBarManager.RefreshAllRoleBarHeadStatus(HeadStatusType.Hp);
        }
        else
        {
            Engine.Utility.Log.Error("找不到player ：{0}", cmd.userid);
        }
    }

    [Execute]
    public void Execute(stSetPKValuePropertyUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IEntity en = es.FindPlayer(cmd.userid);
        if (en != null)
        {
            en.SetProp((int)PlayerProp.PKValue, (int)cmd.pkvalue);
            stPropUpdate prop = new stPropUpdate();
            prop.uid = en.GetUID();
            prop.nPropIndex = (int)PlayerProp.PKValue;
            prop.oldValue = en.GetProp((int)PlayerProp.PKValue);
            prop.newValue = (int)cmd.pkvalue;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
        }
        else
        {
            Engine.Utility.Log.Error("找不到player ：{0}", cmd.userid);
        }
    }

    [Execute]
    public void Execute(stSendGoodNessDataUserCmd cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IEntity en = es.FindPlayer(cmd.userid);
        if (en != null)
        {
            //// 时间(分钟) 大于0表示恶，否则表示善  暂时没用
            en.SetProp((int)PlayerProp.GoodNess, (int)cmd.goodstate);//GameCmd.enumGoodNess.GoodNess_Badman:
            stPropUpdate prop = new stPropUpdate();
            prop.uid = en.GetUID();
            prop.nPropIndex = (int)PlayerProp.GoodNess;
            prop.oldValue = en.GetProp((int)PlayerProp.GoodNess);
            prop.newValue = (int)cmd.goodstate;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, prop);
        }
        else
        {
            Engine.Utility.Log.Error("找不到player ：{0}", cmd.userid);
        }
    }
    #endregion

    #region 变身

    [Execute]
    public void Excute(stTransfigurationScriptUserCmd_S cmd)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        Client.IEntity player = es.FindPlayer(cmd.userid);
        if (player != null)
        {
            DataManager.Manager<RideManager>().TryUnRide((obj) =>
            {
                player.SendMessage(Client.EntityMessage.EntityCommand_Change, new Client.ChangeBody()
                {
                    resId = (int)cmd.modelid,
                    param = cmd.taskid,
                    callback = null,
                    modleScale = 1
                });
            },
            null);
        }
    }

    [Execute]
    public void Excute(stUnTransfigurationScriptUserCmd_S cmd)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        Client.IEntity player = es.FindPlayer(cmd.userid);
        if (player != null)
        {
            player.SetProp((int)Client.PlayerProp.TransModelResId, 0);
            player.SendMessage(Client.EntityMessage.EntityCommand_Restore, new Client.ChangeBody()
            {
                param = cmd.taskid,
                callback = null,
                modleScale = 1
            });
        }
    }
    #endregion
}
