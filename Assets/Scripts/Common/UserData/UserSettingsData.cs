using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IUserData 잠재적 수정사항을 표시해서 인터페이스 구현을 선택했음
//자동으로 인터페이스 함수 만듬
public class UserSettingsData : IUserData
{
    //사운드 on / off 여부
    public bool Sound { get; set; }

    public void SetDefaultData()
    {
        //GetType()을 호출해 클래스명 출력하고 함수명을 그대로 출력
        Logger.Log($"{GetType()}::SetDefaultData");

        Sound = true;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
        try
        {
            Sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
            result = true;

            Logger.Log($"Sound:{Sound}");
        }
        catch (System.Exception e)
        {
            Logger.Log($"Load failed (" + e.Message + ")");
        }

        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::Save Data");

        bool result = false;
        try
        {
            //사운드가 트루면 1, 펄스면 0
            PlayerPrefs.SetInt("Sound", Sound ? 1 : 0);
            PlayerPrefs.Save();
            result = true;
            Logger.Log($"Sound : {Sound}");
        }
        catch(System.Exception e)
        {
            Logger.Log("Save failed(" + e.Message + ")");
        }

        return result;
    }
}
