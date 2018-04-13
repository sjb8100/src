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
	public static partial class HoursemanShipUPDegreeExtensions
	{
		/// <summary>
		/// 通过[等阶]二分快速查表
		/// </summary>
		/// <param name="degree">等阶</param>
		/// <returns></returns>
		public static HoursemanShipUPDegree Query(this List<HoursemanShipUPDegree> sorted, uint degree)
		{
			var key = new HoursemanShipUPDegree() { degree = degree };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(HoursemanShipUPDegree);
		}

		#region Comparer
		class Comparer1 : Comparer<HoursemanShipUPDegree>
		{
			public override int Compare(HoursemanShipUPDegree a, HoursemanShipUPDegree b)
			{
				{ var n = a.degree.CompareTo(b.degree); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}