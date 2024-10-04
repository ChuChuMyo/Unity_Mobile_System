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
        //�κ񿡼��� ����� �ϱ⶧���� UIManager���� �ۼ����� ��ȭ Ȱ��ȭ ��Ȱ��ȭ �Լ� ȣ��
        UIManager.Instance.EnableGoodsUI(true);
        SetCurrChapter();
    }

    public void SetCurrChapter()
    {
        //���� �÷��� �����͸� ������
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        //�÷��� �����͸� �����Դٸ�
        //���� �������� é�� ��ȣ�� ������ �ش� é�� �����͸� ������
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);

        if(currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exist.");
            return;
        }
        //�ش� �����Ͱ� ���������� �����Ѵ�
        //é�͸��� ǥ�����ְ� é�� �̹����� �ε��ؼ� ��������.
        CurrChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBg/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D;

        if(bgTexture != null)
        {
            CurrChapterBg.texture = bgTexture;
        }
    }

    //�κ�UI�������� ���������ְ�, �� Ư�� �ൿ ���� �� 
    //�κ������ ���� ��ư�� ������ SettingUI(������)�� ������ �Լ�
    //���� ��ư�� �������� ó������ �͵�
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        //�α�
        Logger.Log($"{GetType()}::OnClickSettingBtn");
        //������ �ν��Ͻ��� ���̽������̵�����Ŭ������ �������
        var uiData = new BaseUIData();
        //UI�Ŵ����� ���� �κ��丮�� ������
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