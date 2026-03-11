using UnityEngine;


public class PiñataController : MonoBehaviour
{
    public ScoresManager scoresManager_access;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.name == "BatP1")
        {
            Debug.Log("P1 hit the piñata");
            scoresManager_access.increaseP1Hits();
        }
        else if(collision.gameObject.name == "BatP2")
        {
            Debug.Log("P2 hit the piñata");
            scoresManager_access.increaseP2Hits();
        }


    }
}
