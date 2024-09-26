using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using DG.Tweening;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class ConfigController : MonoBehaviour
    {
        public static ConfigController instance;
        public static bool isFirstTimeUser = true;
        public float canvasWidth;
        public Word_GameConfig config;
        public GameObject adSuccessBg, adsuccessPopUp;
        public Text adSuccessText;
        public GameObject preloadeObj;
        public Word_ScreenFader screenFader;
        public Word_SoundController soundController;
        public Word_MusicController musicController;
        public Word_VibrationController vibrationController;
        public Word_NoInternetController noInternetController;
        public Word_StoreController storeController;
        public Word_SettingController settingController;

        public List<GameLevel> gameLevels;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            GetCanvasWidth();
        }

        public bool IsInternetAwailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        void GetCanvasWidth() => canvasWidth = Word_GameManager.instance.homeController.canvas.rect.width;

        public void OpenAdRewardPopUp(int coin)
        {
            adSuccessText.text = $"You Got {coin} Coins !!";
            adSuccessBg.SetActive(true);
            adsuccessPopUp.transform.DOScale(Vector3.one, 0.2f);
        }

        public void CloaseAdRewardPopUp()
        {
            soundController.PlayButton();
            adsuccessPopUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => adSuccessBg.SetActive(false));
        }

        public void OpenPreloadeObj() => preloadeObj.SetActive(true);
        public void ClosePreloadeObj()
        {
            Debug.Log("Close Preloader");
            preloadeObj.SetActive(false);
        }

        public void ResetGame()
        {
            soundController.PlayButton();
            isFirstTimeUser = true;
            // Word_AllPlayerPrefs.DeleteAll();
            // Word_AllPlayerPrefs.Save();
            Word_AllPrefs.unlockedLevel = 0;
            vibrationController.SetEnabled(true);
            soundController.SetEnabled(true);
            musicController.SetEnabled(true);
            settingController.SetSound();
            CurrencyController.SetBalance(10);
        }
    }
}