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
    //������ ������ ������ ��� ������ ǥ������ �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI GoldAmountTxt;
    public Image GoldIcon;
    public TextMeshProUGUI GemAmountTxt;
    public Image GemIcon;

    //��� ���� ������ �ڷ�ƾ�� ���ؼ� ������ �ǵ� �������� �ڷ�ƾ�� ������ �� �ִ� �ڷ�ƾ ���� ����
    //�� ������ �����ϴ� ������ ���� ��ȭ ȹ���� ������ ������ ��û�Ǿ� 
    //�̹� ȹ�� ������ �������ε� ���ο� ȹ�� ���� ��û ó���� ���ԵǸ� ���� ȹ�� ������ ����ϰ�
    //���ο� ȹ�� ����� ����� ����.
    private Coroutine m_GoldIncreaseCo;
    private Coroutine m_GemIncreaseCo;

    //��ȭ �ؽ�Ʈ ���� ������ ���� �ȿ� ����Ǿ�� �ϴ��� �� �ð��� ��� �ִ� ���
    private const float GOODS_INCREASE_DURATION = 0.5f;

    //���� �ΰ��� �޽����� �߻��Ǿ��� �� �� GoodsUI�� �����ڰ� �Ǿ�� �ϴ�
    //�� Ŭ������ Ȱ��ȭ �ɶ� �� Ŭ������ ��ȭ ���� �޽��� �����ڷ� ���
    //�ν��Ͻ��� Ȱ��ȭ �Ǿ� ���� ���� ��ȭ ���� �޽����� �޾� ó��
    private void OnEnable()
    {
        //�޽��� Ÿ���� ���
        //�׸��� �Ű������� �� �޽����� �޾��� �� �ǻ��� �Լ��� �Ѱ��ش�.
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);
    }

    //���� ��ȭ �����͸� �ҷ��� ������ ��� ������ �������ִ� �Լ�
    public void SetValues()
    {
        //�ϴ� ���� �����͸� ��������
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //�����͸� ���������ų� ������ ����
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
        }else
        {
            GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
            GemAmountTxt.text = userGoodData.Gem.ToString("N0");
        }
    }

    //���� gold ��ȭ�� ���� �Ǿ��� �� ������ �Լ� �ۼ�(ȹ�� ����)
    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg) //�Ű������� �޼����� �޴´�.
        //�̷��� �Լ��� ������ �ָ� GoldUI�ν��Ͻ����� GoldUpdate �޽����� �޾��� �� �� �Լ��� �ڵ� ���� ��.
    {
        //���� ���������͸� �����´�
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get); //��ȭ ȹ�� ȿ���� ���
        
        //���� ��ȭ�� �����Ǿ��ٸ� ���� ���� ó��
        if(goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null) //���� ������ �ִٸ� ���
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
        //���� �����͸� �����´�
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
            //ĵ���� ������ ��ġ
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //��ġ�� ������ �ʱ�ȭ
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            //�ν��Ͻ��� ������ GoodsMove ������Ʈ�� �޾ƿ� SetMove �Լ� ȣ��
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

    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg) //�Ű������� �޼����� �޴´�.
                                                           //�̷��� �Լ��� ������ �ָ� GoldUI�ν��Ͻ����� GoldUpdate �޽����� �޾��� �� �� �Լ��� �ڵ� ���� ��.
    {
        //���� ���������͸� �����´�
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }

        AudioManager.Instance.PlaySFX(SFX.ui_get); //��ȭ ȹ�� ȿ���� ���

        //���� ��ȭ�� �����Ǿ��ٸ� ���� ���� ó��
        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCo != null) //���� ������ �ִٸ� ���
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
        //���� �����͸� �����´�
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }

        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
            //ĵ���� ������ ��ġ
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //��ġ�� ������ �ʱ�ȭ
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            //�ν��Ͻ��� ������ GoodsMove ������Ʈ�� �޾ƿ� SetMove �Լ� ȣ��
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
