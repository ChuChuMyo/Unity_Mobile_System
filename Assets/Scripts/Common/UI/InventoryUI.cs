using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using TMPro;

//1. 먼저 어떤 조건에 의해 정렬을 할것인지를 나타내는 이넘값을 선언
public enum InventorySortType
{
    //등급
    ItemGrade,
    //종류
    ItemType,
}

//이벤토리 UI는 유저 인벤토리데이터에서 UI세팅에 필요한 데이터를 가져올 것임
//전용 UI데이터 클래스는 만들지 않음.(UI호출 시 그냥 Base UI데이터를 사용)
public class InventoryUI : BaseUI
{
    //먼저 각 장착 슬롯 컴포넌트 변수를 선언
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;


    //스크롤 뷰를 처리해줄 인피니티 스크롤 변수
    public InfiniteScroll InventoryScrollList;
    //현재 어떤 조건으로 정렬되어 있는지 표시해줄 텍스트
    public TextMeshProUGUI SortBtnTxt;
    //현재 정렬 방식을 갖고 있는 변수 선언 초기 값은 등급으로 설정
    InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        SetInventory();
        SortInventory();
        SetEquippedItems(); //장착된 아이템에 대한 UI처리를 담당할 함수 호출

    }

    void SetInventory()
    {
        //UI화면을 재활용하기 때문에 새롭게 세팅할 때마다 삭제처리를 해주지 않으면
        //기존에 생성된 아이템들이 그대로 남아 있게됨. 그래서 인피니티스크롤 안에있는 Clear함수 를 호출
        InventoryScrollList.Clear();
        //유저가 보유한 아이템을 스크롤 뷰에 만들자
        //우선 유저 인벤토리 데이터를 유저 데이터매니저에서 가져옴
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData != null)
        {
            //순회하며 각 아이템에 대해서 아이템 슬롯 인스턴스를 만들어준다.
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                //실제로 슬롯에 있는 데이터를 실제 아이템 데이터를 넣어주기
                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    //인벤토리 정렬 함수
    void SortInventory()
    {
        //타입에 따라 분기 시킴
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortBtnTxt.text = "GRADE";
                //위에서 인피니티스크롤에 만든 숏데이터리스트 함수를 호출하면서
                //원하는 등급별 정렬 조건 로직을 람다 형식으로 작성해서 넘겨줌
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b데이터를 인벤토리아이템슬롯데이터로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //아이템 ID의 두번째 자릿수가 아이템의 등급을 나타내기 때문에 그 등급 값을 가져와서 비교
                    //여기서 itemB등급을 기준으로 비교한 것은 그래야 내림차순으로 정렬이됨.
                    //높은 등급에서 낮은 등급으로 정렬되기를 원하기 때문에 내림차순으로 
                    //CompareTo : 앞, 뒤 또는 동일한지를 나타내는 정수를 반환
                    //0보다 작음 이 인스턴스가 밸류 앞에 오는 경우 이런식임.
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    //결과 값이 0 이라면 즉, 등급이 같다면 같은 등급내에서는
                    //종류별로 다시 정렬을 해줘야함
                    //종류별 정렬을 아이템 ID에서 등급을 나타내는 두번째 자릿수만을 제외한 나머지 값으로 비교
                    if (compareResult == 0)
                    {
                        var itemAIdStr = itemA.ItemId.ToString();
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                        var itemBIdStr = itemB.ItemId.ToString();
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);
                        compareResult = itemAComp.CompareTo(itemBComp);
                    }
                    return compareResult;
                });
                break;
            case InventorySortType.ItemType:
                SortBtnTxt.text = "TYPE";
                //위에서 인피니티스크롤에 만든 숏데이터리스트 함수를 호출하면서
                //원하는 등급별 정렬 조건 로직을 람다 형식으로 작성해서 넘겨줌
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //a, b데이터를 인벤토리아이템슬롯데이터로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //아이템 ID의 두번째 자릿수가 아이템의 등급을 나타내기 때문에 그 등급 값을 가져와서 비교
                    //여기서 itemB등급을 기준으로 비교한 것은 그래야 내림차순으로 정렬이됨.
                    //높은 등급에서 낮은 등급으로 정렬되기를 원하기 때문에 내림차순으로 
                    //CompareTo : 앞, 뒤 또는 동일한지를 나타내는 정수를 반환
                    //0보다 작음 이 인스턴스가 밸류 앞에 오는 경우 이런식임.
                    //결과 값이 0 이라면 즉, 등급이 같다면 같은 등급내에서는
                    //종류별로 다시 정렬을 해줘야함
                    //종류별 정렬을 아이템 ID에서 등급을 나타내는 두번째 자릿수만을 제외한 나머지 값으로 비교
                    var itemAIdStr = itemA.ItemId.ToString();
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    int compareResult = itemAComp.CompareTo(itemBComp);

                    if (compareResult == 0)
                    {
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }
                    return compareResult;
                });
                break;
            default:
                break;
        }
    }
    
    //장착된 아이템에 대한 UI처리를 담당할 함수 작성
    private void SetEquippedItems()
    {
        //UserInventoryData를 가져온다
        var UserInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //데이터가 null이면 에러로그
        if(UserInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }

        //null 아니면 SetItem, null이면 ClearItem 실행
        if(UserInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(UserInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
        }

        if (UserInventoryData.EquippedShieldData != null)
        {
            ShieldSlot.SetItem(UserInventoryData.EquippedShieldData);
        }
        else
        {
            ShieldSlot.ClearItem();
        }

        if (UserInventoryData.EquippedChestArmorData != null)
        {
            ChestArmorSlot.SetItem(UserInventoryData.EquippedChestArmorData);
        }
        else
        {
            ChestArmorSlot.ClearItem();
        }

        if (UserInventoryData.EquippedBootsData != null)
        {
            BootsSlot.SetItem(UserInventoryData.EquippedBootsData);
        }
        else
        {
            BootsSlot.ClearItem();
        }

        if (UserInventoryData.EquippedGlovesData != null)
        {
            GlovesSlot.SetItem(UserInventoryData.EquippedGlovesData);
        }
        else
        {
            GlovesSlot.ClearItem();
        }

        if (UserInventoryData.EquippedAccessoryData != null)
        {
            AccessorySlot.SetItem(UserInventoryData.EquippedAccessoryData);
        }
        else
        {
            AccessorySlot.ClearItem();
        }
    }
     
    //인벤토리 UI정렬 버튼을 눌렀을 때 현재 정렬 조건을
    //다른 정렬 조건으로 변경하고 그값에 따라 다시 정렬해주는 기능
    public void OnClickSortBtn()
    {

        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_InventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }
        SortInventory();
    }
}
