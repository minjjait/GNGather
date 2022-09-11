using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_Conversation ConversationUI { get; private set; }
    public UI_Inventory InventoryUI { get; private set; }

    enum GameObjects
    {
    }

    public override void Init()
    {
        base.Init();

        ConversationUI = GetComponentInChildren<UI_Conversation>();
        InventoryUI = GetComponentInChildren<UI_Inventory>();

        ConversationUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);
    }
}
