using UnityEngine;
using UnityEngine.Events;
using Util;

[System.Serializable]
public class ObservableInt : ObservableValue<int> {
	public ObservableInt( int initialValue = 0, AlertCondition alertCondition = null ) : base(initialValue, alertCondition ) { }
	
	public static ObservableInt operator ++(ObservableInt a)
	{
		a.Value ++;
		return a;
	}
	public static ObservableInt operator --(ObservableInt a)
	{
		a.Value--;
		return a;
	}
}
[System.Serializable]
public class ObservableFloat : ObservableValue<float> {
	public ObservableFloat( float initialValue = 0, AlertCondition alertCondition = null ) : base(initialValue, alertCondition ) { }
}
[System.Serializable]
public class ObservableString : ObservableValue<string> {
	public ObservableString( string initialValue = "", AlertCondition alertCondition = null ) : base(initialValue, alertCondition ) { }
}
[System.Serializable]
public class ObservableBool : ObservableValue<bool> {
	public ObservableBool( bool initialValue = false, AlertCondition alertCondition = null ) : base(initialValue, alertCondition ) {

	}
}

public class ObservableClass<TClass> : ObservableValue<TClass> where TClass : class
{
	public void ApplyModifications(System.Action<TClass> cfg)
	{
		cfg(Value);
		AlertAll();
	}
}

/// <summary>
/// Note that a generic class cannot be serialized!
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableValue<T> : ObservableValueBase
{
	private class ValueChangedEvent : UnityEvent<T>
	{
	}
	public readonly UnityEvent<T> onChange = new ValueChangedEvent();
	public T Value
	{
		get
		{
			return _value;
		}
		set
		{
			var prev = _value;
			_value = value;
			if (alertCondition == null || alertCondition(prev, _value))
			{
				onChange.Invoke(_value);
			}
		}
	}
	[SerializeField]
	private T _value;

	public delegate bool AlertCondition(T prevVal, T newVal);

	private AlertCondition alertCondition;


	public ObservableValue(T initialValue = default(T), AlertCondition alertCondition = null)
	{
		_value = initialValue;
		this.alertCondition = alertCondition;
	}

	/// <summary>
	/// Notify observers of current value, a force refresh
	/// </summary>
	public override void AlertAll()
	{
		onChange.Invoke(_value);
	}

	/// <summary>
	/// Set the value without notifying observers
	/// </summary>
	/// <param name="value"></param>
	public void StealthSetValue(T value)
	{
		_value = value;
	}
	public override string ToString()
	{
		return _value.ToString();
	}

	public static implicit operator T(ObservableValue<T> v)
	{
		return v.Value;
	}
}

namespace Util
{
	public abstract class ObservableValueBase
	{
		public abstract void AlertAll();
	}

	
}