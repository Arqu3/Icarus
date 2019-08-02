using System.Collections;
using System.Collections.Generic;
using Spark.Internal;
using UnityEngine;

namespace Spark.Initialization
{
	public static class Initializer
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			foreach(var initializeable in Resources.LoadAll<InitializeableScriptableObject>(""))
			{
				initializeable.Initialize();
			}
			MonoSingletonFactory.InitializeSingletons();
		}
	}
}