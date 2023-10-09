using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PowerUpTest : MonoBehaviour
{
    public Animator anim;
    public VisualEffect levelUp;

    private bool levelingUp;

    void Update()
    {
        if(anim != null)
        {
            if (Input.GetButtonDown("Fire1") && !levelingUp)
            {
                anim.SetTrigger("PowerUp");

                if (levelUp != null)
                    levelUp.Play();

                levelingUp = true;
                StartCoroutine (ResetBool(levelingUp, 0.5f));
            }
        }
        
    }
    IEnumerator ResetBool (bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        levelingUp = !levelingUp;
    }
}
