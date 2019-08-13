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

        ui.Hide();
        inventoryShowing = ui.IsShowing;
    }

    private void Start()
    {
        HeroCollection.Instance.GenerateApplying(4);

        for (int i = 0; i < 15; ++i) Inventory.Give(ItemCreator.CreateRandomItem(), out EquipableItem result);
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
