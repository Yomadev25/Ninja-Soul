using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField]
    private Effect[] effects;
    
    public GameObject Spawn(string name, Vector3 position, Quaternion rotation)
    {
        foreach (Effect effect in effects)
        {
            if (name == effect.name)
            {
                return Instantiate(effect.effect, position, rotation);
            }
        }

        return null;
    }
}

[Serializable]
public class Effect
{
    public string name;
    public GameObject effect;
}
