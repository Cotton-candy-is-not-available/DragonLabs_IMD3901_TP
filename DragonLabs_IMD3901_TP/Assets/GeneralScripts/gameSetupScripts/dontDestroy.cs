using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       //might need to check if there is already a player prefab in the scen


        DontDestroyOnLoad(gameObject);

    }

   
}
