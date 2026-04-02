using UnityEngine;

public class activateLobbyObjects : MonoBehaviour
{

    public GameObject singlePlayerPlates;
    public GameObject multiPlayerPlates;

    public GameObject NetAudioManager;
    public GameObject singleAudioManager;

    public GameObject SinglePCPlayer;
    public GameObject[] SinglePCPlayerList;


    public GameObject SingleVRPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (staticClass.SingleON)//if single player was chosen
        {
            singlePlayerPlates.SetActive(true);//turn on single player pressure plates
            singleAudioManager.SetActive(true);//turn on single player audio manager

            multiPlayerPlates.SetActive(false);//turn off multiplayer  pressure plates
            NetAudioManager.SetActive(false);//turn off multiplayer audio manager 

            if (staticClass.PCOn)//if pc mode was chosen
            {
                int randomNum = Random.Range(0, 4);//choose a random player from 0 to 3 in the array

                Debug.Log("random Num: " + randomNum);
                SinglePCPlayerList[randomNum].SetActive(true);//turn the player on
                //SinglePCPlayer.SetActive(true);//turn on PC player prefab
            }
            else if(staticClass.VROn)//if VR mode was chosen
            {
                SingleVRPlayer.SetActive(true);//turn on VR player prefab

            }
        }
        else
        {
            singlePlayerPlates.SetActive(false);//turn off sinlge player plates
            singleAudioManager.SetActive(false);//turn off single player audio manager

        }


        //multiplayer pressure plates and audio manager will be on by default therefore there is no need to check if online was chosen
    }

   
}
