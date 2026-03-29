using UnityEngine;

public class pressurePlateSwitchScenes : MonoBehaviour
{
    public ChooseGame sceneManager;
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        //if a player stands on/goes into plate collider
        sceneManager.switchScenesNetServerRpc(sceneName);
    }
}
