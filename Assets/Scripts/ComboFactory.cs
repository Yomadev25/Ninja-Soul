using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboFactory : MonoBehaviour
{
    [SerializeField]
    private Combo[] combos;

    public Combo[] Combos => combos;
}
