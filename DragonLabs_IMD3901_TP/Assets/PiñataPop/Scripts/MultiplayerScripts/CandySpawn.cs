using Unity.Netcode;
using UnityEngine;

public class CandySpawn : NetworkBehaviour
{
    public NetworkObject[] candy_prefab_list;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnCandyServerRpc()
    {
        Debug.Log("SpawnCandyServerRpc called");

        //instantiate one of the candy prefabs from the list
        foreach (NetworkObject candy in candy_prefab_list)
        {
            //spawn on network
            for(int i = 0; i < 7; i++) //spawn 7 of each of the 3 candy prefabs in the list
            {
                NetworkObject newCandy = Instantiate(candy, transform.position, Quaternion.identity);
                newCandy.GetComponent<NetworkObject>().Spawn();
            }
            //Debug.Log("candies all spawned HOST and CLIENT");
        }
    }
}
