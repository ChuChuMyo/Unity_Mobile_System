using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }
    public bool IsPaused { get; private set; }
    //클리어 처리를 하고 있을 때 스테이지 클리어 체크를 하지 않도록 예외처리 하기 위해 참조할 변수
    public bool IsStageCleared { get; private set; } 

    private int m_SelectedChapter;
    private ChapterData m_CurrChapterData; //현재 플레이 중인 챕터의 챕터 데이터를 담을 변수
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages/"; //스테이지 프리팹을 로드할 디렉토리 상수를 선언
    private Transform m_StageTrs;
    private SpriteRenderer m_Bg;

    private GameObject m_LoadedStage; //현재 스테이지 오브젝트 인스턴스를 가지고 있을 게임오브젝트 변수

    // private Coin[] m_Coins;
    private TextMeshProUGUI currStageTxt;


    protected override void Init()
    {
        m_IsDestroyOnLoad = true; //인게임매니저는 인게임 씬을 벗어나면 삭제되어야 하므로 true

        base.Init();

        InitVariables();
        LoadBg(); //기존에 LoadStage()에 한번에 처리한 부분에서 배경을 호출하는 부분 분리
        LoadStage();
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

    // Start is called before the first frame update
    void Start()
    { 
        //씬 내에서 InGameUIController 스크립트를 가지고 있는 오브젝트를 찾아서 대입
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
            userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
        }
    }

    private void InitVariables()
    {
        Logger.Log($"{GetType()}::InitVariables");

        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        m_CurrStage = 20;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        m_SelectedChapter = userPlayData.SelectedChapter;

        //현재 선택한 챕터의 챕터 데이터를 가져 m_CurrChapterData 변수에 대입
        m_CurrChapterData = DataTableManager.Instance.GetChapterData(m_SelectedChapter);
        if(m_CurrChapterData == null)
        {
            Logger.LogError($"ChapterData does not exist. Chapter:{m_SelectedChapter}");
            return;
        }
    }

    //스테이지를 로드하는 함수
    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        //현재 챕터와 스테이지를 로그로
        Logger.Log($"Chapter:{m_SelectedChapter} Stage:{m_CurrStage}");

        //현재 로드된 스테이지 오브젝트가 있다면 삭제
        if(m_LoadedStage)
        {
            Destroy(m_LoadedStage);
        }
       
        //스테이지 프리팹을 로드하여 GameObject 인스턴스를 생성해 줌.
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S {m_CurrStage}", typeof(GameObject))) as GameObject;
        //생성한 인스턴스를 스테이지 트랜스폼 하위로 위치
        stageObj.transform.SetParent(m_StageTrs);
        //스케일과 포지션 초기화
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        m_LoadedStage = stageObj;

        currStageTxt = stageObj.GetComponentInChildren<TextMeshProUGUI>();
        currStageTxt.text = "Stage " + m_CurrStage;
    }

    private void LoadBg() 
    {
        //챕터 백그라운드 이미지를 로드해서 백그라운드 변수에 셋팅
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3 자릿수 맞추기 001 이런식
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    public void PauseGame()
    {
        IsPaused = true;

        //앞으로 구현할 인게임의 일시정지를 처리하는 코드
        // GameManager.Instance.Paused = true;
        // LevelManager.Instance.ToggleCharacterPause();

        Time.timeScale = 0f;
        //타임스케일을 0으로 바꾸면 그로 인해 영향을 받는 것들이 많아서
        //일시 정지를 한 후 게임 컨텐츠적으로 유저가 어떤 행위를 하도록 구현하는데 많은 제약이 생김
        //그렇다고 다임스케일에 영향을 받지 않도록 타임스케일을 우회해서
        //게임 컨텐츠를 개발하게 되면 많은 시간과 비용을 소모하게 될 수 있으므로 웬만하면 타임스케일을 1로 유지하면서
        //일시정지를 처리하시는 것을 권장
        //어떤 게임이냐에 따라서 단순히 유저의 인풋만 막거나 게임내의 타이머만 제어해도
        //처리가 충분히 될 수 있기 때문에 기회적으로 잘 고민해서 최대한 복잡하지 않게 일시정지 처리
    }

    public void ResumeGame()
    {
        IsPaused = false;

        // GameManager.Instance.Paused = false;
        // LevelManager.Instance.ToggleCharacterResume();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        CheckStageClear();
    }

    private void CheckStageClear()
    {
        if(IsStageCleared)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();
        }
    }

    private void ClearStage()
    {
        Logger.Log($"{GetType()}::ClearStage");

        IsStageCleared = true;

        StartCoroutine(ShowStageClearCo());
    }

    private IEnumerator ShowStageClearCo()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData);

        yield return new WaitForSeconds(1f);

        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if(stageClearUI)
        {
            stageClearUI.CloseUI();
        }

        if(IsAllClear())
        {
            ClearChapter();
        }
        else
        {
            IsStageCleared = false;
            m_CurrStage++;
            LoadStage();
        }
    }

    private bool IsAllClear()
    {
        return m_CurrStage == m_CurrChapterData.TotalStage;
    }

    private void ClearChapter()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does noe exist.");
            return;
        }

        //먼저 챕터 클리어 화면을 보여줄 것

        var uiData = new ChapterClearUIData();
        uiData.chapter = m_SelectedChapter; //챕터 값은 현재 챕터로 설정
        //보상 지급 여부는 현재챕터가 유저 플레이데이터의 MaxClearedChapter 값보다 큰지를 비교
        uiData.earnReward = m_SelectedChapter > userPlayData.MaxClearedChapter;
        UIManager.Instance.OpenUI<ChapterClearUI>(uiData);

        //현재 챕터가 아직 클리어하지 못한 챕터라면
        if(m_SelectedChapter > userPlayData.MaxClearedChapter)
        {
            userPlayData.MaxClearedChapter++;
            userPlayData.SelectedChapter = userPlayData.MaxClearedChapter + 1;
            //선택한 챕터도 방금 해금한 다음 챕터로 설정해 줌
            //이는 로비로 나갔을 때 해금한 챕터가 선택한 챕터로 선택되도록 하기 위함

            userPlayData.SaveData();
        }

        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData != null)
        {
            switch(m_SelectedChapter)
            {
                case 1:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter1, 1);
                    break;
                case 2:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter2, 1);
                    break;
                case 3:
                    userAchievementData.ProgressAchievement(AchievementType.ClearChapter3, 1);
                    break;
                default:
                    break;
            }
        }
    }
}
