using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Mod
{
    public ValueType valueType;
    public ModMathType mathType;
    [ConditionalField(nameof(valueType), ValueType.Float)]
    public float value;
    [ConditionalField(nameof(valueType), ValueType.Int)]
    public int iValue;

    public float GetValue => valueType == ValueType.Float ? value : iValue;
}