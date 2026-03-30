using Unity.Netcode;
using UnityEngine;

public class tempPressurePlate : NetworkBehaviour
{
    public ChooseGame chooseGame_access;
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        chooseGame_access.switchScenesNetServerRpc(sceneName);
        Debug.Log("STEPPED ON PRESSURE PLATE");
    }

}
