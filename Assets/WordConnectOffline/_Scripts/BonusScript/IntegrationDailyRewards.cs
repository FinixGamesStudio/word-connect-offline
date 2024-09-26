using UnityEngine;

namespace WordConnectByFinix
{
    public class IntegrationDailyRewards : MonoBehaviour
    {
        void OnEnable()
        {
            DailyRewards.instance.onClaimPrize += OnClaimPrizeDailyRewards;
        }

        void OnDisable()
        {
            DailyRewards.instance.onClaimPrize -= OnClaimPrizeDailyRewards;
        }

        // this is your integration function. Can be on Start or simply a function to be called
        public void OnClaimPrizeDailyRewards(int day)
        {
            //This returns a Reward object
            DailyReward myReward = DailyRewards.instance.GetReward(day);

            // And you can access any property
            print(myReward.unit);   // This is your reward Unit name
            print(myReward.reward); // This is your reward count

            var rewardsCount = PlayerPrefs.GetInt("MY_REWARD_KEY", 0);
            rewardsCount += myReward.reward;

            PlayerPrefs.SetInt("MY_REWARD_KEY", rewardsCount);
            PlayerPrefs.Save();
        }
    }
}