using Unity.Netcode;
using UnityEngine;

public class cupSpawner : NetworkBehaviour
{
    //Need 2 prefab lists each because cups have tag inside on whater they are player 1 or 2
    public NetworkObject[] cupPrefabListPCPlayer1;
    public NetworkObject[] cupPrefabListPCPlayer2;

    public NetworkObject[] cupPrefabListVRPlayer1;
    public NetworkObject[] cupPrefabListVRPlayer2;

    //player 1 side
    public Transform spawnPos1Cup;
    public Transform spawnPos2Cup;
    public Transform spawnPos3Cup;

    //player 2 side
    public Transform spawnPos4Cup;
    public Transform spawnPos5Cup;
    public Transform spawnPos6Cup;

    void Start()
    {
        if (!IsServer) return;
        Debug.Log("Cup Spawn called");
        //spawn PC cups
        if (staticClass.PCOn == true)
        {
            for (int i = 0; i < 1; i++)
            {
                //instantiate the player 1 cup prefab from the list

                //For player 1
                foreach (NetworkObject cup in cupPrefabListPCPlayer1)
                {
                    NetworkObject newCup1 = Instantiate(cup, spawnPos1Cup.position, Quaternion.identity);
                    newCup1.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 1");

                    NetworkObject newCup2 = Instantiate(cup, spawnPos2Cup.position, Quaternion.identity);
                    newCup2.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 2");


                    NetworkObject newCup3 = Instantiate(cup, spawnPos3Cup.position, Quaternion.identity);
                    newCup3.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 3");
                }


                //For player 2
                foreach (NetworkObject cup in cupPrefabListPCPlayer2)
                {
                    NetworkObject newCup1 = Instantiate(cup, spawnPos4Cup.position, Quaternion.identity);
                    newCup1.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 4");

                    NetworkObject newCup2 = Instantiate(cup, spawnPos5Cup.position, Quaternion.identity);
                    newCup2.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 5");


                    NetworkObject newCup3 = Instantiate(cup, spawnPos6Cup.position, Quaternion.identity);
                    newCup3.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 6");
                }
            }
        }

        //spawn VR cups
        else if (staticClass.VROn == true)
        {
            for (int i = 0; i < 1; i++)
            {
                //instantiate the player 1 cup prefab from the list

                //For player 1
                foreach (NetworkObject cup in cupPrefabListVRPlayer1)
                {
                    NetworkObject newCup1 = Instantiate(cup, spawnPos1Cup.position, Quaternion.identity);
                    newCup1.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 1");

                    NetworkObject newCup2 = Instantiate(cup, spawnPos2Cup.position, Quaternion.identity);
                    newCup2.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 2");


                    NetworkObject newCup3 = Instantiate(cup, spawnPos3Cup.position, Quaternion.identity);
                    newCup3.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 3");
                }


                //For player 2
                foreach (NetworkObject cup in cupPrefabListVRPlayer2)
                {
                    NetworkObject newCup1 = Instantiate(cup, spawnPos4Cup.position, Quaternion.identity);
                    newCup1.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 4");

                    NetworkObject newCup2 = Instantiate(cup, spawnPos5Cup.position, Quaternion.identity);
                    newCup2.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 5");


                    NetworkObject newCup3 = Instantiate(cup, spawnPos6Cup.position, Quaternion.identity);
                    newCup3.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("cup spawn 6");
                }
            }
        }
    }








}
