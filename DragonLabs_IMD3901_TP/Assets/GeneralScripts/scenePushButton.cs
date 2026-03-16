using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class scenePushButton : MonoBehaviour
{
    public float interactRange = 2f;
    //Get both palyer cameras after calling join button in start script or start relay script
    public Camera player1Cam;
    public Camera player2Cam;

    public Camera localPlayerCam;

    public float range = 1f;
    [SerializeField] float speed = 2f;
    private Vector3 startPos;

    private bool isPressed = false;
    private bool isAnimating = false;

    // public startGame LANJoinBool
    // public startGame singlePlayerBool

    // public startRelay relayJoinBool


    [SerializeField] ChooseGame switchScenes;
    [SerializeField] string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //If single player selected

        if (localPlayerCam != null)
        {
            Ray localRay = new Ray(localPlayerCam.transform.position, localPlayerCam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(localRay, out hit, interactRange))
            {
                if (hit.collider.CompareTag("button"))
                {
                    Debug.Log("button seen"); 

                    if (Keyboard.current.pKey.wasPressedThisFrame)
                    {
                        Debug.Log("key pressed");

                        localPress();//calls aniamtion bools
                    }
                }
            }


            //if collsion with VR hand


            //Animated button up and down
            Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    switchScenes.switchScenes(sceneName);

                    isPressed = false;
                }
                else
                    isAnimating = false;
            }
        }

        //if (player1Cam!=null && player2Cam !=null)//as long as player cameras exists
        //{
        //    Ray p1Ray = new Ray(player1Cam.transform.position, player1Cam.transform.forward);
        //    Ray p2Ray = new Ray(player2Cam.transform.position, player2Cam.transform.forward);
        //    RaycastHit hit;
        //    if (Physics.Raycast(p1Ray, out hit, interactRange))
        //    {
        //        if (hit.collider.CompareTag("button"))
        //        {
        //            if (Keyboard.current.iKey.wasPressedThisFrame)
        //            {
        //                Press();
        //            }
        //        }
        //    }

        //    Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
        //    transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        //    if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        //    {
        //        if (isPressed)
        //            isPressed = false;
        //        else
        //            isAnimating = false;
        //    }
        //}
        //else if(player1Cam == null && player2Cam == null)//if player cameras dont exists; 
        //{
           // if(relayJoinBool || LANJoinBool){//are true
                //player1Cam = gameObject.FindObjectWithTag("P1Cam");
                //player2Cam = gameObject.FindObjectWithTag("P2Cam");

           //}

        //}
    }

    public void localPress()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("Local Pressed");
    }
    public void Press()
    {
        isPressed = true;
        isAnimating = true;
        Debug.Log("Pressed");
    }
}
