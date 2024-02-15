using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [SerializeField]
    private Player _player;

    public void PlayerSetup(Player player)
    {
        _player = player;
    }

    public Player GetPlayerData() => _player;
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

    public Player(int id, bool genbu, bool suzaku, bool seiryu, bool byakko, bool knuckles, bool sickles, bool jevalin, bool sword, DateTime startDate, DateTime lastDate)
    {
        this.id = id;
        this.genbu = genbu;
        this.suzaku = suzaku;
        this.seiryu = seiryu;
        this.byakko = byakko;
        this.knuckles = knuckles;
        this.sickles = sickles;
        this.jevalin = jevalin;
        this.sword = sword;
        this.startDate = startDate;
        this.lastDate = lastDate;
    }

    public Player(Player player)
    {
        id = player.id;
        genbu = player.genbu;
        suzaku = player.suzaku;
        seiryu = player.seiryu;
        byakko = player.byakko;
        knuckles = player.knuckles;
        sickles = player.sickles;
        jevalin = player.jevalin;
        sword = player.sword;
        startDate = player.startDate;
        lastDate = player.lastDate;
    }
}
