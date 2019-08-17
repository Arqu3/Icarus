using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    InventoryUI IUI;
    
    private void Awake()
    {
        //Inventory = new PlayerInventory(out InventoryUI ui);
        IUI = new InventoryUI();
        IUI.Hide();
        //ui.Hide();

        if (MissionSingleton.Instance.HasLoot())
        {
            foreach (var item in MissionSingleton.Instance.GetLoot()) ItemCollection.Instance.items.Add(item);
        }

        foreach (var item in ItemCollection.Instance.items) IUI.SetItem(item);
    }

    private void Start()
    {
        HeroCollection.Instance.GenerateApplying(6);

        //for (int i = 0; i < 50; i++)
        //{
        //    IUI.Give(ItemCreator.CreateRandomItem());
        //}
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool toggle = !IUI.IsShowing;
            if (toggle) IUI.Show();
            else IUI.Hide();
        }
    }

    //public PlayerInventory Inventory { get; private set; }
}
