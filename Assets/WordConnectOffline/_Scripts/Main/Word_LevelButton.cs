using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_LevelButton : MonoBehaviour
    {
        public Text levelText;
        public int level;
        public Image background;
        public GameObject lockBtn;
        public RectTransform btnRect;

        public Sprite solvedSprite, currentSprite, lockedSprite;
        public Button levelBtn;

        public void SetlevelBtnDetails(int levelIndex)
        {
            level = levelIndex;
            levelText.text = (level + 1).ToString();
            int unlockedLevel = Word_AllPrefs.unlockedLevel;
            if (level < unlockedLevel)
            {
                levelText.color = Color.white;
                background.sprite = solvedSprite;
            }
            else if (level == unlockedLevel)
            {
                levelText.color = new Color32(0, 82, 0, 255);
                background.sprite = currentSprite;
            }
            else
            {
                levelText.color = new Color32(0, 82, 0, 255);
                levelText.gameObject.SetActive(false);
                lockBtn.SetActive(true);
                background.sprite = lockedSprite;
                levelBtn.interactable = false;
            }
        }

        public void ClickOnLockBtn()
        {
            Word_GameManager.instance.selectLevelController.OpenLockLevelToolTip();
        }

        public void OnButtonClick()
        {
            ConfigController.isFirstTimeUser = false;
            Word_GameState.currentLevel = level;

            Word_Utils.LoadScene(1, true);
            ConfigController.instance.soundController.PlayButton();

            // Set the music
            ConfigController.instance.musicController.Play(Word_Utils.GetRandom(Word_MusicController.Type.Main_0, Word_MusicController.Type.Main_1, Word_MusicController.Type.Main_2));
        }
    }
}