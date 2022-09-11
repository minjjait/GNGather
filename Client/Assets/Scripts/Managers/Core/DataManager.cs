using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Item> ItemDict { get; private set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.Quest> QuestDict  { get; private set; } = new Dictionary<int, Data.Quest>();

	public void Init()
    {
       ItemDict = LoadJson<Data.ItemData, int, Data.Item>("ItemData").MakeDict();
       QuestDict = LoadJson<Data.QuestData, int, Data.Quest>("QuestData").MakeDict();
	}

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
