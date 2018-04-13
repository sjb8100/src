using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client
{
    public enum TipType
    {
        Global = 1,         // 全局位置(相对屏幕原点)
        FollowObj,          // 跟随对象
    }
    public enum TipWindowType
    {
        YesNO,
        Ok,
        CancelOk,
        Custom,
    }
    // Tips管理器
    public interface ITipsManager
    {
        // strTips支持http协议
        void AddTips(string strTips, Vector3 pos, TipType type);
        void ShowTips(string msg);
        void ShowTipsById(uint errorid, params object[] args);
        void ShowTipWindow(TipWindowType type, string tipContent, Action okCallBack, Action cancelCallBack = null, Action closeCallBack = null, string title = "提示", string okstr = null, string cancleStr = null, uint _color = 0x000000);
        void ShowTipWindow(uint okCdTime, uint cancelCdTime, TipWindowType type, string tipContent, Action okCallBack, Action cancelCallBack = null, Action closeCallBack = null, string title = "提示", string okstr = null, string cancleStr = null);
    }

    // 客户端全局管理  以及一些全局性的功能接口
    public interface IClientGlobal
    {
        ISkillSystem GetSkillSystem();

        IEntitySystem GetEntitySystem();
        IControllerSystem GetControllerSystem();

        // 获取Lua系统
        ILuaSystem GetLuaSystem();

        // 获取地图系统
        IMapSystem GetMapSystem();

        // 添加Tips
        ITipsManager GetTipsManager();

        bool IsMainPlayer(IPlayer player);

        bool IsMainPlayer(IEntity entity);

        bool IsMainPlayer(long uid);
        bool IsMainPlayer(uint uid); 
        /**
        @brief 用来处理游戏中退回到登录或者选人界面 或者地图时使用(不清理主角)
        @param bFlag 标识是否清理主角 true清理主角
        */
        void Clear(bool bFlag = false);

        // 游戏退出时使用
        void Release();

        // 关掉游戏中声音
        void MuteGameSound(bool bEnable);

        // 主角对象
        IPlayer MainPlayer
        {
            get;
            set;
        }

        // 网络接口
        INetService netService
        {
            get;
            set;
        }

        // 游戏设置
        IGameOption gameOption
        {
            get;
        }
    }

    // 游戏设置 ini文件
    public interface IGameOption
    {
        // 获取整数值
        int GetInt(string strKey, string strName, int nDefault);
        // 获取字符串
        string GetString(string strKey, string strName, string strDefault);
        // 保存整数
        void WriteInt(string strKey, string strName, int nValue);
        // 保存字符串
        void WriteString(string strKey, string strName, string strValue);

        // 保存
        void Save();
    }
}
