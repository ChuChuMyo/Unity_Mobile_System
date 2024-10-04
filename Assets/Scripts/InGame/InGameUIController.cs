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
        //1) �ΰ����� �Ͻ����� �Ǿ��ִ� �� Ȯ���ؼ� �Ͻ����� ���� �ʾ��� ���� ��ǲ�� ó���� �ֵ���
        if (!InGameManager.Instance.IsPaused)
        {
            HandleInput();
        }
    }
    //2)
    private void HandleInput()
    {
        //Ű���� ESC�� ����� ����̽� ���ư ��ǲ�� �޾ƿ�����
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData);

            InGameManager.Instance.PauseGame();
        }
    }
    //3)�Ͻ����� ��ư�� ������ �� ������ �Լ�
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) //������ ��Ż�ߴ�(���� ���ȴ�)��
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
