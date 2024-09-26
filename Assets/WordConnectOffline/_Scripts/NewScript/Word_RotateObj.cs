using UnityEngine;
using System.Collections;

namespace WordConnectByFinix
{
    public class Word_RotateObj : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
