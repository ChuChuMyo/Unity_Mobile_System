using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using TMPro;
using UnityEngine.UI;
public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList;

    //스크롤뷰에서 현재 선택중인 챕터에 대한 정보를 표시해줄 UI요소들의 최상의 오브젝트 변수 선언
    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt;
    public Button SelectBtn;

    private int SelectedChapter;

    //SetInfo 함수를 오버라이드 해서 필요한 UI 처리
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            return;
        }
        //유저가 현재 선택중인 챕터 정보를 받아 온다.
        SelectedChapter = userPlayData.SelectedChapter;

        SetSelectedChapter(); //현재 선택한 챕터에 대한 UI처리를 해주는 함수 호술
        SetChapterScrollList(); //챕터 목록 스크롤뷰를 세팅하는 함수를 호출

        //스크롤 뷰를 세팅한 다음에 현재 선택중인 챕터로 스크롤 위치를 움직임
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //스크롤이 끝난 후 가장 가까운 아이템으로 자동 이동하는 기능
        //자동 이동이 끝나 후에 처리를 위해 OnSnap에 람다로 원하는 처리
        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if (chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }

    private void SetSelectedChapter()
    {
        //게임 내에 추가된 챕터에 해당하면 선택한 챕터 UI요소들을 활성화 시켜준다.
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //챕터데이터테이블에서 해당 챕터에 대한 데이터를 가져와서 챕터명도 표시
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        else //반대로 아직 게임 내에 추가되지 않은 챕터라면 선택한 챕터 요소의 요소들을 비활성화 해줌
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }

    private void SetChapterScrollList()
    {
        //먼저 스크롤뷰 내부에 이미 존재하는 아이템이 있다면 삭제
        ChapterScrollList.Clear();

        //1번 인덱스 부터 MAX_CHAPTER+1 까지 순회하면서 아이템을 하나씩 추가
        for (int i = 1; i < GlobalDefine.MAX_CHAPTER+2; i++)
        {
            var chpaterItemData = new ChapterScrollItemData();
            chpaterItemData.ChapterNO = i;
            ChapterScrollList.InsertData(chpaterItemData);
        }
    }

    //스크롤 후 자동 이동이 끝난 후 처리를 할 함수
    public void OnSnap(int selectedChapter)
    {
        //현재 선택한 챕터를 매개변수로 받은 챕터로 변경해 주고 SelectedChapter()함수 다시 호출
        SelectedChapter = selectedChapter;
        SetSelectedChapter();
    }

    public void OnClickSelect()
    {
        //UserPlayData를 가져와서
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        if (SelectedChapter <= userPlayData.MaxClearedChapter + 1)
        {
            //플레이 데이터를 변경해주고
            //로비에 있는 현재 선택한 챕터UI를 갱신해 준다.
            userPlayData.SelectedChapter = SelectedChapter;
            LobbyManager.Instance.LobbyUIController.SetCurrChapter();
            CloseUI();
        }
    }
}
