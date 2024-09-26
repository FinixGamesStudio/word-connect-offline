using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace WordConnectByFinix
{
    public class Word_CurrencyBallance : MonoBehaviour
    {
        public Text coinText;
        private void Start()
        {
            UpdateBalance();
            CurrencyController.onBalanceChanged += OnBalanceChanged;
        }

        private void UpdateBalance()
        {
            coinText.text = CurrencyController.GetBalance().ToString();
        }

        private void OnBalanceChanged()
        {
            UpdateBalance();
        }

        private void OnDestroy()
        {
            CurrencyController.onBalanceChanged -= OnBalanceChanged;
        }
    }
}