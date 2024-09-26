using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_ProfileSelection : MonoBehaviour
    {
        public Image frame;
        public Sprite normalRing, greenRing;
        public int avtarIndex;

        public void SetFrame(bool isSelect)
        {
            frame.sprite = isSelect ? greenRing : normalRing;
        }
    }
}
