//*************************************************************************
//	创建日期:	2017-2-7   14:53
//	文件名称:	ClientGlobal.cs
//  创 建 人:   Even	
//	版权所有:	Even.xu
//	说    明:	客户端框架
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using EntitySystem;
using Controller;
using SkillSystem;
using UnityEngine;
using MapSystem;
using Engine.Utility;

/// 游戏框架
namespace Client
{
    /// <summary>
    /// 客户端逻辑框架实例
    /// </summary>
    public class ClientGlobal : IClientGlobal
    {

        static ClientGlobal s_Inst = null;
        public static ClientGlobal Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new ClientGlobal();
            }

            return s_Inst;
        }

        public IPlayer MainPlayer
        {
            get;
            set;
        }
        public INetService netService
        {
            get;
            set;
        }

        public IGameOption gameOption
        {
            get { return m_GameOption; }
        }
        

        private IEntitySystem m_EntitySys = null;           // 实体系统
        private ISkillSystem m_SkillSys = null;             // 技能系统
        private IControllerSystem m_ControllerSys = null;   // 控制器系统
        private IMapSystem m_MapSystem = null;              // 地图系统
        private ILuaSystem m_LuaSystem = null;              // Lua系统
        private ITipsManager m_TipsManager = null;      // Tips管理器
        private IGameOption m_GameOption = null;            // 游戏设置



        //private float startTime = 0;
        /**
        @brief 
        @param bEditor 编辑器使用
        */
        public void Init(bool bEditor = false)
        {
            if (m_LuaSystem==null)
            {
                //m_LuaSystem = LuaSystemCreator.CreateLuaSystem();
            }
            // 实体系统
            if (m_EntitySys == null)
            {
                m_EntitySys = EntitySystemCreator.CreateEntitySystem(this);
                m_EntitySys.Create();
            }

            if(m_SkillSys == null)
            {
                m_SkillSys = SkillSystemCreator.CreateSkillSystem(this);
                m_SkillSys.Init(bEditor);
            }
            if (m_MapSystem == null)
            {
                m_MapSystem = MapSystemCreator.CreateMapSystem(this, bEditor);
            }
            // 控制器
            if (m_ControllerSys == null)
            {
                m_ControllerSys = ControllerSystemCreator.CreateControllerSystem(this);
                m_ControllerSys.ActiveController(ControllerType.ControllerType_KeyBoard);
            }

            if(m_GameOption==null)
            {
                GameOption op = new GameOption();
                op.Create();
                m_GameOption = op;

                // 应用设置
                //op.ApplyOption();
            }
            //startTime = 0;
        }
        
        /// <summary>
        /// 固定时间更新
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
            if (m_EntitySys != null)
            {
                m_EntitySys.Update(dt);
            }

            if (m_MapSystem != null)
            {
                m_MapSystem.Update(dt);
            }
        }

        /// <summary>
        /// 固定时间更新
        /// </summary>
        /// <param name="dt"></param>
        public void LateUpdate(float dt)
        {

        }

        public ISkillSystem GetSkillSystem()
        {
            return m_SkillSys;
        }

        public IEntitySystem GetEntitySystem()
        {
            return m_EntitySys;
        }

        public IControllerSystem GetControllerSystem()
        {
            return m_ControllerSys;
        }

        // 获取地图系统
        public IMapSystem GetMapSystem()
        {
            return m_MapSystem;
        }
        //-------------------------------------------------------------------------------------------------------
        // 添加Tips管理器
        public ITipsManager GetTipsManager()
        {
            if (m_TipsManager == null && m_ControllerSys != null)
            {
                if (m_ControllerSys.GetControllerHelper() != null)
                {
                    m_TipsManager = m_ControllerSys.GetControllerHelper().GetTipsManager();
                }
            }
            return m_TipsManager;
        }

        //-------------------------------------------------------------------------------------------------------
        // 获取Lua系统
        public ILuaSystem GetLuaSystem()
        {
            return m_LuaSystem;
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsMainPlayer(IPlayer player)
        {
            if(player==null)
            {
                return false;
            }

            // 确保第一个是主角
            if( MainPlayer == null)
            {
                return false;
            }

            if(player.GetID() != MainPlayer.GetID())
            {
                return false;
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsMainPlayer(IEntity entity)
        {
            if(entity==null)
            {
                return false;
            }

            if(entity.GetEntityType()!=EntityType.EntityType_Player)
            {
                return false;
            }

            return IsMainPlayer((IPlayer)entity);
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsMainPlayer(long uid)
        {
            if(m_EntitySys==null)
            {
                return false;
            }

            IEntity en = m_EntitySys.FindEntity(uid);
            return IsMainPlayer(en);
        }

        public bool IsMainPlayer(uint uid)
        {
            if (m_EntitySys == null)
            {
                return false;
            }

            IEntity en = m_EntitySys.FindEntity<IPlayer>(uid);
            return IsMainPlayer(en);
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 用来处理游戏中退回到登录或者选人界面 或者地图时使用
        @param bFlag 标识是否清理主角 true清理主角
        */
        public void Clear(bool bFlag = false)
        {
            Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
            if (renderSys != null && bFlag)
            {
                // 设置主Camera
                string strCameraName = "MainCamera";
                Engine.ICamera cam = renderSys.GetCamera(ref strCameraName);
                if (cam != null)
                {
                    cam.Enable(false);
                }
            }
            
            // 停止背景音乐
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null && bFlag)
            {
                audio.StopMusic();
            }

            if (bFlag)
            {
                CmdManager.Instance().Clear();
            }
            if (m_ControllerSys != null)   
            {
                m_ControllerSys.GetCombatRobot().Stop(true);
            }

            if(MainPlayer!=null)
            {
                MainPlayer.SendMessage(EntityMessage.EntityCommand_StopMove, MainPlayer.GetPos());
            }

            if(m_EntitySys!=null)
            {
                m_EntitySys.Clear(bFlag);
            }

            if(m_MapSystem!=null)
            {
                m_MapSystem.ExitMap();
            }
            MainPlayer = null;

            Engine.RareEngine.Instance().UnloadUnusedResources();
        }
        //-------------------------------------------------------------------------------------------------------
        // 游戏退出时使用
        public void Release()
        {
            if (m_LuaSystem != null)              // Lua系统
            {
                m_LuaSystem.Release();
                m_LuaSystem = null;
            }

            if (m_SkillSys != null)
            {
                m_SkillSys.Release();
                m_SkillSys = null;
            }

            if (m_EntitySys != null)           // 实体系统
            {
                m_EntitySys.Release();
                m_EntitySys = null;
            }

            // 技能系统
            if (m_ControllerSys != null)   // 控制器系统
            {
                m_ControllerSys.Release();
                m_ControllerSys = null;
            }

            if (m_MapSystem != null)              // 地图系统
            {
                m_MapSystem.Release();
                m_MapSystem = null;
            }

            // 游戏设置
            if(m_GameOption!=null)
            {
                GameOption op = m_GameOption as GameOption;
                if(op!=null)
                {
                    op.Close();
                }
            }

        }
        //-------------------------------------------------------------------------------------------------------
        // 关掉游戏中声音
        public void MuteGameSound(bool bMute)
        {
            bool bMuteSound = (gameOption.GetInt("Sound", "CheckSound", 1) == 1);
            if (!bMuteSound)
            {
                Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
                if (audio != null)
                {
                    audio.Mute(bMute);
                }
            }
        }
    }
}
