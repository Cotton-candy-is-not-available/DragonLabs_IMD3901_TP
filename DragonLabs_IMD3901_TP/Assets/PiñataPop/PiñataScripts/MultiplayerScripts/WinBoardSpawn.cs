using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class WinBoardSpawn : NetworkBehaviour
{
    public TimerControllerNet timerControllerNet_acces;
    public NetworkObject[] board_prefab;
    public Transform spawnPt;

    public ChooseGame chooseGame_access;
    public WinnerManagerNet winnerManagerNet_acces;

    bool once = false;

    private void Update()
    {
        //calculate the winner once the winboard has been spawned
        if (winnerManagerNet_acces != null && once != true)
        {
            winnerManagerNet_acces.calculateWinnerServerRpc();
            Debug.Log("called calcualteWinnerServerRpc");
            once = true;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnWinBoardServerRpc()
    {
        if (!IsServer) return;
        Debug.Log("CALLED SpawnWinBoardServerRpc");

        foreach (NetworkObject board in board_prefab)
        {
            NetworkObject newBoard = Instantiate(board, spawnPt.position, Quaternion.identity);
            newBoard.GetComponent<NetworkObject>().Spawn();
            Debug.Log("SPAWNED THE WINBOARD");
        }

        //wait for 20 seconds and then switch scenes back to lobby
        StartCoroutine(waitToSwitch());
    }

    IEnumerator waitToSwitch()
    {
        Debug.Log("called waitToSwitch");
        //yield on a new YieldInstruction that waits for 15 seconds.
        yield return new WaitForSeconds(15);

        //change scenes back to the lobby for the host and client 
        chooseGame_access.switchScenesNetServerRpc("Lobby");
    }

}
