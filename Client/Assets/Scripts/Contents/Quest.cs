using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Quest
{   
    public QuestInfo Info { get; } = new QuestInfo();

    public int QuestDbId
    {
        get { return Info.QuestDbId; }
        set { Info.QuestDbId = value; }
    }

    public int TemplateId
    {
        get { return Info.TemplateId; }
        set { Info.TemplateId = value; }
    }

    public bool IsCleared
    {
        get { return Info.IsCleared; }
        set { Info.IsCleared = value; }
    }

}