using DG.Tweening;
using UnityEngine;


namespace WordConnectByFinix
{
    public class Word_ExitPopUpController : MonoBehaviour
    {
        public GameObject exitPopUp;

        private void OnEnable()
        {
            Word_MainController.instance.isDialogActive = true;
        }
        private void OnDisable()
        {
            Word_MainController.instance.isDialogActive = false;
        }
        public void OpenExitPopUp()
        {
            gameObject.SetActive(true);
            exitPopUp.transform.DOScale(Vector3.one, 0.2f);
        }

        public void ExitYes()
        {
            Word_AdmobController.instance.DestroyBannerView();
            ConfigController.instance.soundController.PlayButton();
            Word_Utils.LoadScene(0, true);
        }

        public void ExitNo()
        {
            ConfigController.instance.soundController.PlayButton();
            exitPopUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}