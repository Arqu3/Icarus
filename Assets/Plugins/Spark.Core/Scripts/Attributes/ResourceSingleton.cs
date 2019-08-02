using System;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ResourceSingletonAttribute : Attribute {
	/// <summary>
	/// Path from Resource folder
	/// </summary>
	public string Path { get; set; }
}
