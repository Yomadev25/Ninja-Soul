using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeiryuRoof : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] _renderers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeanTween.value(1, 0, 1f).setOnUpdate((x) =>
            {
                foreach (Renderer renderer in _renderers)
                {
                    Color color = renderer.material.color;
                    color.a = x;
                    renderer.material.color = color;
                }
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeanTween.value(0, 1, 1f).setOnUpdate((x) =>
            {
                foreach (Renderer renderer in _renderers)
                {
                    Color color = renderer.material.color;
                    color.a = x;
                    renderer.material.color = color;
                }
            });
        }
    }
}
