using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public List<PlayerSpawnController> PSpawns;
    public List<ItemSpawnController> ISpawns;
    public PlayerSpawnController LastPS;

    void Awake()
    {
        God.LM = this;
    }

    public PlayerSpawnController GetPSpawn(FirstPersonController pc)
    {
        PlayerSpawnController r = PSpawns[Random.Range(0, PSpawns.Count)];
        if (LastPS != null) PSpawns.Add(LastPS);
        if(PSpawns.Count > 1) PSpawns.Remove(r);
        LastPS = r;
        return r;
    }
}
