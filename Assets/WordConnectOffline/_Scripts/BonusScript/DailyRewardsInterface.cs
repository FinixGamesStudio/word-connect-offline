
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;

namespace WordConnectByFinix
{
    public class DailyRewardsInterface : MonoBehaviour
    {
        //public Canvas canvas;
        public GameObject otherDailyRewardPrefab;        // Prefab containing each daily reward
        public GameObject day7DailyRewardPrefab;        // Prefab containing each daily reward
        public RectTransform rewardScreen;

        public bool isDebug;
        public GameObject panelDebug, bonusScreen;

        public Text textReward;                     // Reward Text to show an explanatory message to the player
        public Button buttonCloseReward;            // The Button to close the Rewards Panel
        public Image imageReward;                   // The image of the reward

        public GameObject timerObj, collectObj;
        public GameObject blackPanel, claimPopUop;
        public Text textTimeDue;                    // Text showing how long until the next claim
        public GridLayoutGroup[] dailyRewardsGroup;   // The Grid that contains the rewards

        private bool readyToClaim;                  // Update flag
        public List<DailyRewardUI> dailyRewardsUI = new List<DailyRewardUI>();

        private DailyRewards dailyRewards;			// DailyReward Instance      

        void Awake()
        {
            //canvas.gameObject.SetActive(false);
            dailyRewards = GetComponent<DailyRewards>();
        }

        void Start()
        {
            InitializeDailyRewardsUI();

            if (panelDebug)
                panelDebug.SetActive(isDebug);
            UpdateUI();
            SetPanel.MoveRightSide(rewardScreen, ConfigController.instance.canvasWidth);

        }


        void OnEnable()
        {
            dailyRewards.onClaimPrize += OnClaimPrize;
            dailyRewards.onInitialize += OnInitialize;
        }

        void OnDisable()
        {
            if (dailyRewards != null)
            {
                dailyRewards.onClaimPrize -= OnClaimPrize;
                dailyRewards.onInitialize -= OnInitialize;
            }
        }

        public void ClickOnAdvanceDayBtn()
        {
            dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0, 0));
            UpdateUI();
        }

        public void ClickOnAdvanceHourBtn()
        {
            dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0));
            UpdateUI();
        }

        public void ClickOnResetBtn()
        {
            dailyRewards.Reset();
            dailyRewards.debugTime = new TimeSpan();
            dailyRewards.lastRewardTime = System.DateTime.MinValue;
            readyToClaim = false;
        }


        public void ClickOnCloseClaimPopUpBtn()
        {
            var keepOpen = dailyRewards.keepOpen;
            CloaseClaimPopUp();
            UpdateUI();
        }

        // Initializes the UI List based on the rewards size
        private void InitializeDailyRewardsUI()
        {
            for (int i = 0; i < dailyRewards.rewards.Count; i++)
            {
                int day = i + 1;
                var reward = dailyRewards.GetReward(day);
                GameObject dailyRewardGo = null;
                if (i == 6)
                    dailyRewardGo = GameObject.Instantiate(day7DailyRewardPrefab) as GameObject;
                else
                    dailyRewardGo = GameObject.Instantiate(otherDailyRewardPrefab) as GameObject;

                DailyRewardUI dailyRewardUI = dailyRewardGo.GetComponent<DailyRewardUI>();
                dailyRewardGo.transform.localScale = Vector2.one;

                dailyRewardUI.claimBtn.onClick.AddListener(() =>
                {
                    dailyRewards.ClaimPrize();
                });

                dailyRewardUI.day = day;
                dailyRewardUI.reward = reward;
                dailyRewardUI.Initialize();

                dailyRewardsUI.Add(dailyRewardUI);
                if (dailyRewardsUI.Count <= 3) // 0 1 2
                    dailyRewardUI.transform.SetParent(dailyRewardsGroup[0].transform, false);
                else if (dailyRewardsUI.Count >= 3 && dailyRewardsUI.Count <= 6) // 3 4 5
                    dailyRewardUI.transform.SetParent(dailyRewardsGroup[1].transform, false);
                else
                    dailyRewardUI.transform.SetParent(dailyRewardsGroup[2].transform, false);
                readyToClaim = false;
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            dailyRewards.CheckRewards();

            bool isRewardAvailableNow = false;

            var lastReward = dailyRewards.lastReward;
            var availableReward = dailyRewards.availableReward;

            foreach (var dailyRewardUI in dailyRewardsUI)
            {
                var day = dailyRewardUI.day;

                if (day == availableReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_AVAILABLE;
                    isRewardAvailableNow = true;
                }
                else if (day <= lastReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.CLAIMED;
                }
                else
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_UNAVAILABLE;
                }

                dailyRewardUI.Refresh();
            }
            if (isRewardAvailableNow)
            {
                collectObj.SetActive(true);
                timerObj.SetActive(false);
            }
            else
            {
                collectObj.SetActive(false);
                timerObj.SetActive(true);
            }
            readyToClaim = isRewardAvailableNow;
        }

        void Update()
        {
            dailyRewards.TickTime();
            // Updates the time due
            CheckTimeDifference();
        }

        private void CheckTimeDifference()
        {
            if (!readyToClaim)
            {
                TimeSpan difference = dailyRewards.GetTimeDifference();

                // If the counter below 0 it means there is a new reward to claim
                if (difference.TotalSeconds <= 0)
                {
                    readyToClaim = true;
                    UpdateUI();
                    return;
                }

                string formattedTs = dailyRewards.GetFormattedTime(difference);

                textTimeDue.text = string.Format(formattedTs);
            }
        }

        // Delegate
        private void OnClaimPrize(int day)
        {
            OpenClaimPopUp();

            DailyReward t = dailyRewards.GetReward(day);

            imageReward.sprite = t.sprite;
            if (t.reward > 0)
                textReward.text = string.Format("You got {0} {1}!", t.reward, t.unit);
            else
                textReward.text = string.Format("You got {0}!", t.unit);

            CurrencyController.CreditBalance(t.reward);
        }

        private void OnInitialize(bool error, string errorMessage)
        {
            if (!error)
            {
                UpdateUI();
                CheckTimeDifference();
            }
        }

        public void ClickOnDailyBonusBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPanel.MoveMiddle(rewardScreen);
        }
        public void ClickOnCloseBonusBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPanel.MoveLeftSide(rewardScreen, ConfigController.instance.canvasWidth);
        }

        public void OpenClaimPopUp()
        {
            blackPanel.SetActive(true);
            claimPopUop.transform.DOScale(Vector3.one, 0.2f);
        }

        public void CloaseClaimPopUp()
        {
            ConfigController.instance.soundController.PlayButton();
            claimPopUop.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => blackPanel.SetActive(false));
        }
    }
}