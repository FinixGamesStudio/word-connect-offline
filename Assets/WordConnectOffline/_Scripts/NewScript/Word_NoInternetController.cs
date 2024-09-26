using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_NoInternetController : MonoBehaviour
    {
        public GameObject popUp;
        public Text noNetText;
        public Button closeBtn;

        private void OnEnable() => Word_MainController.instance.isDialogActive = true;
        private void OnDisable() => Word_MainController.instance.isDialogActive = false;

        public void OpenNoInternetPopUp() => SetPopUp.OpenPopUp(this.gameObject, popUp);

        public void CloseNoInternetPopUp() => SetPopUp.ClosePopUp(this.gameObject, popUp);
    }
}