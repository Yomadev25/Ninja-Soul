using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Byakko_Claw : MonoBehaviour
{
    [SerializeField]
    private GameObject _scratchFx;

    public void Claw(string side)
    {
        if (side == "left")
        {

        }
        else if (side == "right")
        {

        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }
}
