using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spark.StateMachine
{
	public class GenericStateMachine
	{
		private Stack<IMachineState> states;

		public GenericStateMachine(IMachineState baseState)
		{
			states = new Stack<IMachineState>();
			states.Push(baseState);
		}

		public void Update()
		{
			if (states.Count > 0)
			{
				var s = states.Peek();
				if (!s.Update())
				{
					Pop();
				}
			}
		}

		public void Push(IMachineState state)
		{
			state.Enter();
			states.Push(state);
		}
		public IMachineState Pop()
		{
			var s = states.Pop();
			s.Exit();
			return s;
		}
	}
}

