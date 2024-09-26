using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WordConnectByFinix
{
    public class Word_AdmobController : MonoBehaviour
    {
        public static Word_AdmobController instance;

        public bool isInitializeAdmob;

        [Header("Banner")]
        public string androidBannerId = "ca-app-pub-5918737477932362/7183683653";
        public AdPosition bannerPosition;
        BannerView bannerView;

        [Header("Interstitial")]
        public string androidInterstitial = "ca-app-pub-5918737477932362/5890783285";
        InterstitialAd interstitialAd;

        [Header("RewardedVideo")]
        public string androidRewarded = "ca-app-pub-5918737477932362/5870601988";
        private RewardedAd rewardedAd;


        private void Awake()
        {
            instance = this;
        }
        public void Start()
        {
            Invoke(nameof(InitializeAdmob), 0.4f);
        }

        public void InitializeAdmob()
        {
            if (ConfigController.instance.IsInternetAwailable())
            {
                MobileAds.RaiseAdEventsOnUnityMainThread = true;
                MobileAds.Initialize((InitializationStatus initStatus) =>
                {
                    isInitializeAdmob = true;
                    LoadRewardedAd();
                    LoadInterstitialAd();
                });
            }
        }

        #region Banner
        public void LoadBannerAd()
        {
            if (ConfigController.instance.IsInternetAwailable())
            {
                if (bannerView == null)
                    CreateBannerView();
                var adRequest = new AdRequest();
                Debug.Log("Loading banner ad.");
                bannerView.LoadAd(adRequest);
            }
            else
            {
                InitializeAdmob();
            }
        }
        void CreateBannerView()
        {
            Debug.Log("Creating banner view");
            if (bannerView != null)
                DestroyBannerView();
            bannerView = new BannerView(androidBannerId, AdSize.Banner, bannerPosition);
        }

        public void DestroyBannerView()
        {
            if (bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                bannerView.Destroy();
                bannerView = null;
            }
        }
        #endregion

        #region Interstitial

        public void LoadInterstitialAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            var adRequest = new AdRequest();

            InterstitialAd.Load(androidInterstitial, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                    InterstitialAdReloadHandler(interstitialAd);
                });
        }

        public void ShowInterstitialAd()
        {
            if (ConfigController.instance.IsInternetAwailable())
            {
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();
                }
                else
                {
                    Debug.LogError("Interstitial ad is not ready yet.");
                    CloseInterstialAd();
                    InitializeAdmob();
                }
            }
        }

        private void InterstitialAdReloadHandler(InterstitialAd interstitialAd)
        {
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");
                CloseInterstialAd();
                LoadInterstitialAd();
            };
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                CloseInterstialAd();
                LoadInterstitialAd();
            };
        }

        void CloseInterstialAd() => Word_MainController.instance.winningController.SetNextLevel();

        #endregion

        #region Reward

        public void LoadRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
            Debug.Log("Loading the rewarded ad.");
            var adRequest = new AdRequest();
            RewardedAd.Load(androidRewarded, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                    RewardAdReloadHandler(rewardedAd);
                });
        }
        int rewardStr;
        public void ShowRewardedAd(int t)
        {
            rewardStr = t;
            ConfigController.instance.OpenPreloadeObj();
            Invoke(nameof(ShowReward), 1f);
        }

        void ShowReward()
        {
            ConfigController.instance.ClosePreloadeObj();
            if (ConfigController.instance.IsInternetAwailable())
            {
                if (rewardedAd != null && rewardedAd.CanShowAd())
                {
                    rewardedAd.Show((Reward reward) =>
                    {
                        Debug.Log("Give reward to player !!");
                        SetReWard();
                    });
                }
                else
                {
                    ConfigController.instance.ClosePreloadeObj();
                    if (SceneManager.GetActiveScene().buildIndex == 1)
                        Word_MainController.instance.AdNotAvailableTost();
                    else
                    {
                        if (rewardStr == 2)
                            Word_GameManager.instance.fortuneWheel.ShowMessage("Ad not Available !!");
                        else
                            ConfigController.instance.storeController.AdNotAvailableTost();
                    }
                    InitializeAdmob();
                }
            }
            else
            {
                ConfigController.instance.ClosePreloadeObj();
                ConfigController.instance.noInternetController.OpenNoInternetPopUp();
            }
        }

        void SetReWard()
        {
            Debug.Log("Set Reward -- " + rewardStr);
            int coin = 0; bool isSpin = false;
            if (rewardStr == 1)
            {
                isSpin = false;
                coin = 12;
            }
            else if (rewardStr == 2)
            {
                isSpin = true;
            }
            else
            {
                isSpin = false;
                coin = 53;
            }
            if (!isSpin)
            {
                ConfigController.instance.OpenAdRewardPopUp(coin);
                CurrencyController.CreditBalance(coin);
            }
            else
            {
                Word_GameManager.instance.fortuneWheel.ClickOnSpinBtn();
            }
        }

        private void RewardAdReloadHandler(RewardedAd ad)
        {
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");
                LoadRewardedAd();
            };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedAd();
            };
        }


        #endregion
    }
}