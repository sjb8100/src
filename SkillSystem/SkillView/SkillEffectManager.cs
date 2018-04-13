using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using System.Collections;
using Engine.Utility;

namespace SkillSystem
{



    public class SkillEffectManager
    {
        Dictionary<string, SkillEffectTable> m_allEffectTableDict = new Dictionary<string, SkillEffectTable>();
        /// <summary>
        /// 保存所有的技能id对应的 prop key is skillid
        /// </summary>
        Dictionary<string, SkillEffectProp> m_dicEffectProp = new Dictionary<string, SkillEffectProp>();


        static SkillEffectManager m_me = null;

        public static SkillEffectManager Instance
        {
            get
            {
                if (m_me == null)
                {
                    m_me = new SkillEffectManager();
                }

                return m_me;
            }
        }

        static ISkillEffectHelper ms_helper;
        static public ISkillEffectHelper Helper
        {
            get { return ms_helper; }
            set
            {
                ms_helper = value;
                EffectNode.helper = value;
            }
        }


        public SkillConfig GetSkillConfig(string path,bool clearAll)
        {

            Load(path, clearAll);
            SkillConfig config = ScriptableObject.CreateInstance<SkillConfig>();
            
            config.cofinData = m_allEffectTableDict;
            return config;
        }

        public bool LoadSkillAsset(string path,bool clearAll)
        {
            if(clearAll)
            {
                m_allEffectTableDict.Clear();
            }
    
             try
             {
                 //string fullPath = FileUtils.Instance().FullPathFileName(ref path, FileUtils.UnityPathType.UnityPath_CustomPath);
                 //AssetBundle ab = AssetBundle.LoadFromFile(fullPath);
                 AssetBundle ab = FileUtils.Instance().LoadFromFile(path);
                 if(ab != null)
                 {
                     SkillConfig con = ab.LoadAllAssets()[0] as SkillConfig;
                     if(con != null)
                     {
                        
                        foreach(var item in con.cofinData)
                        {
                           // m_allEffectTableDict.Add(item.Key, item.Value);
                            Add(item.Value);
                        }
                     }
                     ab.Unload(false);
                 }
               
             }
             catch (Exception e)
             {
                 Log.Error(e.ToString());
                 return false;
             }
            return true;
        }
        public bool Load(string path, bool clearAll)
        {
            byte[] buff = null;
            try
            {
                buff = ms_helper.OpenFile(path);
                if (buff == null)
                {
                    Log.Error("技能特效列表文件{0}不存在", path);
                    return false;
                }

                string str = Encoding.UTF8.GetString(buff);
                JsonData json_root = JsonMapper.ToObject(str);
                Load(json_root, clearAll);
            }
            catch (Exception e)
            {
                buff = null;
                Log.Error(e.ToString());
                return false;
            }
            buff = null;
            return true;
        }

        bool Load(JsonData json_root, bool clearAll)
        {
            if (clearAll)
                m_allEffectTableDict.Clear();

            for (int i = 0; i < json_root.Count; i++)
            {
                JsonData et_root = json_root[i];

               // SkillEffectTable et = new SkillEffectTable();
                SkillEffectTable et = ScriptableObject.CreateInstance<SkillEffectTable>();
                et.Load(et_root);

                Add(et);
            }

            return true;
        }

        public bool Save(string path)
        {
            JsonData json_root = new JsonData();
            json_root.SetJsonType(JsonType.Array);

            foreach (KeyValuePair<string, SkillEffectTable> pair in m_allEffectTableDict)
            {
                JsonData e_root = new JsonData();
                pair.Value.Save(e_root);

                json_root.Add(e_root);
            }

            try
            {
                StreamWriter sw = new StreamWriter(path);
                JsonWriter js_writer = new JsonWriter(sw);
                js_writer.PrettyPrint = true;
                JsonMapper.ToJson(json_root, js_writer);
                //string json_str = JsonMapper.ToJson(json_root);
                //sw.Write(json_str);
                //Debug.Log(json_str);
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
           // AssetDatabase.CreateAsset(this, Application.dataPath+"skillasset");
            return true;
        }

        public bool Add(SkillEffectTable et)
        {
            try
            {
                if (m_allEffectTableDict.ContainsKey(et.Name))
                {
                    Log.LogGroup("ZDY", "has contain key " + et.Name);
                }
                else
                {
                    m_allEffectTableDict.Add(et.Name, et);
                    Dictionary<string, SkillEffectProp> dic = et.Get();
                    Dictionary<string, SkillEffectProp>.Enumerator iter = dic.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        if (!m_dicEffectProp.ContainsKey(iter.Current.Key))
                        {
                            m_dicEffectProp.Add(iter.Current.Key, iter.Current.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }
            return true;
        }

        public void Remove(SkillEffectTable et)
        {
            m_allEffectTableDict.Remove(et.Name);
        }

        public SkillEffectTable Get(string name)
        {
            SkillEffectTable et;
            if (m_allEffectTableDict.TryGetValue(name, out et) == false)
            {
                return null;
            }

            return et;
        }

        public SkillEffectProp GetEffectPropBySkillID(uint skillID)
        {
            if (m_dicEffectProp.ContainsKey(skillID.ToString()))
            {
                return m_dicEffectProp[skillID.ToString()];
            }
            else
            {
                Log.Error("沒有找到 " + skillID + " 的技能配置数据");
                return null;
            }
        }
        public List<uint> GetAllSkillListByNpcID(uint npcID)
        {
            List<uint> idList = new List<uint>();
            table.NpcDataBase npc = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcID);
            if (npc != null)
            {
                string str = npc.skills;
                string[] filedArray = str.Split(':');
                for (int n = 0; n < filedArray.Length; n++)
                {
                    string field = filedArray[n];
                    string[] idArray = field.Split('-');
                    if (idArray.Length > 0)
                    {
                        uint id = 0;
                        if (uint.TryParse(idArray[0], out id))
                        {
                            if (!idList.Contains(id) && id != 0)
                            {
                                idList.Add(id);
                            }
                        }
                    }
                }
                //bossai
                string[] aiArray = npc.BossAI.Split('_');
                for (int i = 0; i < aiArray.Length; i++)
                {
                    uint aiID = 0;
                    if (uint.TryParse(aiArray[i], out aiID))
                    {
                        table.BossAIDataBase bdb = GameTableManager.Instance.GetTableItem<table.BossAIDataBase>(aiID);
                        if (bdb != null)
                        {
                            if (bdb.do_type == 1)
                            {//1是技能
                                if (!idList.Contains(bdb.do_param1) && bdb.do_param1 != 0)
                                {
                                    idList.Add(bdb.do_param1);
                                }
                            }
                        }
                    }
                }

            }
            return idList;
        }
        public List<uint> GetAllSkillList()
        {
            List<uint> idList = new List<uint>();

            if (m_dicEffectProp != null)
            {
                Dictionary<string, SkillEffectProp>.Enumerator iter = m_dicEffectProp.GetEnumerator();
                while (iter.MoveNext())
                {
                    var dic = iter.Current;
                    uint id = 0;
                    if (uint.TryParse(dic.Key, out id))
                    {
                        if (!idList.Contains(id))
                        {
                            idList.Add(id);
                        }

                    }
                }
            }
            return idList;
        }
    }
}