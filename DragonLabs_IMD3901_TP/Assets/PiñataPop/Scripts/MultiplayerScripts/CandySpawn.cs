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

    /* [ServerRpc(RequireOwnership = false)]
    public void DropCandyServerRpc()
    {
        //confettiPopParticles.Play();
        //candy.gameObject.SetActive(true);
       
        foreach (NetworkObject candy in candyList)
        {
            //candy.gameObject.SetActive(true);
            if (!candy.IsSpawned)
            {
                candy.Spawn(true);
                Debug.Log("candy spawned");
            }
        }
        
        Debug.Log("DropCandyServerRpc has been called");

        confettiPopParticles.Play();
        ShowCandyClientRpc();
    }

    [ClientRpc]
    public void ShowCandyClientRpc()
    {
        Debug.Log("ShowCandyClientRpc has been called");

        confettiPopParticles.Play();
        //candy.gameObject.SetActive(true);

    }*/


}
