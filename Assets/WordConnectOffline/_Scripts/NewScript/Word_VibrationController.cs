using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordConnectByFinix
{
    public class Word_VibrationController : MonoBehaviour
    {
        public void SetEnabled(bool enabled)
        {
            Word_AllPlayerPrefs.SetBool("vibration_enabled", enabled);
        }

        public bool IsEnabled()
        {
            return Word_AllPlayerPrefs.GetBool("vibration_enabled", true);
        }


        public void Vibration()
        {
            if (IsEnabled())
            {
                Handheld.Vibrate();
            }
        }
    }
}