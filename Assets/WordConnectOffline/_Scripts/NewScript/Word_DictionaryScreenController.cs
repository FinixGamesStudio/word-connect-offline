using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace WordConnectByFinix
{
    public class Word_DictionaryScreenController : MonoBehaviour
    {
        public GameObject popUp;
        public Button leftBtn, rightBtn;
        public RectTransform dictionarytBtn;
        public Text currentPageText, noNetWordText;
        public RectTransform dictionaryPrefabTrans;
        public Word_DictionaryPrefab dictionaryPrefab;
        public List<Word_DictionaryPrefab> allDictionaryPrefabs;
        public ScrollRect Target;
        public GameObject noInternetObj, dictionaryObj;
        float stepToMove = 0;
        int pageCount = 0;
        float temp = 0;

        private void Start()
        {
            Target.horizontalScrollbar.value = 0;
        }

        private void OnEnable()
        {
            dictionaryPrefabTrans.anchoredPosition = new Vector2(0, dictionaryPrefabTrans.position.y);
            Word_MainController.instance.isDialogActive = true;
            Invoke(nameof(SetValue), 0.1f);
        }
        private void OnDisable() { Word_MainController.instance.isDialogActive = false; }

        void SetValue() => Target.horizontalScrollbar.value = temp;

        public void SetDictionaryPrefab(string word, List<Definition> defination)
        {
            Word_DictionaryPrefab prefab = Instantiate(dictionaryPrefab);
            Target.horizontalScrollbar.value = 0;
            temp = Target.horizontalScrollbar.value;
            prefab.transform.SetParent(dictionaryPrefabTrans, false);
            prefab.titleWordText.text = word.ToUpper();
            if (allDictionaryPrefabs.Count != 0)
            {
                int t = allDictionaryPrefabs.Count;
                pageCount = 1;
                leftBtn.gameObject.SetActive(false);
                stepToMove = 1f / t;
            }
            prefab.SetDefinationText(defination);
            allDictionaryPrefabs.Add(prefab);
            SetBtnText(allDictionaryPrefabs.Count);
            SetObj(true, word);
        }

        public void DictionaryBtnAnim()
        {
            dictionarytBtn.DOAnchorPosX(-47, 0.4f);
        }

        void SetBtnText(int count)
        {
            if (count == 1)
            {
                leftBtn.gameObject.SetActive(false);
                rightBtn.gameObject.SetActive(false);
            }
            else
            {
                if (count != pageCount)
                    rightBtn.gameObject.SetActive(true);
                else
                    rightBtn.gameObject.SetActive(false);
            }
            SetPageText();
        }

        public void SetObj(bool isNet, string word)
        {
            if (isNet)
            {
                dictionaryObj.SetActive(true);
                noInternetObj.SetActive(false);
            }
            else
            {
                noInternetObj.SetActive(true);
                noNetWordText.text = word;
                dictionaryObj.SetActive(false);
            }
            DictionaryBtnAnim();
        }

        public void ClickOnRightBtn()
        {
            if (pageCount <= allDictionaryPrefabs.Count) pageCount++;
            SetPageText();
            if (Target.horizontalScrollbar.value >= 1) Target.horizontalScrollbar.value = 0;
            Target.horizontalScrollbar.value = Mathf.Clamp(Target.horizontalScrollbar.value + stepToMove, 0, 1);
            if (Mathf.Abs(Target.horizontalScrollbar.value - 0.9999999f) < 0.0001f) Target.horizontalScrollbar.value = 1;
            temp = Target.horizontalScrollbar.value;
            rightBtn.gameObject.SetActive(Target.horizontalScrollbar.value != 1);
            leftBtn.gameObject.SetActive(true);
            if (allDictionaryPrefabs.Count <= 2) { rightBtn.gameObject.SetActive(false); }
            Debug.Log($"Prefab Count - {allDictionaryPrefabs.Count}  -- Scroll Value - {Target.horizontalScrollbar.value}  -- In Right Btn");
        }
        public void ClickOnLeftBtn()
        {
            if (pageCount != 1) pageCount--; if (pageCount > allDictionaryPrefabs.Count) pageCount = 1;
            SetPageText();
            Debug.Log(Target.horizontalScrollbar.value);
            Target.horizontalScrollbar.value = Mathf.Clamp(Target.horizontalScrollbar.value - stepToMove, 0, 1);
            if (Mathf.Abs(Target.horizontalScrollbar.value - 1.490116E-08f) < 0.0001f) Target.horizontalScrollbar.value = 0;
            temp = Target.horizontalScrollbar.value;
            leftBtn.gameObject.SetActive(Target.horizontalScrollbar.value != 0);
            rightBtn.gameObject.SetActive(true);
            Debug.Log($"Prefab Count - {allDictionaryPrefabs.Count}  -- Scroll Value - {Target.horizontalScrollbar.value}  -- In Left Btn");
        }

        void DestroyDictonaryPrefabs()
        {
            if (allDictionaryPrefabs.Count != 0)
            {
                for (int i = 0; i < allDictionaryPrefabs.Count; i++)
                    Destroy(allDictionaryPrefabs[i].gameObject);
                allDictionaryPrefabs.Clear();
            }
        }

        void SetPageText()
        {
            if (allDictionaryPrefabs.Count == 1) pageCount = 1;
            currentPageText.text = $"{pageCount}/{allDictionaryPrefabs.Count}";
        }

        public void OpenDictionryPopUp()
        {
            Word_MainController.instance.wordRegion.ClickOnBtn();
            ConfigController.instance.soundController.PlayButton();
            gameObject.SetActive(true);
            popUp.transform.DOScale(Vector3.one, 0.2f);
        }

        public void CloseDictionryPopUp()
        {
            Word_MainController.instance.wordRegion.ClickOnBtn();
            ConfigController.instance.soundController.PlayButton();
            popUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}