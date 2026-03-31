using Unity.Netcode;
using UnityEngine;

public class CandySpawn : NetworkBehaviour
{
    public NetworkObject[] candy_prefab_list_PC;
    public NetworkObject[] candy_prefab_list_VR;


    [ServerRpc(RequireOwnership = false)]
    public void SpawnCandyServerRpc()
    {
        Debug.Log("SpawnCandyServerRpc called");

        if(staticClass.PCOn == true)
        {
            //instantiate one of the candy prefabs from the list
            foreach (NetworkObject candy in candy_prefab_list_PC)
            {
                //spawn on network
                for (int i = 0; i < 7; i++) //spawn 7 of each of the 3 candy prefabs in the list
                {
                    NetworkObject newCandy = Instantiate(candy, transform.position, Quaternion.identity);
                    newCandy.GetComponent<NetworkObject>().Spawn();
                    newCandy.DestroyWithScene = true;
                }
                //Debug.Log("candies all spawned HOST and CLIENT");
            }
        }
        else if(staticClass.VROn == true)
        {
            //instantiate one of the candy prefabs from the list
            foreach (NetworkObject candy in candy_prefab_list_VR)
            {
                //spawn on network
                for (int i = 0; i < 7; i++) //spawn 7 of each of the 3 candy prefabs in the list
                {
                    NetworkObject newCandy = Instantiate(candy, transform.position, Quaternion.identity);
                    newCandy.GetComponent<NetworkObject>().Spawn();
                    newCandy.DestroyWithScene = true;
                }
                //Debug.Log("candies all spawned HOST and CLIENT");
            }
        }

    }
}
