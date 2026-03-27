using UnityEngine;
using Unity.Netcode;

public class ParkourSpawnPoint : NetworkBehaviour
{
    public GameObject p1;
    public GameObject p2;

    public Transform p1StartPos;
    public Transform p2StartPos;

    public override void OnNetworkSpawn()
    {
        //find both player in the scene
        p1 = GameObject.FindWithTag("Player1");
        p2 = GameObject.FindWithTag("Player2");

        //Set players start positions
        p1.transform.transform.position = p1StartPos.position;
        p2.transform.transform.position = p2StartPos.position;
    }

    void Update()
    {
        
    }
}
