/// <summary>
/// Should this field be visible in the GameTweaker window
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class TweakableFieldAttribute : System.Attribute
{
	public enum Options
	{
		DEFAULT = 0,
		SHARED = 1 << 1
	}
	readonly Options options;
	public TweakableFieldAttribute(Options options = Options.DEFAULT)
	{
		this.options = options;
	}
	public TweakableFieldAttribute(bool sharedAmongAllInstances)
	{
		options = Options.DEFAULT;
		if (sharedAmongAllInstances) options |= Options.SHARED;
	}

	public bool isSharedAmongAllInstances { get { return (options & Options.SHARED) == Options.SHARED; } }
}