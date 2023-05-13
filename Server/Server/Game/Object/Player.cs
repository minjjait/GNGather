using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DB;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class Player : GameObject
	{
		public int PlayerDbId { get; set; }
		public ClientSession Session { get; set; }
		public VisionCube Vision { get; private set; }
		public Dictionary<int, Quest> Quests { get; } = new Dictionary<int, Quest>();
		public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();

		public Player()
		{
			ObjectType = GameObjectType.Player;
			Vision = new VisionCube(this);
		}

		public void OnLeaveGame()
		{
		}
	}
}
