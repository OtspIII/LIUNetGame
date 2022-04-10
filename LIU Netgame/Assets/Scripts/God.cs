using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public static class God
{
    public static GameManager GM;
    public static NetworkManager NM;
    public static LevelManager LM;
    public static LibraryController Library;
    public static TextMeshProUGUI HPText;
    public static TextMeshProUGUI StatusText;
    
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
