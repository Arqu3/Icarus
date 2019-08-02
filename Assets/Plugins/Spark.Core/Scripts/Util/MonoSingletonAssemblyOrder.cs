using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Internal
{
	public class MonoSingletonAssemblyOrder : ScriptableObject
	{
		public string[] assemblies =
		{
				"Assembly-CSharp-firstpass",
				"Assembly-CSharp"
			};
	}
}