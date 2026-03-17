using UnityEngine;
using Unity.Netcode;
    
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
        if (isAnimating)
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
        }
    }

    public void animateButton()
    {
        isPressed = true; 
        isAnimating = true;

        chooseGame_access.switchScenesNet(sceneName);
    }


    [ServerRpc(RequireOwnership = false)]
    public void PressButtonServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if(netObj_button.TryGetComponent<NetworkObject>(out netObj_button))
        {
            netObj_button.ChangeOwnership(rpcParams.Receive.SenderClientId);

            AnimateButtonClientRpc();
        }
    }


    [ClientRpc]
    private void AnimateButtonClientRpc()
    {
        if (IsOwner)
        {
            animateButton();
        }
    }




}
