using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Player/Combo")]
public class Combo : ScriptableObject
{
    public AnimatorOverrideController animation;
    public float damage;
}
