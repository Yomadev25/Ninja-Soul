using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutCamera : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 dir = _player.position - transform.position;

        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, dir, Mathf.Infinity, _layerMask);

        if (hitObjects.Length > 0)
        {
            for (int i = 0; i < hitObjects.Length; ++i)
            {
                Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

                for (int m = 0; m < materials.Length; ++m)
                {
                    //materials[m].SetVector("_CutoutPos", cutoutPos);
                }
            }
        }
    }
}
