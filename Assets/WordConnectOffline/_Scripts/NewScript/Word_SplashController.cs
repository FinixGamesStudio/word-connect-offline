using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


namespace WordConnectByFinix
{
    public class Word_SplashController : MonoBehaviour
    {
        public GameObject Obj;
        public Image fillImage;
        public Text percentageText;
        public Tweener loadingTextAnimation;
        public Tweener loadingImageAnimation;

        private void Awake()
        {

        }

        private void Start()
        {
            if (ConfigController.isFirstTimeUser)
            {
                //if (Word_GameManager.isResetGame)
                //{
                //    Word_GameManager.isResetGame = false;
                //    PlayerPrefs.DeleteAll();
                //    PlayerPrefs.Save();
                //    Debug.Log($"Delete All Player Prefs -- {Word_AllPrefs.unlockedLevel}");
                //}
                StartFillBar();
            }
        }

        void StartFillBar()
        {
            Obj.SetActive(true);
            loadingImageAnimation = fillImage.DOFillAmount(1f, 5).SetEase(Ease.InOutCirc).OnStart(() =>
            {
                Word_GameManager.instance.selectLevelController.GenerateLevels();
            });
            loadingTextAnimation = percentageText.DOText("100%", 5f).SetEase(Ease.InOutCirc).OnComplete(() =>
            {
                this.Obj.SetActive(false);
            });
            loadingTextAnimation.OnUpdate(() =>
            {
                float progress = loadingTextAnimation.ElapsedPercentage() * 100f;
                percentageText.text = $"{progress:F0}%";
            });
        }
    }
}