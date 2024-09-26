using UnityEngine;
using System.Collections;

namespace WordConnectByFinix
{
    public class Word_Compliment : MonoBehaviour
    {
        public Animator anim;
        public SpriteRenderer sRenderer;
        public enum Type { Amazinng, Awesome, Excellent, Great, Good };
        public Sprite[] sprites;

        public void Show(Type type)
        {
            if (!IsAvailable2Show()) return;

            sRenderer.sprite = sprites[(int)type];
            anim.SetTrigger("show");
        }

        public void ShowRandom()
        {
            if (!IsAvailable2Show()) return;

            sRenderer.sprite = Word_Utils.GetRandom(sprites);
            anim.SetTrigger("show");
        }

        private bool IsAvailable2Show()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            return info.IsName("Idle");
        }
    }
}