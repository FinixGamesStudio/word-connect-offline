using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_FortuneWheelPieceController : MonoBehaviour
    {
        public Text coinText;
        public int index, coin;
        [Range(0f, 100f)]
        public float chance = 100f;
        [HideInInspector] public double _weight = 0f;
    }
}