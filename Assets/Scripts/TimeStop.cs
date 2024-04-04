using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Singleton<TimeStop>
{
    private float _speed;
    private bool _restoreTime;

    private void Update()
    {
        if (_restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * _speed;
            }
            else
            {
                Time.timeScale = 1f;
                _restoreTime = false;
            }
        }
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        _speed = restoreSpeed;
        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            _restoreTime = true;
        }

        Time.timeScale = changeTime;
    }

    IEnumerator StartTimeAgain(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _restoreTime = true;
    }
}
