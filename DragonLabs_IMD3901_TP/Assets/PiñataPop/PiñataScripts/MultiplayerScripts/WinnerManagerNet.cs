using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class WinnerManagerNet : NetworkBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI winnerDisplayTXT;

    public NetworkVariable<int> p1_results;
    public NetworkVariable<int> p2_results;
    public NetworkVariable<int> winner; //1 = player 1, 2= player 2

    //access to scripts for game booleans and player points
    public ScoresManagerNet scoresManagerNet_access;

    public override void OnNetworkSpawn()
    {
        //set initial values
        p1_results.Value = 0;
        p2_results.Value = 0;
        winner.Value = 0;
    }

    private void Update()
    {
        if(!IsServer) return;
        //constantly update the timers for both the host and the client
        calculateWinnerServerRpc();
        updateWinnerBoardServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void calculateWinnerServerRpc()
    {
        p1_results.Value = scoresManagerNet_access.P1HitPoints.Value + scoresManagerNet_access.P1CandyPoints.Value;
        p2_results.Value = scoresManagerNet_access.P2HitPoints.Value + scoresManagerNet_access.P2CandyPoints.Value;

        if(p1_results.Value > p2_results.Value) //if p1 has more total points
        {
            winner.Value = 1;
        }
        else //if p2 has more total points
        {
            winner.Value = 2;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void updateWinnerBoardServerRpc()
    {

        if (winner.Value == 1)
        {
            winnerDisplayTXT.text = "PLAYER 1";
        }
        else if(winner.Value == 2)
        {
            winnerDisplayTXT.text = "PLAYER 2";
        }
        updateWinnerBoardClientRpc(); //update everything for the client as well
    }

    [ClientRpc]
    private void updateWinnerBoardClientRpc()
    {
        if (winner.Value == 1)
        {
            winnerDisplayTXT.text = "PLAYER 1";
        }
        else if (winner.Value == 2)
        {
            winnerDisplayTXT.text = "PLAYER 2";
        }
    }

}
