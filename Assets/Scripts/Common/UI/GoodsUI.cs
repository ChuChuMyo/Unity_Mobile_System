using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsUI : MonoBehaviour
{
    //유저가 보유한 보석과 골드 수량을 표시해줄 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;

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
}
