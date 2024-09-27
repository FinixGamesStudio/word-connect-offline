using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace WordConnectByFinix
{
    public class Word_InitializeIAP : MonoBehaviour, IStoreListener
    {
        public static Word_InitializeIAP instance;

        public bool isInializeIAP = false;
        public List<string> productId;

        private IStoreController storeController;
        private IExtensionProvider extensionProvider;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //Initialize();
        }

        public void Initialize()
        {
            //if (!isInializeIAP)
            //{
            //    var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            //    for (int i = 0; i < productId.Count; i++)
            //    {
            //        builder.AddProduct(productId[i], ProductType.Consumable, new IDs
            //        {
            //          {productId[i], GooglePlay.Name},
            //        });
            //    }
            //    UnityPurchasing.Initialize(this, builder);
            //}
        }
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            //storeController = controller;
            //extensionProvider = extensions;
            //isInializeIAP = true;
            //Debug.Log($"IAP Initialized Done With {storeController.products.all.Length} Products");

            //ConfigController.instance.storeController.SetData(controller);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            isInializeIAP = false;
            Debug.Log($"IAP Initialized Failed With Error - {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log($"IAP Initialized Failed With Error - {error}  -*-  Message - {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            ConfigController.instance.storeController.OpenPurchaseFailedPopUp();
            Debug.Log($"IAP Purchase Failed With Product - {product}  -*-  Reason - {failureReason}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Purchase Done => {purchaseEvent.purchasedProduct.receipt}");
            UnifiedReceipt unifiedReceipt = JsonConvert.DeserializeObject<UnifiedReceipt>(purchaseEvent.purchasedProduct.receipt);
            InAppSucessPayload inAppSucessPayload = JsonConvert.DeserializeObject<InAppSucessPayload>(unifiedReceipt.Payload);
            string successPayloadString = inAppSucessPayload.json.Replace(@"\", string.Empty);
            PurchaseDetailPayload purchaseDetailPayload = JsonConvert.DeserializeObject<PurchaseDetailPayload>(successPayloadString);
            ConfigController.instance.storeController.PurchaseSsuccess(purchaseDetailPayload.quantity);
            return PurchaseProcessingResult.Complete;
        }
        string ExtractFirstNumericValue(string input)
        {
            string[] t = input.Split('.');
            string numericString = new string(t[0].Where(char.IsDigit).ToArray());
            return numericString;
        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [System.Serializable]
    public class InAppSucessPayload
    {
        public string json;
        public string signature;
        public List<string> skuDetails;
    }
    [System.Serializable]
    public class PurchaseDetailPayload
    {
        public string orderId;
        public string packageName;
        public string productId;
        public long purchaseTime;
        public int purchaseState;
        public string purchaseToken;
        public int quantity;
        public bool acknowledged;
    }
}