using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    private void Awake()
    {
        Inventory = new PlayerInventory();
    }

    public PlayerInventory Inventory { get; private set; }
}
