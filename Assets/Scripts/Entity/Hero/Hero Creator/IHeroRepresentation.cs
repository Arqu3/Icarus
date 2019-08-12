using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeroRepresentation
{
    HeroEntity Prefab { get; }
    List<EquipableItem> Items { get; }
}
