using TMPro;
using UnityEngine;

public class setJoinCodes : MonoBehaviour
{
    public string textToCopy;

    [Header("Relay code text")]
    public GameObject joinCodeOverlayCanvas;
    public GameObject joinCodeWorldCanvas;
    public TextMeshProUGUI joinCodeDisplayTextOverlay;
    public TextMeshProUGUI joinCodeDisplayTextWorld;

    [Header("LAN text")]
    public GameObject IPAddressOverlayCanvas;
    public TextMeshProUGUI IPAddressDisplayTextOverlay;
    public TextMeshProUGUI IPAddressDisplayTextWorld;

    public void Start()
    {
        //RELAY
        if (staticClass.RelayOn)
        { //if relay was chosen
            joinCodeOverlayCanvas.SetActive(true);//if relay static variable is true
            joinCodeWorldCanvas.SetActive(true);//if relay static variable is true
            joinCodeDisplayTextOverlay.text = staticClass.staticJoinCodeVariable;
            joinCodeDisplayTextWorld.text = staticClass.staticJoinCodeVariable;
            textToCopy = staticClass.staticJoinCodeVariable;//set the text to copy to the join code that we got from the host stating the scene
        }
        //LAN

        if (staticClass.LANOn)//if LAN static variable is true
        {
            joinCodeWorldCanvas.SetActive(true);
            joinCodeDisplayTextWorld.text = staticClass.staticIPAddressVariable;

            IPAddressOverlayCanvas.SetActive(true);
            IPAddressDisplayTextOverlay.text = staticClass.staticIPAddressVariable;
            IPAddressDisplayTextWorld.text = staticClass.staticIPAddressVariable;
        }

        else
        {
            //turn off the multiplayer displays for the join codes
            IPAddressOverlayCanvas.SetActive(false);
            joinCodeOverlayCanvas.SetActive(false);
        }
    }

    public void copyJoinCode()
    {
        // Copy the text to the clipboard.

        GUIUtility.systemCopyBuffer = textToCopy;
        Debug.Log("copied code: " + textToCopy);

    }

}
