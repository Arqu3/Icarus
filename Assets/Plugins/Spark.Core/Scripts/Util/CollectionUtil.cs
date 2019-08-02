using System.Collections.Generic;

namespace System.Linq
{
	public static class CollectionUtil
	{
		public static T Random<T>(this IEnumerable<T> collection)
		{
			if (!collection.Any()) throw new System.InvalidOperationException("The source sequence is empty.");
			return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
		}
		public static T RandomOrDefault<T>(this IEnumerable<T> collection)
		{
			if (!collection.Any()) return default(T);
			return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
		}
	}
}

