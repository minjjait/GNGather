using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public Dictionary<int, Data.Item> Items { get; set; } = new Dictionary<int, Data.Item>();
    public Dictionary<int, Quest> Quests { get; set; } = new Dictionary<int, Quest>();
    public int QuestNPCId { get; set; }
    public int RegionId { get; set; }
    public bool UsingTransfortation { get; set; } = false;
    public int ErrorNum { get; set; } = 0;

    public bool HaveUniqueQuest(int id)
    {
        bool contain = Quests.ContainsKey(id);
        if (contain)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Quest MakeQuest(QuestInfo questInfo)
	{
		Quest quest = null;

		QuestData questData = null;

		Managers.Data.QuestDict.TryGetValue(questInfo.TemplateId, out questData);

		if (questData == null)
			return null;

		quest.QuestDbId = questInfo.QuestDbId;
        quest.TemplateId = questInfo.TemplateId;
        quest.IsCleared = questInfo.IsCleared;

		return quest;
	}
}