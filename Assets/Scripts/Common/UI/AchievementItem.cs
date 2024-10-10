using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Gpm.Ui;
using SuperMaxim.Messaging;

public class AchievementItemData : InfiniteScrollData
{
    public AchievementType AchievementType;
    public int AchieveAmount;
    public bool IsAchieved;
    public bool IsRewardClaimed;
}

public class AchievementItem : InfiniteScrollItem
{
    public GameObject AchievedBg;
    public GameObject UnAchievedBg;
    public TextMeshProUGUI AchievementNameTxt;
    public Slider AchievementProgressSlider;
    public TextMeshProUGUI AchievementProgressTxt;
    public Image RewardIcon;
    public TextMeshProUGUI RewardAmountTxt;
    public Button ClaimBtn;
    public Image ClaimBtnImg;
    public TextMeshProUGUI ClaimBtnTxt;

    private AchievementItemData m_AchievementItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        //매개변수로 받은 전용 데이터를 받아 옮
        m_AchievementItemData = scrollData as AchievementItemData;
        if(m_AchievementItemData == null)
        {
            Logger.LogError("m_AchievementItemData is invalid");
            return;
        }

        //그리고 해당 업적에 대한 데이터를 데이터 테이블 매니저에서 가져오겠음
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        //필요한 데이터를 모두 가져왔으면 본격적으로 UI요소를 세팅
        //업적의 달성 여부에 따라 그에 맞는 백그라운드 이미지 컴포넌트를 활성화
        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        //보상이미지를 보상 타입에 따라 세팅
        var rewardTextureName = string.Empty;
        switch(achievementData.AchievementRewardType)
        {
            case GlobalDefine.RewardType.Gold:
                rewardTextureName = "IconGolds";
                break;
            case GlobalDefine.RewardType.Gem:
                rewardTextureName = "IconGem";
                break;
            default:
                break;
        }

        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}");
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }
        //보상 수령 버튼을 조건에 맞게 활성화 또는 비활성화
        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.gray;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.gray;
    }

    public void OnClickClaimBtn()
    {
        //조건에 맞게 버튼을 비활성화 처리 했지만
        //혹시 모를 이유로 보상을 수령할 조건이 아니라면 예외처리
        if(!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }
        //보상 지급 처리를 하기 위해 필요한 데이터를 가져옮
        //유저 업적 진행 데이터를 가져옮
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("UserAchievementData does not exist");
            return;
        }

        //데이터 테이블에서 업적 데이터도 가져옮
        var achievementData = DataTableManager.Instance.GetAchievementsData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }

        //현재 업적 타입에 맞는 유저 진행 데이터를 가져 옮
        var userAchievedData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);

        //필요한 데이터를 모두 가져왔으니 업적 보상 지급처리
        if(userAchievedData != null)
        {
            //유저 재화데이터를 가져옮
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                //유저 업적 진행 데이터에서 보상 수령 여부를 true값으로 대입
                userAchievedData.IsRewardClaimed = true;
                userAchievementData.SaveData();
                //현재 UI전용 데이터에도 보상 수령 여부를 true값으로 갱신
                m_AchievementItemData.IsRewardClaimed = true;
                //업적 보상 타입에 따라 보상을 지급
                switch (achievementData.AchievementRewardType)
                {
                    case GlobalDefine.RewardType.Gold: //보상이 골드라면
                        //재화데이터의 골드 값에 보상 수량만큼 값을 증가
                        userGoodsData.Gold += achievementData.AchievementRewardAmount;

                        //골드 수령 메세지 발행
                        var goldUpdateMsg = new GoldUpdateMsg();
                        goldUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(goldUpdateMsg);

                        //업적 중에 골드를 획득하는 업적이 있기 때문에 그 업적에대한 처리를 해줌
                        userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                        break;
                    case GlobalDefine.RewardType.Gem: //보상이 골드라면
                        //재화데이터의 골드 값에 보상 수량만큼 값을 증가
                        userGoodsData.Gem += achievementData.AchievementRewardAmount;

                        //골드 수령 메세지 발행
                        var gemUpdateMsg = new GemUpdateMsg();
                        gemUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(gemUpdateMsg);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
