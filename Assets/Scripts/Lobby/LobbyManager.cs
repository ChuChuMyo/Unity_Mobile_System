using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    //로비 컨트롤러를 프로퍼티로 선언
    public LobbyUIController LobbyUIController { get; set; }
    protected override void Init()
    {
        //로비 매니저는 다른 씬으로 전환 할 때 삭제해 줄 것임.
        m_IsDestroyOnLoad = true;
        base.Init();
    }

    private void Start()
    {
        //FindObjectofType은 씬에 존재하는 타입을 찾아 가장 먼저 찾은인스턴스를 넘겨줌
        //로비유아이컨트롤러는 로비씬에서 하나만 존재할것임
        LobbyUIController = FindObjectOfType<LobbyUIController>();
        //유일하게 존재할 것 이니까 널이라면 애러코드 출력
        if (!LobbyUIController)
        {
            Logger.LogError("LobbyUIController does not exist");
            return;
        }

        LobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby);
    }
}
