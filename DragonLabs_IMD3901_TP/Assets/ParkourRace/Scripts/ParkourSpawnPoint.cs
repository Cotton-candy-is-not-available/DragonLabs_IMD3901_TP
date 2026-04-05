using UnityEngine;

public class ParkourSpawnPoint : MonoBehaviour
{
    public GameObject p1;

    public Transform p1StartPos;

    private void Start()
    {
        //find both player in the scene
        p1 = GameObject.FindWithTag("Player");

        //Set players start positions
        p1.transform.transform.position = p1StartPos.position;
    }

    void Update()
    {
        
    }
}
