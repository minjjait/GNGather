﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Game
{
	public partial class GameRoom : JobSerializer
	{
		public void HandleMove(Player player, C_Move movePacket)
		{
            if (player == null)
				return;

			PositionInfo movePosInfo = movePacket.PosInfo;
			ObjectInfo info = player.Info;

			// 다른 좌표로 이동할 경우, 갈 수 있는지 체크
			if (movePosInfo.PosX != info.PosInfo.PosX || movePosInfo.PosY != info.PosInfo.PosY)
			{
				if (Map.CanGo(new Vector2Int(movePosInfo.PosX, movePosInfo.PosY)) == false)
					return;
			}

			info.PosInfo.State = movePosInfo.State;
			info.PosInfo.MoveDir = movePosInfo.MoveDir;
			Map.ApplyMove(player, new Vector2Int(movePosInfo.PosX, movePosInfo.PosY));

			// 다른 플레이어한테도 알려준다
			S_Move resMovePacket = new S_Move();
			resMovePacket.ObjectId = player.Info.ObjectId;
			resMovePacket.PosInfo = movePacket.PosInfo;

			Broadcast(player.CellPos, resMovePacket);
		}

		public void HandleSpeechBubble(Player player, C_SpeechBubble speechPacket)
		{
			if (player == null)
				return;

            Console.WriteLine($"채팅 : {speechPacket.Chat}");

			// 다른 플레이어한테도 알려준다
			S_SpeechBubble resSpeechPacket = new S_SpeechBubble();
			resSpeechPacket.ObjectId = player.Info.ObjectId;
			resSpeechPacket.Chat = speechPacket.Chat;

			Broadcast(player.CellPos, resSpeechPacket);
		}

		public void HandleInteractionFestival(Player player, C_InteractionFestival fesPacket)
		{
			if (player == null)
                return;

			using(AppDbContext db = new AppDbContext())
            {
				FestivalDb fesInfo = db.Festivals
					.Where(f => f.TemplateId == fesPacket.ObjectId).FirstOrDefault();


                Console.WriteLine(fesPacket.ObjectId);
				if(fesInfo == null)
                {
                    Console.WriteLine("정보가 없습니다");
					return;
                }

				S_InteractionFestival resFesPacket = new S_InteractionFestival()
				{
					FestivalName = fesInfo.FestivalName,
					FestivalExplain = fesInfo.FestivalExplain,
					RelatedAddress = fesInfo.RelatedAddress
				};

				player.Session.Send(resFesPacket);
            }

			DbTransaction.QuestSatisfied(player, fesPacket);
		}

		public void HandleAddQuest(Player player, GameRoom room, C_AddQuest questPacket)
        {
			if (player == null)
				return;

			DbTransaction.QuestAccept(player, room, questPacket);
        }

		public void HandleQuestClear(Player player, GameRoom room, C_QuestClear questClearPacket)
		{
			if (player == null)
				return;

			//이미 아이템이 있으면 리턴
			if (player.Items.ContainsKey(questClearPacket.QuestId))
				return;

			DbTransaction.QuestClear(player, room, questClearPacket);
		}

		public void HandleTransfortation(Player player, C_UseTransfortation transfortationPacket)
		{
			if (player == null)
				return;

			PositionInfo movePosInfo = transfortationPacket.PosInfo;
			ObjectInfo info = player.Info;

			info.PosInfo.State = movePosInfo.State;
			info.PosInfo.MoveDir = movePosInfo.MoveDir;
			Map.ApplyMove(player, new Vector2Int(movePosInfo.PosX, movePosInfo.PosY));

			// 다른 플레이어한테도 알려준다
			S_UseTransfortation resTransfortationPacket = new S_UseTransfortation();
			resTransfortationPacket.ObjectId = player.Info.ObjectId;
			resTransfortationPacket.PosInfo = transfortationPacket.PosInfo;

			Broadcast(player.CellPos, resTransfortationPacket);
		}

		public void HandleArrived(Player player, C_TransfortationArrived transfortationPacket)
		{
			if (player == null)
				return;

			PositionInfo movePosInfo = transfortationPacket.PosInfo;
			ObjectInfo info = player.Info;

			info.PosInfo.State = movePosInfo.State;
			info.PosInfo.MoveDir = movePosInfo.MoveDir;
			Map.ApplyMove(player, new Vector2Int(movePosInfo.PosX, movePosInfo.PosY));

			// 다른 플레이어한테도 알려준다
			S_TransfortationArrived resTransfortationPacket = new S_TransfortationArrived();
			resTransfortationPacket.ObjectId = player.Info.ObjectId;
			resTransfortationPacket.PosInfo = transfortationPacket.PosInfo;

			Broadcast(player.CellPos, resTransfortationPacket);
		}

	}
}
