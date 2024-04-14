using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreditHud : MonoBehaviour
{
    [SerializeField]
    private Scrollbar _scrollBar;
    [SerializeField]
    private float _speed;
    public UnityEvent onEnded;

    bool isRun;

    private void Start()
    {
        StartCredit();
    }

    public void StartCredit()
    {
        isRun = true;
        _scrollBar.value = 1f;
    }

    private void Update()
    {
        if (isRun && _scrollBar.value > 0f)
        {
            _scrollBar.value -= _speed * Time.deltaTime;
        }
        else
        {
            End();
        }
    }

    private void End()
    {
        isRun = false;

        _scrollBar.value = 0f;
        onEnded?.Invoke();
    }
}
