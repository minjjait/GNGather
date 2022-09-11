using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{
	UI_SpeechBubble _speechBubble;

	private string _chat;
	public string Chat
	{
		get
		{
			return _chat;
		}
		set
		{
			if (_speechBubble == null)
				return;

			_chat = value;

			_speechBubble.SetSpeechBubble(_chat);
		}
	}

	protected void AddSpeechBubble()
	{
		GameObject go = Managers.Resource.Instantiate("UI/UI_SpeechBubble", transform);
		go.transform.localPosition = new Vector3(0, 0.8f, 0);
		go.name = "SpeechBubble";
		_speechBubble = go.GetComponent<UI_SpeechBubble>();
	}

	void Start()
	{
		Init();
	}

	protected virtual void Init()
	{
		AddSpeechBubble();
	}

}
