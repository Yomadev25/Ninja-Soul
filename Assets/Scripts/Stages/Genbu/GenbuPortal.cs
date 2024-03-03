using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenbuPortal : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(GenbuManager.Instance.clearAllHuts);
    }

    private void Start()
    {
        PlayerData.Instance.SetSpawnPoint(default);
    }
}
