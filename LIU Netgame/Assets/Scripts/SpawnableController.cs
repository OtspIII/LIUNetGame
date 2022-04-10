using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnableController : MonoBehaviour
{
    public ItemSpawnController Spawner;
    
    public void Setup(ItemSpawnController s)
    {
        Spawner = s;
    }

    public void GetTaken(FirstPersonController pc)
    {
        TakeEffects(pc);
        Spawner.TakenFrom(pc);
        Destroy(gameObject);
    }

    public virtual void TakeEffects(FirstPersonController pc)
    {
        Debug.Log("TOOK IT: " + gameObject.name);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        Debug.Log("OCE: " + pc);
        if(pc != null)
            GetTaken(pc);
    }
}
