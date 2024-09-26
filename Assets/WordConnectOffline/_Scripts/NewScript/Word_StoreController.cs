using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_StoreController : MonoBehaviour
    {

        public bool UseFakeStore;
        public int purchaseCoins;
        public RectTransform storeScreen;
        public GameObject successBlackScreen, successPopUp,failedBalckScreen,failedPopUp;
        public Text successText;
        public Image adNotAvailable;
        public List<Word_PurcaseProductController> allIapBtns;

        private void Start()
        {
            SetPanel.MoveRightSide(storeScreen, ConfigController.instance.canvasWidth);
        }

        public void PurchaseSsuccess(int quntity)
        {
            int coins = purchaseCoins * quntity;
            Debug.Log($"purchase Coins - {purchaseCoins} <-> Quentity - {quntity} <-> TotalCoins - {coins}");
            CurrencyController.CreditBalance(coins);
            OpenSuccessPopUp(coins);
        }

        public void SetData(IStoreController controller)
        {
            for (int i = 0; i < controller.products.all.Length; i++)
            {
                allIapBtns[i].SetPrefabData(controller.products.all[i].definition.id, controller);
            }
        }

        bool checkScene()
        {
            return SceneManager.GetActiveScene().buildIndex == 1;
        }

        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            
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

        public void ClickOnAdBtn()
        {
            if (ConfigController.instance.IsInternetAwailable())
                Word_AdmobController.instance.ShowRewardedAd(0);
            else
                ConfigController.instance.noInternetController.OpenNoInternetPopUp();
        }

        public void OpenPurchaseFailedPopUp()
        {
            SetPopUp.OpenPopUp(failedBalckScreen, failedPopUp);
        }

        public void ClosePurchaseFailPopUp()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPopUp.ClosePopUp(failedBalckScreen, failedPopUp);
        }

        public void OpenSuccessPopUp(int coins)
        {
            successText.text = "Congratulations you have bought " + coins + " coins !!";
            SetPopUp.OpenPopUp(successBlackScreen, successPopUp);
        }

        public void CloseSuccessPopUp()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPopUp.ClosePopUp(successBlackScreen,successPopUp);
        }

        public void OpenStore()
        {
            if (Word_InitializeIAP.instance.isInializeIAP)
            {
                if (checkScene()) Word_MainController.instance.isDialogActive = true;
                SetPanel.MoveMiddle(storeScreen);
            }
            else
                ConfigController.instance.noInternetController.OpenNoInternetPopUp();
        }
        public void CloseStore()
        {
            if (checkScene()) Word_MainController.instance.isDialogActive = false;
            ConfigController.instance.soundController.PlayButton();

            SetPanel.MoveLeftSide(storeScreen,ConfigController.instance.canvasWidth);
        }
    }
}