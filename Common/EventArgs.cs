using System.Collections;
using System;

namespace Common
{
	public class EventArgs<T> : EventArgs
	{
		public T Data { get; set; }
	}
}