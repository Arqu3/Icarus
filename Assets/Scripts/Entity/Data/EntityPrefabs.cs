using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = nameof(EntityPrefabs), menuName = "Entity Prefab Data/" + nameof(EntityPrefabs))]
public class EntityPrefabs : EntityPrefabData<EntityPrefabs>
{
    public List<HeroEntity> heroes = new List<HeroEntity>();
    public List<BaseEnemyEntity> enemies = new List<BaseEnemyEntity>();
}

#if UNITY_EDITOR

[CustomEditor(typeof(EntityPrefabs), true)]
public class EntityPrefabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (GUILayout.Button("Refresh"))
        {
            var targetData = target as EntityPrefabs;
            Undo.RecordObject(targetData, "Refreshed entity prefabs");

            targetData.heroes.Clear();
            targetData.enemies.Clear();

            var entities = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs" });
            foreach (var e in entities)
            {
                var entity = AssetDatabase.LoadAssetAtPath<BaseEntity>(AssetDatabase.GUIDToAssetPath(e));

                if (entity)
                {
                    switch (entity.EntityType)
                    {
                        case EntityType.Friendly:
                            targetData.heroes.Add(entity as HeroEntity);
                            break;
                        case EntityType.Neutral:
                            break;
                        case EntityType.Enemy:
                            targetData.enemies.Add(entity as BaseEnemyEntity);
                            break;
                        default:
                            break;
                    }
                }
            }

            EditorUtility.SetDirty(targetData);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif