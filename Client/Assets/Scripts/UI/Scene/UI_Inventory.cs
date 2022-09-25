using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    enum GameObjects
    {
        InventoryBackground
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < 18; i++)
        {
            GetObject((int)GameObjects.InventoryBackground).transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < Managers.Player.Items.Count; i++)
        {
            GetObject((int)GameObjects.InventoryBackground).transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
