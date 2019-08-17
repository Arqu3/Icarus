using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoSingleton<ItemCollection>
{
    public List<EquipableItem> items = new List<EquipableItem>();
}
