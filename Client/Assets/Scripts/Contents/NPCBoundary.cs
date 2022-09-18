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

    //�̰͵� �ϴ� �尡�� 3�ʵ��� ��ȭâ ���� �ٽ� �������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MyPlayer")
        {
            if(gameObject.tag == "FestivalNPC")
            {
                _bc.Chat = $"{Id}�� NPC�Դϴ�~~";
            }
            else if (gameObject.tag == "QuestNPC")
            {
                _bc.Chat = $"{Id}�� ����Ʈ NPC�Դϴ�~~";
            }
            else if (gameObject.tag == "TransfortationNPC")
            {
                _bc.Chat = $"{Id}�� ���� NPC�Դϴ�~~";
            }
        }
    }
}
