using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace WordConnectByFinix
{
    public class Word_CellConst
    {
        public const float CELL_GAP_COEF = 0.08f;
        public const float COL_GAP_COEF = 0.4f;
        public const int HINT_COST = 10;
        public const int NUM_SUBWORLD = 5;
    }
    public class Word_GameState
    {
        public static int currentLevel;
        public static int unlockedLevel = -1;
    }

    public class Utils
    {
        public static GameLevel Load(int level)
        {
            return ConfigController.instance.gameLevels[level];
        }
    }

    public static class SetPanel
    {
        public static void MoveRightSide(RectTransform levelPanelRect , float canvasWidth)
        {
            levelPanelRect.localPosition = new Vector3(-canvasWidth, 0, 0);
            levelPanelRect.anchoredPosition = new Vector2(canvasWidth, 0);
        }

        public static void MoveMiddle(RectTransform levelPanelRect)
        {
            levelPanelRect.DOLocalMove(Vector3.zero, 0.5f);
        }

        public static void MoveLeftSide(RectTransform levelPanelRect, float canvasWidth)
        {
            levelPanelRect.DOLocalMove(new Vector3(canvasWidth, 0, 0), 0.5f);
            levelPanelRect.DOAnchorPos(new Vector2(-canvasWidth, 0), 0.5f).OnComplete(() =>
            {
                levelPanelRect.localPosition = new Vector3(-canvasWidth, 0, 0);
                levelPanelRect.anchoredPosition = new Vector2(canvasWidth, 0);
            });
        }
    }

    public static class SetPopUp
    {
        public static void OpenPopUp(GameObject parent,GameObject popUp)
        {
            parent.SetActive(true);
            popUp.transform.DOScale(Vector3.one, 0.2f);
        }

        public static void ClosePopUp(GameObject parent, GameObject popUp)
        {
            popUp.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => parent.SetActive(false));
        }
    }

    public class Word_Timer
    {
        private static MonoBehaviour behaviour;
        public delegate void Task();

        public static void Schedule(MonoBehaviour _behaviour, float delay, Task task)
        {
            behaviour = _behaviour;
            behaviour.StartCoroutine(DoTask(task, delay));
        }

        private static IEnumerator DoTask(Task task, float delay)
        {
            yield return new WaitForSeconds(delay);
            task();
        }
    }

    public class CommonConst
    {
        public const iTween.DimensionMode ITWEEN_MODE = iTween.DimensionMode.mode2D;

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
    public const bool ENCRYPTION_PREFS = true;
#else
        public const bool ENCRYPTION_PREFS = false;
#endif
    }
}