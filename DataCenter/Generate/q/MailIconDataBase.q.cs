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
	public static partial class MailIconDataBaseExtensions
	{
		/// <summary>
		/// 通过[属性]二分快速查表
		/// </summary>
		/// <param name="dwID">属性</param>
		/// <returns></returns>
		public static MailIconDataBase Query(this List<MailIconDataBase> sorted, uint dwID)
		{
			var key = new MailIconDataBase() { dwID = dwID };
			var comparer = new Comparer1();
			var index = sorted.BinarySearch(key, comparer);
			return index >= 0 ? sorted[index] : default(MailIconDataBase);
		}

		#region Comparer
		class Comparer1 : Comparer<MailIconDataBase>
		{
			public override int Compare(MailIconDataBase a, MailIconDataBase b)
			{
				{ var n = a.dwID.CompareTo(b.dwID); if (n != 0) return n; }
				return 0;
			}
		}
		#endregion
	}
}