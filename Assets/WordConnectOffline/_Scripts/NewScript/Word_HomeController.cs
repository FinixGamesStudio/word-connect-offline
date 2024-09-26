using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WordConnectByFinix
{
    public class Word_HomeController : MonoBehaviour
    {
        public Word_GameManager gameManager;
        [Space]
        [Header("Avatar")]
        public List<Sprite> avtarSprites;
        public List<Word_ProfileSelection> avtars;
        public Image profileImgDashboard, profileScreenImg;
        public InputField userName;
        [Space]
        public RectTransform canvas;
        public Text levelText, userNameText, profileLevelText;
        public GameObject profileBlackScreen, profilePopUp, avtarPopUp,alrtObj;

        public string UserName
        {
            get { return Word_AllPlayerPrefs.GetString("user_name"); }
            set { Word_AllPlayerPrefs.SetString("user_name", value.ToUpper()); }
        }

        public int UserAvatar
        {
            get { return Word_AllPlayerPrefs.GetInt("user_avatar"); }
            set { Word_AllPlayerPrefs.SetInt("user_avatar", value); }
        }

        private void Start()
        {
            SetNameAndProfileAction();
            levelText.text = "Play Level - " + (Word_AllPrefs.unlockedLevel + 1);
            profileLevelText.text = "Level " + (Word_AllPrefs.unlockedLevel + 1);
            if (!ConfigController.isFirstTimeUser)
                Word_GameManager.instance.selectLevelController.GenerateLevels();
        }

        void OnChangeAvtarOrProfile()
        {
            if (!Word_AllPlayerPrefs.HasKey("user_name") || !Word_AllPlayerPrefs.HasKey("user_avatar"))
            { UserName = "guest"; UserAvatar = 0; avtars[UserAvatar].SetFrame(true); }
            userNameText.text = UserName;
            userName.text = UserName;
            Debug.Log($"Avtar {UserAvatar} Name {UserName}");
            profileImgDashboard.sprite = avtarSprites[UserAvatar];
            profileScreenImg.sprite = avtarSprites[UserAvatar];
            avtars[UserAvatar].SetFrame(true);
        }

        void SetNameAndProfileAction()
        {
            OnChangeAvtarOrProfile();
        }

        public void SetProfileName()
        {
            if (userName.text == "")
            {
                alrtObj.SetActive(true);
            }
            else
            {
                alrtObj.SetActive(false);
                UserName = userName.text;
                SetNameAndProfileAction();
            }
        }

        void OffAllGreenRing()
        {
            for (int i = 0; i < avtars.Count; i++)
                avtars[i].SetFrame(false);
        }

        public void ClickOnProfile(int profileIndex)
        {
            ConfigController.instance.soundController.PlayButton();
            OffAllGreenRing();
            //avtars[profileIndex].SetFrame(true);
            UserAvatar = profileIndex;
            SetNameAndProfileAction();
        }

        public void OpenAvtarScreen()
        {
            ConfigController.instance.soundController.PlayButton();
            avtarPopUp.SetActive(true);
        }

        public void CloseAvtarScreen()
        {
            ConfigController.instance.soundController.PlayButton();
            avtarPopUp.SetActive(false);
        }

        public void ClickOnProfileOpenBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            alrtObj.SetActive(false);
            userName.text = UserName;
            SetPopUp.OpenPopUp(profileBlackScreen, profilePopUp);
        }

        public void ClickOnProfileCloseBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPopUp.ClosePopUp(profileBlackScreen, profilePopUp);
        }

        public void ClickOnPlayBtn()
        {
            ConfigController.isFirstTimeUser = false;
            Word_GameState.currentLevel = Word_AllPrefs.unlockedLevel;
            Word_Utils.LoadScene(1, true);
            ConfigController.instance.soundController.PlayButton();
        }

        public void ClickOnLevelBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            gameManager.selectLevelController.OpenLevelScreen();
        }

        public void ClickOnStoreBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            ConfigController.instance.storeController.OpenStore();
        }

        public void ClickOnWheelBtn()
        {
            gameManager.fortuneWheel.OpenFortuneWheel();
        }

        public void ClickOnSettingBtn()
        {
            ConfigController.instance.settingController.ClickOnSettingBtn();
        }
    }
}