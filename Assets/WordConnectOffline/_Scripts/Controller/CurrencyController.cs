using UnityEngine;
using System.Collections;
using System;

namespace WordConnectByFinix
{
    public class CurrencyController
    {
        public const int DEFAULT_CURRENCY = 10;
        public static Action onBalanceChanged;
        public static Action<int> onBallanceIncreased;

        public static int GetBalance()
        {
            return Word_AllPlayerPrefs.GetInt("coins", DEFAULT_CURRENCY);
        }

        public static void SetBalance(int value)
        {
            Word_AllPlayerPrefs.SetInt("coins", value);
            Word_AllPlayerPrefs.Save();
        }

        public static void CreditBalance(int value)
        {
            int current = GetBalance();
            SetBalance(current + value);
            if (onBalanceChanged != null) onBalanceChanged();
            if (onBallanceIncreased != null) onBallanceIncreased(value);
        }

        public static bool DebitBalance(int value)
        {
            int current = GetBalance();
            if (current < value)
            {
                return false;
            }

            SetBalance(current - value);
            if (onBalanceChanged != null) onBalanceChanged();
            return true;
        }
    }
}