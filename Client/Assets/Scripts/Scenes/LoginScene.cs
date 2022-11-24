using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    UI_LoginScene _sceneUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        Managers.Web.BaseUrl = "http://203.255.3.97:5001/api";

        _sceneUI = Managers.UI.ShowSceneUI<UI_LoginScene>();

        Managers.Sound.Play("Login", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}
