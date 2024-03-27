using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class Suzaku_Kama : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashFx;
    public void Attack2(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -10f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -20f);
                break;
            case 3:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -205f);
                break;
            case 4:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -170f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(1);
            }
        }
    }

    public void Dash()
    {
        
    }
}
