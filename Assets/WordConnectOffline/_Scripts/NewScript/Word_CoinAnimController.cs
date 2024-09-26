using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WordConnectByFinix
{
    public class Word_CoinAnimController : MonoBehaviour
    {
        public static Word_CoinAnimController instance;
        public GameObject coinPrefab;

        List<GameObject> coinList = new List<GameObject>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < 12; i++)
            {
                GameObject coin = Instantiate(coinPrefab, gameObject.transform);
                coin.transform.SetParent(gameObject.transform, false);
                coinList.Add(coin);
                coin.SetActive(false);
            }
        }

        public void StartCoinAnimation(Transform startPosition, Transform endPosition)
        {
            for (int i = 0; i < coinList.Count; i++)
            {
                coinList[i].gameObject.transform.SetParent(startPosition, false);
                coinList[i].SetActive(true);
                coinList[i].transform.position = new Vector2((startPosition.position.x + Random.Range(-0.7f, 0.7f)), (startPosition.position.y + Random.Range(-0.7f, 0.7f)));
              
            }
            StartCoroutine(StartMoveAnimation(endPosition));
        }
        IEnumerator StartMoveAnimation(Transform endPosition)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < coinList.Count; i++)
                coinList[i].transform.DOMove(endPosition.position, Random.Range(0.7f, 1f)).SetEase(Ease.OutBack);
            Invoke(nameof(DisableCoin), 1f);
        }
        void DisableCoin()
        {
            for (int i = 0; i < coinList.Count; i++)
                coinList[i].SetActive(false);
        }
    }
}