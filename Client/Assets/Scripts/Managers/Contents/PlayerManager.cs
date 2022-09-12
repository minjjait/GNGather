using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public Dictionary<int, Data.Item> Items { get; private set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Data.Quest> Quests { get; private set; } = new Dictionary<int, Data.Quest>();
    public bool[] QuestCleared { get; private set; } = new bool[18];
}