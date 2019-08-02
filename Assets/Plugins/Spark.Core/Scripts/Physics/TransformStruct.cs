using UnityEngine;
namespace Spark.Physics
{
	[System.Serializable]
	public struct TransformStruct
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		public TransformStruct(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}
		public TransformStruct(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
			scale = Vector3.one;
		}
		public TransformStruct(Vector3 position)
		{
			this.position = position;
			this.rotation = Quaternion.identity;
			scale = Vector3.one;
		}
		/// <summary>
		/// A TransformStruct with no modifications (0 position, 0 rotation, 1 scale)
		/// </summary>
		public static TransformStruct Default { get { return def; } }
		private static readonly TransformStruct def = new TransformStruct(Vector3.zero, Quaternion.identity, Vector3.one);
	}
}

