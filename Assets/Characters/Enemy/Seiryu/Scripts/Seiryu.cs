using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seiryu : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _dashFx;
    [SerializeField]
    private GameObject _slashPrefab;
    [SerializeField]
    private GameObject _stormPrefab;

    public void DashAttack()
    {
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        _dashFx.Play();
        yield return new WaitForSeconds(0.5f);
        _dashFx.Stop();
    }

    public void LeftSlash()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= 90f;
        Instantiate(_slashPrefab, transform.position + transform.forward + Vector3.up, Quaternion.Euler(rotation));
    }

    public void RightSlash()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= 90f;
        Instantiate(_slashPrefab, transform.position + transform.forward + Vector3.up, Quaternion.Euler(rotation));
    }

    public void DragonStorm()
    {
        Instantiate(_stormPrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
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
