using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using SuperMaxim.Messaging;

public class AchievementUI : BaseUI
{
    public InfiniteScroll AchievementScrollList;

    private void OnEnable()
    {
        //업적이 진행 되었을 때 발생하는 메시지<AchievementProgressingMsg> 구독,
        //메시지를 받았을 때 실행할 함수(OnAchievementProgressed)
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetAchievementList();
        SortArchievementList();
    }

    private void SetAchievementList()
    {
        //먼저 스크롤 뷰 이전에 생성된 아이템이 있을 수 있으니 스크롤 리스트를 클리어 처리해줌.
        AchievementScrollList.Clear();

        //업적 목록 세팅
        //먼저 업적 데이터와 유저 업적 진행 데이터를 모두 가져옮
        var achievementDataList = DataTableManager.Instance.GetAchievementDataList();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();


        //Debug.Log(achievementDataList);
        Debug.Log(userAchievementData);
        if(achievementDataList != null && userAchievementData != null)
        {
            //데이터가 모두 정상이라면 업적 데이터 목록을 순회하면서
            //업적 아이템UI에 필요한 데이터를 생성해 주겠음.
            foreach (var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                //생성한 achievementItemData의 변수값을 세팅
                achievementItemData.AchievementType = achievement.AchievementType;

                //만약 유저 업적 진행 데이터에도 해당 업적 데이터가 있다면
                //업적이 얼마나 진행되었는지 세팅
                //업적 달성 여부와 업적 보상 수령 여부도 값을 대입해 줌
                var userAchieveData = userAchievementData.GetUserAchievementProgressData(achievement.AchievementType);
                if(userAchieveData != null)
                {
                    achievementItemData.AchieveAmount = userAchieveData.AchievementAmount;
                    achievementItemData.IsAchieved = userAchieveData.IsAchieved;
                    achievementItemData.IsRewardClaimed = userAchieveData.IsRewardClaimed;
                }
                //그리고 스크롤 뷰에 해당 데이터를 추가하여 업적 목록에 표시되도록 해줌.
                AchievementScrollList.InsertData(achievementItemData);
            }
        }
    }

    private void SortArchievementList()
    {
        //스크롤뷰에 SortDataList()호출하고 이 안에 람다식으로 정렬 로직을 작성
        //비교 대상인 a,b에서 각 업적 아이템 데이터를 받아옮
        AchievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;

            //업적을 달성했지만 보상을 받지 않은 업적을 제일 상위로 정렬
            var AComp = achievementA.IsAchieved && !achievementA.IsRewardClaimed;
            var BComp = achievementB.IsAchieved && !achievementB.IsRewardClaimed;

            int compareResult = BComp.CompareTo(AComp); //CompareTo 비교해서 위치가 같으면 0 -면 앞 +면 뒤

            //만약 조건이 동일하다면 달성하지 못한 업적을 달성한 업적 보다 더 상위에 정렬해 주겠음.
            if (compareResult == 0)
            {
                compareResult = achievementA.IsAchieved.CompareTo(achievementB.IsAchieved);

                if (compareResult == 0) //이 조건마저 같다면 그냥 업적 타입에 따라 정렬
                {
                    compareResult = (achievementA.AchievementType).CompareTo(achievementB.AchievementType);
                }
            }

            return compareResult;
        });
    }

    //업적 진행 메시지가 발행되었을 때 실행할 함수 ( 매개변수 : 메세지 )
    private void OnAchievementProgressed(AchievementProgressMsg msg)
    {
        SetAchievementList();
        SortArchievementList();
    }
}
