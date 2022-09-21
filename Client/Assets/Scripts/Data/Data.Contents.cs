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
	public class QuestData
	{
		public int id;
		public string name;
		public string info;
		public int rewardItemId;
	}

	[Serializable]
	public class QuestLoader : ILoader<int, QuestData>
	{
		public List<QuestData> quests = new List<QuestData>();

		public Dictionary<int, QuestData> MakeDict()
		{
			Dictionary<int, QuestData> dict = new Dictionary<int, QuestData>();
			foreach (QuestData quest in quests)
				dict.Add(quest.id, quest);
			return dict;
		}
	}
	#endregion
}