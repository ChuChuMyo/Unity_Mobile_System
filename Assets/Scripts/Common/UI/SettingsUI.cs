using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SettingsUI : BaseUI
{
    //게임 버전을 표시해줄 텍스트 컴퍼넌트
    public TextMeshProUGUI GameVersionTxt;
    //사운드가 On 일때 활성화 해줄 UI 오브젝트
    public GameObject SoundOnToggle;
    //사운드가 Off일때 활성화 해줄 UI 오브젝트
    public GameObject SoundOffToggle;
    //보완정책 명시
    //텍스트를 누르면 해당 웹사이트 링크로 이동
    const string PRIVACY_POLICY_URL = "https : // www.naver.com/";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //게임 버전 표시 함수
        SetGameVersion();
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();

        if(userSettingsData != null)
        {
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //게임 버전 표시 함수
    void SetGameVersion()
    {
        GameVersionTxt.text = $"Version : {Application.version}";
    }

    void SetSoundSetting(bool sound)
    {
        SoundOnToggle.SetActive(sound);
        SoundOffToggle.SetActive(!sound);
    }

    //사운드온토글을 눌렀을 때 사운드 설정을 off로 바꿔주는 함수
    public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");
        //UI클릭 시 사운드 플레이
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        //설정 데이터 가져옴
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        //유효성 검사
        if(userSettingsData != null)
        {
            userSettingsData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //off를 눌렀을때 On으로 바꿔주는 함수
    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOffToggle");
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingsData != null)
        {
            userSettingsData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.UnMute();
            SetSoundSetting(userSettingsData.Sound);
        }
    }

    public void OnClickPrivacyPolicyURL()
    {
        Logger.Log($"{GetType()}::OnClickPrivacyPolicyURL");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        Application.OpenURL(PRIVACY_POLICY_URL);
    }
}
