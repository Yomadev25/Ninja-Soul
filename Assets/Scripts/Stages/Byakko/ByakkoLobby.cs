using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByakkoLobby : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _doorCenter;
    [SerializeField]
    private GameObject[] _doorLeft;
    [SerializeField]
    private GameObject[] _doorRight;

    private void Start()
    {
        bool leftClear = ByakkoManager.Instance.leftClear;
        bool rightClear = ByakkoManager.Instance.rightClear;

        if (leftClear && rightClear)
        {
            _doorCenter[0].SetActive(false);
            _doorCenter[1].SetActive(true);
        }

        if (leftClear)
        {
            _doorLeft[1].SetActive(false);
            _doorLeft[0].SetActive(true);
        }

        if (rightClear)
        {
            _doorRight[1].SetActive(false);
            _doorRight[0].SetActive(true);
        }
    }
}
