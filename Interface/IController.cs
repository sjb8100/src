using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client
{
    public interface IClickSink
    {
        void OnClickEntity(IEntity entity);
    }

    public interface IController
    {
        // 设置控制对象
        void SetHost(IEntity entity);

        // 设置点击回调
        void SetClickSink(IClickSink sink);

        // 消息处理
        void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null);

        // 获取当前目标对象
        IEntity GetCurTarget();

        void UpdateTarget(IEntity target);
        // 移动到指定位置(同地图)
        bool MoveToTarget(Vector3 vTarget, object moveParam = null,bool showTip = false);

        // 移动到指定位置(支持跨地图)
        void GotoMap(uint uMapID, Vector3 vTarget);

        //直接跳转到指定位置(支持跨地图)
        void GotoMapDirectly(uint uMapID, Vector3 vTarget, uint taskId);

        // 走向npc并访问(支持跨地图)
        void VisiteNPC(uint uMapID, uint npcid, bool b);

        void VisiteNPC(long uid);
        //直接跳转到指定位置
        void VisiteNPC(uint uMapID, uint npcid);

        void VisiteNPCDirectly(uint uMapID, uint npcid,uint taskid,uint x,uint y);
        /// <summary>
        /// 根据技能id找目标
        /// </summary>
        /// <param name="skillID">技能id</param>
        /// <param name="pos">发送给服务器的位置</param>
        /// <param name="en">找到的目标</param>
        /// <param name="en">找到的目标</param>
        /// <returns>true表示可以使用技能</returns>
        bool FindTargetBySkillID(uint skillID , ref Vector3 pos , ref IEntity en,out int errorID);

        void EnterMapImmediacy(uint mapid,uint npcid,uint x,uint y);
        void GoToMapPosition(uint uMapID,uint qwThisID,Vector3 vTarget);
    }


    public enum CombatRobotStatus
    {
        STOP,
        RUNNING,
        PAUSE,
    }
    public interface ICombatRobot
    {
        void Start();

        void StartWithTarget(int ntargetId);

        void StartInCopy(uint nCopyId, uint nWaveid, uint nPosIndex);

        void Stop(bool stopTimer);

        void Stop();

        void Pause();

        void Resume();
        /// <summary>
        /// 点击技能 插入技能
        /// </summary>
        /// <param name="skillID"></param>
        void InsertSkill(uint skillID);
        void AddTips(string msg);
        CombatRobotStatus Status
        {
            get;
        }
        int TargetId
        {
            get;
        }
    }

    public interface IControllerSystem
    {
        void ActiveController(ControllerType ct = ControllerType.ControllerType_KeyBoard);

        IController GetActiveCtrl();

        ICombatRobot GetCombatRobot();
        ISwitchTarget GetSwitchTargetCtrl();
        // 消息处理
        void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null);

        void SetControllerHelper(IControllerHelper ch);
        IControllerHelper GetControllerHelper();
        // 游戏退出时使用
        void Release();
    }

    public interface ISwitchTarget
    {
        void Switch();

        void Clear();

        void Relese();
    }

    public interface IControllerHelper
    {
        void InitSetting();

        /// <summary>
        /// //类型 0-物品本身cd  1-组cd
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseid"></param>
        /// <param name="cd">sec</param>
        void SetMedicineCDInfo(uint baseid,List<GameCmd.CDInfo> cdinfos);
        float GetItemCDByThisId(uint qwThisId);
        void CheckUseMedicine();
        //3)	团队
        bool IsSameTeam(IEntity target);
        //4)	氏族
        bool IsSameFamily(IEntity target);
        //5)	阵营
        bool IsSameCamp(IEntity target);
        //是不是敌对阵营
        bool IsEnemyCamp(IEntity target);

        //npc 是不是宠物
        bool NpcIsMyPet(IEntity npc);
        /// <summary>
        /// 获取自己出站的宠物的npcid
        /// </summary>
        /// <returns></returns>
        uint GetMyPetNpcID();

        bool TryUnRide(Action<object> callback,object param);
        bool TryRide(Action<object> callback, object param);
        ITipsManager GetTipsManager();

        uint GetCommonSkillID();

        bool IsSameTeam(uint nteamid);

        bool IsTeamerCanPick(uint nteamid);

        uint GetCopyLastKillWave();

        bool HasEnoughItem(uint itemBaseID, int needCount);
        /// <summary>
        /// 通过玩家id获取所属的npc列表
        /// </summary>
        /// <param name="playerID">玩家的id</param>
        /// <returns></returns>
        void GetOwnNpcByPlayerID(uint playerID, ref List<INPC> lstNPC);
        /// <summary>
        /// 通过玩家id获取所属宠物
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        INPC GetOwnPetByPlayerID(uint playerID);
        /// <summary>
        /// 通过玩家id获取所属召唤物
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        INPC GetOwnSummonByPlayerID(uint playerID);

        /// <summary>
        /// 背包是否已经满了
        /// </summary>
        /// <returns></returns>
        bool CanPutInKanpsack(uint itemBaseId, uint itemNum);
        /// <summary>
        /// 是不是阵营战
        /// </summary>
        /// <returns></returns>
        bool IsCampFight();

        uint GetAttackPriority();

        /// <summary>
        /// 使用瞬药
        /// </summary>
        void UseAtOnceMedicine();

        /// <summary>
        /// 是否在跟服务器对时
        /// </summary>
        /// <returns></returns>
        bool IsCheckingTime();

        /// <summary>
        /// 获取玩家氏族ID
        /// </summary>
        /// <returns></returns>
        uint GetMainPlayerClanId();
    }
}
