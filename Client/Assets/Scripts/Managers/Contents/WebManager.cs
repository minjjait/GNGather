using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager
{
	//api서버 url
    public string BaseUrl { get; set; } = "http://203.255.3.97:5001/api";

	public void SendPostRequest<T>(string url, object obj, Action<T> res)
	{
		Managers.Instance.StartCoroutine(CoSendWebRequest(url, UnityWebRequest.kHttpVerbPOST, obj, res));
	}

	//request 전송 및 처리
    IEnumerator CoSendWebRequest<T>(string url, string method, object obj, Action<T> res)
	{
		string sendUrl = $"{BaseUrl}/{url}";

		byte[] jsonBytes = null;
		if (obj != null)
		{
			string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
		}

		using (var uwr = new UnityWebRequest(sendUrl, method))
		{
			uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
			uwr.downloadHandler = new DownloadHandlerBuffer();
			uwr.SetRequestHeader("Content-Type", "application/json");

			yield return uwr.SendWebRequest();
			//여기서 줬는데 connection가 뜬다는거는
			//연결이 안되어있다는 뜻이다이가?
			//서버가 그냥 불안정해서 느려서 5초가 걸려
			//blocking?
			//그니까 
			if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
			{
				UI_LoginScene loginSceneUI = Managers.UI.SceneUI as UI_LoginScene;
				UI_Error errorUI = loginSceneUI.ErrorUI;
				Debug.Log(uwr.error);
				errorUI.SetErrorMessage(uwr.error);
			}
			else
			{
				T resObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(uwr.downloadHandler.text);
				res.Invoke(resObj);
			}
		}
	}
}
