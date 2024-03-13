using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genbu_Knuckle_III : MonoBehaviour
{
    [SerializeField]
    private GameObject _stompPrefab;

    public void Stomp()
    {
        Instantiate(_stompPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerManager player = collider.GetComponent<PlayerManager>();
                player.TakeDamage(1);
                break;
            }
        }
    }
}
