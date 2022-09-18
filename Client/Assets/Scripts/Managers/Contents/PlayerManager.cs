using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public Dictionary<int, Data.Item> Items { get; set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.Quest> Quests { get; set; } = new Dictionary<int, Data.Quest>();
    public bool[] QuestCleared { get; set; } = new bool[18];
    public int QuestNPCId { get; set; }
}