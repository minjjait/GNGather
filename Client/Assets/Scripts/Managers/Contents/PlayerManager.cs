using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public Dictionary<int, Data.Item> Items { get; set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.QuestData> Quests { get; set; } = new Dictionary<int, Data.QuestData>();
    public bool[] QuestCleared { get; set; } = new bool[19];
    public int QuestNPCId { get; set; }
    public int RegionId { get; set; }
    public bool UsingTransfortation { get; set; } = false;
    public int ErrorNum { get; set; } = 0;

    public bool HaveUniqueQuest(int id, QuestData quest)
    {
        bool contain = Quests.ContainsKey(id);
        if (contain)
        {
            return false;
        }
        else
        {
            Quests.Add(id, quest);
            return true;
        }
    }
}