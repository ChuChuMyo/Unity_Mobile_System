using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    //�κ� ��Ʈ�ѷ��� ������Ƽ�� ����
    public LobbyUIController LobbyUIController { get; private set; }
    //�ΰ��� �ε� �� ���θ� �� �� �ִ� ���� ����
    //�ΰ��� ���� ��û�� ������ �ϴ� ���� ����
    private bool m_IsLoadingInGame;

    protected override void Init()
    {
        //�κ� �Ŵ����� �ٸ� ������ ��ȯ �� �� ������ �� ����.
        m_IsDestroyOnLoad = true;
        m_IsLoadingInGame = false;
        base.Init();
    }

    private void Start()
    {
        //FindObjectofType�� ���� �����ϴ� Ÿ���� ã�� ���� ���� ã���ν��Ͻ��� �Ѱ���
        //�κ���������Ʈ�ѷ��� �κ������ �ϳ��� �����Ұ���
        LobbyUIController = FindObjectOfType<LobbyUIController>();
        //�����ϰ� ������ �� �̴ϱ� ���̶�� �ַ��ڵ� ���
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
