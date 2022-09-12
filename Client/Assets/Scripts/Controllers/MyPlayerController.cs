﻿using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class MyPlayerController : PlayerController
{
	NPCBoundary _festivalNPC = null;
	NPCBoundary _questNPC = null;

	bool _moveKeyPressed = false;
	bool _doConversation = false;

	protected override void Init()
	{
		base.Init();
	}

	// 키보드 입력
	protected override void UpdateController()
	{
		if (Managers.UI.OpenPopup)
			return;

		GetUIKeyInput();

		switch (State)
		{
			case CreatureState.Idle:
				GetDirInput();
				break;
			case CreatureState.Moving:
				GetDirInput();
				break;
		}

		base.UpdateController();
	}

	protected override void UpdateIdle()
	{
		// 이동 상태로 갈지 확인
		if (_moveKeyPressed)
		{
			State = CreatureState.Moving;
			return;
		}

	}

	void LateUpdate()
	{
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}

	void GetUIKeyInput()
	{
		//인벤토리
        if (Input.GetKeyDown(KeyCode.I))
        {
			if (_questNPC == null)
				return;

			UI_GameScene gameScene = Managers.UI.SceneUI as UI_GameScene;
			UI_Inventory inventoryUI = gameScene.InventoryUI;

			//패킷 보내고
			//내가 현재 가지고 있는 아이템 목록을 긁어서 서버에서 패킷을 보낸다
			//패킷을 받으면 인벤토리 setActive true하고 있는 아이템만 표출하고 나머지 setActive false
        }

		//채팅
		if (Input.GetKeyDown(KeyCode.Return))
		{
			UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
			UI_Conversation conversationUI = gameSceneUI.ConversationUI;
			InputField conversationInputField = conversationUI.gameObject.GetComponent<InputField>();

			if (conversationUI.gameObject.activeSelf)
            {
				_doConversation = false;

				if(conversationInputField.text != "")
				{
					//채팅창 내용 말풍선으로 전달
					C_SpeechBubble speechPacket = new C_SpeechBubble();
					speechPacket.Chat = conversationInputField.text;
					Managers.Network.Send(speechPacket);
				}

				conversationInputField.text = "";
				conversationUI.gameObject.SetActive(false);
            }
            else
            {
				_doConversation = true;
				conversationUI.gameObject.SetActive(true);
				conversationInputField.ActivateInputField();
            }
		}

		//상호작용
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (_festivalNPC == null)
				return;

			C_InteractionFestival fesPacket = new C_InteractionFestival();
			fesPacket.ObjectId = _festivalNPC.Id;
			Managers.Network.Send(fesPacket);
		}
	}

	void GetDirInput()
	{
		if (_doConversation == true)
			return;

		_moveKeyPressed = true;

		if (Input.GetKey(KeyCode.W))
		{
			Dir = MoveDir.Up;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			Dir = MoveDir.Down;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			Dir = MoveDir.Left;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			Dir = MoveDir.Right;
		}
		else
		{
			_moveKeyPressed = false;
		}
	}

	protected override void MoveToNextPos()
	{
		if (_moveKeyPressed == false)
		{
			State = CreatureState.Idle;
			CheckUpdatedFlag();
			return;
		}

		Vector3Int destPos = CellPos;

		switch (Dir)
		{
			case MoveDir.Up:
				destPos += Vector3Int.up;
				break;
			case MoveDir.Down:
				destPos += Vector3Int.down;
				break;
			case MoveDir.Left:
				destPos += Vector3Int.left;
				break;
			case MoveDir.Right:
				destPos += Vector3Int.right;
				break;
		}

		if (Managers.Map.CanGo(destPos))
		{
			if (Managers.Object.FindCreature(destPos) == null)
			{
				CellPos = destPos;
			}
		}

		CheckUpdatedFlag();
	}

	protected override void CheckUpdatedFlag()
	{
		if (_updated)
		{
			C_Move movePacket = new C_Move();
			movePacket.PosInfo = PosInfo;
			Managers.Network.Send(movePacket);
			_updated = false;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "FestivalNPC")
		{
			_festivalNPC = collision.gameObject.GetComponent<NPCBoundary>();
		}
		else if (collision.tag == "QuestNPC")
		{
			_questNPC = collision.gameObject.GetComponent<NPCBoundary>();
		}
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "FestivalNPC")
		{
			_festivalNPC = null;
		}
		else if (collision.tag == "QuestNPC")
		{
			_questNPC = null;
		}
	}
}