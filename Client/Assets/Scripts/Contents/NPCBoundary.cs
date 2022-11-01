using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBoundary : MonoBehaviour
{
    public int Id = 9999;
    BaseController _bc;

    private void Start()
    {    
        _bc = GetComponentInParent<BaseController>();
        Id = int.Parse(_bc.gameObject.name);
    }

    //이것도 일단 드가서 3초동안 대화창 띄우고 다시 사라진다
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MyPlayer")
        {
            if (Id == 9999)
                return;

            if(gameObject.tag == "FestivalNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id].regionName} NPC입니다~~";
            }
            else if (gameObject.tag == "QuestNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id].regionName} 퀘스트 NPC입니다~~";

                Id = Id / 10 + 1;
            }
            else if (gameObject.tag == "TransfortationNPC")
            {
                _bc.Chat = $"이 곳은 {Managers.Data.RegionPosDict[Id].regionName}입니다";
            }
        }
    }
}
