using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGoodsData : IUserData
{
    //보석, int 범위를 벗어날 수 있다.
    public long Gem { get; set; }
    //골드
    public long Gold { get; set; }

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetlDefaultData");

        Gem = 0;
        Gold = 0;
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            //가져올땐 스트링을 long으로 형변환을 시켜준다~
            Gem = long.Parse(PlayerPrefs.GetString("Gem"));
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            result = true;
            Logger.Log($"Gem : {Gem} Gold : {Gold}");
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed(" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SetlDefaultData");

        bool result = false;
        try
        {
            //플레이어프랩스는 롱을 저장할 수 없는데 써야하니 스트링으로 저장해준뒤
            PlayerPrefs.SetString("Gem", Gem.ToString());
            PlayerPrefs.SetString("Gold", Gold.ToString());
            PlayerPrefs.Save();
            result = true;

            Logger.Log($"Gem:{Gem}, Gold:{Gold}");
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }


        return result;
    }
}