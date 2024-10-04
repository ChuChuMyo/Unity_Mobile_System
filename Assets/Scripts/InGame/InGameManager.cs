using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }
    public bool IsPaused { get; private set; }
    //Ŭ���� ó���� �ϰ� ���� �� �������� Ŭ���� üũ�� ���� �ʵ��� ����ó�� �ϱ� ���� ������ ����
    public bool IsStageCleared { get; private set; } 

    private int m_SelectedChapter;
    private int m_CurrStage;
    private const string STAGE_PATH = "Stages/"; //�������� �������� �ε��� ���丮 ����� ����
    private Transform m_StageTrs;
    private SpriteRenderer m_Bg;

    private GameObject m_LoadedStage; //���� �������� ������Ʈ �ν��Ͻ��� ������ ���� ���ӿ�����Ʈ ����

    // private Coin[] m_Coins;
    private TextMeshProUGUI currStageTxt;


    protected override void Init()
    {
        m_IsDestroyOnLoad = true; //�ΰ��ӸŴ����� �ΰ��� ���� ����� �����Ǿ�� �ϹǷ� true

        base.Init();

        InitVariables();
        LoadBg(); //2) ������ LoadStage()�� �ѹ��� ó���� �κп��� ����� ȣ���ϴ� �κ� �и�
        LoadStage();
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

    // Start is called before the first frame update
    void Start()
    { 
        //�� ������ InGameUIController ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� ã�Ƽ� ����
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();
    }

    private void InitVariables()
    {
        Logger.Log($"{GetType()}::InitVariables");

        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        m_CurrStage = 1;

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        m_SelectedChapter = userPlayData.SelectedChapter;
    }

    //���������� �ε��ϴ� �Լ�
    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        //���� é�Ϳ� ���������� �α׷�
        Logger.Log($"Chapter:{m_SelectedChapter} Stage:{m_CurrStage}");

        //���� �ε�� �������� ������Ʈ�� �ִٸ� ����
        if(m_LoadedStage)
        {
            Destroy(m_LoadedStage);
        }
       
        //�������� �������� �ε��Ͽ� GameObject �ν��Ͻ��� ������ ��.
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S {m_CurrStage}", typeof(GameObject))) as GameObject;
        //������ �ν��Ͻ��� �������� Ʈ������ ������ ��ġ
        stageObj.transform.SetParent(m_StageTrs);
        //�����ϰ� ������ �ʱ�ȭ
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        m_LoadedStage = stageObj;

        currStageTxt = stageObj.GetComponentInChildren<TextMeshProUGUI>();
        currStageTxt.text = "Stage " + m_CurrStage;
    }

    private void LoadBg() 
    {
        //é�� ��׶��� �̹����� �ε��ؼ� ��׶��� ������ ����
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3 �ڸ��� ���߱� 001 �̷���
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    public void PauseGame()
    {
        IsPaused = true;

        //������ ������ �ΰ����� �Ͻ������� ó���ϴ� �ڵ�
        // GameManager.Instance.Paused = true;
        // LevelManager.Instance.ToggleCharacterPause();

        Time.timeScale = 0f;
        //Ÿ�ӽ������� 0���� �ٲٸ� �׷� ���� ������ �޴� �͵��� ���Ƽ�
        //�Ͻ� ������ �� �� ���� ������������ ������ � ������ �ϵ��� �����ϴµ� ���� ������ ����
        //�׷��ٰ� ���ӽ����Ͽ� ������ ���� �ʵ��� Ÿ�ӽ������� ��ȸ�ؼ�
        //���� �������� �����ϰ� �Ǹ� ���� �ð��� ����� �Ҹ��ϰ� �� �� �����Ƿ� �����ϸ� Ÿ�ӽ������� 1�� �����ϸ鼭
        //�Ͻ������� ó���Ͻô� ���� ����
        //� �����̳Ŀ� ���� �ܼ��� ������ ��ǲ�� ���ų� ���ӳ��� Ÿ�̸Ӹ� �����ص�
        //ó���� ����� �� �� �ֱ� ������ ��ȸ������ �� ����ؼ� �ִ��� �������� �ʰ� �Ͻ����� ó��
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

        IsStageCleared = false;

        m_CurrStage++;

        LoadStage();
    }
}
