using UnityEngine;
using Util;

[System.Serializable]
public class StateStackFloat : StateStackValue<float>{ }
[System.Serializable]
public class StateStackInt : StateStackValue<int>{ }
[System.Serializable]
public class StateStackBool : StateStackValue<bool>{ }

namespace Util
{
	public abstract class StateStackValueBase
	{

	}
	public class StateStackValue<T> : StateStackValueBase where T : struct
	{

		[SerializeField]
		private T[] stack;
		private int head;

		public T Value { get { return stack[head]; } }

		public StateStackValue(T initial = default(T), int stackSize = 50)
		{
			stack = new T[stackSize];
			stack[0] = initial;
			head = 0;
		}

		public void Push(T value)
		{
			stack[head++] = value;
			Debug.Assert(head < stack.Length, "Stack limit reached");
		}

		public void Pop()
		{
			head--;
			Debug.Assert(head > 0, "Popped root state of value");
		}

		public void Clear()
		{
			head = 0;
		}

		public static implicit operator T(StateStackValue<T> v)
		{
			return v.Value;
		}
	}
}
