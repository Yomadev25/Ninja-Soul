using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    void Update() 
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
