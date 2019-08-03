using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = nameof(ItemModData), menuName = "Item Data/" + nameof(ItemModData))]
public class ItemModData : ItemResourceData<ItemModData>
{
    public List<ItemMod> mods = new List<ItemMod>();
    public List<bool> showBools = new List<bool>();
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
    public ModMathType mathType;
    public float value;
    public int GetIntValue => Mathf.CeilToInt(value);
}

[System.Serializable]
public struct ItemMod
{
    //[Header("Debug")]
    //public bool debug;

    //[Header("Name, type")]
    public string name;
    public ModType modType;
    public ValueType valueType;
    public int tier;

    //[Header("Stats")]
    public Mod power;
    public Mod resourceGain;
    public Mod actionCooldown;
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

                targetData.showBools[i] = EditorGUILayout.Foldout(targetData.showBools[i], mod.name + " - " + mod.modType, true, EditorStyles.foldoutHeader);

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

                EditorGUILayout.EndHorizontal();

                if (!targetData.showBools[i]) continue;

                var property = listProperty.GetArrayElementAtIndex(i);

                foreach (var pro in mod.GetType().GetFields())
                {
                    var m = property.FindPropertyRelative(pro.Name);

                    if (pro.FieldType == typeof(Mod))
                    {
                        EditorGUILayout.LabelField(m.displayName, EditorStyles.boldLabel);
                        foreach(var childPro in pro.FieldType.GetFields())
                        {
                            var mm = m.FindPropertyRelative(childPro.Name);
                            EditorGUILayout.PropertyField(mm);
                        }
                    }
                    else EditorGUILayout.PropertyField(m);
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