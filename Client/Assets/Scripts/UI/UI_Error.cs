using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Error : UI_Base
{
    private Coroutine _coErrorMessage;
    public Image _image;
    enum Texts
    {
        ErrorText
    }

    public override void Init()
    {
        Bind<Text>(typeof(Texts));
    }

    public void SetErrorMessage(string errorMessage)
    {
        gameObject.SetActive(true);
        Color color = _image.color;
        color.a = 1.0f;
        _image.color = color;
        GetText((int)Texts.ErrorText).text = errorMessage;
        _coErrorMessage = StartCoroutine("CoErrorMessage");
    }

    //���� �޼����� 1�� ���� õõ�� ������� ȿ��
    IEnumerator CoErrorMessage()
    {
        for (int i = 0; i < 100; i++)  
        {
            Color color = _image.color;
            color.a -= 0.01f;
            _image.color = color;
            yield return new WaitForSeconds(0.01f); 
        }
        gameObject.SetActive(false);
    }
}