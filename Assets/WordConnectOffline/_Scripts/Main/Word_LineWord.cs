using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordConnectByFinix
{
    public class Word_LineWord : MonoBehaviour
    {

        public string answer;
        public float cellSize;
        public List<Word_CellController> cells = new List<Word_CellController>();
        public int numLetters;
        public float lineWidth;

        public bool isShown, RTL;

        public void Build(bool RTL)
        {
            this.RTL = RTL;
            numLetters = answer.Length;
            float cellGap = cellSize * Word_CellConst.CELL_GAP_COEF;

            for (int i = 0; i < numLetters; i++)
            {
                Word_CellController cell = Instantiate(Word_GameManager.instance.cell);
                cell.letter = answer[i].ToString();
                cell.letterText.transform.localScale = Vector3.one * (cellSize / 90f);
                cell.letterText.fontSize = ConfigController.instance.config.fontSizeInCellMainScene;

                RectTransform cellTransform = cell.GetComponent<RectTransform>();
                cellTransform.SetParent(transform);
                cellTransform.sizeDelta = new Vector2(cellSize, cellSize);
                cellTransform.localScale = Vector3.one;

                float x = cellSize / 2 + i * (cellSize + cellGap);
                float y = cellSize / 2;

                cellTransform.localPosition = new Vector3(x, y);
                cells.Add(cell);
            }
        }

        public void SetLineWidth()
        {
            int numLetters = answer.Length;
            var rt = GetComponent<RectTransform>();
            lineWidth = numLetters * cellSize + (numLetters - 1) * cellSize * Word_CellConst.CELL_GAP_COEF;
            rt.sizeDelta = new Vector2(lineWidth, cellSize);
        }

        public void SetProgress(string progress)
        {
            isShown = true;
            int i = 0;
            foreach (var cell in cells)
            {
                if (progress[i] == '1')
                {
                    cell.isShown = true;
                    cell.letterText.text = cell.letter;
                }
                else
                {
                    isShown = false;
                }
                i++;
            }
        }

        public void ShowAnswer()
        {
            isShown = true;
            foreach (var cell in cells)
            {
                cell.isShown = true;
            }

            StartCoroutine(IEShowAnswer());
        }

        public IEnumerator IEShowAnswer()
        {
            if (!RTL)
            {
                foreach (var cell in cells)
                {
                    cell.isShown = true;
                    cell.Animate();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                for (int i = cells.Count - 1; i >= 0; i--)
                {
                    var cell = cells[i];
                    cell.isShown = true;
                    cell.Animate();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        public void ShowHint()
        {
            if (!RTL)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    var cell = cells[i];
                    if (!cell.isShown)
                    {
                        cell.ShowHint();
                        if (i == cells.Count - 1)
                        {
                            isShown = true;
                        }
                        return;
                    }
                }
            }
            else
            {
                for (int i = cells.Count - 1; i >= 0; i--)
                {
                    var cell = cells[i];
                    if (!cell.isShown)
                    {
                        cell.ShowHint();
                        if (i == 0)
                        {
                            isShown = true;
                        }
                        return;
                    }
                }
            }
        }
    }
}