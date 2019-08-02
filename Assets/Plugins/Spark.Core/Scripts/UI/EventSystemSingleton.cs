using System.Collections;
using System.Collections.Generic;
using Spark.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Spark.UI
{
	[ResourceSingleton]
	public class EventSystemSingleton : MonoSingleton<EventSystemSingleton>
	{
		public SparkStandaloneInputModule inputModule
		{
			get { return GetComponent<SparkStandaloneInputModule>(); }
		}
	}
}

