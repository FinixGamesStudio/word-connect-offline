using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

namespace WordConnectByFinix
{

    public class Word_FortuneWheel : MonoBehaviour
    {
        [Space]
        public List<Word_FortuneWheelPieceController> pieces;

        [Space]
        [Header("References :")]
        [SerializeField] private Transform PickerWheelTransform;
        [SerializeField] private Transform wheelCircle;
        [SerializeField] private GameObject[] blinkLight;
        public Transform coinAnimStartPos, coinAnimEndPos;
        public Image tostMsg;
        public Text tostMsgText;
        public Button spinBtn, adSpinBtn;
        public GameObject wheel;
        public GameObject coinPrefab;
        List<GameObject> coinList = new List<GameObject>();

        [Space]
        [Header("Sounds :")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tickAudioClip;
        [SerializeField][Range(0f, 1f)] private float volume = .5f;
        [SerializeField][Range(-3f, 3f)] private float pitch = 1f;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;

        [Space]
        [Header("Picker wheel pieces :")]
        private UnityAction onSpinStartEvent;
        private UnityAction<Word_FortuneWheelPieceController> onSpinEndEvent;
        private bool _isSpinning = false;
        public bool IsSpinning { get { return _isSpinning; } }
        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;


        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        private List<int> nonZeroChancesIndices = new List<int>();

        private void Start()
        {
            pieceAngle = 360 / pieces.Count;
            halfPieceAngle = pieceAngle / 2f;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);
            CalculateWeightsAndIndices();
            SetupAudio();
            for (int i = 0; i < 12; i++)
            {
                GameObject coin = Instantiate(coinPrefab, gameObject.transform);
                coin.transform.SetParent(gameObject.transform, false);
                coinList.Add(coin);
                coin.SetActive(false);
            }
        }

        private void SetupAudio()
        {
            audioSource.clip = tickAudioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
        }

        IEnumerator ActivateObjects()
        {
            for (int i = 0; i < 25; i++)
            {
                blinkLight[1].SetActive(true);
                blinkLight[0].SetActive(false);
                yield return new WaitForSeconds(0.5f);
                blinkLight[0].SetActive(true);
                blinkLight[1].SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
            CancelInvoke(nameof(ActivateObjects));
        }

        void SetChangeToWin()
        {
            for (int i = 0; i < pieces.Count; i++)
                pieces[i].chance = Random.Range(0, 100);
            CalculateWeightsAndIndices();
        }

        void RotateFortuneWheel()
        {
            if (!_isSpinning)
            {
                SetChangeToWin();
                _isSpinning = true;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke();
                int index = GetRandomPieceIndex();
                Word_FortuneWheelPieceController piece = pieces[index];
                if (piece.chance == 0 && nonZeroChancesIndices.Count != 0)
                {
                    index = nonZeroChancesIndices[Random.Range(0, nonZeroChancesIndices.Count)];
                    piece = pieces[index];
                }
                float angle = -(pieceAngle * index);
                float rightOffset = (angle - halfPieceAngleWithPaddings) % 360;
                float leftOffset = (angle + halfPieceAngleWithPaddings) % 360;
                float randomAngle = Random.Range(leftOffset, rightOffset);
                Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * 4);
                float prevAngle, currentAngle;
                prevAngle = currentAngle = wheelCircle.eulerAngles.z;
                bool isIndicatorOnTheLine = false;
                wheelCircle
                .DORotate(targetRotation, 4, RotateMode.FastBeyond360)
                .SetEase(Ease.OutBack)
                .OnUpdate(() =>
                {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= halfPieceAngle)
                    {
                        if (isIndicatorOnTheLine)
                        {
                            audioSource.PlayOneShot(audioSource.clip);
                        }
                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = wheelCircle.eulerAngles.z;
                })
                .OnComplete(() =>
                {
                    _isSpinning = false;
                    if (onSpinEndEvent != null)
                        onSpinEndEvent.Invoke(piece);

                    onSpinStartEvent = null;
                    onSpinEndEvent = null;
                });
            }
        }

        public void OnSpinStart(UnityAction action)
        {
            onSpinStartEvent = action;
        }

        public void OnSpinEnd(UnityAction<Word_FortuneWheelPieceController> action)
        {
            onSpinEndEvent = action;
        }

        public void ClickOnAdSpinBtn()
        {
            Word_AdmobController.instance.ShowRewardedAd(2);
        }

        public void ClickOnSpinBtn()
        {
            RotateFortuneWheel();
            spinBtn.interactable = false;
            adSpinBtn.interactable = false;
            OnSpinEnd((wheelPiece) =>
            {
                StartCoroutine(ActivateObjects());
                if (wheelPiece.index == 2)
                {
                    EnableBtn(true);
                    ShowMessage("Hard Luck !!");
                }
                else if (wheelPiece.index == 6)
                {
                    EnableBtn(false);
                    ShowMessage("Free Spin !!");
                }
                else
                {
                    EnableBtn(true);
                    GiveReward(wheelPiece.coin);
                }
                spinBtn.interactable = true;
                adSpinBtn.interactable = true;
            });
        }

        void GiveReward(int coin)
        {
            ShowMessage($"You Got {coin} Coins !!");
            Invoke(nameof(StartCoinAnim), 0.4f);
            CurrencyController.CreditBalance(coin);
        }

        void StartCoinAnim()
        {
            StartCoinAnimation(coinAnimStartPos, coinAnimEndPos);
        }

        void EnableBtn(bool isAd)
        {
            if (isAd) { spinBtn.gameObject.SetActive(false); adSpinBtn.gameObject.SetActive(true); }
            else { spinBtn.gameObject.SetActive(true); adSpinBtn.gameObject.SetActive(false); }
        }

        public void ShowMessage(string msg)
        {
            tostMsgText.text = msg;
            tostMsg.gameObject.SetActive(true);
            tostMsg.DOFade(1, 0.5f).OnComplete(() =>
            {
                tostMsg.DOFade(0, 0.3f).SetDelay(0.7f).OnComplete(() =>
                {
                    tostMsg.gameObject.SetActive(false);
                });
            });
        }

        private int GetRandomPieceIndex()
        {
            double r = rand.NextDouble() * accumulatedWeight;
            for (int i = 0; i < pieces.Count; i++)
                if (pieces[i]._weight >= r)
                    return i;
            return 0;
        }

        private void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                Word_FortuneWheelPieceController piece = pieces[i];
                accumulatedWeight += piece.chance;
                piece._weight = accumulatedWeight;
                if (piece.chance > 0)
                    nonZeroChancesIndices.Add(i);
            }
        }

        public void OpenFortuneWheel()
        {
            gameObject.SetActive(true);
            wheel.transform.DOScale(Vector3.one, 0.2f);
        }

        public void CloseFortuneWheel()
        {
            wheel.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => gameObject.SetActive(false));
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