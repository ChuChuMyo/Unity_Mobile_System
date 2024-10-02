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

    //UIó���� ���ֱ� ���� ȣ��Ǵ� UpdataData �Լ� �������̵�
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //�Ű������� ���� ��ũ�� �����͸� �޾� ��
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("Invalid ChapterScrollItemData.");
            return;
        }
        //���� ǥ���ؾ� �� é�� �ѹ��� �۷ι� ���ǿ� �ִ� MAX_CHAPTER���� ��
        //��, ���� �� �����ϴ� �ִ� é�ͺ��� ũ�ٸ�
        if(m_ChapterScrollItemData.ChapterNO > GlobalDefine.MAX_CHAPTER)
        {
            //é�� ǥ�� UI�� ��Ȱ��ȭ ó���ϰ�
            //ComingSoonȰ��ȭ
            CurrChapter.SetActive(false);
            ComingSoonFx.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFx.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);
            //���� �÷��� �����͸� ������
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if(userPlayData != null)
            {
                //���� �ִ�� Ŭ������ é�Ϳ� ���Ͽ� é���� �ر� ���θ� �Ǵ�
                var isLocked = m_ChapterScrollItemData.ChapterNO > userPlayData.MaxClearedChapter + 1;
                //�׸��� �ر� ���ο� ���� �̹��� ������Ʈ���� ó���� ��
                Dim.gameObject.SetActive(isLocked);
                LockIcon.gameObject.SetActive(isLocked);
                //�׵θ� �̹����� �ر� �Ǿ����� ��� �׷��� ������ ��Ӱ� ó��
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }

            //�ش� é�� �ѹ��� �´� ��� �̹����� �ε��Ͽ� ������ ��.
            var bgTexture = Resources.Load($"ChapterBG/Background_{m_ChapterScrollItemData.ChapterNO.ToString("D3")}") as Texture2D;
            if(bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }

}
