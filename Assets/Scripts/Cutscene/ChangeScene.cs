using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cutscene
{
    public class ChangeScene : MonoBehaviour
    {
        public float delay;
        public string scene;

        private void Start()
        {
            Invoke(nameof(LoadScene), delay);
        }

        void LoadScene()
        {
            SceneManager.LoadScene(scene);
        }
    }
}
