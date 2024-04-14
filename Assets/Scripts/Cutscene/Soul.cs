using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
    public class Soul : MonoBehaviour
    {
        public float delay;
        public Transform target;

        private void Start()
        {
            transform.LeanMove(target.position, 0.5f).setDelay(delay);
        }
    }
}
