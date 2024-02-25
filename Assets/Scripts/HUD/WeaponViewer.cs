using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponViewer : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponRoot;
    [SerializeField]
    private Camera _cam;
    private Vector3 _posLastFrame;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _posLastFrame = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - _posLastFrame;
            _posLastFrame = Input.mousePosition;

            var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
            _weaponRoot.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * _weaponRoot.rotation;
        }
    }
}
