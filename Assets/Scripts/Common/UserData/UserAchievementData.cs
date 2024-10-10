using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;
using System;
using System.Linq;

//각 업적 데이터 진행 상황을 저장하는 클래스를 선언
[Serializable]
public class UserAchievementProgressData
{
    public AchievementType AchievementType;
    public int AchievementAmount;
    public bool IsAchieved;
    public bool IsRewardClaimed;
}

//여러 개의 업적 진행 상황 데이터를 리스트의 형태로 프리팹에
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    //UserAchievementProgressData를 담는 리스트 자료구조를 선언
    public List<UserAchievementProgressData> AchievementProgressDataList;
}

//업적 진행 상황을 갱신할 때마다 메세지를 발생해 줄것임
//발생해줄 메세지 클래스도 선언
public class AchievementProgressMsg
{

}

public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();

    //업적이 처음 진행되었을 때 그때 데이터를 생성해 줄것이기 때문에 여기서는 아무런 작업 X
    public void SetDefaultData()
    {

    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            //플레이어프랩스에 있는 Json스트링 데이터를 불러옮
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");

            if(!string.IsNullOrEmpty(achievementProgressDataListJson)) //데이터가 저장된 것이 있다면
            {
                //JsonUtility클래스를 통해 Wrapper클래스로 피싱해 옴
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper
                    = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);

                //래퍼 클래스에 담긴 데이터를 다시 UserAchievementData클래스의 AcnievementProgressDataList 대입
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;

                Logger.Log("AchievementProgressDataList"); //로드한 데이터의 로그 표시
                foreach (var item in AchievementProgressDataList)
                {
                    Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount:{item.AchievementAmount} IsAchieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
                }
            }
            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = new UserAchievementProgressDataListWrapper();
            //저장되어 있는 데이터를 래퍼 클래스로 옮겨준다.
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //래퍼클래스를 Json스트링으로 변환해주고
            string achievementProgressDataListJson = JsonUtility.ToJson("AchievementProgressDataList");
            //그 스트링 값을 플레이어 프랩스에 저장
            PlayerPrefs.SetString("AchievementProgressDataList", achievementProgressDataListJson);

            Logger.Log("AchievementProgressDataList");
            foreach (var item in AchievementProgressDataList)
            {
                Logger.Log($"AchievementType:{item.AchievementType} AchivementAmount:{item.AchievementAmount} " +
                    $"IsAcieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
            }

            PlayerPrefs.Save();

            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }

        return result;
    }

    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(Item => Item.AchievementType == achievementType).FirstOrDefault();

    }

    public void ProgressAchievement(AchievementType achievementType, int achiveAmount)
    {
        var achivementData = DataTableManager.Instance.GetAchievementsData(achievementType);
        if(achivementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);

        if(userAchievementProgressData == null)
        {
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }

        if(!userAchievementProgressData.IsAchieved)
        {
            // 달성한 수치만큼 달성 수치를 증가
            userAchievementProgressData.AchievementAmount += achiveAmount;
            //만약 목표달성 수치보다 초과해서 달성했다면 달성 목표치로 대입
            if (userAchievementProgressData.AchievementAmount > achivementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achivementData.AchievementGoal;
            }

            if(userAchievementProgressData.AchievementAmount == achivementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }

            SaveData();

            //업적 진행 상황이 갱싱되었다는 메세지 발행
            //이 메시지는 업적 UI화면에서 사용할 것임.
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
}
