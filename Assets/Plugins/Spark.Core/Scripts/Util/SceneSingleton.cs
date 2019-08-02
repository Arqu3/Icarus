using UnityEngine;

public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{

	/// <summary>
	/// Get the current instance. Can be null if none exists in the scene.
	/// </summary>
	public static T Current
	{
		get;
		private set;
	}

	protected virtual void Awake()
	{
		Debug.Assert( Current == null, "Multiple InstanceSingletons in a scene? " + GetType().Name );
		Current = ( T ) this;
	}
}
