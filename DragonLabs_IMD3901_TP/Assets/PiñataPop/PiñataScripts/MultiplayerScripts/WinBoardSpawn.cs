using Unity.Netcode;
using UnityEngine;

public class WinBoardSpawn : NetworkBehaviour
{
    public TimerControllerNet timerControllerNet_acces;
    public NetworkObject[] board_prefab;
    public Transform spawnPt;

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

    }
}
