using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstPersonController : NetworkBehaviour
{
    public Camera Eyes;
    public Rigidbody RB;
    public NetworkRigidbody NRB;
    public float MouseSensitivity = 3;
    public float WalkSpeed = 10;
    public float JumpPower = 7;
    public List<GameObject> Floors;
//    public int ID;
    
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<float> XRot = new NetworkVariable<float>();
    public NetworkVariable<float> YRot = new NetworkVariable<float>();
    public NetworkVariable<int> HP = new NetworkVariable<int>();

    
    void Start()
    {
//        ID = Random.Range(0, 100);
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
            Eyes.enabled = false;

        if (IsServer)
        {
            Reset();
        }
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
            Debug.Log("A");
            GetStartSpotServerRPC();
        }
    }
    
    [ServerRpc]
    void GetStartSpotServerRPC(ServerRpcParams rpcParams = default)
    {
        Position.Value = new Vector3(-1,-1,0);
        Debug.Log("B");
    }
    
    [ServerRpc]
    void UpdatePosServerRPC(Vector3 move,bool jump, float xRot,float yRot)
    {
        HandleMove(move,jump,xRot,yRot);
        Position.Value = transform.position;
        XRot.Value = transform.rotation.y;
        YRot.Value = Eyes.transform.rotation.x;
    }


    
    void Update()
    {
        if (!IsOwner) return;
        God.HPText.text = HP.Value + "/" + 100;
        God.StatusText.text = "Gun";
        //Lobbyist.Text = transform.position.ToString();
        float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
        float yRot = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        
        Vector3 move = Vector3.zero;
        bool jump = false;

        if (WalkSpeed > 0)
        {
            
            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;
//            move = move.normalized * WalkSpeed;
            if (JumpPower > 0 && Input.GetKeyDown(KeyCode.Space))
                jump = true;
//                move.y = JumpPower;
//            else
//                move.y = RB.velocity.y;
//            RB.velocity = move;
        }
        HandleMove(move,jump,xRot,yRot);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(Eyes.transform.position + Eyes.transform.forward,Eyes.transform.rotation);
        }
        
        
    }

    public void HandleMove(Vector3 move,bool jump, float xRot,float yRot)
    {
        if (!IsServer)
        {
            UpdatePosServerRPC(move,jump,xRot,yRot);
//            return;
        }
        move = move.normalized * WalkSpeed;
        if (jump && OnGround())
            move.y = JumpPower;
        else
            move.y = RB.velocity.y;
        RB.velocity = move;
        transform.Rotate(0,xRot,0);
        Eyes.transform.Rotate(yRot,0,0);
    }
    
    public void Shoot(Vector3 pos,Quaternion rot)
    {
        if (!IsServer)
        {
            ShootServerRPC(pos,rot);
            return;
        }
        ServerShoot(pos,rot);
    }
    
    [ServerRpc]
    void ShootServerRPC(Vector3 pos,Quaternion rot)
    {
        ServerShoot(pos,rot);
    }

    void ServerShoot(Vector3 pos, Quaternion rot)
    {
        ProjectileController p = Instantiate(God.Library.Projectile, pos,rot);
        p.Setup(this);
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

    public void Reset()
    {
        HP.Value = 100;
    }

    public void TakeDamage(int amt)
    {
        HP.Value -= amt;
        ParticleGnome pg = Instantiate(God.Library.Blood, transform.position, Quaternion.identity);
        pg.Setup(10);
    }
}


