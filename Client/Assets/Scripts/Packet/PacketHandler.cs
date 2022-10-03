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

		string path = Application.dataPath;
		loginPacket.UniqueId = path.GetHashCode().ToString();
		Managers.Network.Send(loginPacket);
	}

	// 로그인 OK + 캐릭터 목록
	public static void S_LoginHandler(PacketSession session, IMessage packet)
	{
		S_Login loginPacket = (S_Login)packet;
		Debug.Log($"LoginOk({loginPacket.LoginOk})");

		// TODO : 로비 UI에서 캐릭터 보여주고, 선택할 수 있도록
		if (loginPacket.Players == null || loginPacket.Players.Count == 0)
		{
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{Random.Range(0, 10000).ToString("0000")}";
			Managers.Network.Send(createPacket);
		}
		else
		{
			// 무조건 첫번째 로그인
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

	public static void S_PingHandler(PacketSession session, IMessage packet)
	{
		C_Pong pongPacket = new C_Pong();
		Debug.Log("[Server] PingCheck");
		Managers.Network.Send(pongPacket);
	}

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

	public static void S_QuestListHandler(PacketSession session, IMessage packet)
	{
		S_QuestList questListPacket = packet as S_QuestList;

		Managers.Player.Quests.Clear();

		foreach (QuestInfo quest in questListPacket.Quests)
		{
			bool success = Managers.Player.HaveUniqueQuest(quest.TemplateId, Managers.Data.QuestDict[quest.TemplateId]);
			if(success)
				Managers.Player.QuestCleared[quest.TemplateId] = quest.IsCleared;
		}
	}

	public static void S_QuestSatisfiedHandler(PacketSession session, IMessage packet)
	{
		S_QuestSatisfied questSatisfiedPacket = packet as S_QuestSatisfied;

        if (Managers.Player.Quests.ContainsKey(questSatisfiedPacket.Quest.TemplateId))
        {
			Managers.Player.QuestCleared[questSatisfiedPacket.Quest.TemplateId] = true;
        }
	}
	public static void S_QuestClearHandler(PacketSession session, IMessage packet)
	{
		S_QuestClear questClearPacket = packet as S_QuestClear;

		Managers.Player.Items.Add(questClearPacket.ItemId, Managers.Data.ItemDict[questClearPacket.ItemId]);
	}


	#endregion
}