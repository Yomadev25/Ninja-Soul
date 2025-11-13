using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheat : Singleton<Cheat>
{
    [SerializeField]
    private GameObject console;

    private int ctrlPressCount = 0;
    private float lastCtrlPressTime = 0f;
    private float ctrlTimeWindow = 0.5f; // Time window to press Ctrl twice
    
    private List<KeyCode> requiredKeys = new List<KeyCode> { KeyCode.A, KeyCode.N, KeyCode.T };
    private int currentKeyIndex = 0;
    private float lastKeyPressTime = 0f;
    private float keyTimeWindow = 1.5f; // Time window to complete the sequence
    
    private bool ctrlSequenceComplete = false;

    void Start()
    {
        SetEnableConsole(false);
    }

    private void Update()
    {
        // Check for Ctrl press twice
        if (!ctrlSequenceComplete)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                if (Time.time - lastCtrlPressTime <= ctrlTimeWindow)
                {
                    ctrlPressCount++;
                    if (ctrlPressCount >= 2)
                    {
                        ctrlSequenceComplete = true;
                        currentKeyIndex = 0;
                        lastKeyPressTime = Time.time;
                    }
                }
                else
                {
                    ctrlPressCount = 1;
                }
                lastCtrlPressTime = Time.time;
            }
            
            // Reset if too much time has passed
            if (Time.time - lastCtrlPressTime > ctrlTimeWindow && ctrlPressCount > 0)
            {
                ctrlPressCount = 0;
            }
        }
        // Check for A-N-T sequence after Ctrl is pressed twice
        else
        {
            if (Input.GetKeyDown(requiredKeys[currentKeyIndex]))
            {
                currentKeyIndex++;
                lastKeyPressTime = Time.time;
                
                if (currentKeyIndex >= requiredKeys.Count)
                {
                    SetEnableConsole(true);
                    
                    // Reset the sequence
                    ResetSequence();
                }
            }
            else if (Input.anyKeyDown)
            {
                // Wrong key pressed - reset sequence
                ResetSequence();
            }
            
            // Reset if too much time has passed
            if (Time.time - lastKeyPressTime > keyTimeWindow && currentKeyIndex > 0)
            {
                ResetSequence();
            }
        }
    }
    
    private void ResetSequence()
    {
        ctrlPressCount = 0;
        currentKeyIndex = 0;
        ctrlSequenceComplete = false;
    }

    public void SetEnableConsole(bool enable)
    {
        if (console != null)
        {
            console.SetActive(enable);
        }
    }

    // commands

    [ConsoleMethod( "close", "Close console" )]
	public static void CloseConsole()
	{
		Instance.SetEnableConsole(false);
	}

    [ConsoleMethod("unlock_weapon", "Unlock all weapons")]
    public static void UnlockAllWeapons()
    {
        Player player = PlayerData.Instance.GetPlayerData();
        player.jevalin = true;
        player.knuckles = true;
        player.sickles = true;
        player.sword = true;
        PlayerData.Instance.PlayerSetup(player);
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("Hikari"));
    }

    [ConsoleMethod("last_boss", "Teleport to last boss")]
    public static void TeleportToLastBoss()
    {
        TransitionManager.Instance.SceneFadeIn(0.5f, () =>
            SceneManager.LoadScene("F_Cutscene 1"));
    }
}
