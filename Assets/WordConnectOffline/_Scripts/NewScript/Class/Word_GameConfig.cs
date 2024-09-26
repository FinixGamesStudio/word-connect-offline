using UnityEngine;
using System;

namespace WordConnectByFinix
{
    [System.Serializable]
    public class Word_GameConfig
    {
        [Header("")]
        public int fontSizeInDiskSelectLevel;
        public int fontSizeInDiskMainScene;
        public int fontSizeInCellMainScene;
        [Header("")]
        public bool isWordRightToLeft = false;
    }
}