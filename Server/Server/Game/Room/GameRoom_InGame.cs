using Google.Protobuf;
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

			// TODO : 검증
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

			// TODO : 검증 할게 있나..?

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
					.Where(f => f.FestivalDbId == fesPacket.ObjectId).FirstOrDefault();

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
		}
	}
}
