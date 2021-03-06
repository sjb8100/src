//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace table
{
	public static partial class EquipAddBaseExtensions
	{
		/// <summary>
		/// 通过[物品ID]二分快速查表
		/// </summary>
		/// <param name="equipID">物品ID</param>
		/// <returns></returns>
		public static EquipAddBase Query(this List<EquipAddBase> sorted, uint equipID)
		{
			var key = new EquipAddBase() { equipID = equipID };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(EquipAddBase);
		}

		#region Comparer
		class Comparer1 : Comparer<EquipAddBase>
		{
			public override int Compare(EquipAddBase a, EquipAddBase b)
			{
				{ var n = a.equipID.CompareTo(b.equipID); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}
