using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Stat
{
    public abstract StatType GetSType();
    public ValueType valueType;
    public ModMathType mathType;
    [ConditionalField(nameof(valueType), ValueType.Float)]
    public float value;
    [ConditionalField(nameof(valueType), ValueType.Int)]
    public int iValue;

    public float GetValue => valueType == ValueType.Float ? value : iValue;
    public bool IsUsed() => Mathf.Abs(GetValue) > ItemModData.UseThreshold;
}