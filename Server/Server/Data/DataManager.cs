using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Data
{
	public interface ILoader<Key, Value>
	{
		Dictionary<Key, Value> MakeDict();
	}

	public class DataManager
	{
		public static Dictionary<int, Item> ItemDict { get; private set; } = new Dictionary<int, Item>();
		public static Dictionary<int, Quest> QuestDict { get; private set; } = new Dictionary<int, Quest>();

		public static void LoadData()
		{
			ItemDict = LoadJson<Data.ItemData, int, Data.Item>("ItemData").MakeDict();
			QuestDict = LoadJson<Data.QuestData, int, Data.Quest>("QuestData").MakeDict();
		}

		static Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
		{
			string text = File.ReadAllText($"{ConfigManager.Config.dataPath}/{path}.json");
			return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(text);
		}
	}
}
