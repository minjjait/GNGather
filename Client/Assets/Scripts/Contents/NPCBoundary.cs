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
            if(gameObject.tag == "FestivalNPC")
            {
                _bc.Chat = $"{Id}번 NPC입니다~~";
            }
            else if (gameObject.tag == "QuestNPC")
            {
                _bc.Chat = $"{Id}번 퀘스트 NPC입니다~~";
            }
            else if (gameObject.tag == "TransfortationNPC")
            {
                _bc.Chat = $"{Id}번 버스 NPC입니다~~";
            }
        }
    }
}
