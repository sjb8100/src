// 表格相关的扩充代码
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Cmd;
using GameCmd;
using Common;
using table;

namespace table
{
	public partial class SelectRoleDataBase
	{
		/// <summary>
		/// 职业id
		/// </summary>
		public enumProfession Profession
		{
			get { return (enumProfession)professionID; }
			set { professionID = (uint)value; }
		}
		/// <summary>
		/// 性别id
		/// </summary>
		public enmCharSex Sex
		{
			get { return (enmCharSex)sexID; }
			set { sexID = (uint)value; }
		}

		public static SelectRoleDataBase Where(GameCmd.enumProfession profession, GameCmd.enmCharSex sexman)
		{
            return (from i in Table.Query<SelectRoleDataBase>() where i.Profession == profession && i.Sex == sexman select i).FirstOrDefault();
		}

        //public static SelectRoleDataBase Where(enumProfession profession, enmCharSex sexman)
        //{
        //}
	}
    
   

	public partial class QuestDataBase
	{
		/// <summary>
		/// 类型
		/// </summary>
		public GameCmd.TaskType Type
		{
            get { return (GameCmd.TaskType)dwType; }
			set { dwType = (uint)value; }
		}
	}

	public partial class NpcDataBase
	{
		/// <summary>
		/// 是否可攻击
		/// </summary>
        //public Cmd.MapScreenUserCmd.enumAttackType AttackType
        //{
        //    get { return (Cmd.MapScreenUserCmd.enumAttackType)dwAttackType; }
        //    set { dwAttackType = (uint)value;}
        //}

        ///// <summary>O
        ///// 是否可访问
        ///// </summary>
        //public Cmd.MapScreenUserCmd.enumVisitType VisitType
        //{
        //    get { return (Cmd.MapScreenUserCmd.enumVisitType)dwVisitType; }
        //    set { dwVisitType = (uint)value; }
        //}
        public bool IsMonster()
        {
            return dwType == 0 && dwMonsterType != 0;
        }
		public static table.NpcDataBase Where(uint baseid)
		{
			return Table.Query<table.NpcDataBase>().FirstOrDefault(i => i.dwID == baseid);
		}
	}

    //public partial class EquipStrengthenDataBase
    //{
    //    public static EquipStrengthenDataBase Where(ItemType itemType, uint itemlevel, uint isPhysicl)
    //    {
    //        return (from i in Table.Query<EquipStrengthenDataBase>() where i.dwItemType == (ushort)itemType && i.dwLevel == itemlevel && i.dwIsPhysical == isPhysicl select i).FirstOrDefault();
    //    }

    //    public static EquipStrengthenDataBase Where(uint HoleNum, uint isPhysicl, ItemType itemType)
    //    {
    //        return (from i in Table.Query<EquipStrengthenDataBase>() where i.dwItemType == (ushort)itemType && i.dwHoleMax == HoleNum && i.dwIsPhysical == isPhysicl select i).FirstOrDefault();
    //    }
    //}

    //public partial class SynthesisTypeDataBase
    //{
    //    public static List<SynthesisTypeDataBase> Where(uint firstType)
    //    {
    //        return (from i in Table.Query<SynthesisTypeDataBase>() where i.firstType == firstType select i).ToList();
    //    }

    //    public static SynthesisTypeDataBase Where(uint firstType, uint secondType)
    //    {
    //        return (from i in Table.Query<SynthesisTypeDataBase>() where i.firstType == firstType && i.secondType == secondType select i).FirstOrDefault();
    //    }
    //}
    /// <summary>
    /// 装备升星表
    /// </summary>
    //public partial class EquipLevelDatabase
    //{
    //    /// <summary>
    //    /// 根据道具基本ID和道具升星等级获取
    //    /// </summary>
    //    /// <param name="baseID"></param>
    //    /// <param name="level"></param>
    //    /// <returns></returns>
    //    public static EquipLevelDatabase Where(uint baseID, uint level)
    //    {
    //        return (from i in Table.Query<EquipLevelDatabase>() where i.wdID == baseID && i.wdLevel == level select i).FirstOrDefault();
    //    }
    //}

    public partial class RandomNameDataBase
    {
        /// <summary>
        /// 获取随机名字前缀列表
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        //public static List<RandomNameDataBase> PrefixList()
        //{
        //    return (from i in Table.Query<RandomNameDataBase>() where i.namePrefix != string.Empty select i).ToList();
        //}
        ///// <summary>
        ///// 获取随机名字男性后缀列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<RandomNameDataBase> MaleList()
        //{
        //    return (from i in Table.Query<RandomNameDataBase>() where i.maleName != string.Empty select i).ToList();
        //}
        ///// <summary>
        ///// 获取随机名字女性后缀列表
        ///// </summary>
        ///// <returns></returns>
        //public static List<RandomNameDataBase> FemaleList()
        //{
        //    return (from i in Table.Query<RandomNameDataBase>() where i.femaleName != string.Empty select i).ToList();
        //}
    }

}

