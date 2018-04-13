using Engine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Client
{
    // 查找条件器
    public interface IFindCondition<T>
    {
        bool Conform(T en);
    }

    public interface IEntitySystem
    {
        // 创建
        void Create();

        // 游戏退出时使用
        void Release();

        // 创建实体
        /**
        @brief 创建实体
        @param etype 实体类型
        @param data 实体数据
        @param bImmediate 是否立即创建
        */
        IEntity CreateEntity(EntityType etype, EntityCreateData data, bool bImmediate = false);

        // 删除实体
        void RemoveEntity(IEntity entity);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 各种查询接口
        /**
        @brief 查找角色
        @param 
        */
        IPlayer FindPlayer(uint uid);

        /**
        @brief 查找怪物
        @param 
        */
        IMonster FindMonster(uint uid);

        INPC FindNPC(uint uid);

        IBox FindBox(uint uid);

        INPC FindNPCByBaseId(int baseId, bool nearest = false);

        IPet FindPet(uint uind);
        IItem FindItem(uint uid);

        IRobot FindRobot(uint uid);
        IEntity FindEntity(long uid);

        IPuppet FindPuppet(uint uid);
        //挂机寻怪接口
        IEntity FindEntityByArea_PkModel(MapAreaType areaType, PLAYERPKMODEL pkmodel, UnityEngine.Vector3 center, uint rang,uint attackPriority , long filterTarget = 0);

        T FindEntity<T>(uint uid);

        void FindAllEntity<T>(ref List<T> lst);

        // 查找最近的对象
        T FindNearstEntity<T>(IFindCondition<T> condition);

        // 查找最近的对象
        T FindNearstEntity<T>();

        // 获得最近的player  list  由近到远
        void FindNearstPlayer(ref List<IPlayer> lstPlayer);

        //检测玩家是否在视野范围
        bool CheckEntityVisible(IEntity e);

        //检测玩家是否在搜索范围内（跟自动挂机打怪距离一样）
        bool CheckSearchRange(IEntity e);

        // 根据一定的范围查找对象 范围判定在IFindCondition里面实现
        void FindEntityRange<T>(IFindCondition<T> condition, ref List<T> lst);

        // showEntity
        void ShowEntity(bool bShow);

        //-------------------------------------------------------------------------------------------------------
        // 屏幕点选实体
        /**
        @brief 实体拾取方法 // 只查找生物类实体
        @param 根据屏幕上点击位置，返回点中的实体列表
        */
        bool PickupEntity(Vector2 pos, ref List<IEntity> lstEntity);


        //系统更新
        void Update(float dt);

        /**
        @brief 设置同屏人数
        @param 
        */
        int MaxPlayer { get; set; }

        /**
        @brief 清理 退出时清理数据
        @param bFlag 标识是否清理主角 true清理主角
        */
        void Clear(bool bFlag = false);
        /// <summary>
        /// 根据阵营 找范围内离自己最近的的
        /// </summary>
        /// <param name="camtype"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        IEntity FindEntityByCampType(CampType camtype, uint range);
        /// <summary>
        /// 找队友
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        IEntity FindTeamerByRange(uint range);
        /// <summary>
        /// 找宠物
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        IEntity FindPetByRange(uint range);
        IEntity FindSummonedByRange(uint range);

        IEntity FindAllCreatureByRange(uint range);

        IEntity FindMinHpTeamer(uint range);

        // 移动服务器时间
        uint serverTime
        {
            get;
        }

        /// <summary>
        /// 获取正在显示的实体uid
        /// </summary>
        /// <returns></returns>
        List<long> GetEntityUids();
    }

    // 运动碰撞回调 
    public interface IEntityCallback
    {
        // 移动结束
        void OnMoveEnd(IEntity entity, object param = null);

        // 落地
        void OnToGround(IEntity entity, object param = null);

        // 飞行结束
        void OnFlyEnd(IEntity entity, object param = null);

        // 更新回调
        void OnUpdate(IEntity entity, object param = null);

        /// <summary>
        /// 发生碰撞
        /// </summary>
        /// <param name="obj">检测对象</param>
        /// <param name="obj">碰撞信息</param>
        void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // EntityPart
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IEntityPart
    {
        /**
        @brief 
        @param 
        */
        void Release();

        /**
        @brief 获取PartID
        */
        EntityPart GetPartID();

        /**
        @brief 创建
        @param 
        */
        bool Create(IEntity master);

        // 更新
        void Update(float dt);
    }

    public interface IEquipPart : IEntityPart
    {
        // 换装数据
        void ChangeEquip(int nPos, int id);

        int GetEquip(int nPos);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////    
    public interface IEntity
    {
        // 渲染对象
        IRenderObj renderObj { get; }

        // 实体销毁
        void Release();

        void Update(float dt);

        void SetData(object data);
        object GetData();

        uint GetID();

        // 实体UID 服务器运行时ID
        long GetUID();

        // 获取实体名称
        string GetName();

        // 设置实体运动碰撞回调 
        void SetCallback(IEntityCallback callback);

        // 给实体发送消息
        object SendMessage(EntityMessage cmd, object param = null);

        // 获取部件
        IEntityPart GetPart(EntityPart enityPart);
        // 获取位置
        Vector3 GetPos();

        // 获取方向
        Vector3 GetDir();

        // 获取旋转欧拉角
        Vector3 GetRotate();

        // 获取半径
        float GetRadius();

        // 获取碰撞检测标识
        ColliderCheckType GetColliderCheckFlag();

        ////////////////////////////////////////////////////////

        // 获取实体类型
        EntityType GetEntityType();

        /**
        @brief 设置属性
        */
        void SetProp(int nPropID, int nValue);
        /**
        @brief 获取属性
        */
        int GetProp(int nPropID);

        // 更新多条属性
        void UpdateProp(EntityCreateData data);

        // Hittest
        bool HitTest(Ray ray, out RaycastHit rayHit);

        Engine.Node GetNode();

        // 获取动画状态，兼容外部接口 不建议使用
        AnimationState GetAnimationState(string strAniName);
        /// <summary>
        /// 设置是否显示残影
        /// </summary>
        /// <param name="isShow"></param>
        void SetShowGhost(bool isShow);

        void ChangeState(CreatureState state, object param = null);

        CreatureState GetCurState();

        bool IsDead();

        bool IsPlayingAnim(string clipName);

        void GetLocatorPos(string strLocatorName, Vector3 vOffset, Quaternion rot, ref Vector3 pos, bool bWorld = false);
        //是否隐身
        bool IsHide();

        // 获取对象名称
        string GetObjName();
        //直接获取transform 去除连点的get写法
        Transform GetTransForm();
    }

    public interface IWorldObj : IEntity
    {
    }

    // 生物接口
    public interface ICreature : IWorldObj
    {
        //uint mapID
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 是否在战斗状态
        /// </summary>
        bool IsFighting
        {
            set;
            get;
        }
        /// <summary>
        /// 是否隐身
        /// </summary>
        //bool IsHide
        //{
        //    set;
        //    get;
        //}
    }

    // 玩家接口
    public interface IPlayer : ICreature
    {
        // 是否是主角
        bool IsMainPlayer();

        void SetExtraProp(GameCmd.t_MainUserData data);

        void RefrushServerPos(Vector3 pos);

        // 获取时装数据
        void GetSuit(out List<GameCmd.SuitData> lstSuit);
    }

    // NPC接口
    public interface INPC : ICreature
    {

        bool CanSelect();

        bool IsMonster();

        bool IsSummon();

        bool IsPet();

        bool IsRobot();

        bool IsMercenary();

        bool IsCanAttackNPC();

        bool WhetherShowHeadTips();
        bool BelongOther { get; set; }

        bool BelongMe { get; set; }
        bool IsTrap();
        //队伍ID
        uint TeamID { get; set; }
        //无队伍时给个玩家ID
        uint OwnerID { get; set; }

        bool IsBoss();
        //是不是从属主角
        bool IsMainPlayerSlave();
        //是不是从属某个实体
        bool IsSlaveEntity(IEntity en);
    }

    // 机器人
    public interface IRobot : ICreature
    {
        // 获取时装数据
        void GetSuit(out List<GameCmd.SuitData> lstSuit);
    }

    // 怪物接口
    public interface IMonster : ICreature
    {

    }

    // 物品接口
    public interface IItem : IEntity
    {

    }

    // 物品掉落盒子
    public interface IBox : IWorldObj
    {
        bool CanPick();

        bool CanAutoPick();
        void AddTrigger(IEntityCallback callback);
    }

    /// <summary>
    /// 宠物接口
    /// </summary>
    public interface IPet : ICreature
    {
        void SetExtraData(GameCmd.PetData pet);
        uint PetBaseID
        {
            get;
        }
        GameCmd.PetTalent GetReplaceTalent();
        GameCmd.PetTalent GetAttrTalent();
        List<GameCmd.PetSkillObj> GetPetSkillList();
        void AddPetSkill(int skillid, int index);
        void PetSkillUpGrade(int skillid);
        void SetPetSkillLock(int skillid, bool bLock);
        void DeletePetSkill(int skillid);

        GameCmd.PetTalent GetAttrPlan();
        int GetExPlan();
        uint GetInheritNum();
        uint GetFreeResetAttrNum();
        void SetFreeResetNum(uint num);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 家园对象
    public interface IHomeObj : IWorldObj
    {
    }
    // 植物
    public interface IPlant : IHomeObj
    {

    }
    // 动物
    public interface IAnimal : IHomeObj
    {

    }
    // 许愿树
    public interface ITree : IHomeObj
    {

    }
    // 矿产
    public interface IMinerals : IHomeObj
    {

    }
    // 土地
    public interface ISoil : IHomeObj
    {

    }
    // 木偶（玩家）
    public interface IPuppet : IWorldObj
    {

    }
}
