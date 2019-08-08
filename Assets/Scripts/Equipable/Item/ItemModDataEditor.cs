using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(ItemModData), true)]
public class ItemModDataEditor : Editor
{
    ItemModData targetData;
    SerializedProperty listProperty;

    bool drawDefault = false;

    enum SortMode
    {
        None = -1,
        Name = 0,
        Type = 1,
        StatType = 2,
        Tier = 3
    }

    SortMode sortMode = SortMode.None;

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
        drawDefault = EditorGUILayout.Toggle("Draw default editor", drawDefault);
        if (drawDefault)
        {
            base.OnInspectorGUI();
            return;
        }

        serializedObject.Update();

        var col = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add new mod"))
        {
            AddNew();
            return;
        }
        GUI.backgroundColor = col;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Show all")) ToggleAllShow(true);
        if (GUILayout.Button("Hide all")) ToggleAllShow(false);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Sort mode", EditorStyles.largeLabel);
        if (GUILayout.Button("Sort")) Sort();
        sortMode = (SortMode)EditorGUILayout.EnumPopup(sortMode);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (targetData.mods.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mods", EditorStyles.whiteLargeLabel);
            for (int i = 0; i < targetData.mods.Count; ++i)
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

                if (GUILayout.Button("Copy"))
                {
                    AddCopy(i);
                    break;
                }

                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();

                if (!targetData.showBools[i]) continue;

                var property = listProperty.GetArrayElementAtIndex(i);

                foreach (var field in mod.GetType().GetFields())
                {
                    var relativeProp = property.FindPropertyRelative(field.Name);

                    if (field.FieldType.IsSubclassOf(typeof(Stat)))
                    {
                        EditorGUILayout.Space();

                        var val = (Stat)field.GetValue(mod);
                        bool showAsUsed = val.IsUsed();

                        if (!mod.debugShowUnused && !showAsUsed) continue;

                        string extra = showAsUsed ? " - " + val.GetValue.ToString() : "";

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

            if (GUILayout.Button("Validate mods"))
            {
                var unusedMods = (from m in targetData.mods where m.GetUsedStats().Count() == 0 select m).ToArray();
                if (unusedMods.Length == 0) Debug.Log("No unused mods found");
                else
                {
                    Debug.Log("----FOUND UNUSED MODS----");
                    unusedMods.ToList().ForEach(x => Debug.Log(x.name));
                    Debug.Log("----END OF WARNING----");
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    #region Sorting

    void Sort()
    {
        if (sortMode != SortMode.None) Undo.RecordObject(targetData, "Sorted list");

        switch (sortMode)
        {
            case SortMode.None:
                break;
            case SortMode.Name:
                targetData.mods = targetData.mods.OrderBy(x => x.name).ToList();
                break;
            case SortMode.Type:
                targetData.mods = targetData.mods.OrderBy(x => x.modType).ToList();
                break;
            case SortMode.StatType:
                targetData.mods = targetData.mods.OrderBy(x => x.GetUsedStats().FirstOrDefault().type).ToList();
                break;
            case SortMode.Tier:
                targetData.mods = targetData.mods.OrderBy(x => x.tier).ToList();
                break;
            default:
                break;
        }

        ToggleAllShow(false);
    }

    #endregion

    #region Add/remove

    void Remove(int index)
    {
        Undo.RecordObject(targetData, "Removed mod");
        targetData.mods.RemoveAt(index);
        targetData.showBools.RemoveAt(index);
    }

    void AddNew()
    {
        Add(new ItemMod());
    }

    void AddCopy(int index)
    {
        Add(ObjectCopier.Clone(targetData.mods[index]));
    }

    void Add(ItemMod mod)
    {
        Undo.RecordObject(targetData, "Added mod");
        targetData.mods.Add(mod);
        targetData.showBools.Add(true);
    }

    #endregion

    #region Help func

    void ToggleAllShow(bool show)
    {
        for (int i = 0; i < targetData.showBools.Count; ++i) targetData.showBools[i] = show;
    }

    #endregion
}

#endif