using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using DG.Tweening;

namespace WordConnectByFinix
{
    public class Word_WordRegion : MonoBehaviour
    {
        public Word_TextPreview textPreview;
        public Word_Compliment compliment;

        public List<Word_LineWord> lines = new List<Word_LineWord>();
        public List<string> validWords = new List<string>();

        private GameLevel gameLevel;
        private int numWords, numCol, numRow;
        private float cellSize, startFirstColX;
        private bool hasLongLine;
        public Image alradyWordExist;
        public int continueJoin;
        public RectTransform rt;


        private void Awake()
        {
            //rt = GetComponent<RectTransform>();
        }

        public void Load(GameLevel gameLevel)
        {
            this.gameLevel = gameLevel;

            var wordList = Word_Utils.BuildListFromString<string>(this.gameLevel.answers);
            validWords = Word_Utils.BuildListFromString<string>(this.gameLevel.validWords);
            numWords = wordList.Count;

            numCol = numWords <= 4 ? 1 :
                         numWords <= 10 ? 2 : 3;

            numRow = (int)Mathf.Ceil(numWords / (float)numCol);

            int maxCellInWidth = 0;

            for (int i = numRow; i <= numWords; i += numRow)
            {
                maxCellInWidth += wordList[i - 1].Length;
            }

            if (numWords % numCol != 0) maxCellInWidth += wordList[numWords - 1].Length;

            if (numCol > 1)
            {
                float coef = (maxCellInWidth + (maxCellInWidth - numCol) * Word_CellConst.CELL_GAP_COEF + (numCol - 1) * Word_CellConst.COL_GAP_COEF);
                cellSize = rt.rect.width / coef;
                float maxSize = rt.rect.height / (numRow + (numRow + 1) * Word_CellConst.CELL_GAP_COEF);
                if (maxSize < cellSize)
                {
                    cellSize = maxSize;
                    startFirstColX = (rt.rect.width - cellSize * coef) / 2f;
                }
            }
            else
            {
                cellSize = rt.rect.height / (numRow + (numRow - 1) * Word_CellConst.CELL_GAP_COEF + 0.8f);
                float maxSize = rt.rect.width / (maxCellInWidth + (maxCellInWidth - 1) * Word_CellConst.CELL_GAP_COEF);

                if (maxSize < cellSize)
                {
                    hasLongLine = true;
                    cellSize = maxSize;
                }
            }

            string[] levelProgress = GetLevelProgress();
            bool useProgress = false;

            if (levelProgress.Length != 0)
            {
                useProgress = CheckLevelProgress(levelProgress, wordList);
                if (!useProgress) ClearLevelProgress();
            }

            int lineIndex = 0;
            foreach (var word in wordList)
            {
                Word_LineWord line = Instantiate(Word_GameManager.instance.lineWord);
                line.answer = word.ToUpper();
                line.cellSize = cellSize;
                line.SetLineWidth();
                line.Build(ConfigController.instance.config.isWordRightToLeft);

                if (useProgress)
                    line.SetProgress(levelProgress[lineIndex]);

                line.transform.SetParent(transform);
                line.transform.localScale = Vector3.one;
                line.transform.localPosition = Vector3.zero;

                CheckWordCompeleteOrNot(line);
                lines.Add(line);
                lineIndex++;
            }
            if (completeWords.Count != 0)
                SendStartWordMeaning();
            //if (completeWords.Count != 0) Word_MainController.instance.dictionaryScreenController.DictionaryBtnAnim();
            SetLinesPosition();
        }

        public List<string> completeWords;
        public List<string> noNetWords;
        bool noNet = false;

        void CheckWordCompeleteOrNot(Word_LineWord lines)
        {
            if (lines.isShown)
                completeWords.Add(lines.answer);
        }

        public void SendStartWordMeaning()
        {
            if (completeWords.Count != 0)
            {
                for (int i = 0; i < completeWords.Count; i++)
                {
                    Debug.Log($"Complete Words - {completeWords[i]}");
                    Word_MainController.instance.meaningFindController.GetMeaning(completeWords[i]);
                }
            }
        }

        private void SetLinesPosition()
        {
            if (numCol >= 2)
            {
                float[] startX = new float[numCol];
                startX[0] = startFirstColX;

                for (int i = 1; i < numCol; i++)
                {
                    startX[i] = startX[i - 1] + lines[numRow * i - 1].lineWidth + cellSize * Word_CellConst.COL_GAP_COEF;
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    int lineX = i / numRow;
                    int lineY = numRow - 1 - i % numRow;

                    float x = startX[lineX];
                    float gapY = (rt.rect.height - cellSize * numRow) / (numRow + 1);
                    float y = (lineY + 1) * gapY + lineY * cellSize;

                    lines[i].transform.localPosition = new Vector2(x, y);
                }
            }
            else
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    float x = rt.rect.width / 2 - lines[i].lineWidth / 2;
                    float y;
                    if (hasLongLine)
                    {
                        float gapY = (rt.rect.height - numRow * cellSize) / (numRow + 1);
                        y = gapY + (cellSize + gapY) * (numRow - i - 1);
                    }
                    else
                    {
                        y = 0.4f * cellSize + (cellSize + cellSize * Word_CellConst.CELL_GAP_COEF) * (numRow - i - 1);
                    }
                    lines[i].transform.localPosition = new Vector2(x, y);
                }
            }
        }

        public void CheckAnswer(string checkWord)
        {
            Word_LineWord line = lines.Find(x => x.answer == checkWord);
            if (line != null)
            {
                if (!line.isShown)
                {
                    textPreview.SetAnswerColor();
                    line.ShowAnswer();
                    SetCompliment(continueJoin);
                    continueJoin++;
                    Invoke(nameof(CheckGameComplete), 0.4f);
                    if (continueJoin == 4) continueJoin = 0;

                    CheckWordCompeleteOrNot(line);
                    if (line.isShown && ConfigController.instance.IsInternetAwailable())
                    {
                        noNet = false;
                        Word_MainController.instance.meaningFindController.GetMeaning(line.answer);
                    }
                    else
                    {
                        noNet = true;
                        noNetWords.Add(line.answer);
                        Word_MainController.instance.dictionaryScreenController.SetObj(false, line.answer.ToUpper());
                    }

                    ConfigController.instance.soundController.Play(Word_SoundController.Others.Match);
                }
                else
                {
                    if (continueJoin != 0) continueJoin = 0;
                    Debug.Log("Word Alrady Exist");
                    ShowMessage();
                    textPreview.SetExistColor();
                }
            }
            else if (validWords.Contains(checkWord.ToLower()))
            {
                Debug.Log("Extra Word -- " + checkWord);
                if (Word_GameState.currentLevel >= Word_AllPrefs.unlockedLevel)
                    Word_ExtraWordManager.instance.ProcessWorld(checkWord);
            }
            else
            {
                if (continueJoin != 0) continueJoin = 0;
                textPreview.SetWrongColor();
            }
            textPreview.FadeOut();
        }
        public void ClickOnBtn()
        {
            if (noNet && ConfigController.instance.IsInternetAwailable())
            {
                for (int i = 0; i < noNetWords.Count; i++)
                {
                    Debug.Log($"No InterNet Words - {noNetWords[i]}");
                    Word_MainController.instance.meaningFindController.GetMeaning(noNetWords[i]);
                }
                noNet = false;
                noNetWords.Clear();
            }
        }
        void SetCompliment(int t)
        {
            if (t != 0)
            {
                switch (t)
                {
                    case 1:
                        compliment.Show(Word_Compliment.Type.Good);
                        break;
                    case 2:
                        compliment.Show(Word_Compliment.Type.Great);
                        break;
                    case 3:
                        compliment.Show(Word_Compliment.Type.Excellent);
                        break;
                    case 4:
                        compliment.Show(Word_Compliment.Type.Awesome);
                        break;
                    default:
                        compliment.Show(Word_Compliment.Type.Amazinng);
                        break;
                }
            }
        }

        public void ShowMessage()
        {
            alradyWordExist.gameObject.SetActive(true);
            alradyWordExist.DOFade(1, 0.7f).OnComplete(() =>
            {
                alradyWordExist.DOFade(0, 0.1f).SetDelay(0.5f).OnComplete(() =>
                {
                    alradyWordExist.gameObject.SetActive(false);
                });
            });
        }

        private void CheckGameComplete()
        {
            SaveLevelProgress();
            var isNotShown = lines.Find(x => !x.isShown);
            if (isNotShown == null)
            {
                ClearLevelProgress();
                Word_MainController.instance.OnComplete();

                if (lines.Count >= 6)
                {
                    compliment.ShowRandom();
                }
            }
        }

        public void HintClick()
        {
            int ballance = CurrencyController.GetBalance();
            if (ballance >= Word_CellConst.HINT_COST)
            {
                var line = lines.Find(x => !x.isShown);

                if (line != null)
                {
                    line.ShowHint();
                    CurrencyController.DebitBalance(Word_CellConst.HINT_COST);
                    CheckGameComplete();

                    Word_AllPrefs.AddToNumHint(Word_GameState.currentLevel);
                }
            }
            else
            {
                if (!Word_InitializeIAP.instance.isInializeIAP)
                    CurrencyController.CreditBalance(100);
                else
                    ConfigController.instance.storeController.OpenStore();
            }
            ConfigController.instance.soundController.PlayButton();
            ConfigController.instance.vibrationController.Vibration();
        }

        public void SaveLevelProgress()
        {
            if (!Word_AllPrefs.IsLastLevel()) return;

            List<string> results = new List<string>();
            foreach (var line in lines)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var cell in line.cells)
                {
                    sb.Append(cell.isShown ? "1" : "0");
                }
                results.Add(sb.ToString());
            }

            Word_AllPrefs.levelProgress = results.ToArray();
        }

        public string[] GetLevelProgress()
        {
            if (!Word_AllPrefs.IsLastLevel()) return new string[0];
            return Word_AllPrefs.levelProgress;
        }

        public void ClearLevelProgress()
        {
            if (!Word_AllPrefs.IsLastLevel()) return;
            Word_AllPlayerPrefs.DeleteKey("level_progress");
        }

        public bool CheckLevelProgress(string[] levelProgress, List<string> wordList)
        {
            if (levelProgress.Length != wordList.Count) return false;

            for (int i = 0; i < wordList.Count; i++)
            {
                if (levelProgress[i].Length != wordList[i].Length) return false;
            }
            return true;
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                Word_Timer.Schedule(this, 0.5f, () =>
                {
                    UpdateBoard();
                });
            }
        }

        private void UpdateBoard()
        {
            string[] progress = GetLevelProgress();
            if (progress.Length == 0) return;

            int i = 0;
            foreach (var line in lines)
            {
                line.SetProgress(progress[i]);
                i++;
            }
        }
    }
}