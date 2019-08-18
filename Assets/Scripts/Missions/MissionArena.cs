using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionArena : MonoBehaviour
{
    private void Awake()
    {
        int diff = MissionSingleton.Instance.Current.Difficulty;
        var arr = new[] { 4, 8, 12, 40 };
        int amount = arr[diff];
        var enemies = EntityPrefabs.Instance.enemies;

        for (int i = 0; i < amount; ++i)
        {
            Vector3 r = Random.insideUnitSphere * 10f;
            r.y = 0f;
            var e = Instantiate(enemies[Random.Range(0, enemies.Count)]);
            e.transform.position += r;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        while(true)
        {
            yield return new WaitForSeconds(1f);

            var e = FindObjectsOfType<BaseEnemyEntity>();
            if (e.Length <= 0)
            {
                foreach (var hero in HeroCollection.Instance.GetSelected()) hero.LevelUp();
                MoveHeroes();

                int numItems = MissionSingleton.Instance.Current.Difficulty + 1;
                var items = new EquipableItem[numItems];
                for (int i = 0; i < numItems; ++i) items[i] = ItemCreator.CreateRandomItem();
                var loot = new MissionLoot(items);
                MissionSingleton.Instance.GiveLoot(loot);

                var ui = new MissionCompleteUI(loot);
                ui.Show();
                yield break;
            }

            var h = FindObjectsOfType<HeroEntity>();
            if (h.Length <= 0)
            {
                MoveHeroes();

                var ui = new MissionCompleteUI(null);
                ui.Show();
                yield break;
            }
        }
    }

    void MoveHeroes()
    {
        foreach (var h in HeroCollection.Instance.GetSelected()) h.state = HeroState.Recruited;
    }
}
