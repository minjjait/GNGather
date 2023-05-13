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
		_coConversation = StartCoroutine("CoConversation");
		GetText((int)Texts.Text).text = chat;
	}

	IEnumerator CoConversation()
    {
		yield return new WaitForSeconds(3.0f);
		gameObject.SetActive(false);
    }
}