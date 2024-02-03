using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Player/Combo")]
public class Combo : ScriptableObject
{
    public enum WeaponType
    {
        Sword,
        Fist
    }

    public AnimatorOverrideController animation;
    public float damage;

    [Header("Effect Offset")]
    public Vector3 eulerAngle;
}
