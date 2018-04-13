using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Engine.Utility;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;
using table;

partial class Protocol
{
    //////////////////////////////////////////////////////////////////////
    /// 机器人
    /**
    @brief 机器人升级
    @param 
    */
    [Execute]
    public void Execute(GameCmd.stRobotLevelUpMagicUserCmd_S cmd)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }
        Client.IRobot robot = es.FindRobot(cmd.npc_id);
        if (robot == null)
        {
            return;
        }
        uint baseID = (uint)robot.GetProp((int)Client.EntityProp.BaseID);
        uint job = (uint)robot.GetProp((int)Client.RobotProp.Job);
        uint level = (uint)robot.GetProp((int)Client.CreatureProp.Level);
        string name = robot.GetName();
        EntityCreateData createData = RoleUtil.BuildRobotEntityData(cmd.npc_id, baseID, job, level, name);

        stPropUpdate lvprop = new stPropUpdate();
        stPropUpdate prop = new stPropUpdate();
        prop.uid = robot.GetUID();
        lvprop.uid = robot.GetUID();

        prop.nPropIndex = (int)CreatureProp.Hp;
        lvprop.nPropIndex = (int)CreatureProp.Level;

        prop.oldValue = robot.GetProp((int)CreatureProp.Hp);
        lvprop.oldValue = robot.GetProp((int)CreatureProp.Level);

        robot.UpdateProp(createData);

        prop.newValue = robot.GetProp((int)CreatureProp.Hp);
        lvprop.newValue = robot.GetProp((int)CreatureProp.Level);

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, lvprop);

    }

    /**
    @brief 机器人采集
    @param 
    */
    [Execute]
    public void Execute(GameCmd.stRobotCollectMagicUserCmd_S cmd)
    {
        long id = EntitySystem.EntityHelper.MakeUID(EntityType.EntityTYpe_Robot, cmd.npc_id);
        Client.stUninterruptMagic param = new Client.stUninterruptMagic();
        param.time = cmd.sec;
        param.type = GameCmd.UninterruptActionType.UninterruptActionType_CJ;
        param.uid = id;
        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSSTART, param);
    }
    [Execute]
    public void ChangeServer(GameCmd.stChangeServerMapScreenUserCmd_S cmd)
    {
        //切换服务器
        DataManager.Manager<BuffDataManager>().ClearAllBuff();
    }

    /// <summary>
    /// 切换地图回应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnChangeMapRes(stChangeMapScreenUserCmd_CS msg)
    {
        //成功
        if (msg.ret == 0)
        {

        }
    }

    /// <summary>
    /// 玩家第一次进入地图
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stFirstMainUserPosMapScreenUserCmd_S cmd)
    {
        //UserData.SetMsgTypeToEnterScene(); // 加载地图时，阻塞消息

        //resetSceneData();

     
        Action<UIPanelBase> loadingAction = (panelBase) =>
        {
            UserData.MapID = cmd.mapid;
            // UserData.CurrentCountryID = cmd.countryid;
            UserData.Pos = new MapVector2(cmd.x, cmd.y);

            IMapSystem mapSystem = ClientGlobal.Instance().GetMapSystem();
            if (mapSystem != null)
            {
                // 统一处理地图ID
                if (DataManager.Manager<MapDataManager>().CurLineNum != cmd.line && mapSystem.GetMapID() == cmd.mapid)
                {
                    IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                    if (es != null)
                    {
                        es.Clear();
                    }
                    GameCmd.stTimeTickMoveUserCmd_C cmdinfo = new GameCmd.stTimeTickMoveUserCmd_C
                    {
                        client_time = (uint)(Time.realtimeSinceStartup * 1000)
                    };
                    NetService.Instance.SendCheckTime(cmdinfo);

                }
                //uint uSceneID = (cmd.countryid << 8) | cmd.mapid;
                mapSystem.EnterMap(cmd.mapid, new Vector3(cmd.x, 0, -cmd.y));
                Log.Info("进入场景后的第一个消息 id={0}", cmd.mapid);
            }
    
            UserData.MapID = cmd.mapid;
            //UserData.CurrentCountryID = cmd.countryid;
            UserData.Pos = new MapVector2(cmd.x, cmd.y);

            Engine.Utility.Log.Error("EnterMap:({0},{1})", cmd.x, -cmd.y);
            MapDataManager mgr = DataManager.Manager<MapDataManager>();
            if (mgr.CurLineNum != cmd.line)
            {
                if (!mgr.IsDefalultLine)
                {
                    string mapName = mapSystem.GetMapName();
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Copy_Commond_changeline, mapName, cmd.line);
                }
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ChangeLinePanel);
                //             stRequestLineInfoMapScreenUserCmd_C changeLine = new stRequestLineInfoMapScreenUserCmd_C();
                //             NetService.Instance.Send(changeLine);
            }
            mgr.CurLineNum = cmd.line;
        };

        if (UserData.MapID != cmd.mapid)
        {
            DataManager.Manager<UIPanelManager>().ShowLoading(progress: 0);
        }
        //else
        //{
        //    loadingAction.Invoke(null);
        //}

        loadingAction.Invoke(null);

    }

    /// <summary>
    /// 进入地图数据全部发完
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stEndOfIntoSceneDataUserCmd cmd)
    {
        // 此消息仅做通知用，不用处理
        Log.Info("进入地图数据全部发完");
    }

    /// <summary>
    /// 刷新当前场景id
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stRefreshSceneIdDataUserCmd_S cmd)
    {
        // 更换地图
        IMapSystem mapSystem = ClientGlobal.Instance().GetMapSystem();
        if (mapSystem != null && UserData.IsMainRoleID(cmd.userid))
        {
            uint uid = (cmd.sceneid & 0xff);
            uint uCountryid = (cmd.sceneid >> 8);

            mapSystem.EnterMap(uid, Vector3.zero);
        }
    }

    /// <summary>
    /// 场景中实体数据
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stMapDataMapScreenUserCmd_S cmd)
    {
        for (int i = 0; i < cmd.npclist.Count; i++)
        {
            var npcData = cmd.npclist[i];
            EntityCreator.Instance().AddNPC(npcData, npcData.mapnpcdata.npcdata.master_id);
        }
        //批量添加物品 没有结束时间 待补充
        for (int i = 0; i < cmd.itemlist.Count; i++)
        {
            var data = cmd.itemlist[i];
            if (data.byHideType == SceneItemHideType.SceneItem_OneUserSee)
            {
                if (data.SeeOwner == MainPlayerHelper.GetPlayerID())
                {
                    EntityCreator.Instance().AddBox(data, data.remain_time);
                }

            }
            else if (data.byHideType == SceneItemHideType.SceneItem_AllSee)
            {
                EntityCreator.Instance().AddBox(data, data.remain_time);
            }
        }
        for (int i = 0; i < cmd.userlist.Count; i++)
        {
            var roleData = cmd.userlist[i];
            // 若是主角，则更新主角的坐标和朝向
            if (UserData.IsMainRoleID(roleData.mapuserdata.userdata.dwUserID))
            {
                ProtoBuf.IExtensible send;
                if (cmd.npclist.Count > 0)
                {
                    send = new GameCmd.stReturnObjectPosMagicUserCmd_S()
                    {
                        userpos = new GameCmd.stEntryPosition() { cur_pos = roleData.cur_pos, byDir = roleData.byDir },
                    };
                }
                else
                {
                    send = new GameCmd.stReturnObjectPosMagicUserCmd_S()
                    {
                        userpos = new GameCmd.stEntryPosition() { cur_pos = roleData.cur_pos, byDir = roleData.byDir },
                    };
                }
                NetService.Instance.SendToMe(send);
                continue;
            }

            //IPlayer player = Protocol.Instance.AddPlayer(roleData.mapuserdata);
            // if (player != null)
            //{
            //MapVector2 mapPos = MapVector2.FromCoordinate(roleData.x, roleData.y);
            Vector3 pos = new Vector3(roleData.cur_pos.x * 0.01f, 0, -roleData.cur_pos.y * 0.01f); // 服务器到客户端坐标转换
            //player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
            //Vector3 rot = GameUtil.S2CDirection(roleData.byDir);
            //player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
            EntityCreator.Instance().AddPlayer(roleData.mapuserdata, pos, roleData.byDir, roleData.poslist, true);
            // }

        }
    }

    [Execute]
    public void Execute(GameCmd.stReturnBinaryDataUserCmd_S cmd)
    {
        switch (cmd.name)
        {
            case "mapinfo.xml": // 地图列表配置文件
                {
                    var xml = System.Text.Encoding.UTF8.GetString(cmd.data);
                    UserData.MapList = new List<UserData.MapInfo>();
                    foreach (var map in XDocument.Parse(xml).Root.Elements("map"))
                    {
                        var info = new UserData.MapInfo();
                        info.Deserialize(map);
                        UserData.MapList.Add(info);
                    }
                }
                break;
            case "mapconfig.xml": // 当前地图的详细配置信息 *_client.xml
                {
                    var xml = System.Text.Encoding.UTF8.GetString(cmd.data);
                    //BattleScene.Instance.ParseMapConfig(xml);
                }
                break;
            default:
                Log.Warning(string.Format("未处理的二进制数据, name={0}", cmd.name));
                break;
        }
    }

    /// <summary>
    /// 刷新移动速度
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stChangeMoveSpeedDataUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        switch (cmd.type)
        {
            case (uint)SceneEntryType.SceneEntry_NPC:
                {
                    INPC npc = es.FindNPC(cmd.id);
                    if (npc != null)
                    {
                        npc.SetProp((int)WorldObjProp.MoveSpeed, (int)cmd.mvspd);
                        npc.SendMessage(EntityMessage.EntityCommand_SetAniSpeed, (object)1.0f);
                    }
                }
                break;
            case (uint)SceneEntryType.SceneEntry_Player:
                {
                    IPlayer player = es.FindPlayer(cmd.id);
                    if (player != null)
                    {
                        player.SetProp((int)WorldObjProp.MoveSpeed, (int)cmd.mvspd);
                        player.SendMessage(EntityMessage.EntityCommand_SetAniSpeed, (object)1.0f);
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 物品掉落到场景中
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddMapObjectRes(stAddMapObjectMapScreenUserCmd_S cmd)
    {
        if (cmd.data.byHideType == SceneItemHideType.SceneItem_OneUserSee)
        {
            if (cmd.data.SeeOwner == MainPlayerHelper.GetPlayerID())
            {
                EntityCreator.Instance().AddBox(cmd.data, cmd.data.remain_time);
            }
        }
        else if (cmd.data.byHideType == SceneItemHideType.SceneItem_AllSee)
        {
            EntityCreator.Instance().AddBox(cmd.data, cmd.data.remain_time);
        }
    }

    /// <summary>
    /// 场景中移除物品
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRemoveMapObjectRes(stRemoveMapObjectMapScreenUserCmd_S cmd)
    {
        //Engine.Utility.Log.Info("OnRemoveMapObjectRes ID:" + cmd.qwThisID);

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        {
            EntityCreator.Instance().RemovePlayer(cmd.qwThisID);
            IBox box = es.FindBox(cmd.qwThisID);
            if (box != null)
            {
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = box.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);
                es.RemoveEntity(box);
            }
            else
            {
                Engine.Utility.Log.Error("找不到box id{0}", cmd.qwThisID);
            }
        }
    }

    [Execute]
    public void OnRemoveItems(GameCmd.stBatchRemoveItemMapScreenUserCmd_S cmd)
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
            IBox box = es.FindBox(cmd.id[i]);
            if (box != null)
            {
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = box.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);
                es.RemoveEntity(box);
            }
            else
            {
                Engine.Utility.Log.Error("找不到box id{0}", cmd.id[i]);
            }
        }
    }

    [Execute]
    public void SyncTeammatePos(stSynTeamPosRelationUserCmd_S cmd)
    {
        DataManager.Manager<MapDataManager>().SyncTeammatePos(cmd);
    }
    //dolua testtrace(24, 50, true) 画更新点
    [Execute]
    public void SyncTracePos(stNpcTraceMoveUserCmd_S cmd)
    {
        if(Application.isEditor)
        {

            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem(); ;
            if (mapSys == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: mapSys is null");
                return;
            }
            mapSys.TraceServerPos(cmd.add, (int)cmd.x, (int)cmd.y);
        }
    }
}

