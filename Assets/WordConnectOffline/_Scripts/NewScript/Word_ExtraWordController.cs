using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_ExtraWordController : MonoBehaviour
    {
        public Button claimButton;
        public Text progressText;
        public float current, target;
        private int claimQuantity;
        public RectTransform extraWordScreen;
        public Transform claimTr;
        public Image fillImg;
        public Transform animStartTrans, animEndTrans;

        private void Start()
        {
            SetPanel.MoveRightSide(extraWordScreen, ConfigController.instance.canvasWidth);
        }

        public void OpenExtaWod()
        {
            Word_MainController.instance.isDialogActive = true;
            target = Word_AllPrefs.extraTarget;
            current = Word_AllPrefs.extraProgress;
            Debug.Log($"Target -- {Word_AllPrefs.extraTarget} *** Current -- {Word_AllPrefs.extraProgress}");
            UpdateUI();
            SetPanel.MoveMiddle(extraWordScreen);
        }

        public void ClaimExtraWord()
        {
            claimQuantity = (int)target / 5 * 2;
            current -= (int)target;
            Word_AllPrefs.extraProgress = (int)current;
            UpdateUI();
            StartCoroutine(ClaimEffect());
            Word_ExtraWordManager.instance.OnClaimed();
            if (Word_AllPrefs.extraTarget == 5 && Word_AllPrefs.totalExtraAdded > 10)
            {
                Word_AllPrefs.extraTarget = 10;
                target = 10;
                UpdateUI();
            }
            ConfigController.instance.soundController.PlayButton();
        }

        private IEnumerator ClaimEffect()
        {
            yield return new WaitForSeconds(0.1f);
            SetPanel.MoveLeftSide(extraWordScreen, ConfigController.instance.canvasWidth);
            Word_MainController.instance.isDialogActive = false;
            Word_CoinAnimController.instance.StartCoinAnimation(animStartTrans, animEndTrans);
            CurrencyController.CreditBalance(5);
            //Word_MainController.instance.ShowHintCoin();
        }

        public void FillSlider(float fillAmount)
        {
            fillImg.fillAmount = fillAmount;
        }

        private void UpdateUI()
        {
            claimButton.interactable = (current >= target);
            progressText.text = current + " / " + target;
            FillSlider(current / target);
        }

        public void CloseExtraWordPopUp()
        {
            Word_MainController.instance.isDialogActive = false;
            ConfigController.instance.soundController.PlayButton();
            SetPanel.MoveLeftSide(extraWordScreen, ConfigController.instance.canvasWidth);
        }
    }
}