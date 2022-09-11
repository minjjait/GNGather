using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FestivalInfo : UI_Popup
{
    public string FestivalName;
    public string FestivalExplain;
    public string RelatedAddress;

    enum Buttons
    {
        CancelButton,
        RelatedAddressButton
    }

    enum Texts
    {
        FestivalNameText,
        FestivalExplainText,
        RelatedAddressText,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(
            (PointerEventData evt) => { ClosePopupUI(); Managers.UI.OpenPopup = false; });
    }
    
    public void SetInfo()
    {
        GetText((int)Texts.FestivalNameText).text = FestivalName;
        GetText((int)Texts.FestivalExplainText).text = FestivalExplain;
        GetText((int)Texts.RelatedAddressText).text = RelatedAddress;

        GetButton((int)Buttons.RelatedAddressButton).gameObject.BindEvent(
            (PointerEventData evt) => Application.OpenURL(RelatedAddress));
    }
}
