using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody RB;
    public NetworkObject NO;
    public float Lifetime = 10;
    
    public void Setup(FirstPersonController pc)
    {
        NO.Spawn();
        RB.velocity = transform.forward * 10;
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0 && NetworkManager.Singleton.IsServer) 
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        if (pc != null)
        {
            pc.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
