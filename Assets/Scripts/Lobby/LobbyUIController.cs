using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void Init()
    {
        //로비에서는 켜줘야 하기때문에 UIManager에서 작성해준 재화 활성화 비활성화 함수 호출
        UIManager.Instance.EnableGoodsUI(true);
    }

    //로비UI프리팹을 생성시켜주고, 뭐 특정 행동 했을 때 
    //로비씬에서 설정 버튼을 누르면 SettingUI(프리팹)가 열리는 함수
    //설정 버튼을 눌렀을때 처리해줄 것들
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        //로그
        Logger.Log($"{GetType()}::OnClickSettingBtn");
        //데이터 인스턴스를 베이스유아이데이터클래스로 만들어줌
        var uiData = new BaseUIData();
        //UI매니저를 통해 인벤토리를 열게함
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
    private void Update()
    {
        MandleInput();
    }

    private void MandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            if (frontUI != null)
            {
                frontUI.CloseUI();
            }
            else
            {
                var uiData = new ConfirmUIData();
                uiData.ConfirmType = ConfirmType.OK_CANCEL;
                uiData.TitleTxt = "Quit";
                uiData.DescTxt = "Do you want to quit game?";
                uiData.OkBtnTxt = "Quit";
                uiData.CancelBtnTxt = "Cancel";
                uiData.OnClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
    }
}