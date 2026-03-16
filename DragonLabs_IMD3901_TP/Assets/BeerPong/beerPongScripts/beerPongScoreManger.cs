using TMPro;
using Unity.Netcode;
using UnityEngine;

public class beerPongScoreManger : NetworkBehaviour
{
    //initialize text on the scoreboard
    public TextMeshProUGUI p1PointsText;
    public TextMeshProUGUI p2PointsText;
    //public TextMeshProUGUI P2Hits_txt;
    //public TextMeshProUGUI P2Candy_txt;

    public NetworkVariable<int> p1PointsVariable;
    public NetworkVariable<int> p2PointsVariable;
    //public NetworkVariable<int> P2HitPoints;
    //public NetworkVariable<int> P2CandyPoints;


    public override void OnNetworkSpawn()
    {
        //set initial value
        p1PointsVariable.Value = 0;
        p2PointsVariable.Value = 0;
        //P2HitPoints.Value = 0;
        //P2CandyPoints.Value = 0;

        //upate the values when they are changed
        p1PointsVariable.OnValueChanged += OnP1PointsChanged;
        p2PointsVariable.OnValueChanged += OnP2PointsChanged;
        //P2HitPoints.OnValueChanged += OnP2HitPointsChanged;
        //P2CandyPoints.OnValueChanged += OnP2CandyPointsChanged;

        //set initial value of all the texts when the object spawns
        UpdateP1PointsText(p1PointsVariable.Value);
        UpdateP2PointsText(p2PointsVariable.Value);
        //UpdateP2HitPointsText(P2HitPoints.Value);
        //UpdateP2CandyPointsText(P2CandyPoints.Value);

    }

    //PLAYER 1 HIT POINTS---------
    private void OnP1PointsChanged(int oldValue, int newValue)
    {
        UpdateP1PointsText(newValue);
    }
    private void UpdateP1PointsText(int value)
    {
        p1PointsText.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)] //host and client are able to ask the server to update the teampoints
    public void addP1PointServerRpc()
    {
        //increasae the value of points on both host and client since its a network variable
        p1PointsVariable.Value += 1;
        Debug.Log("added p1 hit point");
    }

    //PLAYER 1 CANDY POINTS---------
    private void OnP2PointsChanged(int oldValue, int newValue)
    {
        UpdateP2PointsText(newValue);
    }
    private void UpdateP2PointsText(int value)
    {
        p2PointsText.text = value.ToString();
    }
    [ServerRpc(RequireOwnership = false)]
    public void addP2PointServerRpc()
    {
        p2PointsVariable.Value += 1;
        Debug.Log("added p1 candy point");
    }


    ////PLAYER 2 HIT POINTS---------
    //private void OnP2HitPointsChanged(int oldValue, int newValue)
    //{
    //    UpdateP2HitPointsText(newValue);
    //}
    //private void UpdateP2HitPointsText(int value)
    //{
    //    P2Hits_txt.text = value.ToString();
    //}
    //[ServerRpc(RequireOwnership = false)]
    //public void addP2HitPointServerRpc()
    //{
    //    P2HitPoints.Value += 1;
    //    Debug.Log("added p2 hit point");
    //}


    ////PLAYER 2 CANDY POINTS---------
    //private void OnP2CandyPointsChanged(int oldValue, int newValue)
    //{
    //    UpdateP2CandyPointsText(newValue);
    //}
    //private void UpdateP2CandyPointsText(int value)
    //{
    //    P2Candy_txt.text = value.ToString();
    //}
    //[ServerRpc(RequireOwnership = false)]
    //public void addP2CandyPointServerRpc()
    //{
    //    P2CandyPoints.Value += 1;
    //    Debug.Log("added p2 candy point");
    //}


}