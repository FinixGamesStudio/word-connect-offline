//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Word_Connect
//{
//    public class Word_DialogController : MonoBehaviour
//    {
//        public static Word_DialogController instance;
//        public Word_ExitPopUpController exitPopUpController;
//        public Word_WinningController winningController;
//        public Word_ExtraWordController extraWordController;
//        public Word_HowToPlayController howToPlayController;

//        private void Awake()
//        {
//            instance = this;
//        }

//        public void OpenExitPopUp()
//        {
//            ConfigController.instance.soundController.PlayButton();
//            exitPopUpController.OpenExitPopUp();
//        }

//        public void OpenWinningPopUp()
//        {
//            Word_AdmobController.instance.DestroyBannerView();
//            ConfigController.instance.settingController.CheckPopUpTrue();
//            ConfigController.instance.vibrationController.Vibration();
//            winningController.nextBtn.interactable = false;
//            winningController.gameObject.SetActive(true);
//        }

//        public void openExtraWordPopUp()
//        {
//            ConfigController.instance.soundController.PlayButton();
//            extraWordController.gameObject.SetActive(true);
//        }

//        public void OpenHowToPlayPopUp()
//        {
//            howToPlayController.OpenHowToPlayPopUp();
//        }
//    }
//}