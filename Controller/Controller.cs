using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using System.Collections;
using Engine;
using Engine.Utility;
using table;
namespace Controller
{

    // 处理屏幕点击事件
    class Controller : IController
    {
        private IClientGlobal m_ClientGlobal = null;

        //控制器 开关;
        public bool isOpen = true;

        // 触摸按下
        private bool m_bTouchDown = false;

        // 设置控制对象
        private IEntity m_Host = null;
        // 点击回调
        private IClickSink m_ClickSink = null;

        // 前身预测最大距离
        private const int FORWARD_MAXDIS = 10;
        // 角度缓冲范围
        private const float ANGLE_RANGLE = 60.0f;
        // 当前目标
        private IEntity m_curTarget = null;
        private IEntity m_preTarget = null;
        int m_effctId = -1;//选中特效（脚底光圈）
        IEffect m_ClickEffct = null;
        const uint RED_EFFECT_ID = 7;
        const uint GREEN_EFFECT_ID = 8;

        bool m_bLongPress = false;
        int m_nTipsCount = 0;

        bool m_bJoyStickPress = false;
        KeyCode m_downKey;
        public Controller(Client.IClientGlobal clientGlobal)
        {
            m_ClientGlobal = clientGlobal;
            isOpen = true;

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_BEGINRIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKLL_LONGPRESS, OnContrllerEvent);

            List<KeyCode> keyList = new List<KeyCode>();
            foreach (var item in Enum.GetNames(typeof(KeyCode)))
            {
                KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), item);
                keyList.Add(key);
            }


            RareEngine.Instance().RegisterKey(ref keyList);

            CmdManager.Instance().Create();

            MoveController.Instance.Init(clientGlobal);

        }

        // 游戏退出时使用
        public void Release()
        {
            if (m_ClickEffct != null)
            {
                Engine.RareEngine.Instance().GetRenderSystem().RemoveEffect(m_ClickEffct);
                m_ClickEffct = null;
            }
            CmdManager.Instance().Close();

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_BEGINRIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnContrllerEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKLL_LONGPRESS, OnContrllerEvent);
        }

        public void OnContrllerEvent(int nEventID, object param)
        {
            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY)
            {
                stRemoveEntity re = (stRemoveEntity)param;
                if (m_curTarget == null || m_curTarget.GetUID() == re.uid)
                {
                    UpdateTarget(null);
                }
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_TARGETCHANGE)
            {
                Client.stTargetChange targetChange = (Client.stTargetChange)param;

                m_curTarget = targetChange.target;
                AddCircleEffect(m_curTarget);
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_BEGINRIDE) // 取消选中特效
            {
                if (m_curTarget != null)
                {
                    stEntityBeginRide beginRide = (stEntityBeginRide)param;

                    if (m_curTarget.GetUID() == beginRide.uid)
                    {
                        m_curTarget.SendMessage(EntityMessage.EntityCommand_RemoveLinkEffect, m_effctId);
                    }
                }
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_RIDE) // 上马选中特效
            {
                if (m_curTarget != null)
                {
                    stEntityRide beginRide = (stEntityRide)param;

                    if (m_curTarget.GetUID() == beginRide.uid)
                    {
                        AddCircleEffect(m_curTarget);
                    }
                }
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_UNRIDE) // 下马选中特效
            {
                if (m_curTarget != null)
                {
                    stEntityUnRide beginRide = (stEntityUnRide)param;

                    if (m_curTarget.GetUID() == beginRide.uid)
                    {
                        AddCircleEffect(m_curTarget);
                    }
                }
            }
            else if (nEventID == (int)Client.GameEventID.SKLL_LONGPRESS)
            {
                stSkillLongPress press = (stSkillLongPress)param;

                m_bLongPress = press.bLongPress;
                if (!m_bLongPress)
                {
                    m_nTipsCount = 0;
                }

            }
        }

        public void SetHost(IEntity entity)
        {
            m_Host = entity;
            MoveController.Instance.SetHost(entity);
        }

        // 设置点击回调
        public void SetClickSink(IClickSink sink)
        {
            m_ClickSink = sink;
        }

        public void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }

            switch (code)
            {
                case MessageCode.MessageCode_Begin:
                    {
                        m_bTouchDown = true;
                        break;
                    }
                case MessageCode.MessageCode_End:    // 屏幕点击事件 非UI区域
                    {
                        OnTouchEnd(param1, param2, param3);
                        break;
                    }
                case MessageCode.MessageCode_JoystickChanging:
                    {
                        m_bJoyStickPress = true;
                        OnJoystick(param1, param2, param3);
                        break;
                    }
                case MessageCode.MessageCode_JoystickEnd:
                    {
                        m_bJoyStickPress = false;
                        OnJoystickEnd(param1, param2, param3);
                        break;
                    }
                case MessageCode.MessageCode_Key:
                    {
                        KeyCode key = (KeyCode)param1;
                        float fRotateY = 0;
                        IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
                        if (mapSys != null)
                        {
                            MapDataBase mapdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapSys.GetMapID());
                            if (mapdb != null)
                            {
                                fRotateY = mapdb.RotateY;
                            }
                        }

                        if (key == KeyCode.A)
                        {
                            OnJoystick(-90f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.S)
                        {
                            OnJoystick(-180f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.W)
                        {
                            OnJoystick(0 + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.D)
                        {
                            OnJoystick(90f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.Y)
                        {

                        }
                        else if (key == KeyCode.U)
                        {

                        }
                        break;
                    }
                case MessageCode.MessageCode_KeyDown:
                    {
                        KeyCode key = (KeyCode)param1;
                        float fRotateY = 0;
                        IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
                        if (mapSys != null)
                        {
                            MapDataBase mapdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapSys.GetMapID());
                            if (mapdb != null)
                            {
                                fRotateY = mapdb.RotateY;
                            }
                        }
                        if (key == KeyCode.A)
                        {
                            OnJoystick(-90f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.S)
                        {
                            OnJoystick(-180f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.W)
                        {
                            OnJoystick(0 + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        else if (key == KeyCode.D)
                        {
                            OnJoystick(90f + fRotateY, param2, param3);
                            m_downKey = key;
                        }
                        List<table.HotKeyDataBase> hotKeyList = GameTableManager.Instance.GetTableList<HotKeyDataBase>();
                        stHotKeyDown hd = new stHotKeyDown();
                        for (int i = 0; i < hotKeyList.Count; i++)
                        {
                            HotKeyDataBase hkdb = hotKeyList[i];
                            if (hkdb.hotkey == key.ToString())
                            {
                                hd.type = hkdb.moduleType;
                                hd.pos = hkdb.btnpos;
                                break;
                            }
                        }
                        EventEngine.Instance().DispatchEvent((int)GameEventID.HOTKEYPRESSDOWN, hd);
                        break;
                    }
                case MessageCode.MessageCode_KeyUp:
                    {
                        KeyCode key = (KeyCode)param1;
                        if (key == m_downKey)
                        {
                            OnJoystickEnd(param1, param2, param3);
                        }
                        break;
                    }
                case MessageCode.MessageCode_ButtonX:
                    {
                        OnSkillButton(code, param1, param2, param3);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取当前目标对象
        public IEntity GetCurTarget()
        {
            return m_curTarget;
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 移动到场景指定目标点(不支持跨同一地图)
        @param vTarget 目标位置
        */
        public bool MoveToTarget(Vector3 vTarget, object moveParam = null, bool showTip = false)
        {
            if (m_bLongPress)
            {
                stSkillLongPress longPress = new stSkillLongPress();
                longPress.bLongPress = false;
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
            }
            //Engine.Utility.Log.Info("MoveToTarget {0}", vTarget);
            bool ret = MoveController.Instance.MoveToTarget(vTarget, moveParam);
            if (ret && showTip)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, true);
            }
            return ret;
        }

        // 移动地图上指定点(支持跨地图)
        public void GotoMap(uint uMapID, Vector3 vTarget)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }

            // 添加跨地图指令
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, true);
            CmdManager.Instance().AddCmd(Cmd.GotoMap, player.GetUID(), (long)uMapID, vTarget.x, vTarget.z);
        }

        public void GotoMapDirectly(uint uMapID, Vector3 vTarget, uint taskId)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }

            // 添加跨地图指令(直接跨地图)
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, true);
            CmdManager.Instance().AddCmd(Cmd.GotoMapDirecte, player.GetUID(), (long)uMapID, taskId, vTarget.x, vTarget.z);
        }

        // 移动到NPC并访问(支持跨地图)
        public void VisiteNPC(uint uMapID, uint npcid, bool bSearch)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }
            // 添加跨地图指令
            if (bSearch == false)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, bSearch);
            }
            CmdManager.Instance().AddCmd(Cmd.GotoNPC, player.GetUID(), (long)uMapID, npcid, null, null);

        }
        public void VisiteNPC(uint uMapID, uint npcid)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, true);

            CmdManager.Instance().AddCmd(Cmd.GotoNPC, player.GetUID(), (long)uMapID, npcid, null, null);
        }
        public void VisiteNPC(long npcUID)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }
            // 添加跨地图指令
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, true);
            CmdManager.Instance().AddCmd(Cmd.GotoNPCDirecte, player.GetUID(), npcUID, null, null, null);
        }

        public void VisiteNPCDirectly(uint uMapID, uint npcid, uint taskid, uint x, uint y)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }
            CmdManager.Instance().AddCmd(Cmd.VisiteNpcDirecte, player.GetUID(), (long)uMapID, npcid, taskid, new UnityEngine.Vector2(x, y));
        }
        //跳转到该地图某个地点VTrans   寻路到目标地点VTar
        public void GoToMapPosition(uint uMapID, uint qwThisID, Vector3 vTarget)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }
            CmdManager.Instance().AddCmd(Cmd.GoToMapPosition, player.GetUID(), (long)uMapID, vTarget.x, vTarget.z, qwThisID);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /**
        @brief Button按钮处理
        @param 
        */
        private void OnSkillButton(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null)
        {

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLBUTTON_CLICK, null);
            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            if (mapSys == null)
            {
                return;
            }

            // 查找怪物
            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return;
            }

            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                return;
            }
            ISkillPart skillPart = player.GetPart(EntityPart.Skill) as ISkillPart;
            if (skillPart == null)
            {
                return;
            }
            // 获取主角技能栏上的技能ID
            uint uSkillID = (uint)param1;

            bool canUse = skillPart.IsSkillCanUse(uSkillID);
            if (!canUse)
            {
                Client.stTipsEvent en = skillPart.GetSkillNotUseReason(uSkillID);
                EventEngine.Instance().DispatchEvent((int)GameEventID.TIPS_EVENT, en);
                Log.LogGroup("ZDY", "当前不能使用技能 {0} err {1} {2}", uSkillID, en.errorID, en.tips);
                //Log.Trace( "当前状态不能使用技能" );
                return;
            }
            Vector3 targetPos = player.GetPos();


            // 查找最近的目标
            Client.IEntity target = GetCurTarget();
            //if ( target == null || target.IsDead() )//死亡的忽略
            //{
            //    IMapSystem ms = ControllerSystem.m_ClientGlobal.GetMapSystem();
            //    if ( ms == null )
            //    {
            //        return;
            //    }
            //    MapAreaType atype = ms.GetAreaTypeByPos( m_ClientGlobal.MainPlayer.GetPos() );
            //    PLAYERPKMODEL pkmodel = (PLAYERPKMODEL)m_ClientGlobal.MainPlayer.GetProp( (int)PlayerProp.PkMode );
            //    target = es.FindEntityByArea_PkModel( atype , pkmodel , player.GetPos() , 100000 );
            //    if ( target != null )
            //    {
            //        UpdateTarget( target );
            //    }
            //}

            Vector3 pos = Vector2.zero;
            int skillerror = 0;
            bool bCanUse = FindTargetBySkillID(uSkillID, ref pos, ref target, out skillerror);
            if (bCanUse)
            {
                if (target == null)
                {
                    CmdManager.Instance().AddCmd(Cmd.Attack, player.GetUID(), 0, (object)uSkillID, null, targetPos);
                }
                else
                {
                    table.SkillDatabase skillData = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(uSkillID);
                    if (skillData == null)
                    {
                        Engine.Utility.Log.Error("DoAttack: 没有找到技能{0}配置", uSkillID);
                        return;
                    }
                    if (skillData.dwAttackDis > 0 && skillData.dwAttackDis < Vector3.Distance(player.GetPos(), targetPos))
                    {
                        if (m_bJoyStickPress)
                        {
                            stSkillLongPress longPress = new stSkillLongPress();
                            longPress.bLongPress = false;
                            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);

                            Engine.Utility.Log.LogGroup("ZDY", "摇杆按下 不去寻怪");
                            return;
                        }
                    }
                    CmdManager.Instance().AddCmd(Cmd.Attack, player.GetUID(), target.GetUID(), (object)uSkillID);
                }
            }
            else
            {



                if (skillerror != 0)
                {

                    if (m_nTipsCount == 0)
                    {
                        m_nTipsCount++;
                        Client.stTipsEvent en = new Client.stTipsEvent();
                        en.errorID = (uint)skillerror;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.TIPS_EVENT, en);
                        stSkillLongPress longPress = new stSkillLongPress();
                        longPress.bLongPress = false;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
                    }


                }
                else
                {
                    Log.Error("不能使用技能 skillerror is 0 查上面的logerror");
                }

            }


        }
        /// <summary>
        ///目标可以选 返回ture
        /// </summary>
        /// <returns></returns>
        bool IsTargetCanSelect()
        {
            if (m_curTarget != null)
            {
                if (!m_curTarget.IsDead())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 根据技能id找目标
        /// </summary>
        /// <param name="skillID">技能id</param>
        /// <param name="pos">发送给服务器的位置</param>
        /// <param name="en">找到的目标</param>
        /// <returns>true表示可以使用技能</returns>
        public bool FindTargetBySkillID(uint skillID, ref Vector3 pos, ref IEntity en, out int skillError)
        {
           
            bool bCanUse = false;
            pos = Vector3.zero;
            skillError = 0;
            IPlayer player = m_ClientGlobal.MainPlayer;
            uint findRange = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");
            table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillID, 1);
            if (db != null)
            {
                if (db.targetType == (int)SkillTargetType.Target)
                {
                    en = FindTargetByTarget(db, ref pos, out skillError);
                    if (en == null)
                    {
                        bCanUse = false;
                    }
                    else
                    {
                        pos = en.GetPos();
                        bCanUse = true;
                    }
                }
                else if (db.targetType == (int)SkillTargetType.TargetPoint)
                {
                    en = FindTargetByPoint(db, ref pos);
                    if (pos == Vector3.zero)
                    {
                        pos = player.GetPos();
                    }
                    bCanUse = true;
                }
                else if (db.targetType == (int)SkillTargetType.TargetForwardPoint)
                {
                    en = FindTargetByForwardPoint(db, ref pos);
                    if (pos == Vector3.zero)
                    {
                        pos = player.GetPos();
                    }
                    bCanUse = true;
                }
                else if (db.targetType == (int)SkillTargetType.MyPoint)
                {
                    en = FindTargetByTarget(db, ref pos, out skillError);
                    pos = player.GetPos();
                    bCanUse = true;
                }
            }

            if (en == null && !bCanUse)
            {
                //stTipsEvent tipsen = new stTipsEvent();
                //tipsen.errorID = (int)LocalTextType.Skill_Commond_shifanggaijinengxuyaoxuanzemubiao; 
                //  tipsen.tips = "找不到目标，不能使用技能" + skillID;


                IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
                if (cs != null)
                {
                    ICombatRobot combot = cs.GetCombatRobot();
                    if (combot.Status != CombatRobotStatus.STOP)
                    {
                        // combot.AddTips(string.Format("找不到目标，不能使用技能 {0}", skillID));
                    }
                    else
                    {
                        // EventEngine.Instance().DispatchEvent((int)GameEventID.TIPS_EVENT, tipsen);
                    }
                }
            }
            //if (bCanUse && MoveController.Instance.IsJoyStickState())
            //{
            //    if (player.GetCurState() == CreatureState.Move)
            //    {
            //        player.SendMessage(EntityMessage.EntityCommand_StopMove, player.GetPos());
            //    }

            //    stForbiddenJoystick info = new stForbiddenJoystick();
            //    info.playerID = player.GetUID();
            //    info.bFobidden = true;
            //    EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_FORBIDDENJOYSTICK, info);
            //}
            return bCanUse;
        }

        Client.IEntity FindTargetByForwardPoint(table.SkillDatabase db, ref Vector3 pos)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            Client.IEntity en = null;
            if (db.targetParam == (int)SkillTargetObjType.Self)
            {
                if (IsTargetCanSelect())
                {
                    pos = GetSelfForwardPos(db);
                    en = player;
                }
                else
                {
                    en = player;
                    UpdateTarget(en);
                    pos = GetSelfForwardPos(db);
                }
            }
            else
            {
                Log.Error("配置出粗");
            }
            return en;
        }
        Client.IEntity FindTargetByPoint(table.SkillDatabase db, ref Vector3 pos)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            Client.IEntity en = null;
            if (db.targetParam == (int)SkillTargetObjType.Self)
            {
                if (!IsCurTargetSelf())
                {
                    en = player;
                    UpdateTarget(en);
                }
                pos = en.GetPos();
            }
            else if (db.targetParam == (int)SkillTargetObjType.Friend)
            {
                if (IsCurTargetFrendly())
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindNearFriends();
                    if (en == null)
                    {
                        en = player;
                    }
                    UpdateTarget(en);
                }
                pos = en.GetPos();
            }
            else if (db.targetParam == (int)SkillTargetObjType.Enemy)
            {
                int skillerror = 0;
                if (IsCurTargetEnemy(out skillerror))
                {
                    en = m_curTarget;
                    pos = en.GetPos();
                }
                else
                {
                    en = FindNearEnemy();
                    if (en == null)
                    {
                        en = player;
                        pos = GetSelfForwardPos(db);
                    }
                    else
                    {
                        UpdateTarget(en);
                        pos = en.GetPos();
                    }

                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Pet)
            {
                if (IsTargetCanSelect())
                {
                    en = m_curTarget;
                    IEntity pet = GetMyPetNpc();
                    if (pet != null)
                    {
                        pos = pet.GetPos();
                    }

                }
                else
                {
                    en = GetMyPetNpc();
                    if (en != null)
                    {
                        pos = en.GetPos();
                        UpdateTarget(en);
                    }
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Summoned)
            {
                en = FindNearSummoned();
                if (en != null)
                {
                    pos = en.GetPos();
                }
                if (IsTargetCanSelect())
                {
                    UpdateTarget(en);
                }

            }
            else if (db.targetParam == (int)SkillTargetObjType.All)
            {
                if (IsTargetCanSelect())
                {
                    en = m_curTarget;
                }
                else
                {
                    IEntitySystem es = m_ClientGlobal.GetEntitySystem();
                    if (es != null)
                    {
                        en = es.FindNearstEntity<ICreature>();
                        if (en == null)
                        {
                            en = player;
                        }
                        UpdateTarget(en);
                    }
                }
                pos = en.GetPos();
            }
            else if (db.targetParam == (int)SkillTargetObjType.SelfAndTeamer)
            {
                if (IsCurTargetNoAttackTeammate())
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindMinHpTeamer();
                    if (en == null)
                    {
                        en = player;
                    }
                    UpdateTarget(en);
                }
                pos = en.GetPos();
            }
            else
            {
                Log.Error("配置出错");
            }

            return en;
        }
        Client.IEntity FindTargetByTarget(table.SkillDatabase db, ref Vector3 pos, out int skillError)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            Client.IEntity en = null;
            skillError = 0;
            if (db.targetParam == (int)SkillTargetObjType.Self)
            {
                if (!IsCurTargetSelf())
                {
                    en = player;
                    UpdateTarget(en);
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Friend)
            {
                if (IsCurTargetFrendly())
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindNearFriends();
                    if (en == null)
                    {
                        en = player;
                    }
                    UpdateTarget(en);
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Enemy)
            {
                if (IsCurTargetEnemy(out skillError))
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindNearEnemy();
                    if (en == null)
                    {
                        if (skillError == 0)
                        {
                            skillError = (int)LocalTextType.Skill_Commond_zhaobudaokegongjidemubiao;
                        }
                        else
                        {
                            Log.Error("找不到附近目标 使用上面的错误码");
                        }

                        return null;
                    }
                    UpdateTarget(en);
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Pet)
            {//只有自己的宠物
                if (m_curTarget != null)
                {
                    en = GetMyPetNpc();
                    if (en == null)
                    {
                        Log.Error("只能对自己的战魂使用");
                        skillError = (int)LocalTextType.Skill_Commond_zhinengduizijidezhanhunshiyong;
                    }
                }
                else
                {
                    en = GetMyPetNpc();
                    if (en != null)
                    {
                        UpdateTarget(en);
                    }
                    else
                    {
                        Log.Error("找不到自己的战魂");
                        skillError = (int)LocalTextType.Skill_Commond_zhaobudaozijidezhanhunwufashiyong;
                    }
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.Summoned)
            {//只有自己的召唤物
                if (IsTargetCanSelect())
                {
                    en = FindNearSummoned();
                    if (en == null)
                    {
                        Log.Error("只能对自己的宝宝使用");
                        skillError = (int)LocalTextType.Skill_Commond_zhinnegduizijidebaobaoshiyong;
                    }
                }
                else
                {
                    en = FindNearSummoned();
                    if (en != null)
                    {
                        UpdateTarget(en);
                    }
                    else
                    {
                        Log.Error("找不到自己的宝宝");
                        skillError = (int)LocalTextType.Skill_Commond_zhaobudaozijidebaobaowufashiyong;
                    }
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.All)
            {
                if (IsTargetCanSelect())
                {
                    en = m_curTarget;
                }
                else
                {
                    IEntitySystem es = m_ClientGlobal.GetEntitySystem();
                    if (es != null)
                    {
                        en = FindNearCreature();
                        if (en == null)
                        {
                            en = player;
                        }
                        UpdateTarget(en);
                    }
                }

            }
            else if (db.targetParam == (int)SkillTargetObjType.FriendBody)
            {
                if (IsCurTargetFrendlyBody())
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindNearFriendBody();
                    if (en == null)
                    {
                        if (m_curTarget == null)
                        {
                            Log.Error("找不到友方尸体，无法使用");
                            skillError = (int)LocalTextType.Skill_Commond_zhaobudaoyoufangshitiwufashiyong;
                        }
                        else
                        {
                            Log.Error("只能对友方尸体使用");
                            skillError = (int)LocalTextType.Skill_Commond_zhinengduiduifangshitishiyog;
                        }
                        return null;
                    }
                    UpdateTarget(en);
                }
            }
            else if (db.targetParam == (int)SkillTargetObjType.SelfAndTeamer)
            {
                if (IsCurTargetNoAttackTeammate())
                {
                    en = m_curTarget;
                }
                else
                {
                    en = FindMinHpTeamer();
                    if (en == null)
                    {
                        en = player;
                    }
                    UpdateTarget(en);
                }
            }

            return en;
        }

        /// <summary>
        /// 获取自己前方距离的点
        /// </summary>
        /// <returns></returns>
        Vector3 GetSelfForwardPos(SkillDatabase db)
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            uint attackDis = db.dwAttackDis;
            Vector3 myPos = player.GetPos();
            Vector3 dir = player.GetDir();
            Vector3 targetPos = myPos + dir * attackDis;
            return targetPos;
        }
        /// <summary>
        /// 当前目标是否是自己
        /// </summary>
        /// <returns></returns>
        bool IsCurTargetSelf()
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (IsTargetCanSelect())
            {
                if (m_curTarget.GetUID() == player.GetUID())
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 当前目标是不是队友
        /// </summary>
        /// <returns>true 是队友</returns>
        bool IsCurTargetTeammate()
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (IsTargetCanSelect())
            {
                IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                if (cs == null)
                {
                    return false;
                }

                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return false;
                }
                return ch.IsSameTeam(m_curTarget);
            }
            return false;
        }
        /// <summary>
        /// 当前不可攻击的队友
        /// </summary>
        /// <returns></returns>
        bool IsCurTargetNoAttackTeammate()
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (IsTargetCanSelect())
            {
                IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                if (cs == null)
                {
                    return false;
                }

                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return false;
                }
                if (ch.IsSameTeam(m_curTarget))
                {
                    if (player != null)
                    {
                        ISkillPart sp = player.GetPart(EntityPart.Skill) as ISkillPart;
                        if (sp != null)
                        {
                            int error = 0;
                            if (!sp.CheckCanAttackTarget(m_curTarget, out error))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 是不是召唤物
        /// </summary>
        /// <returns></returns>
        bool IsCurTargetSummoned()
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (IsTargetCanSelect())
            {
                IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                if (cs == null)
                {
                    return false;
                }

                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return false;
                }
                // return ch.NpcIsPet( m_curTarget );
            }
            return false;
        }
        IEntity GetMyPetNpc()
        {
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (!IsTargetCanSelect())
            {
                IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                if (cs == null)
                {
                    return null;
                }

                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return null;
                }
                uint npcID = ch.GetMyPetNpcID();

                IEntitySystem es = m_ClientGlobal.GetEntitySystem();
                if (es != null)
                {
                    return es.FindNPC(npcID);
                }
            }
            return null;
        }
        IEntity FindNearSummoned()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindSummonedByRange(range);
            }
            return null;
        }
        IEntity FindNearPet()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindPetByRange(range);
            }
            return null;
        }
        IEntity FindMinHpTeamer()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindMinHpTeamer(range);
            }
            return null;
        }
        IEntity FindNearEnemy()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindEntityByCampType(CampType.Enemy, range);
            }
            return null;
        }
        /// <summary>
        /// 当前目标是友方
        /// </summary>
        /// <returns>ture是友方</returns>
        bool IsCurTargetFrendly()
        {
            if (!IsTargetCanSelect())
            {
                return false;
            }
            IPlayer player = m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int skillerror = 0;
                if (!m_skillPart.CheckCanAttackTarget(m_curTarget, out skillerror))
                {
                    return true;
                }
            }
            return false;
        }
        bool IsCurTargetFrendlyBody()
        {
            if (m_curTarget == null)
            {
                return false;
            }
            if (IsCurTargetFrendly() && m_curTarget.IsDead())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 当前目标是敌方
        /// nSkillError 技能无法使用错误码
        /// </summary>
        /// <returns></returns>
        bool IsCurTargetEnemy(out int nSkillError)
        {
            nSkillError = 0;
            if (!IsTargetCanSelect())
            {
                return false;
            }
            IPlayer player = m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                if (m_skillPart.CheckCanAttackTarget(m_curTarget, out nSkillError))
                {
                    return true;
                }
            }
            return false;
        }
        IEntity FindNearFriends()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindEntityByCampType(CampType.Firendly, range);
            }
            return null;
        }
        IEntity FindNearFriendBody()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindEntityByCampType(CampType.FriendlyBody, range);
            }
            return null;
        }
        IEntity FindNearCreature()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindAllCreatureByRange(range);
            }
            return null;
        }
        IEntity FindTeammate()
        {
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                return es.FindTeamerByRange(range);
            }
            return null;
        }

        // 拾取的实体列表
        private List<IEntity> pickEntity = new List<IEntity>();
        //-------------------------------------------------------------------------------------------------------
        // 触摸弹起
        private void OnTouchEnd(object param1 = null, object param2 = null, object param3 = null)
        {
            if (!m_bTouchDown)
            {
                return;
            }

            if (m_Host == null)
            {
                return;
            }
            stSkillLongPress longPress = new stSkillLongPress();
            longPress.bLongPress = false;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
            // 发起移动投票，询问其它系统，是否可以移动
            stVoteEntityMove move;
            move.uid = m_Host.GetUID();
            if (!Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, move))
            {
                return;
            }

            if (m_Host.GetCurState() == CreatureState.Dead)
            {
                return;
            }
            Vector2 touchPos = new Vector2((float)param1, (float)param2);

            IEntitySystem entitySys = m_ClientGlobal.GetEntitySystem();
            if (entitySys == null)
            {
                return;
            }

            if (!entitySys.PickupEntity(touchPos, ref pickEntity))
            {
                // 没有点到实体对象的情况下，点击地面移动主角
                ClickGround(touchPos);
                m_ClientGlobal.GetControllerSystem().GetSwitchTargetCtrl().Clear();

                IControllerSystem cs = ControllerSystem.m_ClientGlobal.GetControllerSystem();
                if (cs == null)
                {
                    Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
                    return;
                }

                ICombatRobot combot = cs.GetCombatRobot();
                if (combot.Status != CombatRobotStatus.STOP)
                {
                    combot.Pause();
                }
            }
            else
            {
                if (pickEntity.Count == 1)
                {
                    if (pickEntity[0].GetEntityType() != EntityType.EntityType_Box)
                    {
                        UpdateTarget(pickEntity[0]);
                    }
                    else
                    {
                        IBox box = pickEntity[0] as IBox;
                        if (box.CanPick())
                        {
                            MoveToTarget(pickEntity[0].GetPos());
                        }
                        else
                        {
                            m_ClientGlobal.GetTipsManager().ShowTips("该物品不属于您~");
                        }
                    }
                }
                else
                {
                    // 按一定规则进行筛选 // TODO: 后面补充
                    // 选择目标

                    if (pickEntity[0].GetEntityType() != EntityType.EntityType_Box)
                    {
                        UpdateTarget(pickEntity[0]);
                    }
                    else
                    {
                        IBox box = pickEntity[0] as IBox;
                        if (box.CanPick())
                        {
                            MoveToTarget(pickEntity[0].GetPos());
                            //                             m_ClientGlobal.netService.Send(new GameCmd.stPickUpItemPropertyUserCmd_C()
                            //                             {
                            //                                 qwThisID = pickEntity[0].GetID(),
                            //                             });
                        }
                        else
                        {
                            m_ClientGlobal.GetTipsManager().ShowTips("该物品不属于您~");
                        }
                    }
                }

                // 点击回调
                if (m_ClickSink != null)
                {
                    m_ClickSink.OnClickEntity(pickEntity[0]);
                }
            }

            pickEntity.Clear();
            m_bTouchDown = false;
        }

        // JoystickChanging
        private void OnJoystick(object param1 = null, object param2 = null, object param3 = null)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)(int)Client.GameEventID.JOYSTICK_PRESS, null);
            float fAngle = (float)param1;
            if (fAngle < 0)
            {
                fAngle = 360.0f + fAngle;
            }

            MoveController.Instance.SetTargetAngle(fAngle);
        }

        // JoystickEnd
        private void OnJoystickEnd(object param1 = null, object param2 = null, object param3 = null)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)(int)Client.GameEventID.JOYSTICK_UNPRESS, null);
            MoveController.Instance.StopMove();
        }

        //-------------------------------------------------------------------------------------------------------
        private void ClickGround(Vector2 vTohchPos)
        {
            // 没有点到实体对象的情况下，点击地面移动主角
            IMapSystem mapSys = m_ClientGlobal.GetMapSystem();
            Vector3 scenePos = Vector3.zero;
            if (mapSys != null)
            {
                Engine.TerrainInfo info;
                if (!mapSys.GetScenePos(vTohchPos, out info))
                {
                    return;
                }
                scenePos = info.pos;
                // 移动到指定位置，包含寻路，并同步消息
                MoveToTarget(scenePos);
                ShowClickEffect(info);
                // 清理正在执行的命令
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);

                CmdManager.Instance().Clear();
            }
        }

        private void ShowClickEffect(Engine.TerrainInfo info)
        {
            if (info == null)
            {
                return;
            }

            Vector3 targetPos = info.pos;
            targetPos.y = 0;
            if (!m_ClientGlobal.GetMapSystem().CanWalk(targetPos))
            {
                return;
            }

            if (m_ClickEffct != null)
            {
                Engine.RareEngine.Instance().GetRenderSystem().RemoveEffect(m_ClickEffct);
                m_ClickEffct = null;
            }

            table.ResourceDataBase data = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(10);
            if (data != null)
            {
                string strSelectEffct = data.strPath;
                Engine.RareEngine.Instance().GetRenderSystem().CreateEffect(ref strSelectEffct, ref m_ClickEffct, null);
            }

            if (m_ClickEffct == null)
            {
                Engine.Utility.Log.Error("点地特效为空");
                return;
            }
            m_ClickEffct.GetNode().GetTransForm().gameObject.SetActive(false);
            Vector3 pos = info.pos;
            pos.y += 0.1f;
            m_ClickEffct.GetNode().SetWorldPosition(pos);

            Quaternion rot = new Quaternion();
            rot.SetLookRotation(Vector3.forward, info.normal);

            m_ClickEffct.GetNode().SetWorldRotate(rot);

            m_ClickEffct.GetNode().GetTransForm().gameObject.SetActive(true);
        }

        //-------------------------------------------------------------------------------------------------------
        // 目标更新
        public void UpdateTarget(IEntity target)
        {
            // 排除玩家自己
            if (m_ClientGlobal.IsMainPlayer(target))
            {
                return;
            }
            m_preTarget = m_curTarget;
            m_curTarget = target;
            
            // 发送事件
            Client.stTargetChange targetChange = new Client.stTargetChange();
            targetChange.target = m_curTarget;
            if (targetChange.target != null)
            {
                if (targetChange.target.GetProp((int)EntityProp.EntityState) == (int)GameCmd.SceneEntryState.SceneEntry_Hide)
                {
                    if (!IsCurTargetTeammate())
                    {
                        return;
                    }
                }
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);

        }

        void AddCircleEffect(IEntity entity)
        {
            if (m_effctId != -1 && m_preTarget != null)
            {
                m_preTarget.SendMessage(EntityMessage.EntityCommand_RemoveLinkEffect, m_effctId);
                m_effctId = -1;
            }

            if (entity == null)
            {
                return;
            }

            int pkmodel = m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.PkMode);
            PLAYERPKMODEL model = (PLAYERPKMODEL)pkmodel;

            float scale = 1f;
            uint effectId = 0;
            if (entity is INPC)
            {
                INPC npc = entity as INPC;
                scale = npc.GetRadius();
                if (!npc.IsMonster())
                {
                    effectId = GREEN_EFFECT_ID;
                }
                else
                {
                    uint masterid = (uint)npc.GetProp((int)Client.NPCProp.Masterid);
                    if (masterid != 0)
                    {
                        if (masterid == m_ClientGlobal.MainPlayer.GetID())
                        {
                            effectId = GREEN_EFFECT_ID;
                        }
                        else
                        {
                            //别人的战魂和召唤物，点选时根据主人状态判断，可攻击时为红圈，不可攻击时为绿圈
                            IEntity otherEntity = m_ClientGlobal.GetEntitySystem().FindPlayer(masterid);
                            if (otherEntity != null)
                            {
                                Client.ISkillPart skillPart = m_ClientGlobal.MainPlayer.GetPart(EntityPart.Skill) as Client.ISkillPart;
                                if (skillPart != null)
                                {
                                    int skillerror = 0;
                                    bool canAttack = skillPart.CheckCanAttackTarget(otherEntity, out skillerror);
                                    effectId = canAttack ? RED_EFFECT_ID : GREEN_EFFECT_ID;
                                }
                            }
                        }
                    }
                    else
                    {
                        effectId = RED_EFFECT_ID;
                    }
                }
            }
            else
            {
                Client.ISkillPart skillPart = m_ClientGlobal.MainPlayer.GetPart(EntityPart.Skill) as Client.ISkillPart;
                if (skillPart != null)
                {
                    int skillerror = 0;
                    bool canAttack = skillPart.CheckCanAttackTarget(entity, out skillerror);
                    effectId = canAttack ? RED_EFFECT_ID : GREEN_EFFECT_ID;
                }
            }

            table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>(effectId);
            if (edb != null)
            {
                AddLinkEffect node = new AddLinkEffect();
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("找不到特效资源路径配置{0}", edb.resPath);
                }
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                node.nFollowType = (int)edb.flowType;
                node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
                node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                node.bIgnoreRide = false; // 特效要挂在坐骑上
                node.scale = new Vector3(scale, scale, scale);
                if (node.strEffectName.Length != 0)
                {
                    m_effctId = (int)entity.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void OnSwitchTarget()
        {
            m_ClientGlobal.GetControllerSystem().GetSwitchTargetCtrl().Switch();
        }

        public void EnterMapImmediacy(uint mapid, uint npcid, uint x, uint y)
        {
            //直接传送到目标点
            GameCmd.sTaskHelpGoToScriptUserCmd_CS cmd = new GameCmd.sTaskHelpGoToScriptUserCmd_CS();
            //cmd.mapid = mapid;
            //cmd.npcid = npcid;
            //cmd.x = x;
            //cmd.y = y;
            if (m_ClientGlobal.netService != null)
            {
                m_ClientGlobal.netService.Send(cmd);
            }
        }

    }
}
