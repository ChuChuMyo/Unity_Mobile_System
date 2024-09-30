using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using Gpm.Ui;//Gpm.UI에있는 클래스를 상속해서 쓰기위한 using


//인벤토리아이템슬롯 인스턴스 생성을 위해 필요한 데이터 클래스이다.
//인피니티스크롤데이터를 상속받을려면 using Gpm.UI 가 필요하고
//쓰는 이유는 인피니티 스크롤컴포넌트를 사용하여 스크롤 아이템을 생성하기 위해서 이다.
public class InventoryItemSlotData : InfiniteScrollData
{
    //필요한 데이터는 유저 아이템과 동일하게 시리얼넘버랑 아이디 이다.
    public long SerialNumber;
    public int ItemId;
}

public class InventoryItemSlot : InfiniteScrollItem
{
    //등급에 따라 이미지를 처리해줄 백그라운드이미지
    public Image ItemGradeBg;
    //아이템이 무엇인지에 따라 아이템 이미지를 처리해줄 아이콘이미지
    public Image ItemIcon;
    //생성해준 클래스를 변수로 선언해서 사용(당연한거)
    InventoryItemSlotData m_InventoryIteSlotData;
    //현재이 Gpm.UI에있는 인피니티스크롤에서 뭐가 필요한지 생각하고
    //지금 여기선 인벤토리 아이템에 있는 UpdataData를 오버라이드 해준다
    //슬롯 UI에 대한 처리를 하게 함.
    //인피니티스크롤데이터를 매개변수로 받아와서 UI를 세팅해줄 함수
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //스크롤 데이터를 인벤토리 아이템 슬롯 데이터로 변환해서 받는다.
        m_InventoryIteSlotData = scrollData as InventoryItemSlotData;
        //null 이면 로그에러
        if(m_InventoryIteSlotData == null)
        {
            Logger.LogError("m_InventoryIteSlotData is invalid.");
            return;
        }
        //아이템 등급에 따른 백그라운드이미 처리
        //(이수식이면) 아이템 ID에서 등급 숫자만 추출 가능, 이것을 (ItemGrade)이넘 값으로 변환해서 받아온다.
        var itemGrade = (ItemGrade)((m_InventoryIteSlotData.ItemId / 1000) % 10);//11001
        //이렇게 받아온 이넘값을 그대로 이미지 명으로 사용
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //null체크 이상이 없으면 아이템그레이드 백그라운드 이미지 컴포넌트에 해당 텍스처를 세팅해줌
        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //일반등급의 아이템 ID로 이미지를 명명해 두었음. 아이템 ID를 등급값만 1로 치환해보겠음
        StringBuilder sb = new StringBuilder(m_InventoryIteSlotData.ItemId.ToString());
        //두번째 자리에 강제로 1을 넣어줌
        sb[1] = '1';
        //그걸 다시 문자열로 변환
        var itemIconName = sb.ToString();
        //이렇게 아이콘 이미지 명이 완성 되었음
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        //null검사 해주고 이상이 없으면 아이템 등급 이미지와 마찬가지로 아이콘이미지 컴포넌트의 텍스처를 세팅
        if(itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    public void OnClickInventoryItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_InventoryIteSlotData.SerialNumber;
        uiData.ItemId = m_InventoryIteSlotData.ItemId;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
