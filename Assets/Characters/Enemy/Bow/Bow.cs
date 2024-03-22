using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private GameObject _arrowPrefab;
    [SerializeField]
    private Transform _shootPos;

    public void Shoot()
    {
        Instantiate(_arrowPrefab, _shootPos.position, _shootPos.rotation);
    }
}
