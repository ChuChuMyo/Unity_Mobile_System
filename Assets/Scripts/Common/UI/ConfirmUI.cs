using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum ConfirmType
{
    //단순히 알림성 팝업으로 특정 내용과 함께 확인 버튼만 보여지며
    //이버튼을 눌러도 닫히게 하는 이넘 타입
    OK,
    //유저가 어떤 행위를 하려는 것이 맞는지 재차 물어보며
    //그렇다면 확인 버튼을 눌러 그 행위를 실행하고
    //아니라면 취소 버튼을 눌러 취소하는 팝업
    OK_CANCEL,
}

public class ConfirmUIData : BaseUIData
{
    //팝업 유형을 구분하는 변수
    public ConfirmType ConfirmType;
    //화면 제목에 들어갈 텍스트
    public string TitleTxt;
    //설명을 표시할 텍스트
    public string DescTxt;
    //확인 버튼에 보여질 텍스트
    public string OkBtnTxt;
    //확인 버튼을 누를 때 행동
    public Action OnClickOKBtn;
    //취소 버튼에 보여질 텍스트
    public string CancelBtnTxt;
    //취소 버튼을 누를 때 행동
    public Action OnClickCancelBtn;
}

public class ConfirmUI : BaseUI
{
    //화면 제목 텍스트 선언
    public TextMeshProUGUI TitleTxt = null;
    //설명 텍스트 선언
    public TextMeshProUGUI DescTxt = null;
    //확인 버튼 선언
    public Button OKBtn = null;
    //취소 버튼 선언
    public Button CancelBtn = null;
    //확인 버튼 텍스트 선언
    public TextMeshProUGUI OKBtnTxt = null;
    //취소 버튼 텍스트 선언
    public TextMeshProUGUI CancelBtnTxt = null;

    //화면을 열때 매개변수로 받은 UIData를 저장할 변수 선언
    ConfirmUIData m_ConfirmUIData = null;
    //확인 버튼을 눌렀을 시 액션을 선언
    Action m_OnClickOKBtn = null;
    //취소 버튼을 눌렀을 시 액션을 선언
    Action m_OnClickCancelBtn = null;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //매개변수로 받은 UI데이터를 저장
        m_ConfirmUIData = uiData as ConfirmUIData;

        TitleTxt.text = m_ConfirmUIData.TitleTxt;
        DescTxt.text = m_ConfirmUIData.DescTxt;
        OKBtnTxt.text = m_ConfirmUIData.OkBtnTxt;
        CancelBtnTxt.text = m_ConfirmUIData.CancelBtnTxt;
        m_OnClickOKBtn = m_ConfirmUIData.OnClickOKBtn;
        m_OnClickCancelBtn = m_ConfirmUIData.OnClickCancelBtn;
        //ok버튼과 cancel버튼을 활성화
        //ConfirmType이 ok면 ok버튼만, cancel이면 ok, cancel 버튼 둘다 활성화
        OKBtn.gameObject.SetActive(true);
        CancelBtn.gameObject.SetActive(m_ConfirmUIData.ConfirmType == ConfirmType.OK_CANCEL);
    }

    //확인 버튼 클릭 시 처리를 위한 함수
    public void OnClickOKBtn()
    {
        //? 키워드 : 널이 아니면 액션을 실행 시켜주는 키워드
        m_OnClickOKBtn?.Invoke();
        CloseUI();
    }

    //취소 버튼 클릭시 처리를 위한 함수
    public void OnClickCancelBtn()
    {
        m_OnClickCancelBtn?.Invoke();
        CloseUI();
    }
}
