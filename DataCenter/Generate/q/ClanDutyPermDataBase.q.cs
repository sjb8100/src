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
	public static partial class ClanDutyPermDataBaseExtensions
	{
		/// <summary>
		/// 通过[职阶]二分快速查表
		/// </summary>
		/// <param name="job">职阶</param>
		/// <returns></returns>
		public static ClanDutyPermDataBase Query(this List<ClanDutyPermDataBase> sorted, uint job)
		{
			var key = new ClanDutyPermDataBase() { job = job };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(ClanDutyPermDataBase);
		}

		#region Comparer
		class Comparer1 : Comparer<ClanDutyPermDataBase>
		{
			public override int Compare(ClanDutyPermDataBase a, ClanDutyPermDataBase b)
			{
				{ var n = a.job.CompareTo(b.job); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}
