using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Engine.Utility;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;
using table;
using UnityEngine.Profiling;

partial class Protocol
{
    /// <summary>
    /// 增加一个npc
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [Execute]
    public void Execute(GameCmd.stAddMapNpcAndPosMapScreenUserCmd_S cmd)
    {
        //AddNPC(cmd.data);
        Profiler.BeginSample("stAddMapNpcAndPosMapScreenUserCmd_S");
        EntityCreator.Instance().AddNPC(cmd.data, cmd.master_id);
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }
        uint userUID = ClientGlobal.Instance().MainPlayer.GetID();
        if (userUID == cmd.master_id && cmd.pet_id != 0)
        {
            DataManager.Manager<PetDataManager>().AddPetToNpc(cmd.pet_id, cmd.data.mapnpcdata.npcdata.dwTempID);
        }

        GameCmd.stNpcBelongListMapScreenUserCmd_S belongcmd = new GameCmd.stNpcBelongListMapScreenUserCmd_S();
        belongcmd.teamid = cmd.data.mapnpcdata.npcdata.owernteamid;
        belongcmd.userid = cmd.master_id;
        belongcmd.npcid = cmd.data.mapnpcdata.npcdata.dwTempID;
        Execute(belongcmd);
        RoleStateBarManager.RefreshAllRoleBarHeadStatus(HeadStatusType.Hp);
        Profiler.EndSample();
    }
    [Execute]
    public void AddNpcEffect(stAddMapEffectMapScreenUserCmd_S cmd)
    {
        Vector3 pos = new Vector3(cmd.cur_pos.x/100, 0, -cmd.cur_pos.y/100);
        CloseTerrainPos(ref pos);
        AddEffect(pos, cmd.spe_effect);
    }
    void AddEffect(Vector3 pos, uint effectID)
    {
        FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(effectID);
        if (edb != null)
        {
            // 使用资源配置表
            ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.resPath);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                return;
            }
            string path = resDB.strPath;


            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }
            IEffect effect = null;
            rs.CreateEffect(ref path, ref effect, (obj) =>
            {
                if (obj == null)
                {
                    Log.Error("fx load failed: " + path);
                    return;
                }
                if (obj.GetNodeTransForm() == null)
                {
                    return;
                }
                obj.GetNodeTransForm().localPosition = pos;
                obj.GetNodeTransForm().localRotation = Quaternion.identity;
                obj.GetNodeTransForm().localScale = Vector3.one;
                //DoPlay(obj.GetNodeTransForm().gameObject, position, rotation, scale);
            });

        }
    }
    private void CloseTerrainPos(ref Vector3 curPos)
    {
        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            return;
        }

        // 再计算移动速度
        IScene m_curScene = rs.GetActiveScene();
        if (m_curScene == null)
        {
            return;
        }

        //和周围墙做碰撞修正
        // 计算得到新位置，还需要做一次碰撞检测，看是否可以行走
        //Ray rayLine = new Ray(m_vLastPos, pos - m_vLastPos);
        //Engine.ColliderInfo colliderInfo = new Engine.ColliderInfo();
        //float fDis = Vector3.Distance(pos, m_vLastPos);
        //if (m_curScene.GetColliderInfo(ref rayLine, ref colliderInfo))
        //{
        //    //Engine.Utility.Log.Trace("Dis:{0}", colliderInfo.distance);
        //    if (colliderInfo.distance < 1.5f) // 应当是玩家的半径
        //    {
        //        //                     if (colliderInfo.distance < fDis)
        //        //                     {
        //        //                         pos = m_vLastPos + Vector3.Normalize(m_vSpeed) * colliderInfo.distance;
        //        //                     }
        //        //                     else
        //        //                     {
        //        //            m_jumpSpeed =  Vector3.up * _jumpUp;
        //        //             m_vSpeed = m_jumpSpeed ;
        //        m_vSpeed = m_vSpeed - Vector3.Dot(m_vSpeed, colliderInfo.normal) * colliderInfo.normal;
        //        pos = m_vLastPos + m_vSpeed * Time.deltaTime;
        //        /*                    }*/
        //    }
        //}

        Engine.TerrainInfo info;
        if (m_curScene.GetTerrainInfo(ref curPos, out info))
        {
            //斜坡速度衰减.... 去掉
            //Vector3 right = Vector3.Cross(info.normal, m_dir);
            //Vector3 speed = Vector3.Cross(right, info.normal);
            //float cos = Vector3.Dot(m_dir, speed);

            //m_fSpeedTerrainFact = cos;
        }

        curPos.y = info.pos.y;
    }
    // NPC移动
    [Execute]
    public void Execute(GameCmd.stNpcMoveMoveUserCmd cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        MapVector2 mapPos = MapVector2.FromCoordinate(cmd.dst_x, cmd.dst_y);
        Vector3 pos = new Vector3(mapPos.x, 0, -mapPos.y); // 服务器到客户端坐标转换
        IEntity en = null;
        en = es.FindNPC(cmd.dwNpcTempID);
        if (en == null)
        {
            //Engine.Utility.Log.Info("机器人 移动pos {0}", pos);
            en = es.FindRobot(cmd.dwNpcTempID);
        }
        if (en != null)
        {
            Engine.Utility.Log.LogGroup("XXF","{0}Move to{1},{2}",en.GetName(), pos.x,pos.z);

            Move move = new Move();
            //move.m_dir = Global.S2CDirection(cmd.dir);
            move.strRunAct = EntityAction.Run; // 动作名需要统一处理
            move.m_target = pos;
            move.m_speed = en.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE;
            // Log.Error( "npc pos is" + pos.ToString() );
            en.SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);
        }
    }

    //[Execute]
    //public static void Excute(stNpcCommonChatCmd cmd)
    //{
    //    //var npc = Npc.All[cmd.dwOPDes];
    //    //while (npc == null)
    //    //{
    //    //    yield return 0;
    //    //}

    //    //ShowSpeak(npc, cmd.szInfo);
    //}

    /// <summary>
    /// 在地图上删除NPC
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [Execute]
    public void Excute(GameCmd.stRemoveMapNpcMapScreenUserCmd_S cmd)
    {
        Profiler.BeginSample("stRemoveMapNpcMapScreenUserCmd_S");
        //RemoveNpc(cmd.dwTempID);
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        {
            EntityCreator.Instance().RemoveNPC(cmd.dwTempID);
            //Engine.Utility.Log.LogGroup( "ZDY" , "single remove entity ----------------" + cmd.dwTempID );
            //PetDataManager petData = DataManager.Manager<PetDataManager>();
            //if(petData.NpcIsPet(cmd.dwTempID))
            //{
            //    petData.OnPetDead( cmd.dwTempID );
            //}
            IEntity npc = es.FindNPC(cmd.dwTempID);
            if (npc == null)
            {
                npc = es.FindRobot(cmd.dwTempID);
            }
            if (npc != null)
            {
                long uid = npc.GetUID();

                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = uid;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);

                NpcAscription.Instance.OnRemoveBelongData(uid);
            }
            es.RemoveEntity(npc);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// 批量删除npc
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(GameCmd.stBatchRemoveNpcMapScreenUserCmd_S cmd)
    {
        Profiler.BeginSample("stBatchRemoveNpcMapScreenUserCmd_S");
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }
        for (int i = 0; i < cmd.id.Count; ++i)
        {
            EntityCreator.Instance().RemoveNPC(cmd.id[i]);
            //  Engine.Utility.Log.LogGroup( "ZDY" , "stBatchRemoveNpcMapScreenUserCmd_S remove entity ----------------" + cmd.id[i] );
            IEntity npc = es.FindNPC(cmd.id[i]);
            if (npc == null)
            {
                npc = es.FindRobot(cmd.id[i]);
            }

            if (npc != null)
            {
                long uid = npc.GetUID();
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = uid;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);

                NpcAscription.Instance.OnRemoveBelongData(uid);
            }

            es.RemoveEntity(npc);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// NPC附加属性消息
    /// </summary>	
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(GameCmd.stNpcAddDataMapScreenUserCmd_S cmd)
    {
        //var npc = Npc.All[cmd.id];
        //if (npc == null) return;
        //if (npc.TableInfo.dwType == 8)//宠物
        //{
        //    var role = Role.All[cmd.ownerid];
        //    if (role)
        //    {
        //        role.FollowingDemon = npc;
        //        role.SendRequestForOtherDemon();
        //        //Debug.LogError(string.Format("Get demon owner role {0} in scene!!!",role.ServerInfo.dwUserID));
        //    }
        //    else
        //    {
        //        //Debug.LogError("Can't get demon owner!");
        //    }
        //}
        //npc.additionDataType = (enumAdditonDataType)cmd.type;
        //switch (npc.additionDataType)
        //{
        //    case enumAdditonDataType.AddDataType_Turret: ///< 召唤类攻击型NPC
        //        {
        //            //var addData = cmd.data as t_AddData_Turrent;
        //            //if (addData == null) break;

        //            npc.MasterID = cmd.ownerid;
        //        }
        //        break;
        //    default:
        //        Debug.LogWarning("未处理的NPC附加属性：" + npc.additionDataType);
        //        break;
        //}
    }

    /// <summary>
    /// NPC第一次进入场景消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Excute(GameCmd.stFirstNpcIntoSceneMapScreenUserCmd_S cmd)
    {
        Log.Info(string.Format("NPC第一次进入场景消息，可控制播放出生特效，暂时用不到 \n{0}", cmd.Dump()));
    }
    [Execute]
    public void Execute(GameCmd.stEnmityDataUserCmd_S cmd)
    {
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eRefreshEnemyList, cmd);
    }

    [Execute]//Npc归属人列表返回信息
    public void Execute(GameCmd.stNpcBelongListMapScreenUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        INPC npc = es.FindNPC(cmd.npcid);
        if (npc != null)
        {
            npc.OwnerID = cmd.userid;
            npc.TeamID = cmd.teamid;
            bool belongOther = false;
            if (cmd.userid != 0)
            {
                if (cmd.userid != ClientGlobal.Instance().MainPlayer.GetID())
                {
                    belongOther = true;
                }
                else
                {
                    npc.BelongMe = true;
                }
            }
            if (!belongOther &&( cmd.teamid != 0 || cmd.clanid != 0))
            {
                if (DataManager.Manager<TeamDataManager>().IsJoinTeam == true)
                {
                    if (DataManager.Manager<TeamDataManager>().TeamId != cmd.teamid)
                    {
                        belongOther = true;
                    }
                }
                else if (DataManager.Manager<ClanManger>().IsJoinClan == true)
                {
                    if (DataManager.Manager<ClanManger>().ClanId != cmd.clanid)
                    {
                        belongOther = true;
                    }
                }
                else
                {
                    belongOther = true;
                }
            }

            npc.BelongOther = belongOther;
            stEntityChangename e = new stEntityChangename();
            e.uid = npc.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, e);

            stRefreshNPCBelongParam param = new stRefreshNPCBelongParam()
            {
                npcid = npc.GetUID(),
                teamid = cmd.teamid,
                ownerid = cmd.userid,
                clanid = cmd.clanid,
                ownerName = cmd.ownername,
            };
            NpcAscription.Instance.OnBelongChanged(param);
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eRefreshNpcBelong, param);

        }
    }

    [Execute]
    public void OnBossTalk(stBossSpeakScriptUserCmd_S cmd)
    {
        DataManager.Manager<RoleStateBarManager>().SetTalkingBossID(cmd.npc_id);
    }
}