//=========================================================================================
//
//    Author: zhudianyu
//    
//    CreateTime:  #CreateTime#
//
//=========================================================================================
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using table;
namespace EntitySystem
{
    class Pet : FightCreature, IPet
    {
        private EntityCreateData m_Data = null;
        // 属性数组
        private Engine.Utility.SecurityInt[] m_PlayerProp = new Engine.Utility.SecurityInt[PetProp.End - PetProp.Begin];

        uint petBaseID;
        public uint PetBaseID
        {
            get
            {
                return (uint)GetProp((int)PetProp.BaseID);
            }
        }
        /// <summary>
        /// 替换天赋
        /// </summary>
        PetTalent replacetalent;
        /// <summary>
        /// 属性点
        /// </summary>
        PetTalent attrpoint;
        //升级加点方案
        PetTalent attrplan;

        int exPlan;

        //继承次数
        uint petInheritNum;
        //免费洗点次数
        uint freeResetAttrNum;
        List<PetSkillObj> petSkillList = new List<PetSkillObj>();
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Pet;
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)PetProp.Begin || nPropID >= (int)PetProp.End)
            {
                return;
            }

            m_PlayerProp[nPropID - (int)PetProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)PetProp.Begin)
            {
                return base.GetProp(nPropID);
            }
            if (nPropID < (int)PetProp.End)
            {
                return m_PlayerProp[nPropID - (int)PetProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < PetProp.End - PetProp.Begin; ++i)
            {
                m_PlayerProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //public override bool CreateEntityView(EntityCreateData data)
        //{

        //    //m_Data = data;
        //    //base.CreateEntityView(data);
        //    //PetDataBase db = GameTableManager.Instance.GetTableItem<PetDataBase>(PetBaseID);
        //    //if (db != null)
        //    //{
        //    //    SetName(db.petName);
        //    //    ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<ResourceDataBase>(db.modelID);
        //    //    if (rdb != null)
        //    //    {
        //    //        string respath = rdb.strPath;
        //    //        if (!m_EntityView.Create(ref respath, OnCreateRenderObj))
        //    //        {
        //    //            Engine.Utility.Log.Error("CreateEntityView failed:{0}", respath);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        Engine.Utility.Log.Error("请检查Resource 表格 宠物模型路径为空 宠物模型id 是 " + db.modelID);
        //    //    }
        //    //}
        //    return true;
        //}
        //private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        //{
        //    // 加载部件
        //    if (m_EntityView == null)
        //    {
        //        return;
        //    }

        //    // 根据全局配置设置阴影
        //    int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
        //    m_EntityView.SetShadowLevel(nShadowLevel);
        //}
        public override bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            m_Data = data;
            // 设置属性
            InitProp();
            ApplyProp(data);

            // 创建外观对象
            //  CreateEntityView( data );

            return base.Create(data, colliderCheckType);

        }
        public void SetExtraData(GameCmd.PetData pet)
        {
            SetName(pet.name);
            petBaseID = pet.base_id;
            replacetalent = pet.replace_talent;
            attrpoint = pet.attr_point;
            petSkillList = pet.skill_list;
            exPlan = pet.ex_plan;
            petInheritNum = pet.inherit_time;
            freeResetAttrNum = pet.free_reset_attr_time;
            attrplan = pet.attr_plan;
        }
        public PetTalent GetAttrPlan()
        {
            return attrplan;
        }
        public int GetExPlan()
        {
            return exPlan;
        }
        public uint GetInheritNum()
        {
            return petInheritNum;
        }
        public uint GetFreeResetAttrNum()
        {
            return freeResetAttrNum;
        }
        public void SetFreeResetNum(uint num)
        {
            freeResetAttrNum = num;
        }
        public PetTalent GetReplaceTalent()
        {
            return replacetalent;
        }
        public PetTalent GetAttrTalent()
        {
            return attrpoint;
        }
        public List<PetSkillObj> GetPetSkillList()
        {
            return petSkillList;
        }
        bool HasLearnPetSkill(PetSkillObj obj)
        {
            bool isContain = false;
            foreach (var skill in petSkillList)
            {
                if (skill.id == obj.id)
                {
                    return true;
                }
            }
            return isContain;
        }
        public void PetSkillUpGrade(int skillid)
        {
            for (int i = 0; i < petSkillList.Count; i++)
            {
                PetSkillObj obj = petSkillList[i];
                if (obj.id == skillid)
                {
                    petSkillList[i].lv += 1;
                }
            }
        }
        public void SetPetSkillLock(int skillid, bool bLock)
        {
            for (int i = 0; i < petSkillList.Count; i++)
            {
                PetSkillObj obj = petSkillList[i];
                if (obj.id == skillid)
                {
                    petSkillList[i].@lock = bLock;
                }
            }
        }
        public void AddPetSkill(int skillid, int index)
        {
            PetSkillObj obj = new PetSkillObj() { id = skillid, lv = 1 };
            if (!HasLearnPetSkill(obj))
            {//服务器发索引从1开始 （lua)
                int location = index - 1;
                if (location < 0)
                {
                    Engine.Utility.Log.Error(" 数组越界 index 等于或小于0");
                }
                if (location <= petSkillList.Count)
                {
                    Engine.Utility.Log.LogGroup("ZDY", " insert pet skill " + skillid.ToString());
                    petSkillList.Insert(location, obj);
                }
                else
                {
                    Engine.Utility.Log.Error(" 数组越界");
                }
            }
        }
        public void DeletePetSkill(int skillid)
        {
            for (int i = 0; i < petSkillList.Count; i++)
            {
                PetSkillObj obj = petSkillList[i];
                if (obj.id == skillid)
                {
                    Engine.Utility.Log.LogGroup("ZDY", " remove pet skill " + skillid.ToString());
                    petSkillList.RemoveAt(i);
                    break;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreatePart()
        {
            //return true;
            AddPart(EntityPart.Skill);
            AddPart(EntityPart.Buff);
            return true;
        }

    }
}
