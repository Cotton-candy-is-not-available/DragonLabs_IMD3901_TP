using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class minigameButtonAnimate : NetworkBehaviour
{
    public float pressDistance = 2;
    public float pressSpeed = 3;

    Vector3 startPos;
    Vector3 targetPos;

    bool isAnimating = false;
    bool isPressed = false;

    private NetworkObject netObj_button;
    public ChooseGame chooseGame_access;
    public string sceneName;


    private void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
        netObj_button = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        /*if (isAnimating)
        {
            Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    isPressed = false;
                }
                else
                {
                    isAnimating = false;
                }
            }
        }*/
    }

    [ServerRpc(RequireOwnership = false)]

    public void animateButtonServerRpc(NetworkObjectReference specificButton)
    {
        if (specificButton.TryGet(out NetworkObject networkObject))
        {
            Debug.Log("ACCESSED!!!!!!!!!!!!!!!!!!!!!!!!!!");

            //set animaiong and pressing bools to true
            isPressed = true;
            isAnimating = true;

            if (isAnimating)
            {
                Debug.Log("TILE IS ANIMATING");

                Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
                //specificButton.= Quaternion.Lerp(transform.localPosition, target, pressSpeed * Time.deltaTime);
                networkObject.transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.localPosition, target) < 0.001f)
                {
                    if (isPressed)
                    {
                        isPressed = false;
                    }
                    else
                    {
                        isAnimating = false;
                    }
                }
            }


        }

















        /*

        isPressed = true;
        isAnimating = true;

        if (isAnimating)
        {
            Vector3 target = isPressed ? startPos - Vector3.up * pressDistance : startPos;
            //specificButton.TryGet<NetworkTransform>(). = Vector3.MoveTowards(transform.localPosition, target, pressSpeed * Time.deltaTime);


            if (Vector3.Distance(transform.localPosition, target) < 0.001f)
            {
                if (isPressed)
                {
                    isPressed = false;
                }
                else
                {
                    isAnimating = false;
                }
            }
        }
        */

    }


    [ServerRpc(RequireOwnership = false)]
    public void PressButtonServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if(netObj_button.TryGetComponent<NetworkObject>(out netObj_button))
        {
            netObj_button.ChangeOwnership(rpcParams.Receive.SenderClientId);

            //AnimateButtonClientRpc();
        }
    }

    /*
    [ClientRpc]
    private void AnimateButtonClientRpc()
    {
        if (IsOwner)
        {
            animateButton();
        }
    }
    */

    //tell the server we want to change scenes now
    [ServerRpc(RequireOwnership = false)] //allows for both the host or the client to call the function
    public void switchSceneOnButtonServerRpc()
    {
        chooseGame_access.switchScenesNetServerRpc(sceneName);
        Debug.Log("Switched the scene on button press!!!");
        changeSceneForClientRpc();
    }

    [ClientRpc]
    private void changeSceneForClientRpc() //run the swithc scenes function on the client's side
    {
        if (IsOwner)
        {
            chooseGame_access.switchScenesNetServerRpc(sceneName);
        }
    }




}
