using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _sceneUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.Map.LoadMap(1);

        _sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        Managers.Sound.Play("Game", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        
    }
}
