using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Transfortation : UI_Base
{
    int _regionId;

    enum GameObjects
    {
        Background
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        for(int i = 0;i < 18; i++)
        {
            GameObject go = Get<GameObject>((int)GameObjects.Background)
                .gameObject.transform.GetChild(i).gameObject;
            go.gameObject.BindEvent((PointerEventData data)
                => { _regionId = int.Parse(go.name); RideKickboard(); });
        }
    }

    private void RideKickboard()
    {
        Debug.Log(_regionId);
                
        //초기화
        Managers.Player.UsingTransfortation = true;
        Managers.UI.CloseAllPopupUI();
        UI_GameScene gameScene = Managers.UI.SceneUI as UI_GameScene;
        gameScene.CloseAll();
        Managers.Player.RegionId = 0;
        
        GameObject go = GameObject.FindGameObjectWithTag("MyPlayer");
        MyPlayerController mpc = go.transform.GetComponent<MyPlayerController>();

        mpc.CellPos = new Vector3Int(250, 0, 0);
        mpc.State = CreatureState.Idle;
        mpc.Dir = MoveDir.Down;
        mpc.transform.position = new Vector3(250, 0, 0);
        mpc.CheckUpdatedFlag();
        Invoke("UseTransfortation", 5.0f);
    }

    void UseTransfortation()
    {//도착 이후
        Managers.Player.UsingTransfortation = false;
        
        GameObject go = GameObject.FindGameObjectWithTag("MyPlayer");
        MyPlayerController mpc = go.transform.GetComponent<MyPlayerController>();

        RegionPos regionPos = Managers.Data.RegionPosDict[_regionId];
        mpc.CellPos = new Vector3Int(regionPos.posX, regionPos.posY, 0);
        mpc.State = CreatureState.Idle;
        mpc.Dir = MoveDir.Down;
        mpc.transform.position = new Vector3(regionPos.posX, regionPos.posY, 0);

        mpc.CheckUpdatedFlag();
    }
}
