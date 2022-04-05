using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NetworkManager NM;
    public TextMeshProUGUI StatusText;
    public NetStatus NStatus;
    public string PlayerDebug;

    void Awake()
    {
        God.GM = this;
        if (NM != null) God.NM = NM;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NM.StartClient();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            NM.StartHost();
        }

        NStatus = God.GetStatus();
        StatusText.text = NStatus.ToString() + " / " + PlayerDebug;
    }
    
    
}
