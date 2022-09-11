using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Conversation : UI_Base
{
    enum Texts
    {
        ConversationText
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
    }
}
