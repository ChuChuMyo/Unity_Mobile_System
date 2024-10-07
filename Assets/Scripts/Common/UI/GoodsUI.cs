using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using SuperMaxim.Messaging;

public class GoldUpdateMsg
{
    public bool isAdd;
}

public class GemUpdateMsg
{
    public bool isAdd;
}


public class GoodsUI : MonoBehaviour
{
    //유저가 보유한 보석과 골드 수량을 표시해줄 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    public Image GoldIcon;
    public TextMeshProUGUI GemAmountTxt;
    public Image GemIcon;

    //골드 증가 연출을 코루틴을 통해서 실행할 건데 실행중인 코루틴을 참조할 수 있는 코루틴 변수 선언
    //이 변수를 선언하는 이유는 만약 재화 획득이 빠르게 여러변 요청되어 
    //이미 획득 연출이 진행중인데 새로운 획득 연출 요청 처리가 오게되면 기존 획득 연출을 취소하고
    //새로운 획득 연출로 덮어쓰기 위함.
    private Coroutine m_GoldIncreaseCo;
    private Coroutine m_GemIncreaseCo;

    //재화 텍스트 증가 연출이 몇초 안에 실행되어야 하는지 그 시간을 담고 있는 상수
    private const float GOODS_INCREASE_DURATION = 0.5f;

    //위에 두가지 메시지가 발생되었을 때 이 GoodsUI가 구독자가 되어야 하니
    //이 클래스가 활성화 될때 이 클래스를 재화 변동 메시지 구독자로 등록
    //인스턴스가 활성화 되어 있을 때만 재화 변동 메시지를 받아 처리
    private void OnEnable()
    {
        //메시지 타입을 명시
        //그리고 매개변수에 이 메시지를 받았을 때 실생할 함수를 넘겨준다.
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);
    }

    //유저 재화 데이터를 불러와 보석과 골드 수량을 세팅해주는 함수
    public void SetValues()
    {
        //일단 유저 데이터를 가져오고
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //데이터를 못가져오거나 없으면 오류
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
        }else
        {
            GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodData.Gem.ToString("N0");
        }
    }

    //먼저 gold 재화가 변경 되었을 시 실행할 함수 작성(획득 연출)
    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg) //매개변수로 메세지를 받는다.
        //이렇게 함수를 선언해 주면 GoldUI인스턴스에서 GoldUpdate 메시지를 받았을 때 이 함수가 자동 실행 됨.
    {
        //먼저 유저데이터를 가져온다
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get); //재화 획득 효과음 재생
        
        //만약 재화가 증가되었다면 증가 연출 처리
        if(goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null) //기존 연출이 있다면 취소
            {
                StopCoroutine(m_GoldIncreaseCo);
            }
            m_GoldIncreaseCo = StartCoroutine(IncreaseGoldCo());
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }

    }

    private IEnumerator IncreaseGoldCo()
    {
        //유저 데이터를 가져온다
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            //캔버스 하위에 위치
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //위치와 스케일 초기화
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            //인스턴스에 연동된 GoodsMove 컴포넌트를 받아와 SetMove 함수 호출
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gold;

        while(elapsedTime < GOODS_INCREASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCREASE_DURATION);
            GoldAmountTxt.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GoldAmountTxt.text = destValue.ToString("N0");
    }

    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg) //매개변수로 메세지를 받는다.
                                                           //이렇게 함수를 선언해 주면 GoldUI인스턴스에서 GoldUpdate 메시지를 받았을 때 이 함수가 자동 실행 됨.
    {
        //먼저 유저데이터를 가져온다
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get); //재화 획득 효과음 재생

        //만약 재화가 증가되었다면 증가 연출 처리
        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCo != null) //기존 연출이 있다면 취소
            {
                StopCoroutine(m_GemIncreaseCo);
            }
            m_GemIncreaseCo = StartCoroutine(IncreaseGemCo());
            
        }
        else
        {
            GemAmountTxt.text = userGoodsData.Gem.ToString("N0");
        }
    }

    private IEnumerator IncreaseGemCo()
    {
        //유저 데이터를 가져온다
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            //캔버스 하위에 위치
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //위치와 스케일 초기화
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            //인스턴스에 연동된 GoodsMove 컴포넌트를 받아와 SetMove 함수 호출
            gemObj.GetComponent<GoodsMove>().SetMove(i, GemIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFX.ui_increase);

        var elapsedTime = 0f;
        var currTextValue = Convert.ToInt64(GemAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gem;

        while (elapsedTime < GOODS_INCREASE_DURATION)
        {
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCREASE_DURATION);
            GemAmountTxt.text = currValue.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GemAmountTxt.text = destValue.ToString("N0");
    }
}
