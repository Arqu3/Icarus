using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatTest : MonoBehaviour
{
    HeroEntity hero;

    private void Awake()
    {
        hero = FindObjectOfType<HeroEntity>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(hero.MaxHealth);
            Debug.Log(hero.Health);

            var item = new EquipableItem();
            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Multiplicative, value = 1.1f, valueType = ValueType.Float }
            });
            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Multiplicative, value = 1.1f, valueType = ValueType.Float }
            });
            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Multiplicative, value = 1.1f, valueType = ValueType.Float }
            });

            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Additive, iValue = 10, valueType = ValueType.Int }
            });
            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Additive, iValue = 10, valueType = ValueType.Int }
            });
            item.mods.Add(new ItemMod
            {
                resourceGain = new ResourceStat(),
                actionCooldown = new CooldownStat(),
                power = new PowerStat(),
                health = new HealthStat { mathType = ModMathType.Additive, iValue = 10, valueType = ValueType.Int }
            });
            item.OutputUsedStats();

            hero.EquipmentSlots[0].Equip(item);

            Debug.Log(hero.MaxHealth);
            Debug.Log(hero.Health);
            Debug.Log(hero.HealthPercentage);
        }

        if (Input.GetKeyDown(KeyCode.K)) hero.GetModifier().OutputHealthDecorators();
    }
}
