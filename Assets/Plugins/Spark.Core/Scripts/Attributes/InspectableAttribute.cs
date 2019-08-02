using System;

/// <summary>
/// Mark a class to tell the inspector to look at its internal values when marked with the ShowInInspector Attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class InspectableAttribute : Attribute
{

}