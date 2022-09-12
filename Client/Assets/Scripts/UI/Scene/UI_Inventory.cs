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

        for(int i = 0; i < 18; i++)
        {
            GetObject((int)GameObjects.InventoryBackground).transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
