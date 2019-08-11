using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    InventoryUI IUI;
    bool inventoryShowing = true;

    private void Awake()
    {
        Inventory = new PlayerInventory(out InventoryUI ui);
        IUI = ui;
        inventoryShowing = ui.IsShowing;
    }

    private void Start()
    {
        for (int i = 0; i < 16; ++i) Inventory.Give(ItemCreator.CreateRandomItem(), out EquipableItem result);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryShowing = !inventoryShowing;
            if (inventoryShowing) IUI.Show();
            else IUI.Hide();
        }
    }

    public PlayerInventory Inventory { get; private set; }
}
