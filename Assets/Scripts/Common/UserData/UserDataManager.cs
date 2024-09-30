using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    //저장된 유저 데이터 존재 여부
    public bool ExistsSavedData { get; set; }
    //모든 유저 데이터 인스턴스를 저장하는 컨테이너
    //모든 UserData클래스는 IUserData 인터페이스를 구현하기 때문에
    //IUserData 타입으로 컨테이너를 선언하면 모든 유저 데이터 클래스를 이 컨테이너에 저장할 수 있음
    public List<IUserData> UserDataList { get; set; } = new List<IUserData>();

    protected override void Init()
    {
        //싱클톤인스턴스 처리가 인잇 함수에서 실행 되기 떄문에 해줘여함.
        base.Init();
        //모든 유저 데이터를 UserDataList에 추가
        //셋팅 데이터 - 설정창
        UserDataList.Add(new UserSettingsData());
        //굿즈 데이터 - 재화
        UserDataList.Add(new UserGoodsData());
        //인벤토리 데이터 - 아이템
        UserDataList.Add(new UserInventoryData());
    }
    //모든 유저데이터를 기본 값으로 초기화 하는 함수
    public void SetDefaultUserData() 
    {
        for (int i = 0; i < UserDataList.Count; i++)
        {
            //리스트 안에 나를 내가 실행 시켜줌
            UserDataList[i].SetDefaultData();
        }
    }

    //모든 유저 데이터 클래스에 LoadData함수를 호출해주는 함수
    public void LoadUserData()
    {
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;
        //만약 저장된 데이터가 존재한다면
        if (ExistsSavedData)
        {
            //모든 유저데이터 클래스에 LoadData를 호출
            for (int i = 0; i < UserDataList.Count; i++)
            {
                UserDataList[i].LoadData();
            }
        }
    }

    //모든 유저데이터 클래스의 SaveData함수를 호출해서 모든 유저 데이터를 저장하는 함수
    public void SaveUserData()
    {
        bool hasSaveError = false;

        for (int i = 0; i < UserDataList.Count; i++)
        {
            //세이브가 성공적으로 이루어 졋는지 확인해주는 불 지역 변수
            bool isSaveSuccess = UserDataList[i].SaveData();
            //만약 이제 에러가 났다면
            if (!isSaveSuccess)
            {
                hasSaveError = true;
            }
        }
        //이렇게 되면 포문을 빠져나왔을 때 즉, 모든 세이브 과정이 끝났을 때
        //하나라도 에러가 발생한 유저데이터클래스가 있다면 hasSaveError = true가 될 것임.
        //세이브에러가 하나라도 발생하지 않았다면(세이브가 정상적으로 이뤄졋을 때만)
        if (!hasSaveError)
        {
            ExistsSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
        }
    }
    //여기서 T 오브젝트타입은 찾고자 하는 UserData의 클래스 타입 (class, IUserData)
    public T GetUserData<T>() where T : class, IUserData
    {
        //타입이 T인 것 중에 첫번째 인스턴스를 리턴하거나 없으면 null리턴
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}