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
            NetworkObject newCandy = Instantiate(candy, transform.position, Quaternion.identity);

            //spawn on network
            newCandy.Spawn();
            Debug.Log("candy spawned");
        }
    }



    /* [ServerRpc(RequireOwnership = false)]
     public void DespawnCandyServerRpc()
     {


     }*/

}
