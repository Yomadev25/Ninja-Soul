using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Create Enemy")]
public class Enemy : ScriptableObject
{
    public string name;

    [Header("Field Of View")]
    public float viewRadius;
    public float viewAngle;
}
