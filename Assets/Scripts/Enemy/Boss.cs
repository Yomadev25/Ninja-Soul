using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM("Boss");
    }
}
