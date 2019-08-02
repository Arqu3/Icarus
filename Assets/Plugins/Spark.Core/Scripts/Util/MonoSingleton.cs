using UnityEngine;
using System.Linq;
using Spark.Internal;

public class MonoSingleton<TSingleton> : MonoBehaviour where TSingleton : MonoSingleton<TSingleton> {
	public static TSingleton Instance { get; private set; }
	
	// Called automatically by the MonoSingletonFactory at game start
	internal static void Create()
	{
		var attr = typeof(TSingleton).GetCustomAttributes(typeof(ResourceSingletonAttribute), true).FirstOrDefault() as ResourceSingletonAttribute;
		if (attr != null)
		{
			var path = attr.Path;
			if (path == null || path == "")
			{
				path = MonoSingletonFactory.ResourceBasePath + "/" + typeof(TSingleton).Name;
			}
			var res = Resources.Load<TSingleton>(path);
			if(res == null)
			{
				Debug.LogError("Object is null for: " + path);
			}
			var obj = Instantiate(res);
			DontDestroyOnLoad(obj.gameObject);
			Instance = obj;
		}
		else
		{
			var obj = new GameObject(typeof(TSingleton).Name).AddComponent<TSingleton>();
			DontDestroyOnLoad(obj.gameObject);
			Instance = obj;
		}
	}

	
}
