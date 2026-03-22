using UnityEngine;
using Unity.Netcode;

public class VRNetcodeManager : NetworkBehaviour
{
    public static VRNetcodeManager Instance;

    public Camera VrCamera;

    // Update is called once per frame
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            VrCamera.enabled = false;
        }

        //check that there is only one object in the scene with this script

        //if there is another object with this script set this as player 2
        if (Instance != null)
        {
            Instance = this;
            gameObject.tag = "Player2";//give them the player 1 tag

            VrCamera.tag = "p2Camera";//set camera tags
            Debug.Log("P2 Camera tag: " + VrCamera.tag);


        }
        else//otherwise they are player 1
        {
            Instance = this;
            gameObject.tag = "Player1";//give them the player 1 tag

            VrCamera.tag = "p1Camera";//set camera tags
            Debug.Log("P1 Camera tag: " + VrCamera.tag);


        }
    }
}
