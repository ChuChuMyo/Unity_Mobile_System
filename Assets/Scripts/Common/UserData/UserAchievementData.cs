using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperMaxim.Messaging;
using System;
using System.Linq;

//�� ���� ������ ���� ��Ȳ�� �����ϴ� Ŭ������ ����
[Serializable]
public class UserAchievementProgressData
{
    public AchievementType AchievementType;
    public int AchievementAmount;
    public bool IsAchieved;
    public bool IsRewardClaimed;
}

//���� ���� ���� ���� ��Ȳ �����͸� ����Ʈ�� ���·� �����տ�
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    //UserAchievementProgressData�� ��� ����Ʈ �ڷᱸ���� ����
    public List<UserAchievementProgressData> AchievementProgressDataList;
}

//���� ���� ��Ȳ�� ������ ������ �޼����� �߻��� �ٰ���
//�߻����� �޼��� Ŭ������ ����
public class AchievementProgressMsg
{

}

public class UserAchievementData : IUserData
{
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();

    //������ ó�� ����Ǿ��� �� �׶� �����͸� ������ �ٰ��̱� ������ ���⼭�� �ƹ��� �۾� X
    public void SetDefaultData()
    {

    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            //�÷��̾��������� �ִ� Json��Ʈ�� �����͸� �ҷ���
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");

            if(!string.IsNullOrEmpty(achievementProgressDataListJson)) //�����Ͱ� ����� ���� �ִٸ�
            {
                //JsonUtilityŬ������ ���� WrapperŬ������ �ǽ��� ��
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper
                    = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);

                //���� Ŭ������ ��� �����͸� �ٽ� UserAchievementDataŬ������ AcnievementProgressDataList ����
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;

                Logger.Log("AchievementProgressDataList"); //�ε��� �������� �α� ǥ��
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
            //����Ǿ� �ִ� �����͸� ���� Ŭ������ �Ű��ش�.
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //����Ŭ������ Json��Ʈ������ ��ȯ���ְ�
            string achievementProgressDataListJson = JsonUtility.ToJson("AchievementProgressDataList");
            //�� ��Ʈ�� ���� �÷��̾� �������� ����
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
            // �޼��� ��ġ��ŭ �޼� ��ġ�� ����
            userAchievementProgressData.AchievementAmount += achiveAmount;
            //���� ��ǥ�޼� ��ġ���� �ʰ��ؼ� �޼��ߴٸ� �޼� ��ǥġ�� ����
            if (userAchievementProgressData.AchievementAmount > achivementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achivementData.AchievementGoal;
            }

            if(userAchievementProgressData.AchievementAmount == achivementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }

            SaveData();

            //���� ���� ��Ȳ�� ���̵Ǿ��ٴ� �޼��� ����
            //�� �޽����� ���� UIȭ�鿡�� ����� ����.
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
}
