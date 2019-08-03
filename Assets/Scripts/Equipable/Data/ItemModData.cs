using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = nameof(ItemModData), menuName = "Item Data/" + nameof(ItemModData))]
public class ItemModData : ItemResourceData<ItemModData>
{
    public List<ItemMod> mods = new List<ItemMod>();
    public List<bool> showBools = new List<bool>();

    public const float UseThreshold = 0.0001f;
}

public enum ModType
{
    Prefix = 0,
    Suffix = 1
}

public enum ModMathType
{
    Additive = 0,
    Multiplicative = 1
}

public enum ValueType
{
    Int = 0,
    Float = 1
}

[System.Serializable]
public struct Mod
{
    public ValueType valueType;
    public ModMathType mathType;
    public float value;
    public int IntValue => Mathf.CeilToInt(value);

    public float GetValue => valueType == ValueType.Float ? value : IntValue;
}

[System.Serializable]
public struct ItemMod
{
#if UNITY_EDITOR

    public bool debugShowUnused;

#endif

    //[Header("Name, type")]
    public string name;
    public ModType modType;
    public int tier;

    //[Header("Stats")]
    public Mod health;
    public Mod power;
    public Mod resourceGain;
    public Mod actionCooldown;

    public Mod[] GetAllMods => new[] { health, power, resourceGain, actionCooldown };
}

#if UNITY_EDITOR

[CustomEditor(typeof(ItemModData), true)]
public class ItemModDataEditor : Editor
{
    ItemModData targetData;
    SerializedProperty listProperty;

    private void OnEnable()
    {
        targetData = target as ItemModData;
        listProperty = serializedObject.FindProperty(nameof(targetData.mods));
        if (targetData.showBools.Count != targetData.mods.Count)
        {
            targetData.showBools = new List<bool>();
            for (int i = 0; i < targetData.mods.Count; ++i) targetData.showBools.Add(true);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var col = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add new mod"))
        {
            Add();
            return;
        }
        GUI.backgroundColor = col;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Show all"))
        {
            for (int i = 0; i < targetData.showBools.Count; ++i) targetData.showBools[i] = true;
        }        
        if (GUILayout.Button("Hide all"))
        {
            for (int i = 0; i < targetData.showBools.Count; ++i) targetData.showBools[i] = false;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (targetData.mods.Count > 0)
        {
            for(int i = 0; i < targetData.mods.Count; ++i)
            {
                var mod = targetData.mods[i];

                EditorGUILayout.BeginHorizontal();

                targetData.showBools[i] = EditorGUILayout.Foldout(targetData.showBools[i], mod.name + " - " + mod.modType + "(" + mod.tier.ToString() + ")", true, EditorStyles.foldoutHeader);

                GUILayout.FlexibleSpace();

                col = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove"))
                {
                    if (EditorUtility.DisplayDialog("Remove?", "This will remove " + mod.name, "Confirm", "Cancel"))
                    {
                        Remove(i);
                        break;
                    }
                }
                GUI.backgroundColor = col;

                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();

                if (!targetData.showBools[i]) continue;

                var property = listProperty.GetArrayElementAtIndex(i);

                foreach (var field in mod.GetType().GetFields())
                {
                    var relativeProp = property.FindPropertyRelative(field.Name);

                    if (field.FieldType == typeof(Mod))
                    {
                        EditorGUILayout.Space();

                        var val = ((Mod)field.GetValue(mod)).value;
                        bool showAsUsed = Mathf.Abs(val) > ItemModData.UseThreshold;

                        if (!mod.debugShowUnused && !showAsUsed) continue;

                        string extra = showAsUsed ? " - "  + val.ToString() : "";

                        if (showAsUsed)
                        {
                            col = GUI.contentColor;
                            GUI.contentColor = Color.green;
                        }
                        EditorGUILayout.LabelField(relativeProp.displayName + extra, showAsUsed ? EditorStyles.boldLabel : EditorStyles.label);
                        if (showAsUsed) GUI.contentColor = col;

                        foreach (var childPro in field.FieldType.GetFields())
                        {
                            EditorGUILayout.PropertyField(relativeProp.FindPropertyRelative(childPro.Name));
                        }
                    }
                    else EditorGUILayout.PropertyField(relativeProp);
                }

                EditorGUILayout.Space();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void Remove(int index)
    {
        Undo.RecordObject(targetData, "Removed mod");
        targetData.mods.RemoveAt(index);
        targetData.showBools.RemoveAt(index);
    }

    void Add()
    {
        Undo.RecordObject(targetData, "Added mod");
        targetData.mods.Add(new ItemMod());
        targetData.showBools.Add(true);
    }
}

#endif