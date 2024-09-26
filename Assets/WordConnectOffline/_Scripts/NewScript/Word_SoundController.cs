using UnityEngine;
using System.Collections;
using System;

namespace WordConnectByFinix
{
    public class Word_SoundController : MonoBehaviour
    {
        public AudioSource audioSource, loopAudioSource;
        public enum Button { Default };
        public enum Others { Match, Win };

        [HideInInspector]
        public AudioClip[] buttonClips;
        [HideInInspector]
        public AudioClip[] otherClips;

       // public static Word_SoundController instance;

        private void Awake()
        {
          //  instance = this;
        }

        private void Start()
        {
            UpdateSetting();
        }

        public bool IsMuted()
        {
            return !IsEnabled();
        }

        public bool IsEnabled()
        {
            return Word_AllPlayerPrefs.GetBool("sound_enabled", true);
        }

        public void SetEnabled(bool enabled)
        {
            Word_AllPlayerPrefs.SetBool("sound_enabled", enabled);
            UpdateSetting();
        }

        public void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void Play(AudioSource audioSource)
        {
            if (IsEnabled())
            {
                audioSource.Play();
            }
        }

        public void PlayButton(Button type = Button.Default)
        {
            int index = (int)type;
            audioSource.PlayOneShot(buttonClips[index]);
        }

        public void Play(Others type, float volume = 1)
        {
            int index = (int)type;
            audioSource.volume = volume;
            audioSource.PlayOneShot(otherClips[index]);
        }

        public void PlayLooping(Others type, float volume = 1)
        {
            int index = (int)type;
            loopAudioSource.volume = volume;
            loopAudioSource.PlayOneShot(otherClips[index]);
        }

        public void StopLooping()
        {
            loopAudioSource.Stop();
        }

        public void UpdateSetting()
        {
            audioSource.mute = IsMuted();
            loopAudioSource.mute = IsMuted();
        }
    }
}