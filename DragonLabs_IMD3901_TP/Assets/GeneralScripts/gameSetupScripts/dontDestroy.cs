using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static dontDestroy Instance;

    void Start()
    {
        //check that there is only one object in the scene with this script
        if (Instance != null)
        {
            Destroy(gameObject);//if there is another object with this script destroy it
            return;
        }
        // end of new code

        Instance = this;

        DontDestroyOnLoad(gameObject);

    }

   
}
