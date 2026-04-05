using UnityEngine;

public class localPlayerLobbySpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject localPlayer;
    [SerializeField] Vector3 lobbyStartPos;


    void Start()
    {
        if (staticClass.SingleON)
        {//if single player mode was chosen
            localPlayer = GameObject.FindWithTag("Player");//find the player

            Debug.Log("local player: " + localPlayer);

            lobbyStartPos = new Vector3(0.05f, 0.46f, -19.02f);//set the spawn position

            localPlayer.transform.position = lobbyStartPos;//make the player spawn here
            Debug.Log("local player transform: " + localPlayer.transform.position);

        }
    }

}
