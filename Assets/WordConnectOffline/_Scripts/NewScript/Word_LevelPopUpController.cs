using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WordConnectByFinix
{
    public class Word_LevelPopUpController : MonoBehaviour
    {
        public Text levelIndex, titleText;
        public List<Word_LevelButton> parentLevelBtn;
        public Transform levelBtnParent;
        public RectTransform levelRect;
    }
}