using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseUIData
{
    //함수를 담을 수 있는 변수라고 생각
    //동일한 UI화면에 대해서도 어떤 상황에서는 A라는 기능을 실행해줘야하고
    //어떤 상황에서는 B라는 기능을 실행해줘야 할 때가 있음.
    //그렇기 때문에 UI화면 클래스 안에서 이런 OnShow나 OnClose를 정의하는 것보다
    //그 화면을 열겠다고 UI매니저를 호출할 때 어떤 행위를 해줘야 될지 정의해서
    //넘겨주는 것이 더 유연하게 원하는 기획 내용을 구현할 수 있다.

    //UI화면을 열었을 때 해주고 싶은 행위를 정의
    public Action OnShow;
    //UI화면을 닫으면서 실행해야 되는 기능 정의
    public Action OnClose;
}

public class BaseUI : MonoBehaviour
{
    //UI 열어 줄때 재생할 애니메이션 변수
    public Animation m_UIOpenAnim;

    public Action m_OnShow;
    public Action m_OnClose;
    
    public virtual void Init(Transform anchor)
    {
        Logger.Log($"{GetType()} init.");

        m_OnShow = null;
        m_OnClose = null;
        //anchor : UI캔버스 컴포넌트의 트랜스폰
        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
        {
            Logger.LogError("UI does not have rectTransform.");
            return;
        }

        //기본 값으로 전부 초기화
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
    }

    public virtual void SetInfo(BaseUIData uiData)
    {
        Logger.Log($"{GetType()} set info ");
        m_OnShow = uiData.OnShow;
        m_OnClose = uiData.OnClose;
    }

    //UI 화면을 실제로 열어서 화면에 표시해 주는 함수
    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }

        m_OnShow?.Invoke();//널검사랑 동일함
        //? 키워드를 사용한 예외 처리
        //_action?.Invoke(3); //if(action != null)를 ?키워드를 사용하여 null임을 체크

        m_OnShow = null;//실행 후에는 널로 초기화
    }

    //UI화면을 닫는 함수
    public virtual void CloseUI(bool isCloseAll = false)
    {
        //isCloseAll : 씬을 전환하거나 할때 열려있는 화면을
        //전부 다 닫아줄 필요가 있을 때
        //true 값을 넘겨줘서 화면을 닫을 때 필요한 처리들을
        //다 무시하고 화면만 닫아주기 위해서 사용할 함수임
        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        //CloseUI에 이 인스턴스를 매개변수로 넣어줌
        //닫을 때 호출 될때 진짜 닫아줌
        UIManager.Instance.CloseUI(this);
    }

    //닫기 버튼을 눌렀을 때 실행하는 함수
    //거의 대부분 UI에는 닫기 버튼이 있으므로
    //여기서 아예 닫기 버튼 기능을 구현
    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
