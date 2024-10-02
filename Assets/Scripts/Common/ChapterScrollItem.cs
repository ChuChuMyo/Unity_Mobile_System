using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Gpm.Ui;
public class ChapterScrollItemData : InfiniteScrollData
{
    public int ChapterNO;
}

public class ChapterScrollItem : InfiniteScrollItem
{
    public GameObject CurrChapter;
    public RawImage CurrChapterBg;

    public Image Dim;
    public Image LockIcon;
    public Image Round;

    public ParticleSystem ComingSoonFx;
    public TextMeshProUGUI ComingSoonTxt;

    private ChapterScrollItemData m_ChapterScrollItemData;

    //UI처리를 해주기 위해 호출되는 UpdataData 함수 오버라이딩
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //매개변수로 받은 스크롤 데이터를 받아 줌
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("Invalid ChapterScrollItemData.");
            return;
        }
        //만약 표시해야 할 챕터 넘버가 글로벌 정의에 있는 MAX_CHAPTER으ㅟ 값
        //즉, 게임 내 존재하는 최대 챕터보다 크다면
        if(m_ChapterScrollItemData.ChapterNO > GlobalDefine.MAX_CHAPTER)
        {
            //챕터 표시 UI는 비활성화 처리하고
            //ComingSoon활성화
            CurrChapter.SetActive(false);
            ComingSoonFx.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFx.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);
            //유저 플레이 데이터를 가져옴
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if(userPlayData != null)
            {
                //현재 최대로 클리어한 챕터와 비교하여 챕터의 해금 여부를 판단
                var isLocked = m_ChapterScrollItemData.ChapterNO > userPlayData.MaxClearedChapter + 1;
                //그리고 해금 여부에 따라 이미지 컴포넌트들을 처리해 줌
                Dim.gameObject.SetActive(isLocked);
                LockIcon.gameObject.SetActive(isLocked);
                //테두리 이미지는 해금 되었으면 밝게 그렇지 않으면 어둡게 처리
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }

            //해당 챕터 넘버에 맞는 배경 이미지를 로딩하여 세팅해 줌.
            var bgTexture = Resources.Load($"ChapterBG/Background_{m_ChapterScrollItemData.ChapterNO.ToString("D3")}") as Texture2D;
            if(bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }

}
