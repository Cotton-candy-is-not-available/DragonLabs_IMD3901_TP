using UnityEngine;

public class activatePressurePlates : MonoBehaviour
{

    public GameObject singlePlayerPlates;
    public GameObject multiPlayerPlates;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (staticClass.SingleON)//if single player was chosen
        {
            singlePlayerPlates.SetActive(true);//turn on single player pressure plates
            multiPlayerPlates.SetActive(false);//turn off multiplayer player pressure plates

        }
        else
        {
            singlePlayerPlates.SetActive(false);//turn off sinlge player plates
        }


        //multiplayer pressure plates will be on by default therefore there is no need to check if online was chosen
    }

   
}
