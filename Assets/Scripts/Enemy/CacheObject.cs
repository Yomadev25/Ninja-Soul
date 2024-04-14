using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheObject : MonoBehaviour
{
    public int id;

    private void Awake()
    {
        id = GetInstanceID();
        int i = PlayerPrefs.GetInt(id.ToString(), 1);
        if (i == 0)
        {
            Destroy(gameObject);
        }

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead, (sender) =>
        {
            if (sender.gameObject == this)
            {
                PlayerPrefs.GetInt(GetInstanceID().ToString(), 0);
            }
        });
    }
}
