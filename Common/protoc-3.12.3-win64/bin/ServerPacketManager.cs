using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.CMove, MakePacket<C_Move>);
		_handler.Add((ushort)MsgId.CMove, PacketHandler.C_MoveHandler);		
		_onRecv.Add((ushort)MsgId.CLogin, MakePacket<C_Login>);
		_handler.Add((ushort)MsgId.CLogin, PacketHandler.C_LoginHandler);		
		_onRecv.Add((ushort)MsgId.CCreatePlayer, MakePacket<C_CreatePlayer>);
		_handler.Add((ushort)MsgId.CCreatePlayer, PacketHandler.C_CreatePlayerHandler);		
		_onRecv.Add((ushort)MsgId.CEnterGame, MakePacket<C_EnterGame>);
		_handler.Add((ushort)MsgId.CEnterGame, PacketHandler.C_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.CPong, MakePacket<C_Pong>);
		_handler.Add((ushort)MsgId.CPong, PacketHandler.C_PongHandler);		
		_onRecv.Add((ushort)MsgId.CSpeechBubble, MakePacket<C_SpeechBubble>);
		_handler.Add((ushort)MsgId.CSpeechBubble, PacketHandler.C_SpeechBubbleHandler);		
		_onRecv.Add((ushort)MsgId.CInteractionFestival, MakePacket<C_InteractionFestival>);
		_handler.Add((ushort)MsgId.CInteractionFestival, PacketHandler.C_InteractionFestivalHandler);		
		_onRecv.Add((ushort)MsgId.CAddQuest, MakePacket<C_AddQuest>);
		_handler.Add((ushort)MsgId.CAddQuest, PacketHandler.C_AddQuestHandler);		
		_onRecv.Add((ushort)MsgId.CQuestClear, MakePacket<C_QuestClear>);
		_handler.Add((ushort)MsgId.CQuestClear, PacketHandler.C_QuestClearHandler);		
		_onRecv.Add((ushort)MsgId.CUseTransfortation, MakePacket<C_UseTransfortation>);
		_handler.Add((ushort)MsgId.CUseTransfortation, PacketHandler.C_UseTransfortationHandler);		
		_onRecv.Add((ushort)MsgId.CTransfortationArrived, MakePacket<C_TransfortationArrived>);
		_handler.Add((ushort)MsgId.CTransfortationArrived, PacketHandler.C_TransfortationArrivedHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}