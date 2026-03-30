using UnityEngine;

public class pressurePlateSwitchScenes : MonoBehaviour
{
    public ChooseGame sceneManager;
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        //if a player stands on/goes into plate collider
        if (staticClass.SingleON)//if single player mode is active, use single player switch scenes
        {
            sceneManager.switchScenes(sceneName);
        }
        else//else if online is active , use rpc switch scenes
        {
            sceneManager.switchScenesNetServerRpc(sceneName);
        }
    }
}
