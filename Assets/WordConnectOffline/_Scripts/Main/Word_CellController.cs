using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_CellController : MonoBehaviour
    {
        public Text letterText;
        public string letter;
        public bool isShown;
        public Image cellImage;
        public Sprite gereenCellSprite, creamCellSprite;

        public bool isHint = false;

        private Vector3 originLetterScale;

        private void Start()
        {
            if (letterText.text == "")
                cellImage.sprite = creamCellSprite;
            else
                cellImage.sprite = gereenCellSprite;
        }

        public void Animate()
        {
            Vector3 beginPosition = Word_MainController.instance.textPreview.transform.position;
            originLetterScale = letterText.transform.localScale;
            Vector3 middlePoint = Word_Utils.GetMiddlePoint(beginPosition, transform.position, -0.3f);
            Vector3[] waypoint = { beginPosition, middlePoint, transform.position };
            ShowText();
            if (!isHint)
            {
                letterText.transform.position = beginPosition;
                letterText.transform.localScale = Word_MainController.instance.textPreview.text.transform.localScale;
                letterText.transform.SetParent(Word_MainController.instance.textFlyTransform);
                iTween.MoveTo(letterText.gameObject, iTween.Hash("path", waypoint, "time", 0.2f, "oncomplete", "OnMoveToComplete", "oncompletetarget", gameObject));
                iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.2f));
            }
        }

        private void OnMoveToComplete()
        {
            letterText.transform.SetParent(transform);
            iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale * 1.3f, "time", 0.15f, "oncomplete", "OnScaleUpComplete", "oncompletetarget", gameObject));
        }

        private void OnScaleUpComplete()
        {
            iTween.ScaleTo(letterText.gameObject, iTween.Hash("scale", originLetterScale, "time", 0.15f));
        }

        public void ShowHint()
        {
            isShown = true;
            isHint = true;
            originLetterScale = letterText.transform.localScale;
            ShowText();
            OnMoveToComplete();
        }

        public void ShowText()
        {
            cellImage.sprite = gereenCellSprite;
            letterText.text = letter;
        }
    }
}
