using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Rigidbody rb;

    private void Start()
    {
        rb.velocity = transform.forward * _speed;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
