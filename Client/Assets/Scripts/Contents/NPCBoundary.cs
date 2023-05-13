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

    //���� �� ������ text ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MyPlayer")
        {
            if (Id == 9999)
                return;

            if(gameObject.tag == "FestivalNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id / 10 + 1].regionName} NPC�Դϴ�~~";
            }
            else if (gameObject.tag == "QuestNPC")
            {
                _bc.Chat = $"{Managers.Data.RegionPosDict[Id].regionName} ����Ʈ NPC�Դϴ�~~";
            }
        }
    }
}
