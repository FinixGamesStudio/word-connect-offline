using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_GameManager : MonoBehaviour
    {
        public static Word_GameManager instance;
        public GameObject donotDestroyOnLoad;
        [Header("Utils")]
        public Word_LetterController letter;
        public Word_CellController cell;
        public Word_LineWord lineWord;

        public Word_MusicController.Type music = Word_MusicController.Type.None;

        public int levelIndex = 0;
        public Text versionText;

        [Space(20)]
        public Word_HomeController homeController;
        public Word_SelectLevelController selectLevelController;
        public Word_StoreController storeController;
        public Word_FortuneWheel fortuneWheel;
        public GameObject dailyBonusScreen;

        public static bool isResetGame = false;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            versionText.text = Application.version;
            Word_AllPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);
        }

        private void Start()
        {
            Word_AllPlayerPrefs.Save();
            ConfigController.instance.musicController.Play(music);
        }
       
        private void OnApplicationPause(bool pause)
        {
            Debug.Log("On Application Pause");
            Word_AllPlayerPrefs.Save();
        }
    }
}