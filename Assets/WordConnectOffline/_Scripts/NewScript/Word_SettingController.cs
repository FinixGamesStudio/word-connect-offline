using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_SettingController : MonoBehaviour
    {
        public Sprite onSprite, offSprite;
        public Image soundToggleImg, musicToggleImg, vibrationToggleImg;
        public GameObject settingPopUp, resetBtn;
        public string privacyUri;
        bool isSettingOpen;
        bool isSoundOn;
        bool isMusicOn;
        bool isVibrationOn;

        private void Start()
        {
            Invoke(nameof(SetSound), 0.1f);
        }
        public void SetSound()
        {
            IsSoundOn = ConfigController.instance.soundController.IsEnabled();
            IsMusicOn = ConfigController.instance.musicController.IsEnabled();
            IsVibrationOn = ConfigController.instance.vibrationController.IsEnabled();
        }

        public bool IsSoundOn
        {
            get { return isSoundOn; }
            set
            {
                isSoundOn = value;
                soundToggleImg.sprite = isSoundOn ? onSprite : offSprite;
            }
        }

        public bool IsMusicOn
        {
            get { return isMusicOn; }
            set
            {
                isMusicOn = value;
                musicToggleImg.sprite = isMusicOn ? onSprite : offSprite;
            }
        }
        public bool IsVibrationOn
        {
            get { return isVibrationOn; }
            set
            {
                isVibrationOn = value;
                vibrationToggleImg.sprite = isVibrationOn ? onSprite : offSprite;
            }
        }

        public void ClickOnSettingBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            isSettingOpen = !isSettingOpen;
            if (SceneManager.GetActiveScene().buildIndex == 1) resetBtn.gameObject.SetActive(false);
            else resetBtn.gameObject.SetActive(true);
            if (isSettingOpen)
            {
                gameObject.SetActive(true);
                settingPopUp.transform.DOScale(Vector3.one, 0.2f);
            }
            else
            {
                settingPopUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
            }
        }

        public void ClickOnResetBtn()
        {
            ConfigController.instance.ResetGame();
            isSettingOpen = false;
            settingPopUp.transform.DOScale(Vector3.zero, 0f).OnComplete(() => gameObject.SetActive(false));
            Invoke(nameof(ReloadScene), 0.2f);
        }

        void ReloadScene()
        {
            Word_Utils.LoadScene(0, true);
        }

        public void ClickRateUsBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            //Application.OpenURL(rateUsUri);
        }

        public void ClickOnPrivacyBtn()
        {
            ConfigController.instance.soundController.PlayButton();
            Application.OpenURL(privacyUri);
        }

        public void ClickOnSoundToggle()
        {
            IsSoundOn = !IsSoundOn;
            ConfigController.instance.soundController.PlayButton();
            ConfigController.instance.soundController.SetEnabled(IsSoundOn);
        }

        public void ClickOnMusicToggle()
        {
            IsMusicOn = !IsMusicOn;
            ConfigController.instance.soundController.PlayButton();
            ConfigController.instance.musicController.SetEnabled(IsMusicOn, true);
        }

        public void ClickOnVibrationToggle()
        {
            IsVibrationOn = !IsVibrationOn;
            ConfigController.instance.vibrationController.SetEnabled(IsVibrationOn);
        }
    }
}