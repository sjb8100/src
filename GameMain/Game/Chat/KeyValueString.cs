using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using Common;

/// <summary>
/// 可实现一个字符串中内嵌多个键值对，序列化采用json方案
/// </summary>
public class KeyValueString : Dictionary<string, object>
{
	public string Value
	{
		get
		{
			return Json.Serialize(this);
		}
		set
		{
			this.Clear();
			if (string.IsNullOrEmpty(value))
				return;
			this.AddRange(Json.Deserialize<Dictionary<string, object>>(value));
		}
	}

	public KeyValueString() { }
	public KeyValueString(string value) { this.Value = value; }
	public KeyValueString(IEnumerable<XAttribute> value)
	{
		this.AddRange(value);
	}

	public void Add(XAttribute value)
	{
		this[value.Name.ToString()] = value.Value;
	}
	public void AddRange(IEnumerable<XAttribute> value)
	{
		foreach (var a in value)
			this.Add(a);
	}

	public bool TryGetValue<T>(string key, out T value)
	{
		object v;
		if (this.TryGetValue(key, out v) && v is T)
		{
			value = (T)v;
			return true;
		}
		value = default(T);
		return false;
	}

	public override string ToString()
	{
		return Value;
	}

	public static implicit operator string(KeyValueString kvs)
	{
		return kvs == null ? null : kvs.ToString();
	}
}