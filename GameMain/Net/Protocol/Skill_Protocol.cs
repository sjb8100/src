using Client;
using Common;
using Engine;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine.Utility;
// 技能相关网络协议处理
partial class Protocol
{
    public class SkillHelper
    {
        // 根据ID获取技能部件
        public static ISkillPart GetSkillPart(uint uid, EntityType type)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("GetEntitySystem failed!");
                return null;
            }
            if(type == EntityType.EntityType_NPC)
            {
               IRobot robot = es.FindEntity<IRobot>(uid);
                if(robot != null)
                {
                    type = EntityType.EntityTYpe_Robot;
                }
            }
            long id = EntitySystem.EntityHelper.MakeUID(type, uid);
            IEntity player = es.FindEntity(id);
            if (player == null)
            {
                Engine.Utility.Log.Error("GetAttack failed!");
                return null;
            }

            ISkillPart skillPart = player.GetPart(EntityPart.Skill) as ISkillPart;
            return skillPart;
        }
        /// <summary>
        /// 获取主角的skillpart
        /// </summary>
        /// <returns></returns>
        public static ISkillPart GetMainPlayerSkillPart()
        {
            if (ClientGlobal.Instance().MainPlayer == null)
            {
                Log.Error("mainplayer is not exist");
                return null;
            }
            uint userID = ClientGlobal.Instance().MainPlayer.GetID();
            return GetSkillPart(userID, EntityType.EntityType_Player);
        }
    }

    /// <summary>
    /// 学习技能请求
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="nextLv">目标等级</param>
    public void LearnSkillReq(uint skillId, uint nextLv)
    {
        GameCmd.stLearnSkillUserCmd_C cmd = new GameCmd.stLearnSkillUserCmd_C();
        cmd.dwLevel = nextLv;
        cmd.dwSkillID = skillId;
        SendCmd(cmd);
    }

    /// <summary>
    /// 初始化技能位置
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnArenaSkillLocationInit(stSendUsePosListSkillUserCmd_S cmd)
    {
        DataManager.Manager<LearnSkillDataManager>().OnInitLocation(cmd);
    }
    [Execute]
    public void OnArenaSetSkillLocation(stSetUsePosSkillUserCmd_CS cmd)
    {
        DataManager.Manager<LearnSkillDataManager>().OnSetLocation(cmd);
    }

    [Execute]
    public void OnArenaSetSkillState(stSendSkillStatusSkillUserCmd_S cmd)
    {
        DataManager.Manager<LearnSkillDataManager>().OnSetSkillState(cmd);
    }

    [Execute]
    public void OnRecieveAutoAttack(stSetAutoFlagSkillUserCmd_CS cmd)
    {
        DataManager.Manager<LearnSkillDataManager>().OnAutoAttack(cmd);
    }
    /// <summary>
    /// 初始化技能
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void AddSkill(GameCmd.stAddSkillUserCmd_S cmd)
    {
        ISkillPart skillPart = SkillHelper.GetMainPlayerSkillPart();
  
        List<GameCmd.stAddSkillUserCmd_S.Item> skillList = new List<stAddSkillUserCmd_S.Item>();
        skillList = cmd.data;

        List<uint> updateSkillIds = new List<uint>();
        Dictionary<uint, SkillInfo> skillInfos = new Dictionary<uint, SkillInfo>();
        SkillInfo skillInfo = null;

       
        for (int i = 0; i < skillList.Count;i++ )
        {
            var item = skillList[i];
            uint totalid = item.id;

            ushort skillID = (ushort)(totalid >> 16);
            ushort level = (ushort)totalid;

            uint coldTime = item.cold;
            skillInfo = new SkillInfo((uint)skillID, (uint)level, coldTime);
            DataManager.Manager<LearnSkillDataManager>().InitUserSkill((uint)skillID, (uint)level, coldTime,item.hookfalg);
            DataManager.Manager<ArenaSetSkillManager>().InitUserSkill((uint)skillID, (uint)level, coldTime);
            DataManager.Manager<ClanManger>().InitUserSkill((uint)skillID, (uint)level, coldTime);
            Log.LogGroup("ZDY", "add skill {0} lev = {1}" , skillID ,level);
            if (skillPart != null)
            {
                skillPart.OnAddSkill((uint)skillID, (uint)level, coldTime);
            }
            else
            {
                Log.Error("skillpart is null");
            }


            if (!skillInfos.ContainsKey(skillID))
            {
                skillInfos.Add(skillID, skillInfo);
            }
        }

        //氏族技能
        DataManager.Manager<ClanManger>().OnPlayerSkillUpdate(skillInfos.Keys.ToList());
        DataManager.Manager<LearnSkillDataManager>().OnSkillAdd(skillInfos, (GameCmd.SkillAddType)cmd.type);
        DataManager.Manager<LearnSkillDataManager>().DispatchRedPoingMsg();
    }
    /// <summary>
    /// 技能伤害
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stMultiAttackDownMagicUserCmd_S cmd)
    {
        EntityType type = EntitySystem.EntityHelper.GetEntityEtype(cmd.byAttackerType);
        ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.dwAttackerID, type);
        if (skillPart == null)
        {
            Engine.Utility.Log.Error("获取type :{0} id:{1}技能部件失败！", type, cmd.dwAttackerID);
            return;
        }
        // 处理技能结果
        skillPart.OnDamage(cmd);
        //服务器发送打断


    }
    /// <summary>
    /// 瞬移矫正位置
    /// </summary>
    /// <param name="cmd"></param>
    /*      enumMoveType_Immediate = 0;                 // 刷新位置
            enumMoveType_Delay_0s = 1;                  // 立即高速移动, 技能(英勇冲锋)
            enumMoveType_Delay_500m = 2;                // 延迟500毫秒高速移动, 技能(巨刃跳劈)
            enumMoveType_Delay_2000m = 4;               // 不变方向高速移动, 技能(紧急撤退)
            enumMoveType_RandPos = 99;                  // 场景中随机位置
            enumMoveType_Correct = 100;                 // 修正当前位置
            enumMoveType_GoTo = 101;                    // goto用
     * */
    [Execute]
    public void Execuete(GameCmd.stReturnObjectPosMagicUserCmd_S cmd)
    {
        //冲锋应该是goto  瞬移应该是enumMoveType_Immediate  此处待后台修改 前台先山寨一下
        if (cmd.byMoveType == stReturnObjectPosMagicUserCmd_S.enumMoveType.enumMoveType_GoTo || cmd.byMoveType == stReturnObjectPosMagicUserCmd_S.enumMoveType.enumMoveType_Immediate)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("GetEntitySystem failed!");
                return;
            }

            stEntryPosition userPos = cmd.userpos;
            if (userPos != null)
            {
                IEntity player = es.FindEntity<IPlayer>(userPos.dwTempID);
                if (player != null)
                {
                    //MapVector2 mapPos = MapVector2.FromCoordinate(userPos.x, userPos.y);
                    Vector3 pos = new Vector3(userPos.cur_pos.x * 0.01f, 0, -userPos.cur_pos.y*0.01f); // 服务器到客户端坐标转换
                    ISkillPart skillPart = SkillHelper.GetSkillPart(userPos.dwTempID, EntityType.EntityType_Player);

                    if (skillPart != null)
                    {
                        if (skillPart.IsFastMove())
                        {
                            Move move = new Move();
                            move.m_target = pos;
                            move.m_ignoreStand = true;
                            player.SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);

                        }
                        else
                        {
                            //不是在释放快速移动技能 立刻设置位置 否则延迟设置

                            player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                            //Vector3 rot = GameUtil.S2CDirection(userPos.byDir);
                            //player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);

                        }
                        IEntity skillTarget = skillPart.GetSkillTarget();
                        if (skillTarget != null && skillTarget.GetUID() != player.GetUID())
                        {
                            SkillDatabase db = skillPart.GetCurSkillDataBase();
                            if (db != null)
                            {
                                if (db.targetType == (int)SkillTargetType.TargetForwardPoint)
                                {
                                    return;
                                }
                            }
                            player.SendMessage(EntityMessage.EntityCommand_LookTarget, skillTarget.GetPos());
                        }
                    }
                }
            }

            if (cmd.npcpos != null)
            {
                INPC npc = es.FindEntity<INPC>(cmd.npcpos.dwTempID);
                if (npc != null)
                {
                    //MapVector2 mapPos = MapVector2.FromCoordinate(cmd.npcpos.x, cmd.npcpos.y);
                    Vector3 pos = new Vector3(cmd.npcpos.cur_pos.x * 0.01f, 0, -cmd.npcpos.cur_pos.y*0.01f); // 服务器到客户端坐标转换
                    ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.npcpos.dwTempID, EntityType.EntityType_NPC);

                    if (skillPart != null)
                    {  //不是在释放快速移动技能 立刻设置位置 否则延迟设置
                        npc.SendMessage(EntityMessage.EntityCommand_StopMove, (object)pos);
                    }
                }
            }

            EventEngine.Instance().DispatchEvent((int)GameEventID.CAMERA_MOVE_END);
        }
    }
    /// <summary>
    ///请求打断技能
    /// </summary>
    /// <param name="cmd"></param>
    public void SendInterruptSkill()
    {
        GameCmd.stNotifyUninterruptEventMagicUserCmd_CS cmd = new GameCmd.stNotifyUninterruptEventMagicUserCmd_CS();
        cmd.etype = stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Break;
        cmd.actiontype = (int)GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ;
        cmd.desid = MainPlayerHelper.GetPlayerID();
        ClientGlobal.Instance().netService.Send(cmd);

    }
    /// <summary>
    /// 通知进度条状态
    /// </summary>
    /// <param name="cmd"></param>  // 请求攻击 stMultiAttackUpMagicUserCmd
    [Execute]
    public void Execute(GameCmd.stNotifyUninterruptMagicUserCmd_S cmd)
    {
        if (cmd.actiontype == UninterruptActionType.UninterruptActionType_SkillCJ)
        {
            ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.userid, EntityType.EntityType_Player);

            if (skillPart == null)
            {
                Engine.Utility.Log.Error("获取主角技能部件失败！");
                return;
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSBREAK,
     new stGuildBreak() { action = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ, uid = cmd.userid, skillid = skillPart.GetCurSkillDataBase().wdID });
            TimerAxis.Instance().KillTimer(m_uReadSliderTimerID, this);
            skillPart.OnInterruptSkill(cmd.userid, cmd.time, cmd.type, (uint)cmd.actiontype);
        }
        else if (cmd.actiontype == UninterruptActionType.UninterruptActionType_GOHOME)
        {
            long id = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_Player, cmd.userid);

            IEntitySystem pEntitySystem = ClientGlobal.Instance().GetEntitySystem();
            if (pEntitySystem != null)
            {
                IPlayer pPlayer = pEntitySystem.FindPlayer((uint)id);
                if (pPlayer != null)
                {
                    if (pPlayer.IsMainPlayer())
                    {
                        Client.stUninterruptMagic param = new Client.stUninterruptMagic();
                        param.time = cmd.time;
                        param.type = cmd.actiontype;
                        param.uid = id;
                        param.npcId = cmd.npcid;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSSTART, param);
                    }
                    else
                    {


                    }

                }
            }
        }
        else
        {
            long id = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_Player, cmd.userid);

            Client.stUninterruptMagic param = new Client.stUninterruptMagic();
            param.time = cmd.time;
            param.type = cmd.actiontype;
            param.uid = id;
            param.npcId = cmd.npcid;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSSTART, param);
        }
    }


    string GetLocalText(LocalTextType type)
    {
        string str = DataManager.Manager<TextManager>().GetLocalText(type);
        return str;
    }
    /// <summary>
    /// 技能使用失败返回
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void ExecuteSkillFailed(GameCmd.stMultiAttackReturnMagicUserCmd_S cmd)
    {

        ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.dwUserID, EntityType.EntityType_Player);
        if (skillPart == null)
        {
            Engine.Utility.Log.Error("获取主角技能部件失败！");
            return;
        }
        else
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSBREAK,
                new stGuildBreak() { action = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ, uid = cmd.dwUserID, skillid = cmd.wdSkillID });
            TimerAxis.Instance().KillTimer(m_uReadSliderTimerID, this);
            string str = "";// "skill error code = " + cmd.fail_code + " ";
            if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_success)
            {
                //使用成功
                
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_break)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinengbeidaduan);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_use)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinengzhengzaishiyong);
                Log.Error("使用过程 error code {0}" , cmd.fail_code);
            }

            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_outRange)
            {

                Client.IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
                if (cs != null)
                {
                    Client.ICombatRobot robot = cs.GetCombatRobot();
                    if (robot != null && robot.Status == CombatRobotStatus.RUNNING)
                    {
                        if (MainPlayerHelper.GetMainPlayer() == null)
                        {
                            return;
                        }
                        ISkillPart sp = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as ISkillPart;
                        if (sp == null)
                        {
                            return;
                        }

                        if (sp.GetSkillTarget() != null)
                        {
                            Move move = new Move();
                            Vector3 targePos = sp.GetSkillTarget().GetPos();
                            Vector3 dir = targePos - MainPlayerHelper.GetMainPlayer().GetPos();
                            targePos = targePos - dir.normalized * 1f;
                            move.m_target = targePos;
                            move.m_ignoreStand = true;
                            Log.Error("自动挂机，打不到，朝向目标移动");

                            IController ctr = cs.GetActiveCtrl();
                            if(ctr != null)
                            {
                                ctr.MoveToTarget(targePos);
                            }
                            //MainPlayerHelper.GetMainPlayer().SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);
                            // Vector3 lookat = playerSkill.GetSkillTarget().GetNode().GetTransForm().forward;
                            return;
                        }
                    }

                }
              
                str = GetLocalText(LocalTextType.Skill_Commond_mubiaochaochugongjijuli);
                Log.Error("技能使用超超出范围 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_shapeErr)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_bianshenzhuangtaixiabunengshiyongjineng);
                Log.Error("变身不能使用技能 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_petFight)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_chongwuweichuzhanbunengshiyongjineng);
                Log.Error("宠物未出战不能使用技能 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_notExist)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinengbucunzai);
                Log.Error("技能不存在 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_none)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinengweizhicuowu);
                Log.Error("技能未知 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_needAim)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_shifanggaijinengxuyaoxuanzemubiao);
                Log.Error("技能需要目标error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_aimErr)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_mubiaoxuanzecuowu);
                Log.Error("技能目标错误 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_cantAtt)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_dangqianmubiaowufagongji);
                Log.Error("目标无法攻击 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_inCD)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinenglengquezhong);
                Log.Error("技能冷却中 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_lackSP)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_fashuzhibuzu);
                Log.Error("缺蓝 error code {0}", cmd.fail_code);
            }
            else if (cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_flagsErr)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_dangqianjinengzhuangtaicuowu);
                Log.Error("flags error code {0}", cmd.fail_code);
            }
            else if(cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_NeedItemNumNotEnough)
            {
                str = GetLocalText(LocalTextType.Skill_Commond_shiyongchongwujinnegsuoxudaojubuzu);
                Log.Error("使用宠物技能所需道具不足{0}", cmd.fail_code);
            }
            else if(cmd.fail_code == (int)GameCmd.UseSkillFail.UseSkillFail_rideCantUse)
            {
                NetService.Instance.Send(new GameCmd.stDownRideUserCmd_C() { });
                return;
                //str = GetLocalText(LocalTextType.Skill_Commond_qichengzhuangtaiwufashiyongzhujuejineng);
                //Log.Error("骑乘无法使用技能{0}", cmd.fail_code);
            }
            else if (cmd.fail_code == 16)
            {//协议未更新 先写数字防止后期忘了
                str = "眩晕状态，技能不能使用";// GetLocalText(LocalTextType.Skill_Commond_qichengzhuangtaiwufashiyongzhujuejineng);
                Log.Error("瞬移技能不能使用{0}", cmd.fail_code);
            }
            else
            {
                str = GetLocalText(LocalTextType.Skill_Commond_jinengweizhicuowu);
                Log.Error("技能未知错误 ");
            }
            if(cmd.dwUserID == MainPlayerHelper.GetPlayerID())
            {
                TipsManager.Instance.ShowTips(str);
            }
        }

        skillPart.OnUseSkillFailed(cmd.dwUserID, cmd.wdSkillID);
    }


    /// <summary>
    /// 开始释放技能
    /// </summary>
    /// <param name="cmd"></param>  // 请求攻击 stMultiAttackUpMagicUserCmd
    [Execute]
    public void PrepareUseSkill(GameCmd.stPrepareUseSkillSkillUserCmd_S cmd)
    {
        EntityType type = EntitySystem.EntityHelper.GetEntityEtype(cmd.usertype);
        ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.userid, type);
        if (skillPart == null)
        {
            Engine.Utility.Log.Error("获取技能部件失败！" + cmd.userid + " type is " + Enum.GetName(typeof(EntityType), type));
            return;
        }
        if (cmd.level == 0)
        {
            cmd.level = 1;
        }
        SkillDatabase database = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(cmd.skillid, (int)cmd.level);
        if (database == null)
        {
            Engine.Utility.Log.Error(" skill database is null skillid is " + cmd.skillid.ToString() + " level is " + cmd.level.ToString());
            return;
        }
        skillPart.OnPrepareUseSkill(cmd);


        if (type == EntityType.EntityType_Player)
        {
            uint time = (database.dwReadTime);
            if (time > 0)
            {
                TimerAxis.Instance().SetTimer(m_uReadSliderTimerID, time, this, 1);
            }
        }
        else if (type == EntityType.EntityType_NPC)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("GetEntitySystem failed!");
                return;
            }

            INPC npc = es.FindEntity<INPC>(cmd.userid);
            if (npc != null)
            {
                int masterID = npc.GetProp((int)NPCProp.Masterid);
                if (masterID == MainPlayerHelper.GetPlayerID())
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)cmd.skillid);
                    if (db != null)
                    {
                        if (db.petType == 1 || db.petType == 2)
                        {
                            Client.stSkillCDChange st = new Client.stSkillCDChange();
                            st.skillid = (uint)cmd.skillid;
                            st.cd = -1;
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLCD_BEGIN, st);
                            TipsManager.Instance.ShowTips(npc.GetName() +CommonData.GetLocalString("使用") + db.strName);
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 刷新当前进度条新时间
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stChangeUninterruptTimeMagicUserCmd_S cmd)
    {

    }
    /// <summary>
    /// 结束进度条 只有主角有(采集任务什么的有进度条)
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stNotifyUninterruptEventMagicUserCmd_CS cmd)
    {
        GameCmd.UninterruptActionType action = (UninterruptActionType)cmd.actiontype;
        if (action == UninterruptActionType.UninterruptActionType_SkillCJ)
        {
            ISkillPart skillPart = SkillHelper.GetSkillPart(cmd.desid, EntityType.EntityType_Player);
            if (skillPart == null)
            {
                Engine.Utility.Log.Error("获取技能部件失败！");
                return;
            }
            skillPart.OnInterruptEventSkill((uint)cmd.etype);
        }
        else
        {
            if (cmd.etype == stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Break)
            {
                if (ClientGlobal.Instance().IsMainPlayer(cmd.desid) && action == UninterruptActionType.UninterruptActionType_CampCJ)
                {
                    TipsManager.Instance.ShowTips("采集被打断了");
                }

                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSBREAK,
                    new stGuildBreak() { action = action, uid = cmd.desid ,npcId = cmd.npcid });
            }
            else if (cmd.etype == stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Over)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSEND,
                    new stGuildEnd() { action = action, uid = cmd.desid, npcId = cmd.npcid});

                if (action == UninterruptActionType.UninterruptActionType_CampCJ)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.CAMP_ADDCOLLECTNPC,
                    new stCampCollectNpc() { enter = false });
                }
            }
        }
    }

    /// <summary>
    /// 快捷使用道具
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAllShortCutItemList(GameCmd.stSendAllShortCutPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<SettingManager>().OnAllShortCutItemList(cmd);
    }
    /// <summary>
    /// 嘲讽切换目标
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnChangeTarget(stTauntChangeTargetMagicUserCmd_S cmd)
    {
        IEntity en = EntitySystem.EntityHelper.GetEntity(cmd.target_type, cmd.target_id);
        if (en != null)
        {
            ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().UpdateTarget(en);
            //Client.stTargetChange targetChange = new Client.stTargetChange();
            //targetChange.target = en;
            //Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);
        }
    }

    [Execute]
    public void OnReceiveCD(stSendSkillCDMagicUserCmd_S cmd)
    {
        stSkillCDChange st = new stSkillCDChange();
        st.skillid = cmd.skillid;
        st.cd =(int) cmd.cd;
        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLCD_BEGIN, st);
    }

    [Execute]
    public void OnReceiveAutoFightInfo(stSetSkillHookStatusMagicUserCmd_CS cmd)
    {
        DataManager.Manager<LearnSkillDataManager>().OnReceiveAutoFightInfo(cmd);
    }
}
