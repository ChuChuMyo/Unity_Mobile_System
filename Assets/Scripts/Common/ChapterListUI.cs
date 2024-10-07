using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using TMPro;
using UnityEngine.UI;
public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList;

    //��ũ�Ѻ信�� ���� �������� é�Ϳ� ���� ������ ǥ������ UI��ҵ��� �ֻ��� ������Ʈ ���� ����
    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt;
    public Button SelectBtn;

    private int SelectedChapter;

    //SetInfo �Լ��� �������̵� �ؼ� �ʿ��� UI ó��
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            return;
        }
        //������ ���� �������� é�� ������ �޾� �´�.
        SelectedChapter = userPlayData.SelectedChapter;

        SetSelectedChapter(); //���� ������ é�Ϳ� ���� UIó���� ���ִ� �Լ� ȣ��
        SetChapterScrollList(); //é�� ��� ��ũ�Ѻ並 �����ϴ� �Լ��� ȣ��

        //��ũ�� �並 ������ ������ ���� �������� é�ͷ� ��ũ�� ��ġ�� ������
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //��ũ���� ���� �� ���� ����� ���������� �ڵ� �̵��ϴ� ���
        //�ڵ� �̵��� ���� �Ŀ� ó���� ���� OnSnap�� ���ٷ� ���ϴ� ó��
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
        //���� ���� �߰��� é�Ϳ� �ش��ϸ� ������ é�� UI��ҵ��� Ȱ��ȭ �����ش�.
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //é�͵��������̺��� �ش� é�Ϳ� ���� �����͸� �����ͼ� é�͸� ǥ��
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        else //�ݴ�� ���� ���� ���� �߰����� ���� é�Ͷ�� ������ é�� ����� ��ҵ��� ��Ȱ��ȭ ����
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }

    private void SetChapterScrollList()
    {
        //���� ��ũ�Ѻ� ���ο� �̹� �����ϴ� �������� �ִٸ� ����
        ChapterScrollList.Clear();

        //1�� �ε��� ���� MAX_CHAPTER+1 ���� ��ȸ�ϸ鼭 �������� �ϳ��� �߰�
        for (int i = 1; i < GlobalDefine.MAX_CHAPTER+2; i++)
        {
            var chpaterItemData = new ChapterScrollItemData();
            chpaterItemData.ChapterNO = i;
            ChapterScrollList.InsertData(chpaterItemData);
        }
    }

    //��ũ�� �� �ڵ� �̵��� ���� �� ó���� �� �Լ�
    public void OnSnap(int selectedChapter)
    {
        //���� ������ é�͸� �Ű������� ���� é�ͷ� ������ �ְ� SelectedChapter()�Լ� �ٽ� ȣ��
        SelectedChapter = selectedChapter;
        SetSelectedChapter();
    }

    public void OnClickSelect()
    {
        //UserPlayData�� �����ͼ�
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        if (SelectedChapter <= userPlayData.MaxClearedChapter + 1)
        {
            //�÷��� �����͸� �������ְ�
            //�κ� �ִ� ���� ������ é��UI�� ������ �ش�.
            userPlayData.SelectedChapter = SelectedChapter;
            LobbyManager.Instance.LobbyUIController.SetCurrChapter();
            CloseUI();
        }
    }
}
