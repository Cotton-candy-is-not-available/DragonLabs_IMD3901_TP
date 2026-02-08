using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkHandAnimations : NetworkBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;
    public Animator handAnimator;

    void Update()
    {
        if (IsOwner)
        {
            //read the grip and trigger values
            float trigger = triggerValue.action.ReadValue<float>();
            float grip = gripValue.action.ReadValue<float>();

            //change the trigger value in the animator
            handAnimator.SetFloat("Trigger", trigger);
            handAnimator.SetFloat("Grip", grip);
        }
    }
}
