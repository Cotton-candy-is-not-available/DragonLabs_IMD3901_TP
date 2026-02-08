using UnityEngine;
using Unity.Netcode;

public class VRNetcodeManager : NetworkBehaviour
{
    public Camera VrCamera;

    // Update is called once per frame
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            VrCamera.enabled = false;
        }
    }
}
