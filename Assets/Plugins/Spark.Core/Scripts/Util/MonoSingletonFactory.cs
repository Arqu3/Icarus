using System.Reflection;
using UnityEngine;
namespace Spark.Internal
{
	class MonoSingletonFactory
	{
		public const string ResourceBasePath = "Singleton";
		public const string AssemblyOrderResourceName = "AssemblyOrder";
		
		public static void InitializeSingletons()
		{
			var assemblyOrder = Resources.Load<MonoSingletonAssemblyOrder>(ResourceBasePath + "/" + AssemblyOrderResourceName);
#if UNITY_EDITOR
			System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/" + ResourceBasePath);
			if (assemblyOrder == null)
			{
				assemblyOrder = ScriptableObject.CreateInstance<MonoSingletonAssemblyOrder>();
				assemblyOrder.name = AssemblyOrderResourceName;
				assemblyOrder.assemblies = new[] { "Assembly-CSharp-firstpass", "Assembly-CSharp" };
				UnityEditor.AssetDatabase.CreateAsset(assemblyOrder, "Assets/Resources/" + ResourceBasePath + "/" + AssemblyOrderResourceName + ".asset");
			}
#endif
			if(assemblyOrder == null)
			{
				// fallback
				InitializeSingletonsInAssembly(Assembly.Load("Assembly-CSharp-firstpass"));
				InitializeSingletonsInAssembly(Assembly.Load("Assembly-CSharp"));
			}
			else
			{
				foreach(var assembly in assemblyOrder.assemblies)
				{
                    InitializeSingletonsInAssembly(Assembly.Load(assembly));
                }
			}
		}

		private static void InitializeSingletonsInAssembly(Assembly assembly)
		{
			foreach (var v in assembly.GetTypes())
			{
				if (v == typeof(MonoSingleton<>)) continue; // Don't load the base class!
				System.Type baseType;
				if (IsSubclassOfRawGeneric(typeof(MonoSingleton<>), v, out baseType))
				{
					Debug.Log("Loading: " + v);
					baseType = baseType.MakeGenericType(new[] { v });
					var m = baseType.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
					m.Invoke(null, null);
				}
			}
		}

		private static bool IsSubclassOfRawGeneric(System.Type generic, System.Type toCheck, out System.Type genericType)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					genericType = cur;
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			genericType = null;
			return false;
		}
	}
}

