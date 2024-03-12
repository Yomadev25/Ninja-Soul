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
        Invoke(nameof(SetSpawn), 1f);
    }

    private void SetSpawn()
    {
        PlayerData.Instance.SetSpawnPoint(default);
    }
}
