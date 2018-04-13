using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using UnityEngine;
namespace Client
{
    // 技能系统接口
    #region 技能系统接口
    public interface ISkillSystem
    {
        ISkillPart CreateSkillPart();

        IBuffPart CreateBuffPart();

        bool Init(bool isEditor = false);

        // 游戏退出时使用
        void Release();
    }
    public enum SkillPartType
    {
        SKILL_PLAYERPART,
    }
    public enum SkillCampType
    {
        None = 0 ,//无阵营
        SelfAndFriends ,//自己和友方
        Enemys ,//敌方
        SelfAndTeammate ,//自己和队友
    }
    /*
    0－自己
    1－友方
    2－敌方
    3－召唤物
    4－战魂
    5- 全部
    6- 友方尸体
     */
    public enum SkillTargetObjType
    {
        Self = 0 ,//自己
        Friend ,
        Enemy ,
        Summoned ,
        Pet ,
        All ,
        FriendBody ,
        Master, // 主人
        PetAndMaster,//
        SelfAndTeamer,//9、损血量最大的队友及自己
    }
    /*
     0、目标    1、目标点
  2－目标前方攻击距离的点
     */
    public enum SkillTargetType
    {
        Target = 0 ,
        TargetPoint ,
        TargetForwardPoint ,
        MyPoint ,
    }
    public enum SkillMoveType
    {
        NoMove = 0,//不移动
        FastMove = 1,//快速移动
        AtOnceMove = 2,//瞬移
        SkillOverMove = 3,//技能释放完毕移动
    }
    public enum SkillState
    {
        None = 0 ,
        Prepare , //准备期，如吟唱等可被打断的前奏阶段
        Attack ,  //攻击中
        Over ,    //连击收招
    }
    public enum SkillSettingState
    {
        None = 0,//通用状态
        StateOne = 1,//状态一
        StateTwo = 2,//状态二
    }
    [Flags]
    public enum UseMethod
    {//释放方式 客户端未必用
        Passive = 0,//被动
        Immediately = 1,//立即释放
        ContinueLock = 2,//连续锁定
        Area = 3,//区域范围
        Line = 4,//直线范围
        Friend = 5,//友
        Enemy = 6,//敌人

    }
    [Flags]
    public enum UseSkillType
    {
        Immediately = 0 ,//立即释放
        Sing = 1,//读条 吟唱
        GuideSlider = 2 ,//引导
        GuideNoSlider = 3,//引导没有进度条
    }
    [Flags]
    public enum SkillBreakType
    {
        CannotBreak = 0, //不能打断
        CanBreak = 1,//能打断
        MoveCanBreak = 4,//移动可打断
    }
    public interface ISkillPart : IEntityPart
    {
        // 使用技能
        void ReqUseSkill(uint uSkillID);

        void ReqNpcUseSkill(IEntity skillTarget, uint skillid);
        // 获取宿主
        IEntity GetMaster();

        SkillPartType GetSkillPartType();

        //// 伤害处理
        void OnDamage(GameCmd.stMultiAttackDownMagicUserCmd_S cmd);

        //// 技能预备阶段
        void OnPrepareUseSkill(GameCmd.stPrepareUseSkillSkillUserCmd_S cmd);

        // 技能打断通知 进度条通知
        void OnInterruptSkill(uint uUserID,uint uTime,uint uType,uint actionType);

        // 进度条结束通知
        void OnInterruptEventSkill(uint uEventType);
        /// <summary>
        /// 获取当前使用技能的配置数据
        /// </summary>
        SkillDatabase GetCurSkillDataBase();

        int GetCurSkillState();

        void Reset();
        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="skillLev"></param>
        /// <param name="coldtime"></param>
        void OnAddSkill(uint skillID , uint skillLev , uint coldtime);
        /// <summary>
        /// 技能使用失败
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="skillid"></param>
        void OnUseSkillFailed(uint userid , uint skillid);
        /// <summary>
        /// 获取技能的目标
        /// </summary>
        /// <returns></returns>
        IEntity GetSkillTarget();
        /// <summary>
        /// 获取人物当前使用的所有技能
        /// </summary>
        /// <returns></returns>
        Dictionary<uint , SkillDatabase> GetCurSkills();
        /// <summary>
        /// 技能是否可用
        /// </summary>
        /// <returns></returns>
        bool IsSkillCanUse(uint skillID);
        /// <summary>
        /// 获取技能不能用的原因
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        Client.stTipsEvent GetSkillNotUseReason(uint skillID);
        void GetDesPos(stGetDstPosDataUserCmd_CS cmd);
        /// <summary>
        /// 记录要goto的位置
        /// </summary>
        /// <param name="pos"></param>
        void SetGotoPostion(Vector3 pos);
        bool IsFastMove();
        /// <summary>
        /// 根据技能id 获取目标
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        IEntity GetSkillTargetBySkillID(uint skillID);

        void SetCurSkillID(uint skillID);
        /// <summary>
        ///设置当前状态用户挂机拥有的技能id
        /// </summary>
        /// <param name="idlist"></param>
        void SetCurStateList(List<uint> idlist);

        /// <summary>
        /// 返回当前状态主角可以挂机使用的技能数据
        /// </summary>
        /// <returns></returns>
        List<SkillDatabase> GetCurStateSkillList();
        /// <summary>
        /// 检查目标是否能攻击
        /// </summary>
        /// <param name="target">要攻击的目标</param>
        /// <param name="nSkillError">技能无法使用的错误码</param>
        /// <param name="bIgnoreDead">是否忽略目标死亡对象，默认不忽略  判断尸体的时候才忽略</param>
        /// <returns></returns>
        bool CheckCanAttackTarget(IEntity target, out int nSkillError, bool bIgnoreDead = false);

        void RemoveDamage(stMultiAttackDownMagicUserCmd_S data, long uid);

        void AddDamage(stMultiAttackDownMagicUserCmd_S cmd, long uid);

        bool IsHideOtherFx();

        int FxLevel
        {
            get;
        }
        /// <summary>
        /// 使用宠物技能接口 主要和人物的tmpid保持自增一致
        /// </summary>
        /// <param name="cmd"></param>
        void RequestUsePetSkill(stMultiAttackUpMagicUserCmd_C cmd);
    }
    #endregion

#region Buff
    [Flags]
    public enum BuffEffectType
    {
        CANNOT_MOVE = 1<<0,//无法移动			
        CANOT_USESKILL = 1<<1,//无法使用技能			
        CANNOT_USEATTACKANDSKILL = 1<<2,		//无法使用技能和普攻	
        CANNOT_USEITEM = 1<<3,		//无法使用物品	
    }

    public enum BuffBigType
    {
        NoneControl = 0,//非控制
        Control,        //控制
    }
    public enum BuffSmallType
    {
        None = 0,
        XuanYun = 1, //眩晕
    }
    public interface IBuffPart:IEntityPart
    {
        /// <summary>
        /// 状态信息下行消息
        /// </summary>
        /// <param name="cmd"></param>
        void ReciveBuffState(stSendStateIcoListMagicUserCmd_S cmd);
        /// <summary>
        /// 取消某个对象身上的状态图标
        /// </summary>
        /// <param name="cmd"></param>
        void CancleBuffState(stCancelStateIcoListMagicUserCmd_S cmd);
        /// <summary>
        /// 获取身上的buff
        /// </summary>
        /// <returns></returns>
        Dictionary<uint , stStateValue> GetBuffdic();
        /// <summary>
        /// 获取buff配置表通过thisid
        /// </summary>
        /// <param name="thisID"></param>
        /// <returns></returns>
        BuffDataBase GetBuffDataBase(uint thisID);
        /// <summary>
        ///获取buff效果类型  
        /// </summary>
        /// <returns>所有的buff的效果类型的“与”值</returns>
        uint GetBuffEffectType();

        /**
        @brief 获取buff列表
        @param
        */
        void GetBuffList(out List<Client.stAddBuff> lstBuff);

        void ReceiveBuffList(List<stStateValue> list);
    }
#endregion
}