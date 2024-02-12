using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    
}

[Serializable]
public class Player
{
    public int id;

    public bool genbu;
    public bool suzaku;
    public bool seiryu;
    public bool byakko;

    public bool knuckles;
    public bool sickles;
    public bool jevalin;
    public bool sword;

    public DateTime startDate;
    public DateTime lastDate;
}
