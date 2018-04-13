using System.Collections;
using System;
using System.Collections.Generic;

public class Repeated<TLength, TData> : List<TData>, Common.ISerializable
	where TLength : new()
	where TData : new()
{
	public Repeated()
		: base()
	{
	}
	public Repeated(IEnumerable<TData> collection)
		: base(collection)
	{
	}

	#region ISerializable 成员

	public void Serialize(System.IO.Stream s)
	{
		checked
		{
			s.Write<TLength>(this.Count);
		}
		for (var i = 0; i < this.Count; i++)
		{
			s.Write(this[i]);
		}
	}

	public void Deserialize(System.IO.Stream s)
	{
		var count = Convert.ToInt32(s.Read<TLength>());
		this.Clear();
		for (var i = 0; i < count; i++)
		{
			this.Add(s.Read<TData>());
		}
	}

	#endregion
}