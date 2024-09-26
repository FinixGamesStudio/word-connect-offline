using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text;

namespace WordConnectByFinix
{
    public class Word_RandomWordsController : MonoBehaviour
    {
        private int numLetters;
        private string word, panWord;
        private GameLevel gameLevel;
        private const float RADIUS = 90;
        private List<Vector3> letterPositions = new List<Vector3>();
        private List<Vector3> letterLocalPositions = new List<Vector3>();
        private List<Word_LetterController> letterTexts = new List<Word_LetterController>();
        private List<int> indexes = new List<int>();

        private int /*world, subWorld,*/ level;

        public Transform centerPoint;
        public Word_TextPreview textPreview;

        //public static Word_RandomWordsController instance;

        private void Awake()
        {
           // instance = this;
        }

        private void Start()
        {
            //world = Word_GameState.currentWorld;
            //subWorld = Word_GameState.currentSubWorld;
            level = Word_GameState.currentLevel;
        }

        public void Load(GameLevel gameLevel)
        {
            this.gameLevel = gameLevel;
            numLetters = gameLevel.word.Trim().Length;

            float delta = 360f / numLetters;

            float angle = 90;
            for (int i = 0; i < numLetters; i++)
            {
                float angleRadian = angle * Mathf.PI / 180f;
                float x = Mathf.Cos(angleRadian);
                float y = Mathf.Sin(angleRadian);
                Vector3 position = RADIUS * new Vector3(x, y, 0);

                letterLocalPositions.Add(position);
                letterPositions.Add(centerPoint.TransformPoint(position));

                angle += delta;
            }

            Word_MainController.instance.lineDrawer.letterPositions = letterPositions;

            for (int i = 0; i < numLetters; i++)
            {
                Word_LetterController letter = Instantiate(Word_GameManager.instance.letter);
                letter.transform.SetParent(centerPoint);
                letter.transform.localScale = Vector3.one;
                letter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10)));
                letter.letterText.text = gameLevel.word[i].ToString().ToUpper();
                letter.letterText.fontSize = ConfigController.instance.config.fontSizeInDiskMainScene;
                letterTexts.Add(letter);
            }

            indexes = Word_AllPrefs.GetPanWordIndexes(level).ToList();
            if (indexes.Count != numLetters)
            {
                indexes = Enumerable.Range(0, numLetters).ToList();
                indexes.Shuffle(level);
                Word_AllPrefs.SetPanWordIndexes(level, indexes.ToArray());
            }

            GetPanWord();

            Word_Timer.Schedule(this, 0, () =>
            {
                for (int i = 0; i < numLetters; i++)
                {
                    letterTexts[i].transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
                }
            });
        }

        private void GetShuffeWord()
        {
            List<int> origin = new List<int>();
            origin.AddRange(indexes);
            while (true)
            {
                indexes.Shuffle();
                if (!origin.SequenceEqual(indexes)) break;
            }
            GetPanWord();
        }

        private void GetPanWord()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numLetters; i++)
            {
                sb.Append(gameLevel.word[indexes[i]]);
            }
            panWord = sb.ToString();
            textPreview.word = panWord.ToUpper();
        }

        public void Shuffle()
        {
            GetShuffeWord();
            Word_AllPrefs.SetPanWordIndexes(level, indexes.ToArray());

            int i = 0;
            foreach (var text in letterTexts)
            {
                iTween.MoveTo(text.gameObject, iTween.Hash("position", letterLocalPositions[indexes.IndexOf(i)], "time", 0.15f, "isLocal", true));
                text.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10)));
                i++;
            }
            ConfigController.instance.vibrationController.Vibration();
            ConfigController.instance.soundController.PlayButton();
        }
    }
}