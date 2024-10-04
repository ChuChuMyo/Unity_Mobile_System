using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    //�Ͻ����� ������ư ó�� �Լ�
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();

        CloseUI();
    }
    //Ȩ ��ư ó�� �Լ�
    public void OnClickHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
}
