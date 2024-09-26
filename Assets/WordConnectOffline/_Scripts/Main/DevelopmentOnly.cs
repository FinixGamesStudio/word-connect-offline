using UnityEngine;
using System.Collections;
using System.IO;

namespace WordConnectByFinix
{
    public class DevelopmentOnly : MonoBehaviour
    {
        public bool setCoins;
        public int coins;

        public bool unlockAllLevels;
        public int allLevels;

        public bool clearAllPlayerPrefs;

        private void Start()
        {
            if (setCoins)
                CurrencyController.SetBalance(coins);

            if (unlockAllLevels)
            {
                Word_AllPrefs.unlockedLevel = allLevels;
            }

            if (clearAllPlayerPrefs)
            {
                Word_AllPlayerPrefs.DeleteAll();
                Word_AllPlayerPrefs.Save();
            }
        }
    }
}
