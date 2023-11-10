using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    Material[] materials;

    private void Awake()
    {
        
    }

    private void Start()
    {
        materials = GetComponent<Renderer>().materials;
    }

    private void FadeOut()
    {
        if (materials.Length == 0) return;

        foreach (Material material in materials)
        {
            Color color = material.color;
            LeanTween.value(1f, 0.2f, 1f).setOnUpdate((x) =>
            {
                color.a = x;
            });
        }
    }

    private void ResetFade()
    {
        if (materials.Length == 0) return;

        foreach (Material material in materials)
        {
            Color color = material.color;
            LeanTween.value(0.2f, 1f, 1f).setOnUpdate((x) =>
            {
                color.a = x;
            });
        }
    }
}
