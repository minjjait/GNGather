using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GameScene : UI_Scene
{
    public UI_Conversation ConversationUI { get; private set; }
    public UI_Inventory InventoryUI { get; private set; }
    public UI_Transfortation TransfortationUI { get; private set; }
    public UI_Error ErrorUI { get; private set; }

    enum GameObjects
    {
        Minimap
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        ConversationUI = GetComponentInChildren<UI_Conversation>();
        InventoryUI = GetComponentInChildren<UI_Inventory>();
        TransfortationUI = GetComponentInChildren<UI_Transfortation>();
        ErrorUI = GetComponentInChildren<UI_Error>();

        ConversationUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);
        TransfortationUI.gameObject.SetActive(false);
        ErrorUI.gameObject.SetActive(false);
    }

    public void CloseAll()
    {
        ConversationUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);
        TransfortationUI.gameObject.SetActive(false);
        ErrorUI.gameObject.SetActive(false);

    }
}
