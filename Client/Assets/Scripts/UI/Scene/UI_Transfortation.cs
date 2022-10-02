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

        GameObject go = GameObject.FindGameObjectWithTag("MyPlayer");
        MyPlayerController mpc = go.transform.GetComponent<MyPlayerController>();

        C_Move movePacket = new C_Move();
        movePacket.PosInfo = mpc.PosInfo;
        movePacket.RegionId = 9999;
        Managers.Network.Send(movePacket);

    }
}
