using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Client;
using GameCmd;
using Common;
using UnityEngine;
using Engine.Utility;
using table;
using EntitySystem;

namespace SkillSystem
{


    /// <summary>
    /// 效果类型
    /// </summary>
    public enum EffectType
    {
        DINGSHEN = 102,// 无法移动	64	定身

        CHENMO,//无法使用技能	65	沉默
        JINGU,//无法使用道具	66	禁锢
        WUDI,//无法受到伤害	67	无敌

    }
    class PlayerBuffPart : IBuffPart, ITimer
    {
        private static IClientGlobal m_ClientGlobal = null;

        private IEntity m_Master = null;
        private readonly uint m_ubuffCountDownTimerID = 100;
        private readonly uint m_ucolorCountDownTimerID = 101;

        List<uint> m_unCreateEffectList = new List<uint>();
        bool m_bHideOtherPlayer //是否屏蔽其他玩家
        {
            get
            {
                int show = m_ClientGlobal.gameOption.GetInt("SpecialEffects", "BlockPlayers", 0);
                return show == 1 ? true : false;
            }
        }
        bool m_bHideOtherFx //是否屏蔽其他玩家特效
        {
            get
            {
                int show = m_ClientGlobal.gameOption.GetInt("SpecialEffects", "otherEffect", 0);
                return show == 1 ? true : false;
            }
        }
        struct stBuffData
        {
            public uint buffid;
            public Dictionary<uint, stStateValue> buffDataDic;
        }
        /// <summary>
        /// key is buff's thisid
        /// </summary>
        private Dictionary<uint, stBuffData> buffDic = new Dictionary<uint, stBuffData>();

        private ArrayList buffList = new ArrayList();
        uint stateID = 0;
        uint cancleBuffThisID = 0;
        IEntity m_buffColorEntity = null;
        /// <summary>
        /// buff对应的特效字典 key is buffthisid value is effectid
        /// </summary>
        Dictionary<uint, int> effectDic = new Dictionary<uint, int>();
        public PlayerBuffPart(IClientGlobal clientGlobal, SkillSystem skillSystem)
        {
            m_ClientGlobal = clientGlobal;
            TimerAxis.Instance().SetTimer(m_ubuffCountDownTimerID, 500, this);
        }

        public IClientGlobal GetClientGlobal()
        {
            return m_ClientGlobal;
        }

        public Dictionary<uint, stStateValue> GetBuffdic()
        {
            return null;// buffDic;
        }
        public uint GetBuffEffectType()
        {
            uint type = 0;
            Dictionary<uint, stBuffData>.Enumerator iter = buffDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var buff = iter.Current;
                BuffDataBase db = GetBuffDataBase(buff.Key);
                if (db != null)
                {
                    string[] effectArray = db.effectid.Split('_');
                    for (int i = 0; i < effectArray.Length; i++)
                    {
                        string effectid = effectArray[i];
                        uint id = uint.Parse(effectid);
                        StateDataBase stateBase = GameTableManager.Instance.GetTableItem<StateDataBase>(id);
                        if (stateBase != null)
                        {
                            if (stateBase.typeid == (int)EffectType.DINGSHEN)
                            {
                                type |= (uint)BuffEffectType.CANNOT_MOVE;
                                break;
                            }
                        }
                    }
                }
            }

            Dictionary<uint, stBuffData>.Enumerator iterbuff = buffDic.GetEnumerator();
            while (iterbuff.MoveNext())
            {
                var buff = iterbuff.Current;
                BuffDataBase db = GetBuffDataBase(buff.Key);
                if (db != null)
                {
                    string[] effectArray = db.effectid.Split('_');
                    for (int i = 0; i < effectArray.Length; i++)
                    {
                        string effectid = effectArray[i];
                        uint id = uint.Parse(effectid);
                        StateDataBase stateBase = GameTableManager.Instance.GetTableItem<StateDataBase>(id);
                        if (stateBase != null)
                        {
                            if (stateBase.typeid == (int)EffectType.JINGU)
                            {
                                type |= (uint)BuffEffectType.CANNOT_USEITEM;
                                break;
                            }
                        }
                    }
                }
            }
            Dictionary<uint, stBuffData>.Enumerator iterDic = buffDic.GetEnumerator();
            while (iterDic.MoveNext())
            {
                var buff = iterDic.Current;
                BuffDataBase db = GetBuffDataBase(buff.Key);
                if (db != null)
                {
                    string[] effectArray = db.effectid.Split('_');
                    for (int i = 0; i < effectArray.Length; i++)
                    {
                        string effectid = effectArray[i];
                        uint id = uint.Parse(effectid);
                        StateDataBase stateBase = GameTableManager.Instance.GetTableItem<StateDataBase>(id);
                        if (stateBase != null)
                        {
                            if (stateBase.typeid == (int)EffectType.CHENMO)
                            {
                                type |= (uint)BuffEffectType.CANOT_USESKILL;
                                break;
                            }
                        }
                    }
                }
            }
            return type;
        }

        bool VoteCallBack(int nEventID, object param)
        {
            if (!m_ClientGlobal.IsMainPlayer(m_Master))
            {
                return true;
            }
            // uint state = GetBuffEffectType();
            //if ( nEventID == (int)GameVoteEventID.SKILL_CANUSE )
            //{

            //    if((state&(uint)BuffEffectType.CANOT_USESKILL) == (uint)BuffEffectType.CANOT_USESKILL)
            //    {
            //        return false;
            //    }
            //    if ( ( state & (uint)BuffEffectType.CANNOT_USEATTACKANDSKILL ) == (uint)BuffEffectType.CANNOT_USEATTACKANDSKILL )
            //    {
            //        return false;
            //    }
            //}
            //else if ( nEventID == (int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE )
            //{
            //    stVoteEntityMove move = (stVoteEntityMove)param;
            //    if (m_Master.GetUID() == move.uid)
            //    {
            //        if ((state & (uint)BuffEffectType.CANNOT_MOVE) == (uint)BuffEffectType.CANNOT_MOVE)
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 获取buff列表
        @param
        */
        public void GetBuffList(out List<Client.stAddBuff> lstBuff)
        {
            lstBuff = new List<stAddBuff>();
            if (m_Master == null)
            {
                return;
            }

            Dictionary<uint, stBuffData>.Enumerator iter = buffDic.GetEnumerator();
            while (iter.MoveNext())
            {
                stStateValue curBuff = null;
                Dictionary<uint, stStateValue>.Enumerator itBuff = iter.Current.Value.buffDataDic.GetEnumerator();
                while (itBuff.MoveNext())
                {
                    curBuff = itBuff.Current.Value;
                }

                if (curBuff != null)
                {
                    stAddBuff addBuff = new stAddBuff();
                    addBuff.buffid = curBuff.baseid;
                    addBuff.level = curBuff.level;
                    addBuff.lTime = curBuff.ltime;
                    addBuff.uid = m_Master.GetUID();

                    lstBuff.Add(addBuff);
                }
            }
        }

        #region EntiyPart

        public EntityPart GetPartID()
        {
            return EntityPart.Buff;
        }

        public bool Create(IEntity master)
        {
            m_Master = master;

            if (m_ClientGlobal != null)
            {
                if (m_ClientGlobal.IsMainPlayer(m_Master))
                {
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteCallBack);
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, VoteCallBack);
                }
                EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);

            }


            return true;
        }
        void OnEvent(int eventID, object param)
        {
            if (eventID == (int)GameEventID.ENTITYSYSTEM_CREATEENTITY)
            {
                Client.stCreateEntity createEntity = (Client.stCreateEntity)param;
                if (createEntity.uid == m_Master.GetUID())
                {
                    for (int i = m_unCreateEffectList.Count - 1; i >= 0; i--)
                    {
                        uint runID = m_unCreateEffectList[i];
                        CreateEffect(runID);
                        m_unCreateEffectList.Remove(runID);
                    }
                }
            }
        }
        public IEntity GetMaster()
        {
            return m_Master;
        }

        // 更新
        public void Update(float dt)
        {

        }
        public void Release()
        {
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteCallBack);
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, VoteCallBack);

            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
            TimerAxis.Instance().KillTimer(m_ubuffCountDownTimerID, this);
            buffDic.Clear();
            buffDic = null;
            effectDic.Clear();
            effectDic = null;
            buffList.Clear();
            buffList = null;
            m_unCreateEffectList.Clear();
            m_unCreateEffectList = null;
        }
        #endregion
        public bool IsMainPlayer()
        {
            if (m_ClientGlobal.MainPlayer.GetID() == m_Master.GetID())
                return true;
            return false;
        }
        bool IsHideFx()
        {
            if (IsMainPlayer())
            {
                return false;
            }
            IEntity en = m_Master;
            if (en != null)
            {
                INPC npc = en as INPC;
                if (npc != null)
                {
                    if (npc.IsPet() || npc.IsSummon())
                    {
                        if (npc.IsMainPlayerSlave())
                        {
                            return false;
                        }
                        else
                        {
                            return m_bHideOtherFx || m_bHideOtherPlayer;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    //player
                    return m_bHideOtherFx || m_bHideOtherPlayer;
                }
            }
            return false;
        }
        void ChangeCreatureByBuff(IEntity en, uint buffBaseID, bool isControl)
        {
            if (en == null)
            {
                return;
            }
            BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(buffBaseID);
            if (db == null)
            {
                // Log.Error("找不到Buff{0}的配置数据！", buffBaseID);
                return;
            }
            if (db.buffBigType == (int)BuffBigType.Control && db.buffSmallType == (int)BuffSmallType.XuanYun)
            {
                ICreature creature = en as ICreature;
                if (creature != null)
                {
                    if (isControl)
                    {
                        creature.ChangeState(CreatureState.Contrl);
                    }
                    else
                    {
                        creature.ChangeState(CreatureState.Normal);
                    }
                }
            }
        }
        #region buffpart
        /// <summary>
        /// 状态信息下行消息
        /// </summary>
        /// <param name="cmd"></param>
        public void ReciveBuffState(stSendStateIcoListMagicUserCmd_S cmd)
        {
            if(cmd.refresh == 1)
            {
                ClearAllBuff();
            }
            IEntity en = EntitySystem.EntityHelper.GetEntity(cmd.byType, cmd.dwTempID);
            INPC npc = en as INPC;
            if (npc != null)
            {
                if (npc.IsTrap())
                {
                    return;
                }
            }
            ReceiveBuffList(cmd.statelist);

        }
        public void ReceiveBuffList(List<stStateValue> list)
        {
            IEntity en = m_Master;
            for (int i = 0; i < list.Count; i++)
            {
                var state = list[i];
                ChangeCreatureByBuff(en, state.baseid, true);

                //客户端使用毫秒的精度
                state.ltime = state.ltime * 1000;
                stBuffData buffData;
                if (buffDic.TryGetValue(state.thisid, out buffData))
                {
                    if (buffData.buffDataDic.ContainsKey(state.thisid))
                    {
                        buffData.buffDataDic[state.thisid] = state;
                    }
                    else
                    {
                        buffData.buffDataDic.Add(state.thisid, state);

                    }
                    // Buff Update
                    // 更新时间
                }
                else
                {
                    buffData = new stBuffData();
                    buffData.buffid = state.thisid;
                    buffData.buffDataDic = new Dictionary<uint, stStateValue>();
                    buffData.buffDataDic.Add(state.thisid, state);
                    buffDic.Add(state.thisid, buffData);
                    AddBuffEffect(state.baseid, state.thisid);


                    if (en != null)
                    {
                        stAddBuff addBuff = new stAddBuff();
                        addBuff.buffid = state.baseid;
                        addBuff.uid = en.GetUID();
                        addBuff.level = state.level;
                        addBuff.lTime = state.ltime;
                        addBuff.buffThisID = state.thisid;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.BUFF_ADDTOTARGETBUFF, addBuff);
                    }
                }
            }
            if (list.Count > 0)
            {
                int len = list.Count;
                stStateValue state = list[len - 1];

                BuffDataBase bdb = GameTableManager.Instance.GetTableItem<BuffDataBase>(state.baseid);
                if (bdb != null)
                {
                    string str = bdb.buffColor;
                    Color c = SkillSystem.GetColorByStr(str);
                    if(c == null)
                    {
                        return;
                    }
                    if (en != null)
                    {
                        m_buffColorEntity = en;
                        en.SendMessage(EntityMessage.EntityCommond_SetColor, c);
                        if (TimerAxis.Instance().IsExist(m_ucolorCountDownTimerID, this))
                        {
                            TimerAxis.Instance().KillTimer(m_ucolorCountDownTimerID, this);
                        }

                        TimerAxis.Instance().SetTimer(m_ucolorCountDownTimerID, state.ltime, this, 1);
                    }


                }
            }
        }
        void ClearAllBuff()
        {
            List<stStateValue> stateList = new List<stStateValue>();
            foreach (var dic in buffDic)
            {
                uint itemID = dic.Key;
                stBuffData buffdata = dic.Value;
                foreach (var state in buffdata.buffDataDic)
                {
                    stStateValue st = state.Value;
                    stateList.Add(st);
                }
            }
            foreach(var st in stateList)
            {
                RemoveEntityBuff(st.baseid,st.thisid);
            }
            buffDic.Clear();
        }
        void RemoveEntityBuff(uint baseID, uint thisID)
        {
            uint stateID = baseID;
            ChangeCreatureByBuff(m_Master, stateID, false);
            BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(stateID);
            if (db != null)
            {
                if (db.dwShield == 1)
                {//不显示的直接跳过
                    return;
                }
            }
            uint cancleBuffThisID = thisID;
            BuffDataBase baseBuffData = GetBuffDataBase(cancleBuffThisID);
            if (baseBuffData == null)
            {
                // Log.Error("getbuffdatabase error buffthisid is " + cancleBuffThisID.ToString());
                return;
            }
            if (baseBuffData.buffRemoveEffect != 0)
            {
                if (!IsHideFx())
                {
                    EffectViewFactory.Instance().CreateEffect(m_Master.GetUID(), baseBuffData.buffRemoveEffect);
                }
            }
            if (baseBuffData.buffBigType == (int)BuffBigType.Control)
            {
                if (m_Master != null)
                {
                    //Log.Error( "buff cancle" );
                    if (!m_Master.IsDead())
                    {
                        Engine.Utility.Log.LogGroup("ZDY", " CreatureState.Normal ");
                        m_Master.ChangeState(CreatureState.Normal, null);
                    }
                    else
                    {
                        Engine.Utility.Log.LogGroup("ZDY", "CreatureState.Dead ****");
                        m_Master.ChangeState(CreatureState.Dead, null);
                    }
                }
            }
            stBuffData buffData;
            if (buffDic.TryGetValue(cancleBuffThisID, out buffData))
            {
                buffData.buffDataDic.Remove(cancleBuffThisID);
            }

            if (buffData.buffDataDic.Count <= 0)
            {
                RemoveBuffEffect(stateID, cancleBuffThisID);
                buffDic.Remove(cancleBuffThisID);


                if (m_Master != null)
                {
                    stRemoveBuff removeBuff = new stRemoveBuff();
                    removeBuff.buffid = stateID;
                    removeBuff.uid = m_Master.GetUID();
                    removeBuff.buffThisID = cancleBuffThisID;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.BUFF_DELETETARGETBUFF, removeBuff);
                }
            }
        }
        /// <summary>
        /// 取消某个对象身上的状态图标
        /// </summary>
        /// <param name="cmd"></param>
        public void CancleBuffState(stCancelStateIcoListMagicUserCmd_S cmd)
        {
            INPC npc = m_Master as INPC;
            if (npc != null)
            {
                if (npc.IsTrap())
                {
                    return;
                }
            }

            RemoveEntityBuff(cmd.dwTempID, cmd.dwStateThisID);

        }
        #endregion

        void CreateEffect(uint runID)
        {

            BuffDataBase db = GetBuffDataBase(runID);
            if (db == null)
            {
               // Log.Error("读取Buff配置数据出错:{0}", runID);
                return;
            }


            for (int i = 0; i < db.buffEffect.Count; i++)
            {
                var item = db.buffEffect[i];
                if (item == 0)
                {
                    continue;
                }
                FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(item);
                if (edb == null)
                {
                    // Log.Error("get FxResDataBase is null id is " + item.ToString());
                    return;
                }
                if (!IsHideFx())
                {
                    int nEffectID = (int)EffectViewFactory.Instance().CreateEffect(m_Master.GetUID(), item);

                    if (!effectDic.ContainsKey(runID))
                    {
                        effectDic.Add(runID, nEffectID);
                    }
                }
            }
        }
        // 添加buff表现特效效果
        void AddBuffEffect(uint buffID, uint runID)
        {
            if (m_Master == null)
            {
                return;
            }
            if (m_Master.GetNode() == null)
            {
                if (!m_unCreateEffectList.Contains(runID))
                {
                    m_unCreateEffectList.Add(runID);
                }
            }
            else
            {
                CreateEffect(runID);
            }
            BuffDataBase db = GetBuffDataBase(runID);
            if (db == null)
            {
                //Log.Error("读取Buff配置数据出错:{0}", buffID);
                return;
            }


            for (int i = 0; i < db.buffEffect.Count; i++)
            {
                var item = db.buffEffect[i];
                if (item == 0)
                {
                    continue;
                }
                FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(item);
                if (edb == null)
                {
                    // Log.Error("get FxResDataBase is null id is " + item.ToString());
                    return;
                }
                string aniName = edb.targetAniName;
                if (string.IsNullOrEmpty(aniName))
                {
                    return;
                }
                PlayAni anim_param = new PlayAni();
                anim_param.strAcionName = aniName;
                anim_param.fSpeed = 1;
                anim_param.nStartFrame = 0;
                anim_param.nLoop = edb.playAniTime;
                //if(db.buffBigType == (int)BuffBigType.Control)
                //{
                //    anim_param.nLoop = -1;
                //}
                //else
                //{
                //    anim_param.nLoop = 1;
                //}

                anim_param.fBlendTime = 0.2f;
                bool bRide = (bool)m_Master.SendMessage(EntityMessage.EntityCommond_IsRide, null);
                if (bRide)
                {
                    return;
                }
                if (m_Master.GetCurState() == CreatureState.Contrl)
                {//眩晕不播放其他动画
                    anim_param.aniCallback = OnHitCallback;
                    anim_param.callbackParam = m_Master;
                    if (SkillSystem.GetClientGlobal().IsMainPlayer(m_Master.GetID()))
                    {
                        Engine.Utility.Log.Info("buff 播放眩晕动作 {0}", anim_param.strAcionName);
                    }
                    m_Master.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                }
                else
                {
                    if (m_Master.GetCurState() != CreatureState.Move)
                    {//移动中不播放受击动作
                        Client.IControllerSystem cs = m_ClientGlobal.GetControllerSystem();
                        if (cs != null)
                        {
                            Client.ICombatRobot robot = cs.GetCombatRobot();
                            if (robot.Status == CombatRobotStatus.RUNNING)
                            {
                                if (SkillSystem.GetClientGlobal().IsMainPlayer(m_Master.GetID()))
                                {//自动挂机 不播受击

                                    return;
                                }
                            }
                        }

                        m_Master.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                    }

                }
            }
        }

        /// <summary>
        /// 受击动作完后进入stand 动作
        /// </summary>
        /// <param name="strEventName"></param>
        /// <param name="strAnimationName"></param>
        /// <param name="time"></param>
        /// <param name="param"></param>
        void OnHitCallback(ref string strEventName, ref string strAnimationName, float time, object param)
        {
            if (strEventName == "end" && strAnimationName == "Hit001")
            {
                IEntity entity = param as IEntity;
                if (entity.GetCurState() != CreatureState.Move)
                {
                    entity.ChangeState(CreatureState.Normal);
                }
            }
        }
        // 移除buff表现特效效果
        void RemoveBuffEffect(uint buffID, uint runID)
        {
            //BuffDataBase db = GetBuffDataBase(runID);
            //if(db==null)
            //{
            //    Log.Error("读取Buff配置数据出错:{0}", buffID);
            //    return;
            //}
            if (effectDic.ContainsKey(runID))
            {
                uint effectID = (uint)effectDic[runID];
                EffectViewFactory.Instance().RemoveEffect(m_Master.GetUID(), effectID);
                effectDic.Remove(runID);
            }
        }
        public BuffDataBase GetBuffDataBase(uint thisID)
        {
            stStateValue state = null;
            Dictionary<uint, stBuffData>.Enumerator iter = buffDic.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.buffDataDic.TryGetValue(thisID, out state))
                {
                    BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(state.baseid, (int)state.level);
                    if (db != null)
                    {
                        return db;
                    }
                    else
                    {
                        Log.Error("buffdatabase is null id is {0}", state.baseid);
                        return null;
                    }
                }
            }

            return null;
        }

        public void OnTimer(uint uTimerID)
        {
            if (uTimerID == m_ubuffCountDownTimerID)
            {
                Dictionary<uint, stBuffData>.Enumerator iter = buffDic.GetEnumerator();
                while (iter.MoveNext())
                {
                    Dictionary<uint, stStateValue> dic = iter.Current.Value.buffDataDic;
                    Dictionary<uint, stStateValue>.Enumerator dicIter = dic.GetEnumerator();
                    while (dicIter.MoveNext())
                    {
                        stStateValue stateValue = dicIter.Current.Value;
                        stateValue.ltime -= 500;
                    }
                }
            }
            else if (uTimerID == m_ucolorCountDownTimerID)
            {
                if (m_buffColorEntity != null)
                {
                    Color c = new Color(1, 1, 1);
                    m_buffColorEntity.SendMessage(EntityMessage.EntityCommond_SetColor, c);
                }


            }
        }
    }
}
