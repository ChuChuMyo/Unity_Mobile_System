using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    //로비 컨트롤러를 프로퍼티로 선언
    public LobbyUIController LobbyUIController { get; private set; }
    //인게임 로딩 중 여부를 알 수 있는 변수 선언
    //인게임 진입 요청을 여러번 하는 것을 방지
    private bool m_IsLoadingInGame;

    protected override void Init()
    {
        //로비 매니저는 다른 씬으로 전환 할 때 삭제해 줄 것임.
        m_IsDestroyOnLoad = true;
        m_IsLoadingInGame = false;
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

    public void StartInGame()
    {
        if(m_IsLoadingInGame)
        {
            return;
        }

        m_IsLoadingInGame = true;

        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
              {
                  UIManager.Instance.CloseAllOpenUI();
                  SceneLoader.Instance.LoadScene(SceneType.InGame);
              });
    }
}
