using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserData
{
    //기본 값으로 데이터 초기화
    void SetDefaultData();
    //데이터 로드
    bool LoadData();
    //데이터 저장
    bool SaveData();
}
