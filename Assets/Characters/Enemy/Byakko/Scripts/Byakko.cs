using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Byakko : MonoBehaviour
{
    [SerializeField]
    private GameObject _slashFx;
    [SerializeField]
    private GameObject _lastBladePrefab;

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

    public void TripleSlash(int combo)
    {
        GameObject slashFx = Instantiate(_slashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(125f, transform.eulerAngles.y, 0f);
                break;
            case 2:
                eulerAngle = new Vector3(65f, transform.eulerAngles.y, 0f);
                break;
            case 3:
                eulerAngle = new Vector3(125f, transform.eulerAngles.y, 0f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    public void LastBlade()
    {
        Instantiate(_lastBladePrefab,
            new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward,
            Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y - 90f, 0f)));
    }
}
