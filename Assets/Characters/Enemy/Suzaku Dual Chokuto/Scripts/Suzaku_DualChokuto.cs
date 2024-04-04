using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.VFX;

public class Suzaku_DualChokuto : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashFx;

    public void Slash(int side)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (side)
        {
            case 1:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 0);
                break;
            case 2:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 180);
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }
}
