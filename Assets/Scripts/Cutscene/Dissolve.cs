using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Cutscene
{
    public class Dissolve : MonoBehaviour
    {
        public float delay;
        public VisualEffect VFXGraph;
        public float dissolveRate = 0.0125f;
        public float refreshRate = 0.025f;

        [SerializeField]
        private MeshRenderer[] meshRenderers;
        [SerializeField]
        private List<Material> skinnedMaterials;

        void Start()
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                foreach (Material material in meshRenderer.materials)
                {
                    skinnedMaterials.Add(material);
                }
            }
            StartCoroutine(DissolveCo());
        }

        IEnumerator DissolveCo()
        {
            yield return new WaitForSeconds(delay);

            if (skinnedMaterials.Count > 0)
            {
                if (VFXGraph != null)
                {
                    VFXGraph.Play();
                }
                float counter = 0;
                while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
                {
                    counter += dissolveRate;
                    for (int i = 0; i < skinnedMaterials.Count; i++)
                    {
                        skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                    }
                    yield return new WaitForSeconds(refreshRate);
                }
            }
        }
    }
}
