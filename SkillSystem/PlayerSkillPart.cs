using System;
using System.Collections.Generic;
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
using SkillMoveType = Client.SkillMoveType;
namespace SkillSystem
{

    partial class PlayerSkillPart : ISkillPart
    {
        // 游戏全局对象
        public static IClientGlobal m_ClientGlobal = null;
        #region fields
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
        public int FxLevel
        {
            get
            {
                if (IsMainPlayer())
                {
                    Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
                    return renderSys.effectLevel;
                }
                else
                {

                    if (Application.isEditor)
                    {
                        return 0;
                    }
                    return 2;
                }
            }
        }
        // 部件ID
        private readonly EntityPart m_ePartID = EntityPart.Skill;

        private IEntity m_Master = null;

        private IEntity skillTarget;
        public IEntity SkillTarget
        {
            get
            {
                return skillTarget;
            }
            set
            {
                skillTarget = value;
            }
        }
        /// <summary>
        /// 技能主体（宿主）
        /// </summary>
        public IEntity Master
        {
            get
            {
                return m_Master;
            }
        }
        public SkillCaster Caster
        {
            get
            {
                return m_caster;
            }
            set
            {
                m_caster = value;
            }
        }
        private SkillCaster m_caster;

        private Dictionary<uint, SkillDatabase> curSkillDic = new Dictionary<uint, SkillDatabase>();
        /// <summary>
        /// 保存人物当前拥有的技能
        /// </summary>
        public Dictionary<uint, SkillDatabase> CurSkillDic
        {
            get
            {
                return curSkillDic;
            }
            set
            {
                curSkillDic = value;
            }
        }


        //技能效果表
        Dictionary<int, SkillEffectProp> m_skillEffectDict = new Dictionary<int, SkillEffectProp>();
        public Dictionary<int, SkillEffectProp> SkillEffectDic
        {
            set
            {
                m_skillEffectDict = value;
            }
            get
            {
                return m_skillEffectDict;
            }
        }
        //普攻连击技能ID表
        Dictionary<int, int> m_comboSkillDict = new Dictionary<int, int>();
        public Dictionary<int, int> ComboSkillDic
        {
            set
            {
                m_comboSkillDict = value;
            }
            get
            {
                return m_comboSkillDict;
            }
        }
        uint m_lastSkillId = 0;
        //当前攻击技能ID
        uint m_curSkillId = 0;
        public uint CurSkillID
        {
            set
            {

                m_curSkillId = value;
            }
            get
            {
                return m_curSkillId;
            }
        }
        //下一个即将释放的连击技能ID （只有普攻才有）
        int m_nextSkillId = 0;

        public int NextSkillID
        {
            set
            {
                m_nextSkillId = value;
            }
            get
            {
                return m_nextSkillId;
            }
        }

        //是否是连击攻击技能？
        bool m_isComboSkill = false;

        public bool IsComboSkill
        {
            set
            {
                m_isComboSkill = value;
            }
            get
            {
                return m_isComboSkill;
            }
        }
        public float MsComboPreiod
        {
            get
            {
                return ms_combo_period;
            }
        }
        //正在播放中的攻击动作
        AnimationState aniState;
        public AnimationState AttackAnimState
        {
            get
            {
                return aniState;
            }
            set
            {
                //if (value == null)
                //{
                //    Log.Trace("anistate is null");
                //}
                Caster.SetAniState(value);
                aniState = value;
            }
        }
        //连接操作的时间范围
        static readonly float ms_combo_period = 0.2f;
        /// <summary>
        /// 记录已经播放伤害的hitnode 但是还没有伤害数值表现（服务器没有发送伤害值）
        /// </summary>
       // Dictionary<uint, HitNode> hitDic = new Dictionary<uint, HitNode>();

        private uint m_uDamageID = 10;
        //DamageManager damagerManager = null;
        StateMachine<ISkillPart> stateMachine = null;
        /// <summary>
        /// npc将要播放 的技能
        /// </summary>
        GameCmd.stPrepareUseSkillSkillUserCmd_S npcWillUseSkillCmd;
        float skillStiffTime = 0;//技能僵直时间
        public float SkillStiffTime
        {
            get
            {
                return skillStiffTime;
            }
            set
            {
                //if (value == 0)
                //{
                //   // Log.LogGroup("ZDY", "僵直时间为0");
                //}
                skillStiffTime = value;
            }
        }
        //技能开始播放时间
        float skillStartPlayTime = 0;
        public float SkillStartPlayTime
        {
            get
            {
                return skillStartPlayTime;
            }
            set
            {
                skillStartPlayTime = value;
            }
        }
        /// <summary>
        /// 发送move消息
        /// </summary>
        bool bSendMoveMsg = false;
        /// <summary>
        /// 是否活着
        /// </summary>
        bool bLive = true;
        /// <summary>
        /// 普工技能
        /// </summary>
        uint commonSkillID;

        //是否是长按攻击
        bool m_bLongPress = false;
        #endregion
        public PlayerSkillPart(IClientGlobal clientGlobal, SkillSystem skillSystem)
        {
            m_ClientGlobal = clientGlobal;
            //damagerManager = new DamageManager();
        }

        void OnPlayerDie(int eventID, object param)
        {
            if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
            {

                stEntityDead ed = (stEntityDead)param;
                if (ed.uid == m_Master.GetUID())
                {

                    bLive = false;
                    m_caster.Stop();
                    m_caster.Dead();
                }
                else
                {
                    //  Reset();
                }
            }
        }
        /// <summary>
        /// 检查player 能不能被攻击
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CheckPlayerCanAttack(IEntity target, out int nSkillError)
        {
            nSkillError = 0;
            IControllerHelper m_controllerhelper = GetControllerHelper();
            IMapSystem mapsys = m_ClientGlobal.GetMapSystem();
            if (m_controllerhelper == null || mapsys == null)
            {
                Log.Error("m_controllerhelper is null");
                return false;
            }
            if (target == null)
            {
                nSkillError = (int)LocalTextType.Skill_Commond_zhaobudaokegongjidemubiao;
                return false;
            }
            PLAYERPKMODEL pkmodel = (PLAYERPKMODEL)m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.PkMode);

            MapAreaType areaType = mapsys.GetAreaTypeByPos(m_ClientGlobal.MainPlayer.GetPos());
            MapAreaType targetAreaType = mapsys.GetAreaTypeByPos(target.GetPos());
            if (m_ClientGlobal.MainPlayer.GetUID() == target.GetUID())
            {
                Log.LogGroup("ZDY", "自己无法攻击自己");
                nSkillError = (int)LocalTextType.Skill_Commond_zijiwufagongjiziji;
                return false;
            }
            Client.IController cs = GetController();
            if (cs == null)
            {
                Log.Error("IController is null");
                return false;
            }
            IEntity curTar = cs.GetCurTarget();
            //bool sendTips = false;
            //if (curTar != null)
            //{
            //    if (curTar.GetUID() == target.GetUID())
            //    {
            //        sendTips = true;
            //    }
            //}
            if (areaType == MapAreaType.Safe || targetAreaType == MapAreaType.Safe)
            {
                nSkillError = (int)LocalTextType.Skill_Commond_anquanquneiwufagongji;
                Log.LogGroup("ZDY", "安全区内无法不能攻击");
                return false;
            }
            else
            {

                if (pkmodel == PLAYERPKMODEL.PKMODE_M_NORMAL)
                {
                    nSkillError = (int)LocalTextType.Skill_Commond_zhandoumoshubudui;
                    Log.LogGroup("ZDY", "战斗模式不对");
                    return false;
                }
                else if (pkmodel == PLAYERPKMODEL.PKMODE_M_TEAM)
                {
                    if (m_controllerhelper != null && m_controllerhelper.IsSameTeam(target))
                    {

                        nSkillError = (int)LocalTextType.Skill_Commond_tongyigeduiwubunenggongji;
                        Log.LogGroup("ZDY", "同一个队伍不能攻击");
                        return false;
                    }
                }
                else if (pkmodel == PLAYERPKMODEL.PKMODE_M_FAMILY)
                {
                    if (m_controllerhelper != null && m_controllerhelper.IsSameFamily(target))
                    {

                        nSkillError = (int)LocalTextType.Skill_Commond_tongyigeshizubunenggongji;
                        Log.LogGroup("ZDY", "同一个家族不能攻击");
                        return false;
                    }
                }
                else if (pkmodel == PLAYERPKMODEL.PKMODE_M_CAMP)
                {
                    if (m_controllerhelper != null && !m_controllerhelper.IsEnemyCamp(target))
                    {
                        //if (sendTips)
                        //{
                        //    SendTips("同一个阵营不能攻击");
                        //}
                        nSkillError = (int)LocalTextType.Skill_Commond_feididuizhenyingbunenggongji;
                        Log.LogGroup("ZDY", "不是敌对阵营不能攻击");
                        return false;
                    }
                }
                else if (pkmodel == PLAYERPKMODEL.PKMODE_M_JUSTICE)
                {
                    GameCmd.enumGoodNess mainPlayerGoodNess = (GameCmd.enumGoodNess)m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.GoodNess);
                    GameCmd.enumGoodNess targetGoodNess = (GameCmd.enumGoodNess)target.GetProp((int)PlayerProp.GoodNess);
                    //GoodNess_Badman Color.yellow;
                    //GoodNess_Normal Color.white;
                    //GoodNess_Rioter  Color.gray;
                    //GoodNess_Evil Color.red;
                    if (mainPlayerGoodNess == GameCmd.enumGoodNess.GoodNess_Normal || GameCmd.enumGoodNess.GoodNess_Badman == mainPlayerGoodNess)
                    {
                        if (targetGoodNess != GameCmd.enumGoodNess.GoodNess_Rioter && GameCmd.enumGoodNess.GoodNess_Evil != targetGoodNess)
                        {
                            //if (sendTips)
                            //{
                            //    SendTips("目标不符合善恶规则，不能攻击");
                            //}
                            nSkillError = (int)LocalTextType.Skill_Commond_mubiaobufuheshaneguizebunenggongji;
                            Log.LogGroup("ZDY", "目标不符合善恶规则，不能攻击");
                            return false;
                        }
                    }
                    else if (mainPlayerGoodNess == GameCmd.enumGoodNess.GoodNess_Rioter || GameCmd.enumGoodNess.GoodNess_Evil == mainPlayerGoodNess)
                    {
                        if (targetGoodNess != GameCmd.enumGoodNess.GoodNess_Normal && GameCmd.enumGoodNess.GoodNess_Badman != targetGoodNess)
                        {
                            //if (sendTips)
                            //{
                            //    SendTips("目标不符合善恶规则，不能攻击");
                            //}
                            nSkillError = (int)LocalTextType.Skill_Commond_mubiaobufuheshaneguizebunenggongji;
                            Log.LogGroup("ZDY", "目标不符合善恶规则，不能攻击");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        void SendTips(string str, uint errorID = 0)
        {
            stTipsEvent en = new stTipsEvent();
            en.tips = str;
            en.errorID = errorID;
            EventEngine.Instance().DispatchEvent((int)GameEventID.TIPS_EVENT, en);
        }
        /// <summary>
        /// 检查目标是否能攻击
        /// </summary>
        /// <param name="target">要攻击的目标</param>
        /// <param name="nSkillError">技能无法使用的错误码</param>
        /// <param name="bIgnoreDead">是否忽略目标死亡对象，默认不忽略  判断尸体的时候才忽略</param>
        /// <returns></returns>
        public bool CheckCanAttackTarget(IEntity target, out int nSkillError, bool bIgnoreDead = false)
        {
            nSkillError = 0;
            if (target == null)
            {
                Log.LogGroup("ZDY", "be attack target is null");
                return false;
            }
            if (!bIgnoreDead)
            {
                if (target.IsDead())
                {
                    Log.LogGroup("ZDY", "be attack target is IsDead");
                    return false;
                }
            }


            if (target is IPlayer)
            {
                int stateBit = target.GetProp((int)PlayerProp.StateBit);
                if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableAttacked) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableAttacked)
                {
                    nSkillError = (int)LocalTextType.Skill_Commond_dangqianzhuangtaibunengshiyongjineng;
                    Log.LogGroup("ZDY", "服务器不让攻击");
                    return false;
                }
                int entityState = target.GetProp((int)EntityProp.EntityState);

                if ((stateBit & (int)GameCmd.SceneEntryState.SceneEntry_Hide) == (int)GameCmd.SceneEntryState.SceneEntry_Hide)
                {
                    Log.LogGroup("ZDY", "隐身不能作为选择目标");
                    return false;
                }

                return CheckPlayerCanAttack(target, out nSkillError);
            }
            else if (target is IRobot)
            {
                return CheckPlayerCanAttack(target, out nSkillError);
            }
            else
            {
                INPC npc = target as INPC;
                if (npc == null)
                {
                    Log.LogGroup("ZDY", "npc is null");
                    return false;
                }

                IControllerHelper ch = GetControllerHelper();
                if (ch == null)
                {
                    Log.Error("ch is null");
                    return false;
                }

                if (npc.IsCanAttackNPC())
                {
                    if (npc.IsPet() || npc.IsSummon())
                    {
                        int masterID = npc.GetProp((int)NPCProp.Masterid);
                        IEntitySystem es = m_ClientGlobal.GetEntitySystem();
                        if (es != null)
                        {
                            IPlayer p = es.FindEntity<IPlayer>((uint)masterID);
                            if (p != null)
                            {
                                return CheckPlayerCanAttack(p, out nSkillError);
                            }
                            //找不到主人 不能打
                            return false;
                        }
                    }
                    else
                    {
                        bool beAttack = CheckNpcCanAttackByCamp(npc);
                        if (!beAttack)
                        {
                            Log.LogGroup("ZDY", "不符合npc表中pk规则");
                        }
                        return beAttack;
                    }

                }
                else
                {
                    Log.LogGroup("ZDY", "不能攻击的npc");
                    return false;
                }
            }
            return false;
        }
        Dictionary<string, List<int>> camDic = new Dictionary<string, List<int>>();
        List<int> GetCampArray(string str)
        {
            if (camDic.ContainsKey(str))
            {
                return camDic[str];
            }
            else
            {
                string[] campArray = str.Split('_');
                List<int> campList = new List<int>();
                for (int i = 0; i < campArray.Length; i++)
                {
                    int id = 0;
                    if (int.TryParse(campArray[i], out id))
                    {
                        campList.Add(id);
                    }
                }
                camDic.Add(str, campList);
                return campList;
            }
        }
        bool CheckNpcCanAttackByCamp(INPC npc)
        {
            uint campID = 1;//player阵营
            if (Master is INPC)
            {
                INPC attacker = Master as INPC;
                int baseID = attacker.GetProp((int)EntityProp.BaseID);
                NpcDataBase db = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)baseID);
                if (db == null)
                {
                    Log.LogGroup("ZDY", "npcdb == null");
                    return false;
                }
                campID = db.npcCamp;

            }
            NpcCampDataBase campDb = GameTableManager.Instance.GetTableItem<NpcCampDataBase>(campID);
            if (campDb != null)
            {
                int npcID = npc.GetProp((int)EntityProp.BaseID);
                NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<NpcDataBase>((uint)npcID);
                if (npcdb == null)
                {
                    Log.LogGroup("ZDY", "npcdb == null");
                    return false;
                }
                uint npccampID = npcdb.npcCamp;
                if (npccampID == 14)
                {//动态阵营
                    IControllerHelper ch = GetControllerHelper();
                    if (ch != null)
                    {//
                        if (ch.IsCampFight())
                        {
                            //阵营战走单独的逻辑
                            if (ch.IsEnemyCamp(npc))
                            {
                                return true;
                            }
                            Log.LogGroup("ZDY", "同一个阵营不能攻击");
                            return false;
                        }
                        else
                        {
                            if (ch != null && ch.IsSameFamily(npc))
                            {
                                Log.LogGroup("ZDY", "同一个阵营不能攻击");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                List<int> campArray = GetCampArray(campDb.campStr);
                int campIndex = (int)npccampID - 1;
                if (campIndex < campArray.Count)
                {
                    int ruleID = campArray[campIndex];
                    // if (int.TryParse(campArray[campIndex], out ruleID))
                    {
                        if (ruleID == 10000)
                        {

                        }
                        else if (ruleID == 0)
                        {
                            return false;
                        }
                        else if (ruleID == 100)
                        {
                            return false;
                        }
                        else if (ruleID == -100)
                        {
                            return true;
                        }
                        else if (ruleID == -101)
                        {
                            PLAYERPKMODEL pkmodel = (PLAYERPKMODEL)m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.PkMode);
                            if (pkmodel == PLAYERPKMODEL.PKMODE_M_FREEDOM)
                            {
                                return true;
                            }
                            return false;
                        }
                    }

                }
            }
            return false;
        }
        /// <summary>
        /// 判断技能是否可用
        /// </summary>
        /// <param name="param">skillid</param>
        /// <returns></returns>
        bool VoteSkillCanUse(uint paramSkillID, out int errorID, out string tips)
        {
            errorID = 0;
            tips = "";
            if (!m_ClientGlobal.IsMainPlayer(Master))
            {
                return true;
            }

            IPlayer player = Master as IPlayer;
            if (player == null)
            {
                Log.LogGroup("ZDY", "skill vote palyer is null");
                return false;
            }
            if (player.IsDead())
            {
                Log.LogGroup("ZDY", "skill vote palyer is dead");
                return false;
            }
            int stateBit = player.GetProp((int)PlayerProp.StateBit);
            CreatureState cratureState = player.GetCurState();
            if (cratureState == CreatureState.BeginDead)
            {
                Log.LogGroup("ZDY", "BeginDead cannot use skill");
                return false;
            }
            uint skillid = (uint)paramSkillID;

            if (!IsCombo(skillid))
            {//非普攻技能才判断
                if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableSkill) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableSkill)
                {//
                    Log.LogGroup("ZDY", "当前状态不能使用非普攻技能 MainUserStateBit_UnableSkill");
                    errorID = (int)LocalTextType.Skill_Commond_dangqianzhuangtaibunengshiyongjineng;
                    return false;
                }
            }

            if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableAllSkill) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableAllSkill)
            {//
                Log.LogGroup("ZDY", "当前状态不能使用所有技能 MainUserStateBit_UnableAllSkill");
                errorID = (int)LocalTextType.Skill_Commond_dangqianzhuangtaibunengshiyongjineng;
                return false;
            }


            //check cd
            bool cding = EventEngine.Instance().DispatchVote((int)GameVoteEventID.SKILL_CDING, skillid);
            if (cding)
            {
                Log.LogGroup("ZDY", "技能冷却中");
                errorID = (int)LocalTextType.Skill_Commond_jinenglengquezhong;
                return false;
            }
            //check mp
            SkillDatabase db = GetSkillDataBase((uint)paramSkillID);
            if (db != null)
            {
                if (db.dwMoveType != 0)
                {
                    if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove)
                    {//
                        Log.LogGroup("ZDY", "服务器不让用走  MainUserStateBit_UnableMove statebit is {0}", stateBit);
                        return false;
                    }
                    if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove)
                    {//
                        Log.LogGroup("ZDY", "服务器不让用走MainUserStateBit_SerUnableMove staebit is {0}", stateBit);
                        return false;
                    }
                }
                int mp = this.GetMaster().GetProp((int)CreatureProp.Mp);
                int maxMp = GetMaster().GetProp((int)CreatureProp.MaxMp);
                uint needmp = db.costsp + (uint)(db.costspprob * 0.0001f * (uint)maxMp);
                if (needmp > mp)
                {
                    Log.LogGroup("ZDY", "法术值不足");
                    errorID = (int)LocalTextType.Skill_Commond_fashuzhibuzu; ;

                    return false;
                }
                string needstr = db.skillPlayCostItem;
                string[] itemArray = needstr.Split('_');
                if (itemArray.Length > 1)
                {
                    uint itemID = 0, itemCount = 0;
                    if (uint.TryParse(itemArray[0], out itemID))
                    {
                        if (uint.TryParse(itemArray[1], out itemCount))
                        {
                            IControllerHelper ch = GetControllerHelper();
                            if (ch != null)
                            {
                                if (!ch.HasEnoughItem(itemID, (int)itemCount))
                                {
                                    ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                                    if (idb != null)
                                    {
                                        tips = idb.itemName;
                                        Log.LogGroup("ZDY", "道具不足");
                                        errorID = 6;//x道具不足
                                        return false;
                                    }
                                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                                    {
                                        Log.LogGroup("ZDY", "skill vote itemdatabase is null id is " + itemID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            SkillState curSkillState = (SkillState)stateMachine.GetCurStateID();

            float tmeptime = Time.realtimeSinceStartup;
            float delta = tmeptime - SkillStartPlayTime;
            //if (IsCombo(skillid))
            //{
            //    if (m_bTargetChange)
            //    {
            //        SkillDoubleHitDataBase ddb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(CurSkillID);
            //        if (ddb != null)
            //        {//切换目标 要等到一招播放完毕才能释放下一招
            //            if (delta * 1000 < ddb.doublehitend)
            //            {
            //                Log.LogGroup("ZDY", "切换目标 要等到一招播放完毕才能释放下一招 {1} {0}", Time.realtimeSinceStartup, skillid);
            //                return false;
            //            }
            //            else
            //            {
            //                m_bTargetChange = false;
            //                return true;
            //            }
            //        }
            //    }//注释以上代码 用现在的代码 会导致切换目标时连击 动作播放打断 两个特效 解开 会导致挂机时普攻连击连不起来  暂时用下面的方案
            if (IsCombo(skillid))
            {
                return true;
            }
            //}


            if (curSkillState != SkillState.None)
            {
                if (curSkillState == SkillState.Prepare)
                {
                    Log.LogGroup("ZDY", "当前状态不能使用技能  curSkillState == SkillState.Prepare");
                    errorID = (int)LocalTextType.Skill_Commond_dangqianzhuangtaibunengshiyongjineng;
                    return false;
                }

                db = GetCurSkillDataBase();
                if (db == null)
                {
                    return true;
                }
                else
                {
                    if (db.useSkillType == (int)UseSkillType.GuideSlider || db.useSkillType == (int)UseSkillType.GuideNoSlider)
                    {
                        if (curSkillState == SkillState.Attack)
                        {//引导技能判断
                            Log.LogGroup("ZDY", "引导技能判断 技能状态不正确");
                            errorID = (int)LocalTextType.Skill_Commond_dangqianzhuangtaibunengshiyongjineng;
                            return false;
                        }
                    }
                }
                uint temp = (uint)(skillStiffTime * 1000);
                uint tableStiffTime = db.wdStiffTime;
                if (IsCommonSkill(CurSkillID))
                {//普攻的开始转换时间 就算是僵直时间
                    SkillDoubleHitDataBase skilldoubleDb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(CurSkillID);
                    if (skilldoubleDb != null)
                    {
                        tableStiffTime = skilldoubleDb.doublehitBegin;
                    }
                }
                if (temp <= (tableStiffTime))
                {
                    //errorID  = (int)LocalTextType.Skill_Commond_yingzhishijianneibunengshiyongjineng;
                    Log.LogGroup("ZDY", "硬直时间内不能放技能");
                    //m_ClientGlobal.GetControllerSystem().GetCombatRobot().AddTips(string.Format("{2}僵直时间未完 {0} {1}", Time.realtimeSinceStartup, temp, skillid));
                    return false;
                }
                else
                {
                    //  Log.LogGroup("ZDY", "id is " + db.wdID + " 僵直时间是" + db.wdStiffTime + " 已经用的时间是" + temp);
                    //  Log.Trace("stateid is *********" + stateMachine.GetCurStateID().ToString() + "超越僵直时间 可以移动 ");
                    return true;
                }
            }
            return true;
        }
        bool VoteCallBack(int nEventID, object param)
        {
            if (!m_ClientGlobal.IsMainPlayer(Master))
            {
                return true;
            }
            int stateBit = 0;
            IPlayer player = Master as IPlayer;
            if (player != null)
            {
                stateBit = player.GetProp((int)PlayerProp.StateBit);
            }
            CreatureState cratureState = player.GetCurState();
            if (nEventID == (int)GameVoteEventID.SKILL_CANUSE)
            {
                uint skillID = (uint)param;
                int error = 0;
                string tips = "";
                return VoteSkillCanUse(skillID, out error, out tips);

            }
            else if (nEventID == (int)GameVoteEventID.SEND_STOPMOVE)
            {
                if (stateMachine.GetCurStateID() != (int)SkillState.None)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (nEventID == (int)GameVoteEventID.RIDE_CANRIDE)
            {
                if (stateMachine.GetCurStateID() != (int)SkillState.None)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (nEventID == (int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE)
            {
                if (player.IsDead())
                {
                    return false;
                }
                if (cratureState == CreatureState.BeginDead)
                {
                    Log.LogGroup("ZDY", "BeginDead cannot move");
                    return false;
                }
                if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_UnableMove)
                {//
                    Log.LogGroup("ZDY", "服务器不让用走  MainUserStateBit_UnableMove statebit is {0}", stateBit);
                    return false;
                }
                if ((stateBit & (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove) == (int)GameCmd.MainUserStateBit.MainUserStateBit_SerUnableMove)
                {//
                    Log.LogGroup("ZDY", "服务器不让用走MainUserStateBit_SerUnableMove staebit is {0}", stateBit);
                    return false;
                }
                if (CurSkillID == 0)
                {
                    // Log.Info( " CurSkillID == 0  可以移动 " );
                    return true;
                }

                SkillDatabase db = GetCurSkillDataBase();
                if (db != null)
                {
                    //db.wdStiffTime  僵直时间加上50毫秒的动作编辑器出现的误差
                    uint temp = (uint)(skillStiffTime * 1000);
                    if (temp < (db.wdStiffTime))
                    {
                        if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                        {
                            Log.LogGroup("ZDY", "僵直时间未完 已用时间是" + temp);
                        }
                        return false;
                    }
                    else
                    {
                        if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                        {
                            //  string stateName = Enum.GetName(typeof(SkillState), skillState);
                            Log.LogGroup("ZDY", "stateid is *********" + "超越僵直时间 可以移动 ");
                        }
                        return true;
                    }
                }

            }


            return true;
        }
        public bool IsFastMove()
        {
            SkillDatabase db = GetCurSkillDataBase();
            if (db != null)
            {
                int stateID = stateMachine.GetCurStateID();
                //Log.Info("fastmove stateid is " + stateID.ToString());
                //   if (db.dwMoveType == (int)SkillMoveType.FastMove && stateMachine.GetCurStateID() == (int)SkillState.Attack)
                //服务器gotopos消息来两次 导致GetCurStateID() == (int)SkillState.Attack判断失败 会设置人物的位置 
                if (db.dwMoveType == (int)Client.SkillMoveType.FastMove)
                {
                    Log.LogGroup("ZDY", "is fast moving");
                    return true;
                }
            }
            return false;
        }
        void SliderEvent(int eventID, object param)
        {
            if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSEND)
            {
                stGuildEnd guildEnd = (stGuildEnd)param;
            }
        }
        void CreateStateMachine()
        {
            stateMachine = new StateMachine<ISkillPart>(this);
            stateMachine.RegisterState(new SkillNoneState(stateMachine, this));
            stateMachine.RegisterState(new SkillPrepareState(stateMachine, this));
            stateMachine.RegisterState(new SkillAttackState(stateMachine, this));
            stateMachine.RegisterState(new SkillOverState(stateMachine, this));
            stateMachine.ChangeState((int)SkillState.None, null);

        }
        public bool IsMainPlayer()
        {
            if (m_Master == null)
            {
                return false;
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return false;
            }
            if (m_ClientGlobal.MainPlayer.GetID() == m_Master.GetID())
                return true;
            return false;
        }
        public void AddCurSkill(SkillDatabase database)
        {
            if (curSkillDic.ContainsKey(database.wdID))
            {
                curSkillDic[database.wdID] = database;
            }
            else
            {
                curSkillDic.Add(database.wdID, database);
            }
        }
        /// <summary>
        ///设置当前状态用户拥有的技能id
        /// </summary>
        /// <param name="idlist"></param>
        public void SetCurStateList(List<uint> idlist)
        {
            IDList = idlist;
        }
        List<uint> IDList = new List<uint>();
        /// <summary>
        /// 返回当前状态主角可以使用的技能数据
        /// </summary>
        /// <returns></returns>
        public List<SkillDatabase> GetCurStateSkillList()
        {
            List<SkillDatabase> list = new List<SkillDatabase>();

            for (int i = 0; i < IDList.Count; i++)
            {
                var id = IDList[i];
                var iter = curSkillDic.GetEnumerator();
                while (iter.MoveNext())
                {
                    var dic = iter.Current;
                    if (id == dic.Key)
                    {
                        list.Add(dic.Value);
                    }
                }
            }
            return list;
        }
        private SkillDatabase GetSkillDataBase(uint skillID)
        {
            if (skillID == 0)
            {
                //Log.Error( "skillid is 0" );
                return null;
            }

            if (curSkillDic.ContainsKey(skillID))
            {
                return curSkillDic[skillID];
            }
            else
            {
                Log.Info("cufskill dic not contain key " + skillID.ToString());
                return null;
            }
        }



        public void Reset()
        {
            stateMachine.ChangeState((int)SkillState.None, null);
        }
        // @brief 获取PartID
        public EntityPart GetPartID()
        {
            return m_ePartID;
        }

        public SkillEffectProp RegisterSkill(uint skillID)
        {
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
            uint itemId = skillID;

            SkillEffectProp effect = SkillEffectManager.Instance.GetEffectPropBySkillID(skillID);
            if (effect != null)
            {
                if (!m_skillEffectDict.ContainsKey((int)itemId))
                {
                    m_skillEffectDict.Add((int)itemId, effect);

                    if (m_Master.GetEntityType() == EntityType.EntityType_NPC || m_Master.GetEntityType() == EntityType.EntityTYpe_Robot)
                    {
                        if (!CurSkillDic.ContainsKey(itemId))
                        {
                            CurSkillDic.Add(itemId, db);
                        }
                    }
                }
                // effect.GatherResFile(ref lstResource, true);
            }
            return effect;
        }
        /**
        @brief 创建
        @param master 宿主
        */
        public bool Create(IEntity master)
        {
            if (master == null)
            {
                Log.Error("create skillpart fail master is null");
                return false;
            }
            m_Master = master;

            List<uint> skillIDList = GetSkillList(master);

            List<string> lstResource = new List<string>();
            for (int i = 0; i < skillIDList.Count; i++)
            {
                uint skillID = skillIDList[i];
                RegisterSkill(skillID);
            }

            // 预加载技能特效资源
            //             for (int i = 0; i < lstResource.Count; ++i)
            //             {
            //                 string strEffect = lstResource[i].Replace(".prefab", ".fx");
            //                 Engine.RareEngine.Instance().PreloadEffectObj(strEffect, 1);
            //             }
            ////////////////////////////////////////////////////////////////////////////////


            m_caster = new SkillCaster();
            m_caster.Init(this);
            CreateStateMachine();

            if (m_ClientGlobal != null)
            {
                Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnPlayerDie);

                if (m_ClientGlobal.IsMainPlayer(m_Master))
                {
                    EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, SliderEvent);
                    EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, SliderEvent);
                    EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBACK, SliderEvent);

                    EventEngine.Instance().AddEventListener((int)GameEventID.ROBOTCOMBAT_STOP, OnEvent);
                    EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
                    EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);

                    EventEngine.Instance().AddEventListener((int)GameEventID.SKLL_LONGPRESS, OnEvent);

                    EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, EntityStopMove);
                    EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYMOVE, EntityStopMove);

                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteCallBack);
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, VoteCallBack);
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.CHANGE_ANI, VoteCallBack);
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SEND_STOPMOVE, VoteCallBack);
                    EventEngine.Instance().AddVoteListener((int)GameVoteEventID.RIDE_CANRIDE, VoteCallBack);
                }
                EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINDEAD, OnEvent);


            }
            return true;
        }
        /**
    @brief 
    @param 
    */
        public void Release()
        {
            stateMachine.ChangeState((int)SkillState.None, null);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnPlayerDie);


            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, SliderEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, SliderEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBACK, SliderEvent);

            EventEngine.Instance().RemoveEventListener((int)GameEventID.ROBOTCOMBAT_STOP, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKLL_LONGPRESS, OnEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINDEAD, OnEvent);

            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, EntityStopMove);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYMOVE, EntityStopMove);

            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteCallBack);
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, VoteCallBack);
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.CHANGE_ANI, VoteCallBack);
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.SEND_STOPMOVE, VoteCallBack);
            EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.RIDE_CANRIDE, VoteCallBack);

            m_skillEffectDict.Clear();
            m_skillEffectDict = null;
            m_comboSkillDict.Clear();
            m_comboSkillDict = null;
            CurSkillDic.Clear();
            CurSkillDic = null;
        }
        void OnEvent(int eventID, object param)
        {
            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }
            long userUID = m_ClientGlobal.MainPlayer.GetUID();
            if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
            {
                stEntityBeginMove move = (stEntityBeginMove)param;
                if (userUID == move.uid)
                {
                    table.SkillDatabase db = GetCurSkillDataBase();
                    if (db != null)
                    {
                        int stateID = GetCurSkillState();
                        if (stateID == (int)SkillState.Prepare && db.breakType == (uint)SkillBreakType.MoveCanBreak)
                        {
                            BreakSkill();
                            if (db.useSkillType == (int)UseSkillType.Sing || db.useSkillType == (int)(UseSkillType.GuideSlider) || db.useSkillType == (int)UseSkillType.GuideNoSlider)
                            {
                                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSBREAK,
 new stGuildBreak() { action = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ, uid = m_ClientGlobal.MainPlayer.GetID(), skillid = CurSkillID });
                                GameCmd.stNotifyUninterruptEventMagicUserCmd_CS cmd = new GameCmd.stNotifyUninterruptEventMagicUserCmd_CS();
                                cmd.etype = stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Break;
                                m_ClientGlobal.netService.Send(cmd);
                            }
                        }

                    }
                }
            }
            else if (eventID == (int)GameEventID.ROBOTCOMBAT_STOP)
            {
                //BreakSkill();
            }
            else if (eventID == (int)GameEventID.SKLL_LONGPRESS)
            {
                stSkillLongPress lp = (stSkillLongPress)param;
                if (IsMainPlayer())
                {
                    m_bLongPress = lp.bLongPress;
                    if (!lp.bLongPress)
                    {
                        NextSkillID = 0;
                    }
                }

            }
            else if (eventID == (int)GameEventID.ENTITYSYSTEM_TARGETCHANGE)
            {
                NextSkillID = 0;
                m_bTargetChange = true;
            }
            else if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYBEGINDEAD)
            {
                if (IsMainPlayer())
                {
                    IEntity en = GetSkillTarget();
                    if (en != null)
                    {
                        stEntityBeginDead st = (stEntityBeginDead)param;

                        if (st.uid == en.GetUID())
                        {
                            stSkillLongPress longPress = new stSkillLongPress();
                            longPress.bLongPress = false;
                            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
                        }
                    }
                    else
                    {
                        stSkillLongPress longPress = new stSkillLongPress();
                        longPress.bLongPress = false;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
                    }

                }


            }
        }
        public bool IsHideOtherFx()
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
                {//player
                    return m_bHideOtherFx || m_bHideOtherPlayer;
                }
            }
            return false;
        }
        public bool IsLongPress()
        {
            return m_bLongPress;
        }
        /// <summary>
        /// 处理长按时连击技能id
        /// </summary>
        /// <param name="playerSkill"></param>
        public void OnDoLongPressNextSkillID()
        {

            if (m_bLongPress)
            {
                SkillDoubleHitDataBase curdb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>((uint)CurSkillID);
                if (curdb != null)
                {
                    NextSkillID = (int)curdb.nextskillid;
                }
            }
        }
        List<uint> GetSkillListByJob(int job)
        {
            List<uint> skillList = new List<uint>();
            List<SkillDatabase> list = GameTableManager.Instance.GetTableList<SkillDatabase>();

            for (int i = 0; i < list.Count; i++)
            {
                SkillDatabase db = list[i];
                if (db.dwJob == job)
                {
                    if (!skillList.Contains(db.wdID))
                    {
                        if (db.wdID != 0)
                        {
                            skillList.Add(db.wdID);
                        }

                    }
                }
            }

            return skillList;
        }
        List<uint> GetSkillList(IEntity master)
        {
            List<uint> skillList = new List<uint>();
            List<SkillDatabase> list = GameTableManager.Instance.GetTableList<SkillDatabase>();
            if (master.GetEntityType() == EntityType.EntityType_Player)
            {
                IPlayer player = master as IPlayer;
                int job = player.GetProp((int)PlayerProp.Job);
                List<uint> tempList = GetSkillListByJob(job);
                skillList.AddRange(tempList);
            }
            else if (master.GetEntityType() == EntityType.EntityTYpe_Robot)
            {
                int job = master.GetProp((int)RobotProp.Job);
                List<uint> tempList = GetSkillListByJob(job);
                skillList.AddRange(tempList);
            }
            else if (master.GetEntityType() == EntityType.EntityType_NPC)
            {
                INPC npc = master as INPC;
                if (npc == null)
                {
                    Log.LogGroup("ZDY", "npc is null");
                    return skillList;
                }
                int npcType = npc.GetProp((int)NPCProp.ArenaNpcType);
                int baseID = npc.GetProp((int)EntityProp.BaseID);
                if (npcType == (int)GameCmd.ArenaNpcType.ArenaNpcType_Player)
                {
                    int job = npc.GetProp((int)NPCProp.Job);
                    List<uint> tempList = GetSkillListByJob(job);
                    skillList.AddRange(tempList);
                }
                else
                {
                    List<uint> skillIDList = SkillEffectManager.Instance.GetAllSkillListByNpcID((uint)baseID);
                    skillList.Clear();
                    skillList.AddRange(skillIDList);
                }

            }
            return skillList;
        }

        public void Update(float dt)
        {
            if (stateMachine != null)
                stateMachine.Update(dt);

            int stateID = stateMachine.GetCurStateID();
            // if (stateID != (int)SkillState.None && stateID != (int)SkillState.Prepare)
            {//计算僵直时间 从进入skillattack开始计算
                skillStiffTime += dt;
            }
            if (bSendMoveMsg)
            {//同步技能播放时 不移动
                if (stateMachine.GetCurStateID() == (int)SkillState.None)
                {
                    IController ctrl = GetController();
                    if (tempDstPosCmd != null)
                    {
                        ctrl.MoveToTarget(new Vector3(tempDstPosCmd.x, 0, -tempDstPosCmd.y));
                        bSendMoveMsg = false;
                    }

                }
            }
            if (stateID == (int)SkillState.None)
            {
                if (IsLongPress())
                {
                    EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_REUSESKILLLONGATTACK, null);
                }
            }

        }
        public bool IsCombo(uint skillID)
        {
            SkillDoubleHitDataBase db = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>((uint)skillID);
            if (db != null)
            {
                if (db.nextskillid != 0)
                {

                    return true;
                }

            }
            return false;
        }


        SkillState GetTargetSkillState(IEntity entity)
        {
            ISkillPart skillpart = entity.GetPart(EntityPart.Skill) as ISkillPart;
            return (SkillState)skillpart.GetCurSkillState();
        }

        #region ISkillPart
        public void SetCurSkillID(uint skillID)
        {
            CurSkillID = skillID;
        }
        stGetDstPosDataUserCmd_CS tempDstPosCmd = null;
        public void GetDesPos(stGetDstPosDataUserCmd_CS cmd)
        {
            if (stateMachine == null)
                return;

            bSendMoveMsg = true;
            tempDstPosCmd = cmd;
        }
        bool IsCommonSkill(uint skillID)
        {
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID);
            if (db == null)
            {
                return false;
            }
            return db.isnormal;
        }
        void DoSkillCommond(uint uSkillID)
        {
            if (IsCommonSkill(uSkillID))
            {
                IControllerSystem cs = GetCtrollerSys();
                if (cs == null)
                {
                    return;
                }
                if (cs.GetCombatRobot().Status == CombatRobotStatus.RUNNING)
                {
                    return;
                }
                stSkillLongPress longPress = new stSkillLongPress();
                longPress.bLongPress = true;
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);


            }
        }
        void OnUseSkill(object skillObj)
        {
            uint nSkillID = (uint)skillObj;
            IPlayer player = m_ClientGlobal.MainPlayer;
            if (player == null)
            {
                Log.Error("ReqUseSkill:MainPlayer is null");
                return;
            }


            ISkillPart skillPart = player.GetPart(EntityPart.Skill) as ISkillPart;
            if (skillPart == null)
            {
                Log.Error("ReqUseSkill:skillPart is null");
                return;
            }
            //// 只有主角可以使用技能
            if (!m_ClientGlobal.IsMainPlayer(m_Master))
            {
                return;
            }

            // 各种判断是否可能使用技能

            Client.IController ctrl = GetController();
            if (ctrl == null)
            {
                return;
            }

            SkillDatabase database = GetSkillDataBase(nSkillID);
            if (database == null)
            {
                Log.Error("获取不到技能配置数据 skillid is" + nSkillID.ToString());
                return;
            }

            IEntity target = ctrl.GetCurTarget() as IEntity;
            if (m_Master.GetCurState() != CreatureState.Dead)
            {
                bLive = true;
            }

            if (m_Master is IPlayer)
            {
                Client.IControllerHelper ch = GetControllerHelper();
                if (ch == null)
                {
                    return;
                }
                commonSkillID = ch.GetCommonSkillID();
            }
            DoSkillCommond(nSkillID);
            Vector3 sendPos = Vector3.zero;
            SkillState curState = (SkillState)stateMachine.GetCurState().GetStateID();

            if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
            {
                string stateName = Enum.GetName(typeof(SkillState), curState);
                Log.LogGroup("ZDY", "skillid is " + nSkillID + " curstate is " + stateName + "curskillid is " + CurSkillID);
            }


            if ((curState == SkillState.Prepare || curState == SkillState.Attack) && nSkillID == commonSkillID)
            {//判断连击
                SkillDoubleHitDataBase db = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>((uint)CurSkillID);
                if (db != null)
                {
                    float tmeptime = Time.realtimeSinceStartup;
                    float delta = tmeptime - SkillStartPlayTime;
                    if (db.nextskillid != 0)
                    {
                        float aniTime = delta * 1000;
                        SkillDatabase curdb = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)CurSkillID, 1);
                        if (curdb != null)
                        {
                            if (curdb.wdStiffTime < db.doublehitBegin)
                            {
                                // Log.Error("僵直时间小于连击开始生效时间，连击无法生效 僵直时间是 " + curdb.wdStiffTime + " 连击生效时间 " + db.doublehitBegin + " 技能id " + CurSkillID);
                            }
                        }
                        if (CurSkillID == 103)
                        {
                            // Log.Error("anitime is "+aniTime);
                        }
                        if (aniTime <= (float)db.doublehitend && aniTime >= (float)db.doublehitBegin)
                        {
                            NextSkillID = (int)db.nextskillid;
                            //  Log.LogGroup( "ZDY" , "连击 nextskillid " + NextSkillID.ToString() + " AttackAnimState time   " + AttackAnimState.time + " database.wdStiffTime" + database.wdStiffTime + " name is " + AttackAnimState.name );
                            return;
                        }
                        else
                        {
                            //  NextSkillID = 0;
                            return;
                        }

                    }
                }
            }

            else
            {

                skillTarget = target;
                if (!IsSkillCanUse(nSkillID))
                {
                    Log.LogGroup("ZCX", "不能使用技能{0}IsSkillCanUse", nSkillID);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLGUIDE_PROGRESSBREAK,
                        new Client.stGuildBreak() { action = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ, uid = player.GetID(), skillid = nSkillID, msg = "IsSkillCanUse" });
                    return;
                }
                //判断技能可用才赋值当前技能id
                CurSkillID = nSkillID;
                int skillError = 0;
                bool bCanUse = ctrl.FindTargetBySkillID(CurSkillID, ref sendPos, ref target, out skillError);
                if (!bCanUse)
                {
                    Log.LogGroup("ZCX", "不能使用技能{0}FindTargetBySkillID", nSkillID);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLGUIDE_PROGRESSBREAK,
                        new Client.stGuildBreak() { action = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ, uid = player.GetID(), skillid = nSkillID, msg = "FindTargetBySkillID" });
                    return;
                }
                if (nSkillID == 0)
                {
                    Log.Error("skill id is 0");
                    return;
                }
                StopMove();

                FreeSkill(nSkillID, this, m_uDamageID);
                SyncSkill((uint)nSkillID, target, sendPos);
                //释放技能攻击
                stateMachine.ChangeState((int)SkillState.Prepare, this);
                Log.LogGroup("ZCX", "自动攻击 使用技能{0}成功", nSkillID);
            }

            // 这里再次赋值，是为了终止连击
            CurSkillID = nSkillID;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);


            return;
        }
        void StopMove()
        {
            if (Master.GetCurState() == CreatureState.Move)
            {
                Master.SendMessage(EntityMessage.EntityCommand_StopMove, Master.GetPos());
            }
        }
        public bool HasSkillObstacle(uint skillID, Vector3 targetPos, IEntity target)
        {
            if (Master == null)
            {
                return false;
            }
            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
            if (db == null)
            {
                return false;
            }
            Vector3 myPos = Master.GetPos();
            if (db.targetType == (int)SkillTargetType.Target)
            {
                if (target == null)
                {
                    return false;
                }
                targetPos = target.GetPos();

            }
            IMapSystem ms = m_ClientGlobal.GetMapSystem();

            if (ms != null)
            {
                float dis = 0;
                ms.CalcDistance(myPos, targetPos, out dis, TileType.TILE_FLY_BLOCK);
            }
            return false;
        }
        public void ReqUseSkill(uint nSkillID)
        {

            Client.IControllerHelper ch = GetControllerHelper();
            if (ch != null)
            {
                if (!ch.TryUnRide(OnUseSkill, nSkillID))
                {
                    //  OnUseSkill(nSkillID);
                }
            }
        }
        Client.IControllerHelper GetControllerHelper()
        {
            Client.IControllerSystem ctrlSys = m_ClientGlobal.GetControllerSystem();
            if (ctrlSys == null)
            {
                return null;
            }

            Client.IControllerHelper ch = ctrlSys.GetControllerHelper();
            if (ch == null)
            {
                return null;
            }
            return ch;
        }
        Client.IController GetController()
        {
            Client.IControllerSystem ctrlSys = m_ClientGlobal.GetControllerSystem();
            if (ctrlSys == null)
            {
                return null;
            }

            Client.IController ctrl = ctrlSys.GetActiveCtrl();
            if (ctrl == null)
            {
                return null;
            }
            return ctrl;
        }
        public IEntity GetSkillTargetBySkillID(uint skillID)
        {
            IEntity target = null;
            Client.IController ctrl = GetController();
            if (ctrl == null)
            {
                return null;
            }
            Vector3 sendPos = Vector3.zero;
            int skillerror = 0;
            bool bCanUse = ctrl.FindTargetBySkillID(skillID, ref sendPos, ref target, out skillerror);
            return target;
        }

        public void ReqNpcUseSkill(IEntity st, uint skillid)
        {
            SkillTarget = st;
            SkillDatabase database = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillid);
            if (database == null)
            {
                Log.Error("获取不到技能配置数据");
                return;
            }
            m_uDamageID++;

            if ((CreatureState)m_Master.GetCurState() == CreatureState.Move)
            {
                m_Master.SendMessage(EntityMessage.EntityCommand_StopMove, m_Master.GetPos());
            }
            FreeSkill(skillid, this, m_uDamageID);
            //释放技能攻击
            stateMachine.ChangeState((int)SkillState.Prepare, this);
            CurSkillID = skillid;

        }

        public SkillPartType GetSkillPartType()
        {
            return SkillPartType.SKILL_PLAYERPART;
        }
        public SkillDatabase GetCurSkillDataBase()
        {
            return GetSkillDataBase((uint)CurSkillID);
        }

        public IEntity GetSkillTarget()
        {
            return skillTarget;
        }
        // 获取宿主
        public IEntity GetMaster()
        {
            return m_Master;
        }
        /// <summary>
        /// 获取技能状态
        /// </summary>
        /// <returns></returns>
        public int GetCurSkillState()
        {
            if (stateMachine == null)
                return (int)SkillState.None;
            else
                return stateMachine.GetCurState().GetStateID();
        }
        /// <summary>
        /// 获取人物当前使用的所有技能
        /// </summary>
        /// <returns></returns>
        public Dictionary<uint, SkillDatabase> GetCurSkills()
        {
            return CurSkillDic;
        }
        /// <summary>
        /// 技能是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsSkillCanUse(uint skillID)
        {
            bool canUse = EventEngine.Instance().DispatchVote((int)GameVoteEventID.SKILL_CANUSE, skillID);
            return canUse;
        }
        public Client.stTipsEvent GetSkillNotUseReason(uint skillID)
        {
            stTipsEvent en = new stTipsEvent();
            int error;
            string tips = "";
            VoteSkillCanUse(skillID, out error, out tips);
            en.errorID = (uint)error;
            en.tips = tips;
            return en;
        }

        public Vector3 gotoPos = Vector3.zero;
        public void SetGotoPostion(UnityEngine.Vector3 pos)
        {
            gotoPos = pos;
        }
        #endregion
        public void SetFighting(bool isFight)
        {
            dynamic role = m_Master;
            if (m_Master.GetEntityType() == EntityType.EntityType_NPC)
            {
                INPC player = m_Master as INPC;
                player.IsFighting = isFight;
            }
            else if (m_Master.GetEntityType() == EntityType.EntityType_Player)
            {
                IPlayer player = m_Master as IPlayer;
                player.IsFighting = isFight;
            }
            else if (m_Master.GetEntityType() == EntityType.EntityType_Monster)
            {
                IMonster player = m_Master as IMonster;
                player.IsFighting = isFight;
            }
        }
        public IControllerSystem GetCtrollerSys()
        {
            if (m_ClientGlobal != null)
            {
                return m_ClientGlobal.GetControllerSystem();
            }
            return null;
        }
        public void AddDamage(GameCmd.stMultiAttackDownMagicUserCmd_S cmd, long uid)
        {
            //if (damagerManager != null)
            //{
            //    damagerManager.AddDamage(cmd, uid);
            //}
        }
        public void RemoveDamage(stMultiAttackDownMagicUserCmd_S data, long uid)
        {
            //if (damagerManager != null)
            //{
            //    damagerManager.RemoveDamage(data, uid);
            //}

        }

    }
}
