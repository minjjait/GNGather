using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	// Step4
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterGamePacket = packet as S_EnterGame;
	}

	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leaveGameHandler = packet as S_LeaveGame;
	}

	public static void S_SpawnHandler(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = packet as S_Spawn;
	}

	public static void S_DespawnHandler(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;
	}

	public static void S_MoveHandler(PacketSession session, IMessage packet)
	{
		S_Move movePacket = packet as S_Move;
	}
	// Step1
	public static void S_ConnectedHandler(PacketSession session, IMessage packet)
	{
		C_Login loginPacket = new C_Login();

		ServerSession serverSession = (ServerSession)session;
		loginPacket.UniqueId = $"DummyClient_{serverSession.DummyId.ToString("0000")}";
		serverSession.Send(loginPacket);
	}

	// Step2
	// 로그인 OK + 캐릭터 목록
	public static void S_LoginHandler(PacketSession session, IMessage packet)
	{
		S_Login loginPacket = (S_Login)packet;
		ServerSession serverSession = (ServerSession)session;

		// TODO : 로비 UI에서 캐릭터 보여주고, 선택할 수 있도록
		if (loginPacket.Players == null || loginPacket.Players.Count == 0)
		{
			C_CreatePlayer createPacket = new C_CreatePlayer();
			createPacket.Name = $"Player_{serverSession.DummyId.ToString("0000")}";
			serverSession.Send(createPacket);
		}
		else
		{
			// 무조건 첫번째 로그인
			LobbyPlayerInfo info = loginPacket.Players[0];
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = info.Name;
			serverSession.Send(enterGamePacket);
		}
	}

	// Step3
	public static void S_CreatePlayerHandler(PacketSession session, IMessage packet)
	{
		S_CreatePlayer createOkPacket = (S_CreatePlayer)packet;
		ServerSession serverSession = (ServerSession)session;

		if (createOkPacket.Player == null)
		{
			// 생략
		}
		else
		{
			C_EnterGame enterGamePacket = new C_EnterGame();
			enterGamePacket.Name = createOkPacket.Player.Name;
			serverSession.Send(enterGamePacket);
		}
	}

	public static void S_PingHandler(PacketSession session, IMessage packet)
	{
		C_Pong pongPacket = new C_Pong();
	}

	public static void S_SpeechBubbleHandler(PacketSession session, IMessage packet)
	{
		S_SpeechBubble chatPacket = packet as S_SpeechBubble;
	}

	public static void S_InteractionFestivalHandler(PacketSession session, IMessage packet)
	{
		S_InteractionFestival interactionPacket = packet as S_InteractionFestival;
	}

	public static void S_ItemListHandler(PacketSession session, IMessage packet)
	{
		S_ItemList itemListPacket = packet as S_ItemList;
	}
	public static void S_QuestListHandler(PacketSession session, IMessage packet)
	{
		S_QuestList questListPacket = packet as S_QuestList;
	}
	public static void S_AddQuestHandler(PacketSession session, IMessage packet)
	{
		S_AddQuest addQuestPacket = packet as S_AddQuest;
	}
	public static void S_QuestClearHandler(PacketSession session, IMessage packet)
	{
		S_QuestClear questClearPacket = packet as S_QuestClear;
	}

	public static void S_QuestSatisfiedHandler(PacketSession session, IMessage packet)
	{
		S_QuestSatisfied questSatisfiedPacket = packet as S_QuestSatisfied;
	}
	public static void S_UseTransfortationHandler(PacketSession session, IMessage packet)
	{
		S_UseTransfortation useTransdPacket = packet as S_UseTransfortation;
	}
	public static void S_TransfortationArrivedHandler(PacketSession session, IMessage packet)
	{
		S_TransfortationArrived transArrivedPacket = packet as S_TransfortationArrived;
	}
}


