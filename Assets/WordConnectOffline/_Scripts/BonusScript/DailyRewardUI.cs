using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class DailyRewardUI : MonoBehaviour
    {
        public bool showRewardName;

        [Header("UI Elements")]
        public Text textDay;                // Text containing the Day text eg. Day 12
        public Text textReward;             // The Text containing the Reward amount
        public Image imageRewardBackground; // The Reward Image Background
        public Image imageReward;           // The Reward Image
        public Image lableImg, btnImg;
        public Sprite[] lableSprite;
        public Sprite[] btnSprite;
        public GameObject hideObj;
        public GameObject cointextImg;
        public Text claimText;
        public Button claimBtn;


        [Header("Internal")]
        public int day;

        [HideInInspector]
        public DailyReward reward;

        public DailyRewardState state;

        // The States a reward can have
        public enum DailyRewardState
        {
            UNCLAIMED_AVAILABLE,
            UNCLAIMED_UNAVAILABLE,
            CLAIMED
        }

        public void Initialize()
        {
            textDay.text = string.Format("Day {0}", day.ToString());
            if (reward.reward > 0)
            {
                if (showRewardName)
                {
                    textReward.text = reward.reward + " " + reward.unit;
                }
                else
                {
                    textReward.text = reward.reward.ToString();
                }
            }
            else
            {
                textReward.text = reward.unit.ToString();
            }
            imageReward.sprite = reward.sprite;
        }

        // Refreshes the UI
        public void Refresh()
        {
            switch (state)
            {
                case DailyRewardState.UNCLAIMED_AVAILABLE:
                    lableImg.sprite = lableSprite[1];
                    cointextImg.SetActive(false);
                    claimText.text = "CLAIM";
                    claimText.gameObject.SetActive(true);
                    hideObj.SetActive(false);
                    btnImg.sprite = btnSprite[1];    //YELLOW
                    claimText.color = Color.white;
                    break;
                case DailyRewardState.UNCLAIMED_UNAVAILABLE:
                    lableImg.sprite = lableSprite[2];
                    cointextImg.SetActive(true);
                    claimText.gameObject.SetActive(false);
                    hideObj.SetActive(true);
                    btnImg.sprite = btnSprite[2];   //GREEN
                    claimText.color = new Color32(0, 82, 0, 255);
                    break;
                case DailyRewardState.CLAIMED:
                    lableImg.sprite = lableSprite[0];
                    cointextImg.SetActive(false);
                    claimText.text = "CLAIMED";
                    claimText.gameObject.SetActive(true);
                    hideObj.SetActive(true);
                    btnImg.sprite = btnSprite[0];  //RED
                    claimText.color = Color.white;
                    break;
            }
        }
    }
}