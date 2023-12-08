using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Create Enemy")]
public class Enemy : ScriptableObject
{
    public string name;

    [Header("Field Of View")]
    public float viewRadius;
    public float chaseRadius;
    public float combatRadius;
    public float viewAngle;

    [Header("Combat & Abilities")]
    public float delayPerCombo;
    public EnemyCombo[] combos;
}