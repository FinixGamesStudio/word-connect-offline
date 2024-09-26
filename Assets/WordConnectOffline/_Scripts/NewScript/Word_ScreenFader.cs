using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace WordConnectByFinix
{
    public class Word_ScreenFader : MonoBehaviour
    {
        public static Word_ScreenFader instance;
        public const float DURATION = 0.7f;
        public Image panel;
        public GameObject logo;

        private void Awake()
        {
            Debug.Log("Instance is This");
            //if (instance == null)
            //    instance = this;
        }

        public void FadeInLoading(Action onComplete)
        {
            panel.gameObject.SetActive(true);
            logo.gameObject.SetActive(true);
            panel.DOFade(1, DURATION).OnComplete(() =>
            {
                if (onComplete != null) onComplete();
            });
        }

        public void FadeOutLoading(Action onComplete)
        {
            logo.gameObject.SetActive(false);
            panel.DOFade(0, DURATION).OnComplete(() =>
            {
                if (onComplete != null) onComplete();
                //panel.gameObject.SetActive(false);
            });
        }
        public void GotoScene(int sceneIndex)
        {
            FadeInLoading(() =>
            {
                Word_Utils.LoadScene(sceneIndex);
            });
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            FadeOutLoading(null);
            Invoke(nameof(DisablePanel), 0.1f);
        }

        void DisablePanel() => panel.gameObject.SetActive(false);
    }
}