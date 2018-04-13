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
	public static partial class MineDataBaseExtensions
	{
		/// <summary>
		/// 通过[矿石道具ID]二分快速查表
		/// </summary>
		/// <param name="dwID">矿石道具ID</param>
		/// <returns></returns>
		public static MineDataBase Query(this List<MineDataBase> sorted, uint dwID)
		{
			var key = new MineDataBase() { dwID = dwID };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(MineDataBase);
		}

		#region Comparer
		class Comparer1 : Comparer<MineDataBase>
		{
			public override int Compare(MineDataBase a, MineDataBase b)
			{
				{ var n = a.dwID.CompareTo(b.dwID); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}