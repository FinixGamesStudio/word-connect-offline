using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_SelectLevelController : MonoBehaviour
    {
        public Word_GameManager gameManager;
        public List<string> titleName;
        public List<Word_LevelButton> allLevelBtns;
        public List<Word_LevelPopUpController> totalLevelPopUp;
        public int levelIndex;
        public Word_LevelButton levelButton;
        public Word_LevelPopUpController levelPopUpController;
        public Transform levelParent;
        public int totalLevels;
        public ScrollRect scrollRect;
        public int currLevelIndex;
        RectTransform currLevelTransform;
        public RectTransform levelCanvasTrans, levelPanelRect;
        public Image lockLevelToolTip;

        private void Start()
        {
            SetPanel.MoveRightSide(levelPanelRect, ConfigController.instance.canvasWidth);
        }

        public void GenerateLevels()
        {
            if (totalLevels >= allLevelBtns.Count)
            {
                Word_LevelPopUpController word_LevelPopUp = null;
                word_LevelPopUp = Instantiate(levelPopUpController);
                word_LevelPopUp.transform.SetParent(levelParent, false);
                word_LevelPopUp.levelIndex.text = "LEVEL : " + (word_LevelPopUp.transform.GetSiblingIndex() + 1);
                totalLevelPopUp.Add(word_LevelPopUp);

                for (int i = 0; i < 5; i++)
                {
                    Word_LevelButton level = Instantiate(levelButton);
                    level.transform.SetParent(word_LevelPopUp.levelBtnParent, false);
                    word_LevelPopUp.parentLevelBtn.Add(level);
                    allLevelBtns.Add(level);
                    if (i == 4)
                        GenerateLevels();
                }
            }
            if ((totalLevels + 5) == allLevelBtns.Count)
                SetLevel();
        }

        public void SetLevel()
        {
            for (int i = 0; i < allLevelBtns.Count; i++)
            {
                allLevelBtns[i].SetlevelBtnDetails(i);
                if (Word_AllPrefs.unlockedLevel == allLevelBtns[i].level)
                    currLevelTransform = allLevelBtns[i].transform.parent.transform.parent.GetComponent<Word_LevelPopUpController>().levelRect;
            }
        }

        public void OpenLockLevelToolTip()
        {
            lockLevelToolTip.gameObject.SetActive(true);
            lockLevelToolTip.DOFade(1, 0.7f).OnComplete(() =>
            {
                lockLevelToolTip.DOFade(0, 0.6f).SetDelay(0.2f).OnComplete(() =>
                {
                    lockLevelToolTip.gameObject.SetActive(false);
                });
            });
        }

        public static void BringChildIntoView(ScrollRect instance, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            instance.content.DOLocalMove(result, 1f, false);
            instance.horizontalNormalizedPosition = Mathf.Clamp(instance.horizontalNormalizedPosition, 0f, 1f);
            instance.verticalNormalizedPosition = Mathf.Clamp(instance.verticalNormalizedPosition, 0f, 1f);
        }
        public void OpenLevelScreen()
        {
            SetPanel.MoveMiddle(levelPanelRect);
            SetTitalName();
            if (Word_AllPrefs.unlockedLevel > 26)
                BringChildIntoView(scrollRect, currLevelTransform);
        }

        void SetTitalName()
        {
            for (int i = 0; i < totalLevelPopUp.Count; i++)
                totalLevelPopUp[i].titleText.text = titleName[i].ToUpper();
        }

        public void CloseLevelScreen()
        {
            ConfigController.instance.soundController.PlayButton();
            SetPanel.MoveLeftSide(levelPanelRect, ConfigController.instance.canvasWidth);
        }
    }
}