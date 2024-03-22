using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Byakko_LongSword : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashFx;

    public void Slash()
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    public void TigerSlash()
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }
}
