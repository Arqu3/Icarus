using System;

/// <summary>
/// Shows this non-serialized value in the inspector under a special section.
/// The value must be either enumerable, assignable to a string, or support the ToString() function
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ShowInInspectorAttribute : Attribute
{

}
