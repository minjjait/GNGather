using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DB
{
	public partial class DbTransaction : JobSerializer
	{
		public static DbTransaction Instance { get; } = new DbTransaction();

		// Me (GameRoom) -> You (Db) -> Me (GameRoom)
		public static void SavePlayerStatus_AllInOne(Player player, GameRoom room)
		{
			if (player == null || room == null)
				return;

			// Me (GameRoom)
			PlayerDb playerDb = new PlayerDb();
			playerDb.PlayerDbId = player.PlayerDbId;

			// You
			Instance.Push(() =>
			{
				using (AppDbContext db = new AppDbContext())
				{
					db.Entry(playerDb).State = EntityState.Unchanged;
					//db.Entry(playerDb).Property(nameof(PlayerDb.Hp)).IsModified = true;
					bool success = db.SaveChangesEx();
					if (success)
					{
						// Me
					}
				}
			});			
		}

		public static void QuestAccept(Player player, GameRoom room, C_AddQuest questPacket)
		{
			if (player == null || room == null)
				return;

			// 1) DB에다가 저장 요청
			// 2) DB 저장 OK
			// 3) 메모리에 적용
			QuestDb questDb = new QuestDb()
			{
				TemplateId = questPacket.QuestId,
				IsCleared = false,
				OwnerDbId = player.PlayerDbId
			};

			// You
			Instance.Push(() =>
			{
				using (AppDbContext db = new AppDbContext())
				{
					db.Quests.Add(questDb);
					bool success = db.SaveChangesEx();
					if (success)
					{
						/*
						 Me
						room.Push(() =>
						{
							//Item newItem = Item.MakeItem(itemDb);
							//player.Inven.Add(newItem);

							// Client Noti
							{
								S_AddItem itemPacket = new S_AddItem();
								ItemInfo itemInfo = new ItemInfo();
								itemInfo.MergeFrom(newItem.Info);
								itemPacket.Items.Add(itemInfo);

								player.Session.Send(itemPacket);
							 
							}
						});
						*/
					}
				}
			});
		}


		public static void QuestClear(Player player, GameRoom room, C_QuestClear questClearPacket)
		{
			if (player == null || room == null)
				return;

			// 1) DB에다가 저장 요청
			// 2) DB 저장 OK
			// 3) 메모리에 적용
			ItemDb itemDb = new ItemDb
			{
				TemplateId = questClearPacket.QuestId,
				OwnerDbId = player.PlayerDbId
			};

			// You
			Instance.Push(() =>
			{
				using (AppDbContext db = new AppDbContext())
				{
					db.Items.Add(itemDb);
					bool success = db.SaveChangesEx();
					if (success)
					{
						player.Items.Add(questClearPacket.QuestId, DataManager.ItemDict[questClearPacket.QuestId]);

						//Me
						room.Push(() =>
						{
							// Client Noti
							{
								S_QuestClear itemPacket = new S_QuestClear();
								itemPacket.ItemId = questClearPacket.QuestId;

								player.Session.Send(itemPacket);
							}
						});
					}
				}
			});
		}
		public static void QuestSatisfied(Player player, C_Move movePacket)
		{
			if (player == null)
				return;


			int questRegion = movePacket.RegionId / 10;
			if (questRegion == 0)
				questRegion = 18;
			
			if (player.Quests.ContainsKey(questRegion) == false)
				return;

			if (player.Quests[questRegion].IsCleared == true)
				return;

			// You
			Instance.Push(() =>
			{
				using (AppDbContext db = new AppDbContext())
				{
					QuestDb quest = db.Quests
						.Where(q => q.OwnerDbId == player.PlayerDbId && q.TemplateId == questRegion)
						.FirstOrDefault();

					quest.IsCleared = true;

					db.Entry(quest).Property(nameof(QuestDb.IsCleared)).IsModified = true;

					bool success = db.SaveChangesEx();

					if (success)
					{
						player.Room.Push(() =>
						{
							// Client Noti
							{
								//퀘스트 만족 패킷을 보낸다
								S_QuestSatisfied questSatisfiedPacket = new S_QuestSatisfied();
								questSatisfiedPacket.Quest = new QuestInfo();
								QuestInfo quest = new QuestInfo();
								quest.QuestDbId = player.Quests[questRegion].QuestDbId;
								quest.TemplateId = player.Quests[questRegion].TemplateId;
								quest.IsCleared = player.Quests[questRegion].IsCleared = true;
								questSatisfiedPacket.Quest.MergeFrom(quest);

								player.Session.Send(questSatisfiedPacket);
							}
						});
					}
				}
			});
		}
	}
}
