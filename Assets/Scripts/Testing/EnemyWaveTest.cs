using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemyWaveTest : MonoBehaviour
{
    [Header("Testing params")]
    [SerializeField]
    float checkInterval = 0.5f;
    [SerializeField]
    int minEnemiesToSpawn = 3;
    [SerializeField]
    int maxEnemiesToSpawn = 6;
    [SerializeField]
    Transform firstSpawnpoint;
    [SerializeField]
    Transform secondSpawnpoint;

    List<BaseEnemyEntity> currentEnemies = new List<BaseEnemyEntity>();
    List<HeroEntity> currentHeroes = new List<HeroEntity>();

    IEnumerator Start()
    {
        foreach (var h in EntityPrefabs.Instance.heroes) Instantiate(h);

        currentHeroes = FindObjectsOfType<HeroEntity>().ToList();

        yield return null;

        foreach(var hero in currentHeroes)
        {
            foreach(var slot in hero.EquipmentSlots)
            {
                var item = ItemCreator.CreateRandomItem();
                slot.Equip(item);
                if (Random.Range(0f, 1f) < 0.33f) slot.UnEquip(item);
            }
        }

        SpawnEnemies();

        while(true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (!ContainsNonNull(currentHeroes))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (!ContainsNonNull(currentEnemies))
            {
                currentEnemies.Clear();
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        int num = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + 1);
        var enemies = EntityPrefabs.Instance.enemies;

        for(int i = 0; i < num; ++i)
        {
            var enemy = Instantiate(enemies[Random.Range(0, enemies.Count)]);
            enemy.transform.position = Vector3.Lerp(firstSpawnpoint.position, secondSpawnpoint.position, (float)i / num);
            currentEnemies.Add(enemy);
        }
    }

    bool ContainsNonNull(List<HeroEntity> list)
    {
        return (from e in list where e != null select e).Count() > 0;
    }

    bool ContainsNonNull(List<BaseEnemyEntity> list)
    {
        return (from e in list where e != null select e).Count() > 0;
    }
}
