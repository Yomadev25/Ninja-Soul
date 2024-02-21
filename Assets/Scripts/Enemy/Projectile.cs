using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Projectile : MonoBehaviour
{    
    [SerializeField]
    private float _height;
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Vector3 _startPoint;
    [SerializeField]
    private Vector3 _endPoint;
    [SerializeField]
    private Vector3 _control;

    private float time;

    public void Start()
    {
        _control = Vector3.Lerp(_startPoint, _endPoint, 0.5f) + (Vector3.up * _height);
    }

    private void Update()
    {
        if (_startPoint == null || _endPoint == null)
            return;

        time += Time.deltaTime * _speed;
        transform.position = Evaluate(time);
        transform.forward = Evaluate(time + 0.001f) - transform.position;

        if (time >= 1)
        {
            Destroy(gameObject);
        }
    }

    public void InitProjectile(Vector3 startPoint, Vector3 endPoint)
    {
        _startPoint = startPoint;
        _endPoint = endPoint;
    }

    private Vector3 Evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(_startPoint, _control, t);
        Vector3 cb = Vector3.Lerp(_control, _endPoint, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private void OnDrawGizmosSelected()
    {
        if (_startPoint == null || _endPoint == null || _control == null)
            return;

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(Evaluate(i / 20f), 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
