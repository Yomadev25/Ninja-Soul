using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHider : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> _renderers = new List<Renderer>();

    private void Start()
    {
        GetAllRenderers(transform);
    }

    private void GetAllRenderers(Transform parent)
    {
        Renderer renderer = parent.GetComponent<Renderer>();

        if (renderer != null)
        {
            _renderers.Add(renderer);
        }

        GetAllRenderersInChildren(parent);
    }

    private void GetAllRenderersInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Renderer renderer = child.GetComponent<Renderer>();

            if (renderer != null)
            {
                _renderers.Add(renderer);
            }

            GetAllRenderersInChildren(child);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeanTween.value(1, 0.1f, 1f).setOnUpdate((x) =>
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
