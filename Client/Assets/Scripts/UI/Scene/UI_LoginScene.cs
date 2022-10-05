using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class UI_LoginScene : UI_Scene
{
	//숫자,영어를 포함한 5~15사이의문자열
	Regex regexId = new Regex(@"^[0-9a-zA-Z]{5,15}$");
	//영대,소, 숫자, 특수문자 1개이상을 포함한 비밀번호 8~20자
	Regex regexPassword = new Regex(@"^(?=.*[a-z])(?=.*[0-9])(?=.*[\W]).{8,20}$");

	enum GameObjects
	{
		AccountName,
		Password
	}

	enum Images
	{
		CreateBtn,
		LoginBtn
	}

    public override void Init()
	{
        base.Init();

		Bind<GameObject>(typeof(GameObjects));
		Bind<Image>(typeof(Images));

		GetImage((int)Images.CreateBtn).gameObject.BindEvent(OnClickCreateButton);
		GetImage((int)Images.LoginBtn).gameObject.BindEvent(OnClickLoginButton);
	}

	public void OnClickCreateButton(PointerEventData evt)
	{
		string account = Get<GameObject>((int)GameObjects.AccountName).GetComponent<InputField>().text;
        if (regexId.IsMatch(account) == false)
        {
			Debug.Log("잘못된 아이디 형식입니당 숫자,영어를 포함한 5~15사이의문자열로 적으세용");
			return;
        }
		
		string password = Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text;
        if (regexPassword.IsMatch(password) == false)
        {
			Debug.Log("잘못된 비밀번호 형식입니당 영대,소, 숫자, 특수문자 1개이상을 포함한 비밀번호 8~20자");
			return;
        }

		string encryptionPassword = EncryptionSHA256(password);

		CreateAccountPacketReq packet = new CreateAccountPacketReq()
		{
			AccountName = account,
			Password = encryptionPassword
		};

		Managers.Web.SendPostRequest<CreateAccountPacketRes>("account/create", packet, (res) =>
		{
			if(res.CreateOk == false)
            {
				Debug.Log("동일한 아이디가 있습니다");
            }

			Get<GameObject>((int)GameObjects.AccountName).GetComponent<InputField>().text = "";
			Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text = "";
		});
	}

	public void OnClickLoginButton(PointerEventData evt)
	{
		Debug.Log("OnClickLoginButton");

		string account = Get<GameObject>((int)GameObjects.AccountName).GetComponent<InputField>().text;
		if (regexId.IsMatch(account) == false)
		{
			Debug.Log("잘못된 아이디 형식입니당 숫자,영어를 포함한 5~15사이의문자열로 적으세용");
			return;
		}

		string password = Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text;
		if (regexPassword.IsMatch(password) == false)
		{
			Debug.Log("잘못된 비밀번호 형식입니당 영대,소, 숫자, 특수문자 1개이상을 포함한 비밀번호 8~20자");
			return;
		}

		string encryptionPassword = EncryptionSHA256(password);

		LoginAccountPacketReq packet = new LoginAccountPacketReq()
		{
			AccountName = account,
			Password = encryptionPassword
		};

		Managers.Web.SendPostRequest<LoginAccountPacketRes>("account/login", packet, (res) =>
		{
			Debug.Log(res.LoginOk);
			Get<GameObject>((int)GameObjects.AccountName).GetComponent<InputField>().text = "";
			Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text = "";

			if (res.LoginOk)
			{
				Managers.Network.AccountId = res.AccountId;
				Managers.Network.Token = res.Token;
				Managers.Network.UniqueId = encryptionPassword;
				UI_SelectServerPopup popup = Managers.UI.ShowPopupUI<UI_SelectServerPopup>();
				popup.SetServers(res.ServerList);
			}
		});
	}

	private string EncryptionSHA256(string password)
    {
		//입력받은 문자열을 바이트배열로 변환
		byte[] array = Encoding.Default.GetBytes(password);
		byte[] hashValue;
		string result = string.Empty;

		//바이트배열을 암호화된 32byte 해쉬값으로 생성
		using (SHA256 mySHA256 = SHA256.Create())
		{
			hashValue = mySHA256.ComputeHash(array);
		}
		//32byte 해쉬값을 16진수로변환하여 64자리로 만듬
		for (int i = 0; i < hashValue.Length; i++)
		{
			result += hashValue[i].ToString("x2");
		}
		return result;
	}
}
