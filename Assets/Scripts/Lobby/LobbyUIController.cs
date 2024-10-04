using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI CurrChapterNameTxt;
    public RawImage CurrChapterBg;

    public void Init()
    {
        //로비에서는 켜줘야 하기때문에 UIManager에서 작성해준 재화 활성화 비활성화 함수 호출
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrChapter();
    }

    public void SetCurrChapter()
    {
        //유저 플레이 데이터를 가져옴
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        //플레이 데이터를 가져왔다면
        //현재 선택중인 챕터 번호를 가지고 해당 챕터 데이터를 가져옴
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);

        if(currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exist.");
            return;
        }
        //해당 데이터가 정상적으로 존재한다
        //챕터명을 표시해주고 챕터 이미지도 로드해서 세팅해줌.
        CurrChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBg/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;

        if(bgTexture != null)
        {
            CurrChapterBg.texture = bgTexture;
        }
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

    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OnClickCurrChapter");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }

    public void OnClickStartBtn()
    {
        Logger.Log($"{GetType()}::OnClickStartBtn");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        AudioManager.Instance.StopBGM();
        LobbyManager.Instance.StartInGame();
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