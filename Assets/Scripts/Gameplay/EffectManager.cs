using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField]
    private Effect[] effects;
    
    public void Spawn(string name, Vector3 position, Quaternion rotation)
    {
        foreach (Effect effect in effects)
        {
            if (name == effect.name)
            {
                Instantiate(effect.effect, position, rotation);
            }
        }
    }
}

[Serializable]
public class Effect
{
    public string name;
    public GameObject effect;
}
