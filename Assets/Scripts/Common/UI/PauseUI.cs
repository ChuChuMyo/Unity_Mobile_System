using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    //일시정지 해제버튼 처리 함수
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();

        CloseUI();
    }
    //홈 버튼 처리 함수
    public void OnClickHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
}
