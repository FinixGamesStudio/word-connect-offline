using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


namespace WordConnectByFinix
{
    public class Word_ExtraWordManager : MonoBehaviour
    {
        public List<string> extraWords = new List<string>();
        public GameObject existMessage;
        public Transform beginPoint, endPoint;
        public GameObject lightEffect, lightOpenEffect;

        private int level;
        private CanvasGroup existMessageCG;
        private bool isMessageShowing;
        private Text flyText;
        public Text extraText;

        public static Word_ExtraWordManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            level = Word_GameState.currentLevel;

            extraWords = Word_AllPrefs.GetExtraWords(level).ToList();
            existMessage.SetActive(false);
            existMessageCG = existMessage.GetComponent<CanvasGroup>();

            UpdateUI();
        }

        private void UpdateUI()
        {
            lightOpenEffect.SetActive(Word_AllPrefs.extraProgress >= Word_AllPrefs.extraTarget);
        }

        public void ProcessWorld(string word)
        {
            if (extraWords.Contains(word))
            {
                if (isMessageShowing) return;
                isMessageShowing = true;

                ShowMessage("");
            }
            else
            {
                var middlePoint = Word_Utils.GetMiddlePoint(beginPoint.position, endPoint.position, 0.4f);
                Vector3[] waypoint = { beginPoint.position, middlePoint, endPoint.position };

                flyText = Instantiate(extraText);
                flyText.text = word;
                //  flyText.fontSize = 12;
                flyText.transform.position = beginPoint.position;
                flyText.transform.SetParent(Word_MainController.instance.textFlyTransform);
                flyText.transform.localScale = Word_MainController.instance.textPreview.text.transform.localScale;
                iTween.MoveTo(flyText.gameObject, iTween.Hash("path", waypoint, "time", 0.3f, "oncomplete", "OnTextMoveToComplete", "oncompletetarget", gameObject));
                ConfigController.instance.vibrationController.Vibration();
                AddNewExtraWord(word);
            }
        }

        private void ShowMessage(string message)
        {
            existMessage.SetActive(true);
            existMessageCG.alpha = 0;
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageShowComplete"));
        }

        public void AddNewExtraWord(string word)
        {
            extraWords.Add(word);
            Word_AllPrefs.SetExtraWords(level, extraWords.ToArray());
            Word_AllPrefs.extraProgress++;
            Word_AllPrefs.totalExtraAdded++;
            Debug.Log($"Extra Process -- {Word_AllPrefs.extraProgress} *** Total Extra Words -- {Word_AllPrefs.totalExtraAdded}");
        }

        private void OnMessageUpdate(float value)
        {
            existMessageCG.alpha = value;
        }

        private void OnMessageShowComplete()
        {
            Word_Timer.Schedule(this, 0.5f, () =>
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageHideComplete"));
            });
        }

        private void OnMessageHideComplete()
        {
            isMessageShowing = false;
        }

        private void OnTextMoveToComplete()
        {
            UpdateUI();

            if (!lightOpenEffect.activeSelf)
            {
                lightEffect.SetActive(true);
                iTween.RotateAdd(lightEffect, iTween.Hash("z", -60, "time", 0.4f, "oncomplete", "OnLightRotateComplete", "oncompletetarget", gameObject));
            }

            flyText.CrossFadeAlpha(0, 0.3f, true);
            Destroy(flyText.gameObject, 0.3f);
        }

        private void OnLightRotateComplete()
        {
            lightEffect.SetActive(false);
        }

        public void OnClaimed()
        {
            UpdateUI();
        }
    }
}