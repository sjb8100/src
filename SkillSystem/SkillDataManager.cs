using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Engine.Utility;
using System.IO;
namespace SkillSystem
{
    class SkillDataManager
    {
        static SkillDataManager instance = null;
        public static SkillDataManager Insatnce
        {
            get
            {
                if (instance == null)
                    instance = new SkillDataManager();
                return instance;
            }
            set
            {
                instance = value;
            }
        }


        private Dictionary<uint, List<SkillDatabase>> skillDic = new Dictionary<uint, List<SkillDatabase>>();
        /// <summary>
        /// 技能数据管理字典 
        /// key: roleid
        /// list: role拥有的技能列表
        /// </summary>
        public Dictionary<uint, List<SkillDatabase>> SkillDic
        {
            get
            {
                return skillDic;
            }
            set
            {
                skillDic = value;
            }
        }


        public void AddSkillDataBase(uint roleid, SkillDatabase database)
        {
            if (!SkillDic.ContainsKey(roleid))
            {
                List<SkillDatabase> dataList = new List<SkillDatabase>();
                dataList.Add(database);
                SkillDic.Add(roleid, dataList);
            }
            else
            {
                List<SkillDatabase> dataList = skillDic[roleid];
                dataList.Add(database);

            }
        }
        public SkillDatabase GetSkillDataBase(uint roleid, uint skillID)
        {
            if (SkillDic.ContainsKey(roleid))
            {
                List<SkillDatabase> database = SkillDic[roleid];
                for (int i = 0; i < database.Count; i++)
                {
                    var info = database[i];
                    if (info.wdID == skillID)
                    {
                        return info;
                    }

                }
                return null;
            }
            else
            {
                Log.LogGroup("ZDY", "SkillDic不包含key " + roleid.ToString());
                return null;
            }
        }
    }
}
