using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Create Enemy")]
public class Enemy : ScriptableObject
{
    public enum Level
    {
        GENERAL,
        MINI_BOSS,
        BOSS
    }

    public string name;
    public Level level;
    public bool assasinate = true;

    [Header("Field Of View")]
    public float viewRadius;
    public float chaseRadius;
    public float viewAngle;

    [Header("Knock Out")]
    public float knockDuration;

    [Header("Combat & Abilities")]
    public float delayPerCombo;
    public EnemyCombo[] combos;
}
