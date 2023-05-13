using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;
		Managers.Object.Add(enterGamePacket.Player, myPlayer: true);
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGameHandler = packet as S_LeaveGame;
		Managers.Object.Clear();
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = packet as S_Spawn;
		foreach (ObjectInfo obj in spawnPacket.Objects)
		{
			Managers.Object.Add(obj, myPlayer: false);
		}
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;
		foreach (int id in despawnPacket.ObjectIds)
		{
			Managers.Object.Remove(id);
		}
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet)
	{
		S_Move movePacket = packet as S_Move;

		GameObject go = Managers.Object.FindById(movePacket.ObjectId);

		if (go == null)
			return;

		if (Managers.Object.MyPlayer.Id == movePacket.ObjectId)
			return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if (cc == null)
			return;

		cc.PosInfo = movePacket.PosInfo;
	}

	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
	{
		Debug.Log("S_ConnectedHandler");
		C_Login loginPacket = new C_Login();

		loginPacket.UniqueId = Managers.Network.UniqueId;
		Managers.Network.Send(loginPacket);
	}

	// 로그인 OK + 캐릭터 목록
	public static void S_LoginHandler(PacketSession session, IMessage packet)
	{
		S_Login loginPacket = (S_Login)packet;
		Debug.Log($"LoginOk({loginPacket.LoginOk})");

		if (loginPacket.Players == null || loginPacket.Players.Count == 0)
		{
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
			Managers.Network.Send(createPacket);
		}
		else
		{
			//첫번째 로그인
			LobbyPlayerInfo info = loginPacket.Players[0];
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = info.Name;
			Managers.Network.Send(enterGamePacket);
		}
	}

	public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
	{
		S_CreatePlayer createOkPacket = (S_CreatePlayer)packet;

		if (createOkPacket.Player == null)
		{
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
			Managers.Network.Send(createPacket);
		}
		else
		{
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = createOkPacket.Player.Name;
			Managers.Network.Send(enterGamePacket);
		}
	}

	//유저 네트워크 연결 체크
	public static void S_PingHandler(PacketSession session, IMessage packet)
	{
		C_Pong pongPacket = new C_Pong();
		Debug.Log("[Server] PingCheck");
		Managers.Network.Send(pongPacket);
	}

	//말풍선 변경
	public static void S_SpeechBubbleHandler(PacketSession session, IMessage packet)
	{
		S_SpeechBubble chatPacket = packet as S_SpeechBubble;

		GameObject go = Managers.Object.FindById(chatPacket.ObjectId);

		if (go == null)
			return;

		CreatureController cc = go.GetComponent<CreatureController>();
		if (cc == null)
			return;

		cc.Chat = chatPacket.Chat;
	}

	//축제 NPC 정보 
	public static void S_InteractionFestivalHandler(PacketSession session, IMessage packet)
	{
		S_InteractionFestival fesPacket = packet as S_InteractionFestival;

		Managers.UI.OpenPopup = true;
		UI_FestivalInfo fesInfo = Managers.UI.ShowPopupUI<UI_FestivalInfo>();

		fesInfo.FestivalName = fesPacket.FestivalName;
		fesInfo.FestivalExplain = fesPacket.FestivalExplain;
		fesInfo.RelatedAddress = fesPacket.RelatedAddress;

		fesInfo.SetInfo();
	}

    #region QUEST&ITEM
	//아이템 리스트
    public static void S_ItemListHandler(PacketSession session, IMessage packet)
	{
		S_ItemList itemListPacket = packet as S_ItemList;

		Managers.Player.Items.Clear();

		foreach(int itemId in itemListPacket.ItemIds)
        {
            if (Managers.Player.Items.ContainsKey(itemId) == false)
            {
				Managers.Player.Items.Add(itemId, Managers.Data.ItemDict[itemId]);
            }
        }
	}

	//퀘스트 리스트
	public static void S_QuestListHandler(PacketSession session, IMessage packet)
	{
		S_QuestList questListPacket = packet as S_QuestList;

		Managers.Player.Quests.Clear();

		foreach (QuestInfo questInfo in questListPacket.Quests)
		{
			bool success = Managers.Player.HaveUniqueQuest(questInfo.TemplateId);
            if (success)
			{
				Quest quest= Managers.Player.MakeQuest(questInfo);
				Managers.Player.Quests.Add(quest.TemplateId, quest);
            }
		}
	}

	//퀘스트 추가
	public static void S_AddQuestHandler(PacketSession session, IMessage packet)
	{
		S_AddQuest addQuestPacket = packet as S_AddQuest;

		bool success = Managers.Player.HaveUniqueQuest(addQuestPacket.Quest.TemplateId);

		if (success)
		{
			Quest quest = Managers.Player.MakeQuest(addQuestPacket.Quest);
			Managers.Player.Quests.Add(quest.TemplateId, quest);
		}
	}

	//퀘스트 조건 충족시
	public static void S_QuestSatisfiedHandler(PacketSession session, IMessage packet)
	{
		S_QuestSatisfied questSatisfiedPacket = packet as S_QuestSatisfied;

		UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
		UI_Error errorUI = gameSceneUI.ErrorUI;
		errorUI.SetErrorMessage("퀘스트가 완료되었습니다!");

		if (Managers.Player.Quests.ContainsKey(questSatisfiedPacket.Quest.TemplateId))
        {
			Managers.Player.Quests[questSatisfiedPacket.Quest.TemplateId].IsCleared = true;
        }
	}

	//퀘스트 클리어 시
	public static void S_QuestClearHandler(PacketSession session, IMessage packet)
	{
		S_QuestClear questClearPacket = packet as S_QuestClear;

		Managers.Player.Items.Add(questClearPacket.ItemId, Managers.Data.ItemDict[questClearPacket.ItemId]);
	}


	#endregion

	//빠른 이동
	public static void S_UseTransfortationHandler(PacketSession session, IMessage packet)
	{
		S_UseTransfortation transfortationPacket = packet as S_UseTransfortation;

		GameObject go = Managers.Object.FindById(transfortationPacket.ObjectId);

		if (go == null)
			return;

		if (Managers.Object.MyPlayer.Id == transfortationPacket.ObjectId)
		{
			UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
			UI_Error errorUI = gameSceneUI.ErrorUI;
			errorUI.SetErrorMessage("이동 중입니다~");

			UI_Transfortation transfortation = gameSceneUI.TransfortationUI;
			transfortation.gameObject.SetActive(true);
			transfortation.Arrived();
			
			Managers.Object.MyPlayer.PosInfo = transfortationPacket.PosInfo;
			return;
        }

		CreatureController cc = go.GetComponent<CreatureController>();
		if (cc == null)
			return;

		cc.PosInfo = transfortationPacket.PosInfo;
	}

	//빠른 이동 도착
	public static void S_TransfortationArrivedHandler(PacketSession session, IMessage packet)
	{
		S_TransfortationArrived arrivedPacket = packet as S_TransfortationArrived;

		GameObject go = Managers.Object.FindById(arrivedPacket.ObjectId);

		if (go == null)
			return;

		if (Managers.Object.MyPlayer.Id == arrivedPacket.ObjectId)
		{
			Managers.Player.UsingTransfortation = false;
			UI_GameScene gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
			UI_Error errorUI = gameSceneUI.ErrorUI;
			errorUI.SetErrorMessage("도착~!");
			Debug.Log("도착~");
			Managers.Object.MyPlayer.PosInfo = arrivedPacket.PosInfo;
			return;
		}

		CreatureController cc = go.GetComponent<CreatureController>();
		if (cc == null)
			return;

		cc.PosInfo = arrivedPacket.PosInfo;
	}
}