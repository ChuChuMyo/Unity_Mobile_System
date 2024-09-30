using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    //데이터 테이블 파일들이 들어있는 경로를 스트링 값으로 설정
    const string DATA_PATH = "DataTable";
    //싱글톤 인잇 오버라이드
    protected override void Init()
    {
        base.Init();
        LoadChapterDataTable();
        LoadItemDataTable();
    }

    #region CHAPTER_DATA
    //챕터 데이터 테이블 파일명을 갖는 스트링 변수
    const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    //모든 챕터 데이터를 저장할 수있는 컨테이너 즉, 자료구조를 선언
    List<ChapterData> ChapterDataTable = new List<ChapterData>();
    void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
        //var 타입 : 데이터 타입을 신경쓰지 않아도 알아서 변수에 있는 값을 판단해서
        //타입을 인식하는 데이터 타입.
        //지역변수로만 사용 가능
        //반드시 선언과 동시에 초기화(그래야 실질적으로 데이터타입이 정해지기 때문)
        //타입이 복잡한 경우 지역변수로 사용할때는 var사용해도 괜찮다.

        //테이블을 순회하면서 각 데이터를
        //ChapterData인스턴스로 만들어서
        //ChapterDataTable 컨테이너에 넣어줌
        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                //오브젝트 타입이라 지정된 개체의 값을 32비트 부호 있는 정수로 변환
                //Convert.ToInt32를 사용하여 32비트인 인트로 형변환 시켜주는 문법
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                TotalStage = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"])
            };
            ChapterDataTable.Add(chapterData);
        }
    }

    //이렇게 로드한 ChapterDataTable에서 찾고자 하는 ChapterData만 가져오는 함수
    public ChapterData GetChapterData(int chapterNo)
    {
        //특정 챕터 넘버로 챕터 데이터 테이블을 검색해서
        //그 챕터 넘버에 해당하는 데이터를 반환하는 함수
        //링크사용 -> 링크 : 검색 변경을 좀 더 용이하게 해주는 기능이다.
        //만약 링크를 사용하지 않는다면
        /*
        foreach (var item in ChapterDataTable)
        {
            if(item.ChapterNo == chapterNo)
            {
                return item;
            }
        }
        return null;
        */
        //Linq
        //.Where 조건식이 true인 요소만 필터링해준다.
        //FirstOrDefault() 함수 : 매개변수가 생략된 경우 컬렉션의 첫 번째 요소를 반환 합니다.
        //주어진 조건을 만족하는 시퀀스에서 첫 번째 요소를 검색하는 LINQ의 매서드
        //new[] {"A", "B", "C"}.FirstOrDefault();
        //결과//
        //A

        //이 컨테이너 안에 있는 아이템 중에서 챕터 넘버가 매개변수 챕터 넘버 값과 같을 시 리턴
        //이 조건에 부합하는 첫 엘리먼트를 리턴하거나 아니면 이 조건에 맞는 엘리먼트가 없을때는 널을 리턴해줌
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
    }
    #endregion

    #region ITEM_DATA
    //데이터테이블 명을 가진 변수를 선언
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    //아이템데이터를 담을 컨테이너를 선언
    List<ItemData> ItemDataTable = new List<ItemData>();

    void LoadItemDataTable()
    {
        //csv파일을 읽어옴
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");
        //데이터를 참고해서 아이템데이터를
        foreach(var data in parsedDataTable)
        {
            var itemData = new ItemData
            {
                ItemId = Convert.ToInt32(data["item_id"]),
                ItemName = data["item_name"].ToString(),
                AttackPower = Convert.ToInt32(data["attack_power"]),
                Defense = Convert.ToInt32(data["defense"]),
            };
            ItemDataTable.Add(itemData);
        }
    }

    //아이템 데이터 컨테이너에서 특정 아이템 아이디에 데이터를 찾는 함수
    public ItemData GetItemData(int itemid)
    {
        return ItemDataTable.Where(item => item.ItemId == itemid).FirstOrDefault();
    }
    #endregion
}



//챕터 데이터의 각 값을 저장할 수 있도록 만들어야하는 클래스
public class ChapterData
{
    public int ChapterNo;
    public int TotalStage;
    public int ChapterRewardGem;
    public int ChapterRewardGold;
}

//아이템 관련 클래스와 이넘
public class ItemData
{
    public int ItemId;
    public string ItemName;
    public int AttackPower;
    public int Defense;
}

public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory
}

public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}