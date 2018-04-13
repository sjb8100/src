using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

//  游戏设置
namespace Client
{
    class GameOption : IGameOption
    {
        private static string GAME_OPTION = "Config/Option.ini";
        private Engine.Utility.IniFile m_OptionFile = new Engine.Utility.IniFile();

        // 需要保存
        private bool m_bNeedSave = false;

        public bool Create()
        {
            if (!m_OptionFile.Open(GAME_OPTION))
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            Save();
        }


        public int GetInt(string strKey, string strName, int nDefault)
        {
            if(m_OptionFile==null)
            {
                return nDefault;
            }

            return m_OptionFile.GetInt(strKey, strName, nDefault);
        }
        // 获取字符串
        public string GetString(string strKey, string strName, string strDefault)
        {
            if (m_OptionFile == null)
            {
                return strDefault;
            }

            return m_OptionFile.GetString(strKey, strName, strDefault);
        }
        // 保存整数
        public void WriteInt(string strKey, string strName, int nValue)
        {
            if (m_OptionFile != null)
            {
                m_OptionFile.WriteInt(strKey, strName,nValue);
                m_bNeedSave = true;
            }
        }
        // 保存字符串
        public void WriteString(string strKey, string strName, string strValue)
        {
            if (m_OptionFile != null)
            {
                m_OptionFile.WriteString(strKey, strName, strValue);
                m_bNeedSave = true;
            }
        }

        // 保存
        public void Save()
        {
            if (m_OptionFile != null&&m_bNeedSave)
            {
                m_OptionFile.Save();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
//         public void ApplyOption()
//         {
//             // 应用渲染设置
//             int nShadowLevel = ClientGlobal.Instance().gameOption.GetInt("Render", "shadow", 1);
//             Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
//             if(rs!=null)
//             {
//                 rs.SetShadowLevel((Engine.ShadowLevel)nShadowLevel);
//             }
//         }
//         public void SetShadow() 
//         {
//             
//         }
    }
}
