using Unity.Netcode;
using UnityEngine;

public class BatSpawn : NetworkBehaviour
{
    public NetworkObject[] bat_prefab_list_PC;
    public NetworkObject[] bat_prefab_list_VR;

    public Transform spawnPosBat1;
    public Transform spawnPosBat2;

    void Start()
    {
        if (!IsServer) return;
        Debug.Log("BatSpawn called");

        if (staticClass.PCOn == true)
        {
            for (int i = 0; i < 1; i++)
            {
                //instantiate one of the bat prefabs from the list
                foreach (NetworkObject bat in bat_prefab_list_PC)
                {
                    NetworkObject newBat = Instantiate(bat, spawnPosBat1.position, Quaternion.identity);
                    newBat.GetComponent<NetworkObject>().Spawn();
                    newBat.gameObject.name = "BatP1";
                    Debug.Log("BatSpawn p1");

                    NetworkObject newBat2 = Instantiate(bat, spawnPosBat2.position, Quaternion.identity);
                    newBat2.GetComponent<NetworkObject>().Spawn();
                    newBat2.gameObject.name = "BatP2";
                    Debug.Log("BatSpawn p2");
                }
            }
        }
        else if (staticClass.VROn == true)
        {
            for (int i = 0; i < 1; i++)
            {
                //instantiate one of the bat prefabs from the list
                foreach (NetworkObject bat in bat_prefab_list_VR)
                {
                    NetworkObject newBat = Instantiate(bat, spawnPosBat1.position, Quaternion.identity);
                    newBat.GetComponent<NetworkObject>().Spawn();
                    newBat.gameObject.name = "BatP1";
                    Debug.Log("BatSpawn p1");

                    NetworkObject newBat2 = Instantiate(bat, spawnPosBat2.position, Quaternion.identity);
                    newBat2.GetComponent<NetworkObject>().Spawn();
                    newBat.gameObject.name = "BatP2";
                    Debug.Log("BatSpawn p2");
                }
            }
        }
    }

}
