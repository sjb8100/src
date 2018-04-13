
//*************************************************************************
//	创建日期:	2017/8/1 星期二 11:44:56
//	文件名称:	TableKey
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public struct TableKey : IComparable<TableKey>, IEquatable<TableKey>
{
    public uint ID { get; set; }
    public int ChildID { get; set; }
    #region Equatable
    public static bool operator ==(TableKey a, TableKey b)
    {
        if (System.Object.ReferenceEquals(a, b))
            return true;
        if (((object)a == null) || ((object)b == null))
            return false;
        return a.ID == b.ID && a.ChildID == b.ChildID;
    }
    public static bool operator !=(TableKey a, TableKey b)
    {
        return !(a == b);
    }

    #region IEquatable<NetMessageType> 成员

    public bool Equals(TableKey other)
    {
        return this == other;
    }

    #endregion

    public override bool Equals(object obj)
    {
        return obj is TableKey ? this == (TableKey)obj : false;
    }

    public override int GetHashCode()
    {
        return ((int)this.ID << 16) | ((int)this.ChildID & 0x0000FFFF);
    }
    #endregion

    #region IComparable<NetMessageType> 成员

    public int CompareTo(TableKey other)
    {
        if (this.ID > other.ID)
            return 1;
        else if (this.ID < other.ID)
            return -1;

        if (this.ChildID > other.ChildID)
            return 1;
        else if (this.ChildID < other.ChildID)
            return -1;

        return 0;
    }

    #endregion
}
