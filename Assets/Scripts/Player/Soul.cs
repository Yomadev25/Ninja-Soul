using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField]
    private float soul;

    private Transform _player;
    bool isReady;

    private IEnumerator Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        yield return new WaitForSeconds(1f);
        isReady = true;
    }

    private void Update()
    {
        if (!isReady) return;
        this.transform.position = Vector3.MoveTowards(this.transform.position, _player.position + Vector3.up, 7f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.GetSoul(soul);
            }

            Destroy(gameObject);
        }
    }
}
