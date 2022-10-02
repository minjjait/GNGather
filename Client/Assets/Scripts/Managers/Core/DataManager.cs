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
    public Dictionary<int, Data.QuestData> QuestDict  { get; private set; } = new Dictionary<int, Data.QuestData>();
    public Dictionary<int, Data.RegionPos> RegionPosDict  { get; private set; } = new Dictionary<int, Data.RegionPos>();

	public void Init()
    {
        ItemDict = LoadJson<Data.ItemData, int, Data.Item>("ItemData").MakeDict(); 
        QuestDict = LoadJson<Data.QuestLoader, int, Data.QuestData>("QuestData").MakeDict();
        RegionPosDict = LoadJson<Data.RegionPosData, int, Data.RegionPos>("RegionPosData").MakeDict();

    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
