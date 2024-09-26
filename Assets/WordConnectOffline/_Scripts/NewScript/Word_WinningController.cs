using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WordConnectByFinix
{
    public class Word_WinningController : MonoBehaviour
    {
        public Image fillSlider;
        public Text setLastLevelText, giftCoinText, currentLevelText, nextLevelText;
        public Transform coinHolder;
        public RectTransform winningScreen;
        int intForLevelIndex = 0;
        float forSliderValue = 0;
        public GameObject giftObj, previousObj;

        public int giftCoin;
        public Transform giftImg;

        public Transform startPosition, endPosition;
        public GameObject coinPrefab;
        public Button nextBtn;

        List<GameObject> coinList = new List<GameObject>();

        private int numLevels;
        private bool isLastLevel;
        private int level;


        private void Start()
        {
            SetPanel.MoveRightSide(winningScreen, ConfigController.instance.canvasWidth);
        }


        public void OpenWinningPopUp()
        {
            //Word_AdmobController.instance.DestroyBannerView();
            currentLevelText.text = (Word_GameState.currentLevel + 1).ToString();
            giftObj.SetActive(true);
            previousObj.SetActive(true);
            if (Word_GameState.currentLevel >= Word_AllPrefs.unlockedLevel)
            {
                previousObj.SetActive(false);
                OpenGiftReward();
                CheckUnlock();
            }
            else
            {
                giftObj.SetActive(false);
                Word_GameState.currentLevel++;
                int t = Word_GameState.currentLevel;
                nextLevelText.text = $"Level {t + 1}";
            }
            SetPanel.MoveMiddle(winningScreen);
            ConfigController.instance.vibrationController.Vibration();
        }

        void OpenGiftReward()
        {
            Invoke(nameof(SetSlidetData), 0.1f);
            forSliderValue = LastSliderValue();
            intForLevelIndex = LastLevel();
            fillSlider.DOFillAmount(forSliderValue, 0.2f);
            setLastLevelText.text = intForLevelIndex + " / 5";
            Word_MainController.instance.lineDrawer.DisableLine();
            Word_MainController.instance.isDialogActive = true;
            InvokeRepeating(nameof(GiftAnimation), 0.1f, 4.01f);
            nextBtn.interactable = false;
        }

        private void CheckUnlock()
        {
            level = Word_GameState.currentLevel;
            Debug.Log(Word_GameState.currentLevel);

            isLastLevel = Word_AllPrefs.IsLastLevel();

            Word_GameState.currentLevel++;
            Debug.Log(Word_GameState.currentLevel + " --Current Level-");

            if (isLastLevel)
                Word_AllPrefs.unlockedLevel = Word_GameState.currentLevel;
        }

        public void NextClick()
        {
            if (ConfigController.instance.IsInternetAwailable())
                Word_AdmobController.instance.ShowInterstitialAd();
            else
                SetNextLevel();
        }

        public void SetNextLevel()
        {
            ConfigController.instance.soundController.PlayButton();
            Word_Utils.LoadScene(level == numLevels - 1 ? 1 : 1, true);
        }

        // Go For Menu
        public void MenuClick()
        {
            ConfigController.instance.soundController.PlayButton();
            Word_Utils.LoadScene(level == numLevels - 1 ? 1 : 0, true);
        }

        void GiftAnimation()
        {
            giftImg.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2f).OnComplete(() =>
            {
                giftImg.DOScale(Vector3.one, 2f);
            });
        }

        public void SetSlidetData()
        {
            // for slider
            forSliderValue = (forSliderValue + 0.2f);
            fillSlider.DOFillAmount(forSliderValue, 0.4f).OnComplete(() =>
            {
                nextBtn.interactable = true;
            });
            SetSliderPrefs(forSliderValue);

            // for text
            intForLevelIndex++;
            setLastLevelText.text = intForLevelIndex + " / 5";
            SetLastLevelInt(intForLevelIndex);

            if (intForLevelIndex == 5)
                ResetSliderAndGiveReward();
        }

        void ResetSliderAndGiveReward()
        {
            SetLastLevelInt(0); SetSliderPrefs(0);
            Invoke(nameof(StartReward), 0.2f);
        }

        void StartReward()
        {
            coinHolder.gameObject.SetActive(true);
            coinHolder.DOScale(new Vector3(7.5f, 7.5f, 7.5f), 0.01f);
            coinHolder.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f).OnComplete(() =>
            {
                AnimateNumber(0, giftCoin, 1f, giftCoinText);
            });
        }

        float LastSliderValue()
        {
            return Word_AllPlayerPrefs.GetFloat("last_slider");
        }
        void SetSliderPrefs(float slider) => Word_AllPlayerPrefs.SetFloat("last_slider", slider);

        int LastLevel()
        {
            return Word_AllPlayerPrefs.GetInt("last_level");
        }
        void SetLastLevelInt(int level) => Word_AllPlayerPrefs.SetInt("last_level", level);


        public void AnimateNumber(int startNumber, int targetNumber, float duration, Text coinText)
        {
            DOTween.To(() => startNumber, x =>
            {
                startNumber = x;
                coinText.text = startNumber.ToString();
            }, targetNumber, duration).OnComplete(() =>
            {
                setLastLevelText.text = 0 + " / 5";
                fillSlider.DOFillAmount(0, 0.3f);
                CurrencyController.CreditBalance(targetNumber);
                Word_CoinAnimController.instance.StartCoinAnimation(startPosition, endPosition);
                coinHolder.gameObject.SetActive(false);
            });
        }
    }
}