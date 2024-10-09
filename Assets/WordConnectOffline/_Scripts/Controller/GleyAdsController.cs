using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordConnectByFinix
{
    public class GleyAdsController : MonoBehaviour
    {
        private void Start()
        {
            Advertisements.Instance.Initialize();
        }

        private void InterstitialClosed(string advertiser)
        {
            if (Advertisements.Instance.debug)
            {
                Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
                //GleyMobileAds.ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
            }
        }

        //callback called each time a rewarded video is closed
        //if completed = true, rewarded video was seen until the end
        private void CompleteMethod(bool completed, string advertiser)
        {
            if (Advertisements.Instance.debug)
            {
                Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                //GleyMobileAds.ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                if (completed == true)
                {
                    //give the reward
                }
                else
                {
                    //no reward
                }
            }
        }

    }
}