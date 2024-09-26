using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_DictionaryPrefab : MonoBehaviour
    {
        public RectTransform thisTrans;
        public RectTransform dictionaryTextTrans;
        public Text titleWordText;
        public Word_DictionaryText dictionaryText;
        public List<Word_DictionaryText> dictionaryList;
        public List<Definition> definition;
        public GameObject loader;

        public void SetDefinationText(List<Definition> definitions)
        {
            definition = definitions;
            for (int i = 0; i < definition.Count; i++)
            {
                Word_DictionaryText text = Instantiate(dictionaryText);
                text.transform.SetParent(dictionaryTextTrans, false);
                text.meaningText.text = (i + 1) + ". " + definition[i].definition;
                dictionaryList.Add(text);
            }
            //loader.gameObject.SetActive(false);
        }
    }
}
