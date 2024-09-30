using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///먼저 보유한 개별 아이템 정보를 저장할 클래스
///제이슨 직렬화[Serializable]
///플레이어프렙스에서는 정수 실수 문자열 값만 저장할 수 있기 때문에
///이 클래스의 인스턴스가 문자열 값으로 변환이 가능하게 직렬화가 가능하다고 선언을 해주는 것임.
///직렬화란? 인스턴스를 byte나 문자열 값으로 변환하는 것을 의미
/// </summary>

[Serializable]
public class UserItemData
{
    //전체 아이템에서 특정 아이템을 식별하기 위한 id
    //unique value
    public long SerialNumber;
    //해당 아이템의 아이템데이타테이블 상의 ItemID
    //이후 UI를 처리할 때 이 아이템ID를 참고하여 데이터 테이블 매니저에서
    //해당 아이템데이터의 상세 데이터를 가져와서 아이템 명이나 공격력, 방어력과 같은
    //스탯을 표시를 해줄 예정
    public int ItemId;

    //유저 아이템 데이터 생성자
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}

/// <summary>
///유저가 보유한 아이템 리스트를 로컬 디바이스에 로드하고 저장하는데 쓸 래퍼 클래스
///이 클래스를 왜만드느냐 하면 바로 아래에 만들 UserInventoryData에
///동일하게 이 변수를 선안할 예정
///그리고 바로 그 변수에 유저가 보유한 아이템 목록이 저장될 건데
///이렇게 리스트로 된 객체 데이터를 플레이어프랩스로 저장할 때 Json스트링으로
///변환을 해서 저장할 것임
///그런데 유니티에서 제공하는 JsonUtillity라는 Json API클래스가
///리스트를 저장할 때 이 리스트 컨테이너를 바로  Json스트링으로 변환하는 기능을 제공하지 않음.
///꼭 이렇게 래퍼클래스가 있고 그 안에 리스트가 선언된 형태로 만들어줘야
///변환을 제대로 해주기 때문에 이 래퍼 클래스를 만드는 거임.
/// </summary>

//wrapper class to parse data yo JSON using JSONUtillity
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

public class UserInventoryData : IUserData
{
    //먼저 각 장착 슬롯들을 선언
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAccessoryData { get; set; }


    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        //기본적으로 12개의 아이템을 지급해 주도록 하겠음.
        //아이템의 시리얼 넘버는  다른 아이템과 겹치지 않는 고유의 값이어야 한다.
        //실제 각 프로젝트마다 고유의 법칙으로 시리얼 넘버를 만듬
        //아이템ID와 마찬가지로 아이템에 대한 정보를 내포하면서 고유한 값을 갖는 방식으로 만듬
        //규칙이 정해진 게 아니므로 자신의 규칙으로 시리얼 넘버를 만들면 된다.
        // InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 이렇게 쓰고 뒤에 등급 작성
        //현재 시간에 랜덤한 수를 붙여서 시리얼 넘버를 생성해줄거임
        //새로운 유저 아이템 데이터가를 생성환과 동시에 IN
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65002));

        //(0)번째 인덱스에 있는 데이터를 무기 슬롯에
        //(2)번째 인덱스에 있는 데이터를 방패 슬롯에 대입
        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //이제 장착 아이템 데이터를 로드하는 로직을 작성
            //무기 슬롯 데이터를 로드
            //플레이어프랩스에서 "EquippedWeaponData"로 저장되어있는 문자열 데이터가 있다면
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");
            if(!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemId:{EquippedWeaponData.ItemId}");
            }

            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(shieldJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemId:{EquippedShieldData.ItemId}");
            }

            string chestArmorJson = PlayerPrefs.GetString("EquippedChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemId:{EquippedChestArmorData.ItemId}");
            }

            string bootsJson = PlayerPrefs.GetString("EquippedBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemId:{EquippedBootsData.ItemId}");
            }

            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemId:{EquippedGlovesData.ItemId}");
            }

            string accessoryJson = PlayerPrefs.GetString("EquippedAccessoryData");
            if (!string.IsNullOrEmpty(accessoryJson))
            {
                EquippedAccessoryData = JsonUtility.FromJson<UserItemData>(accessoryJson);
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemId:{EquippedAccessoryData.ItemId}");
            }



            //이벤토리아이템디어타 리스트로 저정된 스트링 값이 있는지 확인
            //만약 데이터가 존재한다면 제이슨유틸리티 클래스를 이용해 
            //위에서 만든 래퍼클래스로 저장된 데이터를 받아옴 InventoryItemDataList : 저장한 Key 값
            string inventoryItemDataJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataJson))
            {
                //플레이프레스를 사용할 경우 이 단계가 꼭! 필요한 단계임 리스트컨테이너를 랩핑해주는 것
                UserInventoryItemDataListWrapper itemDataListWrapper =JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataJson);
                //그 래퍼 클래스 내에 있는 인벤토리아이템데이터리스트에 있는데이터를
                //유저이벤토리아이템데이터의 인벤토리아이템데이터 리스트 변수에 대입
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;
                Logger.Log("InventoryItemDataList");//잘 로드 되고있는지 확인
                //시리얼 넘버와 아이템 아이디도 찍어보자
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    //유저가 보유한 아이템을 저장하는 함수
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;

        try
        {
            //반대로 장착 아이템 데이터를 저장하는 로직을 작성
            //"EquippedWeaponData"가 null이어도(장착한 아이템이 없어도)
            //반드시 변환해서 저장해 주어야함.
            //그래야 반대로 로딩을 할 때 슬롯에 아이템이 장착되어 있지 않은 상태라는 것을 인지할 수 있음.
            string weaponJson = JsonUtility.ToJson(EquippedWeaponData);
            PlayerPrefs.SetString("EquippedWeaponData", weaponJson);
            if (EquippedWeaponData != null)
            {
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber} ItemId:{EquippedWeaponData.ItemId}");
            }

            string shieldJson = JsonUtility.ToJson(EquippedShieldData);
            PlayerPrefs.SetString("EquippedWeaponData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber} ItemId:{EquippedShieldData.ItemId}");
            }

            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedWeaponData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber} ItemId:{EquippedChestArmorData.ItemId}");
            }

            string bootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedWeaponData", bootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber} ItemId:{EquippedBootsData.ItemId}");
            }

            string glovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedWeaponData", glovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber} ItemId:{EquippedGlovesData.ItemId}");
            }

            string accessoryJson = JsonUtility.ToJson(EquippedAccessoryData);
            PlayerPrefs.SetString("EquippedWeaponData", accessoryJson);
            if (EquippedAccessoryData != null)
            {
                Logger.Log($"EquippedAccessoryData: SN:{EquippedAccessoryData.SerialNumber} ItemId:{EquippedAccessoryData.ItemId}");
            }


            //저장에 필요한 랩퍼 클래스 인스턴스를 생성
            //그 인스턴스 안에 있는 아이템 데이터리스트에 유저가 현재 보유한 
            //인벤토리 아이템 정보를 담고 있는 인벤토리 아이템 데이터 리스트를 대입
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //이 데이터를 JsonUtillity클래스를 이용해서 스트링으로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //이 스트링 값을 플레이어프렙스에 저장
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);
            Logger.Log("InventoryItemDataList");
            foreach (var item in InventoryItemDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
            }

            PlayerPrefs.Save();
            result = true;
        }
        catch(Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }

        return result;
    }
}