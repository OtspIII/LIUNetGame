using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    public ProjectileController Projectile;
    public ParticleGnome Blood;
    
    void Awake()
    {
        God.Library = this;
    }
}
