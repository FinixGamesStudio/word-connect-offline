using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordConnectByFinix
{
    public class Word_HowToPlayController : MonoBehaviour
    {
        public GameObject howToPlayPopUp;


        private void OnEnable()
        {
            Word_MainController.instance.isDialogActive = true;
        }
        private void OnDisable()
        {
            Word_MainController.instance.isDialogActive = false;
        }
        public void OpenHowToPlayPopUp()
        {
            if (Word_AllPlayerPrefs.GetBool("is_HowToPlay", true))
            {
                gameObject.SetActive(true);
                howToPlayPopUp.transform.DOScale(Vector3.one, 0.2f);
            }
        }

        public void CloseHowToPlayPopUp()
        {
            Word_AllPlayerPrefs.SetBool("is_HowToPlay", false);
            howToPlayPopUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}