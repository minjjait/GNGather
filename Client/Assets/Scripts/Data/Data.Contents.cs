using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	#region Item
	[Serializable]
	public class Item
	{
		public int id;
		public string name;
		public string iconPath;
	}

	[Serializable]
	public class ItemData : ILoader<int, Item>
	{
		public List<Item> items = new List<Item>();

		public Dictionary<int, Item> MakeDict()
		{
			Dictionary<int, Item> dict = new Dictionary<int, Item>();
			foreach (Item item in items)
				dict.Add(item.id, item);
			return dict;
		}
	}
	#endregion

	#region Quest
	[Serializable]
	public class Quest
	{
		public int id;
		public string name;
		public string info;
		public int rewardItemId;
	}

	[Serializable]
	public class QuestData : ILoader<int, Quest>
	{
		public List<Quest> quests = new List<Quest>();

		public Dictionary<int, Quest> MakeDict()
		{
			Dictionary<int, Quest> dict = new Dictionary<int, Quest>();
			foreach (Quest quest in quests)
				dict.Add(quest.id, quest);
			return dict;
		}
	}
	#endregion
}