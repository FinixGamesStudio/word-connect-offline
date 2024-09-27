using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_MainController : MonoBehaviour
    {
        public static Word_MainController instance;
        public Text levelNameText;
        public Text hintCostText;

        private int level;
        private bool isGameComplete;
        private GameLevel gameLevel;
        public Transform textFlyTransform, settingBtnIcon;
        public Word_MusicController.Type music = Word_MusicController.Type.None;
        public bool isDialogActive = false;
        public Image adNotAvailable;

        [Header("COntrollers")]
        public Word_RandomWordsController randomWordsController;
        public Word_WordRegion wordRegion;
        public Word_LineDrawer lineDrawer;
        public Word_TextPreview textPreview;
        public Word_ExitPopUpController exitPopUpController;
        public Word_WinningController winningController;
        public Word_ExtraWordController extraWordController;
        public Word_HowToPlayController howToPlayController;
        public Word_MeaningFindController meaningFindController;
        public Word_DictionaryScreenController dictionaryScreenController;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        void Start()
        {
            SetLevel();
            hintCostText.text = Word_CellConst.HINT_COST.ToString();
            ConfigController.instance.musicController.Play(music);

            //Invoke(nameof(StartBannerAd), 0.8f);
        }

        void StartBannerAd() => Word_AdmobController.instance.LoadBannerAd();

        public void SetLevel()
        {
            level = Word_GameState.currentLevel;

            gameLevel = Utils.Load(level);
            randomWordsController.Load(gameLevel);
            wordRegion.Load(gameLevel);

            if (level == 0)
            {
                Word_Timer.Schedule(this, 0.3f, () =>
                {
                    OpenHowToPlayPopUp();
                });
            }

            Debug.Log(Word_AllPrefs.unlockedLevel + " -- Current Level Index");
            levelNameText.text = $"Level - {Word_GameState.currentLevel + 1}";
        }

        public void OnComplete()
        {
            if (isGameComplete) return;
            isGameComplete = true;

            Word_Timer.Schedule(this, 1f, () =>
            {
                winningController.OpenWinningPopUp();
                ConfigController.instance.soundController.Play(Word_SoundController.Others.Win);
            });
        }

        public void OpenExitPopUp()
        {
            ConfigController.instance.soundController.PlayButton();
            exitPopUpController.OpenExitPopUp();
        }

        public void OpenStoreScreen()
        {
            ConfigController.instance.soundController.PlayButton();
            ConfigController.instance.storeController.OpenStore();
        }

        public void ClickOnDictionaryBtn() => dictionaryScreenController.OpenDictionryPopUp();

        public void OpenExtraWordPopUp()
        {
            ConfigController.instance.soundController.PlayButton();
            extraWordController.OpenExtaWod();
        }

        public void OpenHowToPlayPopUp() => howToPlayController.OpenHowToPlayPopUp();

        public void ClickOnSettingBtn() => ConfigController.instance.settingController.ClickOnSettingBtn();

        public void ClickOnShowAdBtn()
        {
            try
            {
                Debug.Log(Word_AdmobController.instance);
                Word_AdmobController.instance.ShowRewardedAd(1);

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                throw;
            }
        }

        public void AdNotAvailableTost()
        {
            adNotAvailable.gameObject.SetActive(true);
            adNotAvailable.DOFade(1, 0.7f).OnComplete(() =>
            {
                adNotAvailable.DOFade(0, 0.1f).SetDelay(0.5f).OnComplete(() =>
                {
                    adNotAvailable.gameObject.SetActive(false);
                });
            });
        }
    }
}