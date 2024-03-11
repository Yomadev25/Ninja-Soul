using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrb : MonoBehaviour
{
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.position) < 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _player.position + Vector3.up, 7f * Time.deltaTime);
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.Heal(1);
            }

            Destroy(gameObject);
        }
    }
}
