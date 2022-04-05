using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FirstPersonController : NetworkBehaviour
{
    public Camera Eyes;
    public Rigidbody RB;
    public float MouseSensitivity = 3;
    public float WalkSpeed = 10;
    public float JumpPower = 7;
    public List<GameObject> Floors;
    
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            NetSetup();
        }
        else
            Eyes.gameObject.SetActive(false);
    }
    
    public void NetSetup()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = new Vector3(1,1,0);
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            GetStartSpotServerRPC();
        }
    }
    
    [ServerRpc]
    void GetStartSpotServerRPC(ServerRpcParams rpcParams = default)
    {
        Position.Value = new Vector3(-1,-1,0);
    }


    
    void Update()
    {
        if (!IsOwner) return;
        float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
        float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        transform.Rotate(0,xRot,0);
        Eyes.transform.Rotate(yRot,0,0);

        if (WalkSpeed > 0)
        {
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;
            move = move.normalized * WalkSpeed;
            if (JumpPower > 0 && Input.GetKeyDown(KeyCode.Space) && OnGround())
                move.y = JumpPower;
            else
                move.y = RB.velocity.y;
            RB.velocity = move;
        }
        
        
    }

    private void LateUpdate()
    {
        if(IsOwner)
            Position.Value = transform.position;
        else
            transform.position = Position.Value;
    }

    public bool OnGround()
    {
        return Floors.Count > 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Floors.Contains(other.gameObject))
            Floors.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        Floors.Remove(other.gameObject);
    }
}


