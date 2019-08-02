using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtil
{
	/// <summary>
	/// An enumerable for the entire transform hierarchy.
	/// Iterates by depth-first.
	/// </summary>
	/// <param name="transform"></param>
	/// <returns></returns>
	public static IEnumerable<Transform> AsEnumerable(this Transform transform)
	{
		return new TransformEnumerable(transform);
	}
	private class TransformEnumerable : IEnumerable<Transform>
	{
		private Transform transform;

		public TransformEnumerable(Transform transform)
		{
			this.transform = transform;
		}

		public IEnumerator<Transform> GetEnumerator()
		{
			return new TransformEnumerator(transform);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private class TransformEnumerator : IEnumerator<Transform>
	{
		public Transform Current { get; private set; }

		private readonly Transform root;
		private readonly Stack<Transform> stack;
		
		public TransformEnumerator(Transform root)
		{
			this.root = root;
			stack = new Stack<Transform>();
			stack.Push(root);
			Current = null;
		}


		object IEnumerator.Current
		{
			get { return Current; }
		}

		public void Dispose()
		{
			stack.Clear();
			Current = null;
		}

		public bool MoveNext()
		{
			if (stack.Count == 0) return false;
			Current = stack.Pop();

			if (Current)
			{
				for(int i = Current.childCount-1; i >= 0; i--)
				{
					stack.Push(Current.GetChild(i));
				}
			}

			return Current;
		}

		public void Reset()
		{
			stack.Clear();
			stack.Push(root);
			Current = null;
		}
	}
}
