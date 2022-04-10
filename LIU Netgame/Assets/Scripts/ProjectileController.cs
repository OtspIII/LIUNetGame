using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileController : MonoBehaviour
{
    public Rigidbody RB;
    public NetworkObject NO;
    
    public void Setup(FirstPersonController pc)
    {
        NO.Spawn();
        RB.velocity = transform.forward * 10;
    }

    private void OnCollisionEnter(Collision other)
    {
        FirstPersonController pc = other.gameObject.GetComponent<FirstPersonController>();
        if (pc != null)
        {
            pc.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
