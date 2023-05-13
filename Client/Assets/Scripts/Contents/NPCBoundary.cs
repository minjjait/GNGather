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

    //접근 시 정해진 text 출력
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MyPlayer")
        {
            if (Id == 9999)
                return;

            if(gameObject.tag == "FestivalNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id / 10 + 1].regionName} NPC입니다~~";
            }
            else if (gameObject.tag == "QuestNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id].regionName} 퀘스트 NPC입니다~~";
            }
        }
    }
}
