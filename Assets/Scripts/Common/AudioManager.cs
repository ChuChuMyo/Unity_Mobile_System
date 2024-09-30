using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BGM
{
    lobby,
    COUNT,
}
public enum SFX
{
    chapter_clear,
    stage_clear,
    ui_button_click,
    COUNT,
}

public class AudioManager : SingletonBehaviour<AudioManager>
{
    //만들어 줬던 오브젝트에 동적으로 접근할 변수
    public Transform BGMTrs;
    public Transform SFXTrs;
    //오디오 파일을 로드할 경로
    const string AUDIO_PATH = "Audio";
    //모든 BGM 오디오 리소스를 저장할 컨테이너
    Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    //BGM관련해서 현재 재생하고 있는 오디오 소스 컴포넌트
    AudioSource m_CurrBGMSource;
    //모든 SFX 오디오 리소스를 저장할 컨테이너
    Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    //존재하는 모든 BGM파일 목록을 순회 하면서 전용 게임오브젝트를 만들고
    //그 오브젝트에 오디오 소스 컴포넌트를 붙여주고 해당 음원을 연동
    void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = BGMTrs;

            m_BGMPlayer[(BGM)i] = newAudioSource;
        }
    }
    //bgm과 같은 원리로 작성
    void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }

            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = SFXTrs;

            m_SFXPlayer[(SFX)i] = newAudioSource;
        }
    }

    //BGM플레이 함수
    public void PlayBGM(BGM bgm)
    {
        //만약 재생되고 있는 BGM소스가 있다면
        //재생을 멈추고 null값으로 초기화
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }
        //재생하고 싶은 BGM이 존재하는지 확인
        //존재하지 않다면 에러를 발생
        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Logger.LogError($"Invalid clip name. {bgm}");
            return;
        }
        //존재한다면 해당 오디오소스 컴포넌트를 참조시켜주고
        //재생
        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    //bgm 일시정지
    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }

    //bgm 재실행
    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }

    //bgm 아예 정지
    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }

    //sfx 플레이
    public void PlaySFX(SFX sfx)
    {
        //재생하고 싶은 BGM이 존재하는지 확인
        //존재하지 않다면 에러를 발생
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Logger.LogError($"Invalid clip name. {sfx}");
            return;
        }
        //재생
        m_SFXPlayer[sfx].Play();
    }

    public void OnLoadUserData()
    {
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingsData>();
        if(userSettingsData != null)
        {
            if (!userSettingsData.Sound)
            {
                Mute();
            }
        }
    }

    //효과음은 짧은 시간에 재생되고 끝나므로 따로 일시정지 등은 필요없고 뮤트만 시켜줌
    //Mute
    public void Mute()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }

    //UnMute
    public void UnMute()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }

        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
}
