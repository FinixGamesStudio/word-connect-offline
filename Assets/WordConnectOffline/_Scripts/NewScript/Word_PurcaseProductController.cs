using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_PurcaseProductController : MonoBehaviour
    {
        public Text titleText;
        public Text descriptionText;
        public Text priceText;
        public string productID;
        private Product iapProduct;
        private IStoreController controller;
        public int coins;


        public void SetPrefabData(string ID, IStoreController storeController)
        {
            controller = storeController;
            productID = ID;
            iapProduct = storeController.products.WithID(productID);
            if (iapProduct != null && iapProduct.availableToPurchase)
            {
                titleText.text = coins.ToString();
                Debug.Log($" {iapProduct.metadata.localizedDescription}");
                descriptionText.text = iapProduct.metadata.localizedDescription;
                priceText.text = iapProduct.metadata.localizedPriceString;
            }

        }
        public void OnPurchaseClicked()
        {
            if (IsInternetAvailable())
            {
                ConfigController.instance.noInternetController.OpenNoInternetPopUp();
            }
            else
            {
                if (ConfigController.instance.storeController.UseFakeStore) // Use bool in editor to control fake store behavior.
                {
                    StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser; // Comment out this line if you are building the game for publishing.
                    StandardPurchasingModule.Instance().useFakeStoreAlways = true; // Comment out this line if you are building the game for publishing.
                }
                ConfigController.instance.storeController.purchaseCoins = coins;
                controller.InitiatePurchase(productID);
            }
        }
        bool IsInternetAvailable()
        {
            // if internet is available then return false
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
    }
}
