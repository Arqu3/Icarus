using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spark.StateMachine
{
	public interface IMachineState
	{
		void Enter();
		bool Update();
		void Exit();
	}
}

