using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenbuManager : Singleton<GenbuManager>
{
    [SerializeField]
    private Event[] _events;

    [Header("Stage 1")]
    public Hut[] huts;
    public bool clearAllHuts;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += SceneLoaded;        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene s, LoadSceneMode e)
    {
        if (s.name == "HUD") return;

        if (s.name == "Genbu_1")
        {
            if (clearAllHuts)
            {                
                //Camera show exit way to next scene
            }
        }

        if (!s.name.Contains("Genbu"))
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Invoke(nameof(InitStage), 1f);
    }

    private void InitStage()
    {
        if (SceneManager.GetActiveScene().name != "Genbu_1") return;
        EventManager.Instance.ActivateEvent(_events[0]);
    }

    public void SetHut(int id, bool isClear)
    {
        huts[id].isClear = isClear;

        if (huts.All(x => x.isClear))
        {
            EventManager.Instance.ArchieveEvent(_events[0]);
            clearAllHuts = true;
        }
    }

    [Serializable]
    public class Hut
    {
        public int id;
        public bool isClear;
    }
}
