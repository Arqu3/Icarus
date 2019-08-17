using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionArena : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        int diff = MissionSingleton.Instance.Current.Difficulty;

        var arr = new[] { 4, 8, 12, 20 };

        int amount = arr[diff];
        var enemies = EntityPrefabs.Instance.enemies;

        for(int i = 0; i < amount; ++i)
        {
            Vector3 r = Random.insideUnitSphere * 10f;
            r.y = 0f;
            var e = Instantiate(enemies[Random.Range(0, enemies.Count)]);
            e.transform.position += r;
        }

        while(true)
        {
            yield return new WaitForSeconds(1f);

            var e = FindObjectsOfType<BaseEnemyEntity>();
            if (e.Length <= 0)
            {
                Debug.Log("Victory!");

                foreach (var hero in HeroCollection.Instance.GetSelected()) hero.LevelUp();
                MoveHeroes();

                int numItems = diff + 1;
                var loot = new EquipableItem[numItems];
                for (int i = 0; i < numItems; ++i) loot[i] = ItemCreator.CreateRandomItem();
                MissionSingleton.Instance.GiveLoot(new MissionLoot(loot));

                SceneManager.LoadScene("Hub");
            }

            var h = FindObjectsOfType<HeroEntity>();
            if (h.Length <= 0)
            {
                Debug.Log("Defeat!");
                MoveHeroes();
                SceneManager.LoadScene("Hub");
            }
        }
    }

    void MoveHeroes()
    {
        foreach (var h in HeroCollection.Instance.GetSelected()) h.state = HeroState.Recruited;
    }
}
