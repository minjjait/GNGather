using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeechBubble : UI_Base
{
	private Coroutine _coConversation;
	enum Texts
	{
		Text
	}

	public override void Init()
	{
		Bind<Text>(typeof(Texts));

		gameObject.SetActive(false);
	}

	//update
	public void SetSpeechBubble(string chat)
	{
		gameObject.SetActive(true);
		//될 것 같긴 하는데
		//어쨌든 시간 정보를 어디에 쑤셔넣는가
		//TODO : 완벽성 추구
        /* 
		if (Evefefe)//3초보다 작으면차이시간만큼
        {
			StartCoroutine("CoConversation", //시간);
			//연산연산 코루틴 
		}
        else
        {

        }
		*/
		_coConversation = StartCoroutine("CoConversation");
		GetText((int)Texts.Text).text = chat;
	}

	IEnumerator CoConversation()
    {
		yield return new WaitForSeconds(3.0f);
		gameObject.SetActive(false);
    }
}