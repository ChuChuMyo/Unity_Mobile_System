using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {
        
    }

    private void Update()
    {
        //1) 인게임이 일시정지 되어있는 지 확인해서 일시정지 되지 않았을 때만 인풋을 처리해 주도록
        if (!InGameManager.Instance.IsPaused)
        {
            HandleInput();
        }
    }
    //2)
    private void HandleInput()
    {
        //키보드 ESC와 모바일 디바이스 백버튼 인풋을 받아오도록
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            InGameManager.Instance.PauseGame();
        }
    }
    //3)일시정지 버튼을 눌렀을 때 실행할 함수
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) //게임을 이탈했다(앱을 내렸다)면
        {
            if(!InGameManager.Instance.IsPaused)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
            }
        }
    }
}
