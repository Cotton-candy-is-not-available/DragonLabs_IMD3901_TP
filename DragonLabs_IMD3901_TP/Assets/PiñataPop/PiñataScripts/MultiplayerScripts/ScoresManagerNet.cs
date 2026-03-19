using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoresManagerNet : NetworkBehaviour
{
    //initialize text on the scoreboard
    public TextMeshProUGUI P1Hits_txt;
    public TextMeshProUGUI P1Candy_txt;
    public TextMeshProUGUI P2Hits_txt;
    public TextMeshProUGUI P2Candy_txt;

    public NetworkVariable<int> P1HitPoints;
    public NetworkVariable<int> P1CandyPoints;
    public NetworkVariable<int> P2HitPoints;
    public NetworkVariable<int> P2CandyPoints;


    public override void OnNetworkSpawn()
    {
        //set initial value
        P1HitPoints.Value = 0;
        P1CandyPoints.Value = 0;
        P2HitPoints.Value = 0;
        P2CandyPoints.Value = 0;

        //upate the values when they are changed
        P1HitPoints.OnValueChanged += OnP1HitPointsChanged;
        P1CandyPoints.OnValueChanged += OnP1CandyPointsChanged;
        P2HitPoints.OnValueChanged += OnP2HitPointsChanged;
        P2CandyPoints.OnValueChanged += OnP2CandyPointsChanged;

        //set initial value of all the texts when the object spawns
        UpdateP1HitPointsText(P1HitPoints.Value);
        UpdateP1CandyPointsText(P1CandyPoints.Value);
        UpdateP2HitPointsText(P2HitPoints.Value);
        UpdateP2CandyPointsText(P2CandyPoints.Value);

    }

    //PLAYER 1 HIT POINTS---------
    private void OnP1HitPointsChanged(int oldValue, int newValue)
    {
        UpdateP1HitPointsText(newValue);
    }
    private void UpdateP1HitPointsText(int value)
    {
        P1Hits_txt.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)] //host and client are able to ask the server to update the teampoints
    public void addP1HitPointServerRpc()
    {
        //increasae the value of points on both host and client since its a network variable
        P1HitPoints.Value += 1;
        Debug.Log("added p1 hit point");
    }

    //PLAYER 1 CANDY POINTS---------
    private void OnP1CandyPointsChanged(int oldValue, int newValue)
    {
        UpdateP1CandyPointsText(newValue);
    }
    private void UpdateP1CandyPointsText(int value)
    {
        P1Candy_txt.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)] 
    public void addP1CandyPointServerRpc()
    {
        P1CandyPoints.Value += 1;
        Debug.Log("added p1 candy point");
    }


    //PLAYER 2 HIT POINTS---------
    private void OnP2HitPointsChanged(int oldValue, int newValue)
    {
        UpdateP2HitPointsText(newValue);
    }
    private void UpdateP2HitPointsText(int value)
    {
        P2Hits_txt.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)] 
    public void addP2HitPointServerRpc()
    {
        P2HitPoints.Value += 1;
        Debug.Log("added p2 hit point");
    }


    //PLAYER 2 CANDY POINTS---------
    private void OnP2CandyPointsChanged(int oldValue, int newValue)
    {
        UpdateP2CandyPointsText(newValue);
    }
    private void UpdateP2CandyPointsText(int value)
    {
        P2Candy_txt.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)]
    public void addP2CandyPointServerRpc()
    {
        P2CandyPoints.Value += 1;
        Debug.Log("added p2 candy point");
    }


}
