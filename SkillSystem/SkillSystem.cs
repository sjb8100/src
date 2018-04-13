using System;
using System.Collections.Generic;
using System.Text;
using Client;
using Engine.Utility;
using UnityEngine;
namespace SkillSystem
{
    class SkillSystem : ISkillSystem
    {
        // 游戏全局对象
        private static IClientGlobal m_ClientGlobal = null;
        private DamageManager m_danmagemanager = null;
        static Dictionary<uint, List<uint>> m_colorDic = new Dictionary<uint, List<uint>>();

        static Dictionary<string, SkillState> m_stateDic = new Dictionary<string, SkillState>();

        static Dictionary<string, string[]> m_timeStringDic = new Dictionary<string, string[]>();
        static Dictionary<string, Color> m_colorStringDic = new Dictionary<string, Color>();
        public static string[] GetTimeArrayByString(string str)
        {
            if (m_timeStringDic.ContainsKey(str))
            {
                return m_timeStringDic[str];
            }
            else
            {
                string[] timeArray = str.Split('-');
                m_timeStringDic.Add(str, timeArray);
                return timeArray;
            }
        }
        public static SkillState GetSkillStateByString(string stateName)
        {
            if (m_stateDic.ContainsKey(stateName))
            {
                return m_stateDic[stateName];
            }
            else
            {
                Client.SkillState state = (Client.SkillState)Enum.Parse(typeof(Client.SkillState), stateName);
                m_stateDic.Add(stateName, state);
                return state;
            }
        }
        public static List<uint> GetColorList(uint monsterType)
        {
            List<uint> colorList;
            if (!m_colorDic.TryGetValue(monsterType, out colorList))
            {
                colorList = GameTableManager.Instance.GetGlobalConfigList<uint>("FlashColor", monsterType.ToString());
                if (colorList != null)
                {
                    m_colorDic.Add(monsterType, colorList);
                }
            }
            return colorList;
        }
        public static Color GetColorByStr(string str)
        {
            Color c = Color.white;
            if (m_colorStringDic.ContainsKey(str))
            {
                return m_colorStringDic[str];
            }
            else
            {
                string[] colorArray = str.Split('_');
                float c1, c2, c3;
                if (colorArray.Length != 3)
                {
                    //Log.Error("buffcolor 长度错误 buff id "+bdb.dwID);
                    return c;
                }
                if (float.TryParse(colorArray[0], out c1))
                {
                    if (float.TryParse(colorArray[1], out c2))
                    {
                        if (float.TryParse(colorArray[2], out c3))
                        {
                            c = new Color(c1 / 255, c2 / 255, c3 / 255);
                        }
                    }
                }
                m_colorStringDic.Add(str, c);
                return c;
            }
        }
        public ISkillPart CreateSkillPart()
        {
            ISkillPart sp = new PlayerSkillPart(m_ClientGlobal, this);
            return sp;
        }
        public IBuffPart CreateBuffPart()
        {
            IBuffPart buff = new PlayerBuffPart(m_ClientGlobal, this);
            return buff;
        }
        public SkillSystem(IClientGlobal clientGlobal)
        {
            m_ClientGlobal = clientGlobal;
            m_danmagemanager = new DamageManager();
        }
        public DamageManager GetDanmageManager()
        {
            if (m_danmagemanager == null)
            {
                //Log.Error( "m_danmagemanager   is null" );
                return null;
            }

            return m_danmagemanager;
        }
        public static IClientGlobal GetClientGlobal()
        {
            return m_ClientGlobal;
        }

        // 游戏退出时使用
        public void Release()
        {

        }

        public bool Init(bool isEditor = false)
        {
            SkillEffectManager.Helper = SkillEffectHelper.Instance;
            if (isEditor)
            {

            }
            else
            {
                SkillEffectManager.Instance.LoadSkillAsset("effect/role_skill_def.unity3d", true);
                SkillEffectManager.Instance.LoadSkillAsset("effect/npc_skill_def.unity3d", false);
            }


            return true;
        }
    }

    public class SkillSystemCreator
    {
        private static SkillSystem m_SkillSys = null;
        public static ISkillSystem CreateSkillSystem(IClientGlobal clientGlobal)
        {
            if (m_SkillSys == null)
            {
                m_SkillSys = new SkillSystem(clientGlobal);
            }

            return m_SkillSys;
        }


    }
}
