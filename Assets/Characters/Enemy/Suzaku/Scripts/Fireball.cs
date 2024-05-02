using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Transform target;
    bool isFire;
    Vector3 position;
    public GameObject bombFx;
    public Transform owner;

    private IEnumerator Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position += transform.forward;
        yield return new WaitForSeconds(3f);
        isFire = true;
        position = target.position;
        AudioManager.Instance.PlaySFX("Fireball");
    }

    private void Update()
    {
        if (target == null) return;

        if (isFire)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 20f * Time.deltaTime);
            if (Vector3.Distance(transform.position, position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.RotateAround(owner.position + Vector3.up, Vector3.up, 50f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1f);
            }

            GameObject bomb = Instantiate(bombFx, transform.position, Quaternion.identity);
            Destroy(bomb, 1f);
            Destroy(gameObject);
        }

        
    }
}
