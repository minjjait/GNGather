using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuestInfo : UI_Popup
{
    int _id = 9999;
    Data.QuestData _quest = null;

    enum Buttons
    {
        CancelButton,
        YesButton,
        NoButton,
        ClearButton
    }

    enum Texts
    {
        QuestNameText,
        QuestExplainText,
        ClearText
    }

    public override void Init()
    {
        base.Init();

        _id = Managers.Player.QuestNPCId;
        _quest = Managers.Data.QuestDict[_id];
        
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        SetTexts();
        SetButtons();
    }

    void SetTexts()
    {
        GetText((int)Texts.QuestNameText).text = _quest.name;
        GetText((int)Texts.QuestExplainText).text = _quest.info;
    }

    void SetButtons()
    {
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(
            (PointerEventData evt) => { Managers.Sound.Play("ClickSound"); ClosePopupUI(); Managers.UI.OpenPopup = false; });

        Quest quest = null;
        Managers.Player.Quests.TryGetValue(_id, out quest);

        //퀘스트 없을 시
        if(quest == null)
        {
            GetButton((int)Buttons.ClearButton).gameObject.SetActive(false);

            GetButton((int)Buttons.YesButton).gameObject.BindEvent(
                (PointerEventData evt) => { Managers.Sound.Play("ClickSound"); AcceptQuest(); });

            GetButton((int)Buttons.NoButton).gameObject.BindEvent(
                (PointerEventData evt) => { Managers.Sound.Play("ClickSound"); ClosePopupUI(); Managers.UI.OpenPopup = false; });
        }
        else
        {
            GetButton((int)Buttons.YesButton).gameObject.SetActive(false);
            GetButton((int)Buttons.NoButton).gameObject.SetActive(false);

            Data.Item item = null;
            Managers.Player.Items.TryGetValue(_id, out item);

            //퀘스트 이미 클리어 했을 시
            if (item != null)
            {
                GetText((int)Texts.ClearText).text = "이미 클리어한 퀘스트입니다";
                GetButton((int)Buttons.ClearButton).interactable = false;
            }
            else
            {
                if(Managers.Player.Quests[_id].IsCleared == true)//퀘스트 조건 충족시
                {
                    GetText((int)Texts.ClearText).text = "Quest Clear!";
                    GetButton((int)Buttons.ClearButton).gameObject.BindEvent(
                        (PointerEventData evt) => { Managers.Sound.Play("ClickSound"); QuestClear(); });
                }
                else//퀘스트 조건 불충족 시
                {
                    GetText((int)Texts.ClearText).text = "퀘스트 진행 중";
                    GetButton((int)Buttons.ClearButton).interactable = false;
                }
            }
        }
    }

    void AcceptQuest()
    {
        bool success = Managers.Player.HaveUniqueQuest(_id);

        //퀘스트 패킷 보내기(C_ADD_QUEST)
        //DB에 퀘스트 추가
        if (success)
        {
            C_AddQuest questPacket = new C_AddQuest();
            questPacket.QuestId = _id;
            Managers.Network.Send(questPacket);
        }

        GetButton((int)Buttons.YesButton).gameObject.SetActive(false);
        GetButton((int)Buttons.NoButton).gameObject.SetActive(false);

        GetButton((int)Buttons.ClearButton).gameObject.SetActive(true);
        GetText((int)Texts.ClearText).text = "퀘스트 진행 중";
        GetButton((int)Buttons.ClearButton).interactable = false;
    }

    void QuestClear()
    {
        //퀘스트 패킷 보내기(C_QUEST_CLEAR)
        //클리어했다고 DB 추가
        C_QuestClear questClearPacket = new C_QuestClear();
        questClearPacket.QuestId = _id;
        Managers.Network.Send(questClearPacket);

        GetText((int)Texts.ClearText).text = "이미 클리어한 퀘스트입니다";
        GetButton((int)Buttons.ClearButton).interactable = false;
    }
}