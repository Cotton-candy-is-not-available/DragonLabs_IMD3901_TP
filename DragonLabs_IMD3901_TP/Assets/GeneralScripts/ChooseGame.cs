using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using UnityEngine.InputSystem;

public class ChooseGame : MonoBehaviour
{
    private void OnEnable()
    {
        playerInteraction.onInteract += HandleInteraction;
    }

    private void OnDisable()
    {
        playerInteraction.onInteract -= HandleInteraction;
    }

    private void HandleInteraction(GameObject interactableObj)
    {
        if (interactableObj == gameObject)
        {
            Debug.Log("This game was picked");
            if (gameObject.name.Contains("screen1"))
                Debug.Log("Teleporting to game: screen1");
        }
    }
}
