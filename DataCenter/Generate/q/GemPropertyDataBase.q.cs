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
	public static partial class GemPropertyDataBaseExtensions
	{
		/// <summary>
		/// 通过[宝石ID]二分快速查表
		/// </summary>
		/// <param name="gemId">宝石ID</param>
		/// <returns></returns>
		public static GemPropertyDataBase Query(this List<GemPropertyDataBase> sorted, uint gemId)
		{
			var key = new GemPropertyDataBase() { gemId = gemId };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(GemPropertyDataBase);
		}

		#region Comparer
		class Comparer1 : Comparer<GemPropertyDataBase>
		{
			public override int Compare(GemPropertyDataBase a, GemPropertyDataBase b)
			{
				{ var n = a.gemId.CompareTo(b.gemId); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}
