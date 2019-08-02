using UnityEngine;

public abstract class InitializeableScriptableObject : ScriptableObject
{
	protected internal abstract void Initialize();
}
