using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class God
{
    public static GameManager GM;
    public static NetworkManager NM;
    
    public static NetStatus GetStatus()
    {
        return NetworkManager.Singleton.IsHost ? NetStatus.Host : NetworkManager.Singleton.IsServer ? NetStatus.Server : NetStatus.Client;
    }
}


public enum NetStatus
{
    None,
    Host,
    Server,
    Client
}
