using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordConnectByFinix
{
    public class Word_MusicController : MonoBehaviour
    {
        public AudioSource audioSource;
        public enum Type { None, Menu, Main_0, Main_1, Main_2 };
        //public static Word_MusicController instance;

        [HideInInspector]
        public AudioClip[] musicClips;

        private Type currentType = Type.None;

        private void Awake()
        {
           // instance = this;
        }

        public bool IsMuted()
        {
            return !IsEnabled();
        }

        public bool IsEnabled()
        {
            return Word_AllPlayerPrefs.GetBool("music_enabled", true);
        }

        public void SetEnabled(bool enabled, bool updateMusic = false)
        {
            Word_AllPlayerPrefs.SetBool("music_enabled", enabled);
            if (updateMusic)
                UpdateSetting();
        }

        public void Play(Word_MusicController.Type type)
        {
            if (type == Type.None) return;
            if (currentType != type || !audioSource.isPlaying)
            {
                StartCoroutine(PlayNewMusic(type));
            }
        }

        public void Play()
        {
            Play(currentType);
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        private IEnumerator PlayNewMusic(Word_MusicController.Type type)
        {
            Debug.Log("Play Music----------------");
            while (audioSource.volume >= 0.1f)
            {
                audioSource.volume -= 0.2f;
                yield return new WaitForSeconds(0.1f);
            }
            audioSource.Stop();
            currentType = type;
            audioSource.clip = musicClips[(int)type];
            if (IsEnabled())
            {
                audioSource.Play();
            }
            audioSource.volume = 1;
        }

        private void UpdateSetting()
        {
            if (audioSource == null) return;
            if (IsEnabled())
                Play();
            else
                audioSource.Stop();
        }
    }
}