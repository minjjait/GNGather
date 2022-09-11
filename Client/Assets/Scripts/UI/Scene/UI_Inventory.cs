using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    enum GameObjects
    {
        Background
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        for(int i = 0; i < 17; i++)
        {
            GetObject((int)GameObjects.Background).transform.GetChild(i)
                .GetChild(1).gameObject.SetActive(false);
        }
    }
}
