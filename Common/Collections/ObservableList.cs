using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
	/// <summary>
	/// 带有增删事件的List
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class ObservableList<TValue> : IEnumerable<TValue>
	{
		private List<TValue> m_data = new List<TValue>();

		public event EventHandler<EventArgs<TValue>> ItemAdd;
		public event EventHandler<EventArgs<TValue>> ItemUpdate;
		public event EventHandler<EventArgs<TValue>> ItemRemove;

		public TValue this[int index]
		{
			get
			{
				return m_data[index];
			}
			set
			{
				var has = m_data.Contains(value);
				if (has)
					OnItemUpdate(value);
				else
					OnItemAdd(value);
			}
		}

		public void Clear()
		{
			var last = m_data;
			m_data = new List<TValue>();
			foreach (var data in last)
				OnItemRemove(data);
		}

		public int Count { get { return m_data.Count; } }

		public bool Remove(TValue value)
		{
			var has = m_data.Contains(value);
			if (has == false)
				return false;
			m_data.Remove(value);
			OnItemRemove(value);
			return true;
		}

		private void OnItemAdd(TValue value)
		{
			if (ItemAdd != null)
				ItemAdd(this, new EventArgs<TValue>() { Data = value});
		}

		private void OnItemUpdate(TValue value)
		{
			if (ItemUpdate != null)
				ItemUpdate(this, new EventArgs<TValue>() { Data = value});
		}

		private void OnItemRemove(TValue value)
		{
			if (ItemRemove != null)
				ItemRemove(this, new EventArgs<TValue>() { Data = value });
		}

		#region IEnumerable<TValue> Members

		public IEnumerator<TValue> GetEnumerator()
		{
			return this.m_data.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
