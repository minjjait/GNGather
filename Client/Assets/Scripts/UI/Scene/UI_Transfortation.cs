using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Transfortation : UI_Base
{
   // Coroutine _coArrived;
    int _regionId;

    float tempBGM;
    float tempEffect;

    enum GameObjects
    {
        Background,
        BGMVolume,
        EffectVolume
    }

    enum Buttons
    {
        BGMMuteButton,
        EffectMuteButton,
        ExitButton
    }

    enum Texts
    {
        BGMVolumeText,
        EffectVolumeText
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        SoundInit();

        for (int i = 0;i < 18; i++)
        {
            GameObject go = Get<GameObject>((int)GameObjects.Background)
                .gameObject.transform.GetChild(i).gameObject;
            go.gameObject.BindEvent((PointerEventData data)
                => { Managers.Sound.Play("ClickSound"); _regionId = int.Parse(go.name); RideKickboard(); });
        }

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

    }

    private void Update()
    {
        UpdateVolume();
    }

    private void RideKickboard()
    {                
        //초기화
        Managers.Player.UsingTransfortation = true;
        Managers.UI.CloseAllPopupUI();
        UI_GameScene gameScene = Managers.UI.SceneUI as UI_GameScene;
        gameScene.CloseAll();
        Managers.Player.RegionId = 0;

        GameObject go = GameObject.FindGameObjectWithTag("MyPlayer");
        MyPlayerController mpc = go.transform.GetComponent<MyPlayerController>();

        if (mpc == null)
            return;

        C_UseTransfortation transfortationPacket = new C_UseTransfortation();
        PositionInfo posInfo = new PositionInfo();
        posInfo.State = CreatureState.Idle;
        posInfo.MoveDir = MoveDir.Down;
        posInfo.PosX = 250;
        posInfo.PosY = 0;
        transfortationPacket.PosInfo = posInfo;

        Managers.Object.MyPlayer.transform.position = new Vector3(250, 0, 0);

        Managers.Network.Send(transfortationPacket);
    }

    public void Arrived()
    {
        Invoke("DoArrived", 5.0f);
        //_coArrived = StartCoroutine("CoArrived");
        gameObject.SetActive(false);
    }

    void DoArrived()
    {

        //도착 이후
        RegionPos regionPos = Managers.Data.RegionPosDict[_regionId];

        GameObject go = GameObject.FindGameObjectWithTag("MyPlayer");
        MyPlayerController mpc = go.transform.GetComponent<MyPlayerController>();

        C_TransfortationArrived transfortationPacket = new C_TransfortationArrived();
        PositionInfo posInfo = new PositionInfo();
        posInfo.State = CreatureState.Idle;
        posInfo.MoveDir = MoveDir.Down;
        posInfo.PosX = regionPos.posX;
        posInfo.PosY = regionPos.posY;
        transfortationPacket.PosInfo = posInfo;

        Managers.Object.MyPlayer.transform.position = new Vector3(regionPos.posX+0.5f, regionPos.posY+0.5f, 0);

        Managers.Network.Send(transfortationPacket);
    }

    void OnClickExitButton(PointerEventData evt)
    {
        Managers.Sound.Play("ClickSound");
        Application.Quit();
    }

    #region Sound

    void SoundInit()
    {
        GetObject((int)GameObjects.BGMVolume).gameObject.GetComponent<Slider>().value = Managers.Sound.BGMVolume;
        GetObject((int)GameObjects.EffectVolume).gameObject.GetComponent<Slider>().value = Managers.Sound.EffectVolume;
        tempBGM = Managers.Sound.BGMVolume;
        tempEffect = Managers.Sound.EffectVolume;

        GetButton((int)Buttons.BGMMuteButton).gameObject.BindEvent(PointerEventData =>
        {
            GetObject((int)GameObjects.BGMVolume).gameObject.GetComponent<Slider>().value = 0;
            GetButton((int)Buttons.BGMMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_mute"); 
        });

        GetButton((int)Buttons.EffectMuteButton).gameObject.BindEvent(PointerEventData =>
        {
            GetObject((int)GameObjects.EffectVolume).gameObject.GetComponent<Slider>().value = 0;
            GetButton((int)Buttons.EffectMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_mute");
        });
    }

    void UpdateVolume()
    {
        float bgm = GetObject((int)GameObjects.BGMVolume).gameObject.GetComponent<Slider>().value;
        float effect = GetObject((int)GameObjects.EffectVolume).gameObject.GetComponent<Slider>().value;
        Managers.Sound.ChangeVolume(bgm, effect);

        GetText((int)Texts.BGMVolumeText).text = (Mathf.Ceil(bgm * 100)).ToString();
        GetText((int)Texts.EffectVolumeText).text = (Mathf.Ceil(effect * 100)).ToString();

        if (Managers.Sound.BGMVolume <= 0)
        {
            GetButton((int)Buttons.BGMMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_mute");
        }
        else if (Managers.Sound.BGMVolume <= 0.5f)
        {
            GetButton((int)Buttons.BGMMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_low");
        }
        else
        {
            GetButton((int)Buttons.BGMMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_high");
        }

        if (Managers.Sound.EffectVolume <= 0)
        {
            GetButton((int)Buttons.EffectMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_mute");
        }
        else if (Managers.Sound.EffectVolume <= 0.5f)
        {
            GetButton((int)Buttons.EffectMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_low");
        }
        else
        {
            GetButton((int)Buttons.EffectMuteButton).gameObject.GetComponent<Image>().sprite
                = Managers.Resource.Load<Sprite>("Textures/UI/sound_high");
        }
    }
    #endregion
}