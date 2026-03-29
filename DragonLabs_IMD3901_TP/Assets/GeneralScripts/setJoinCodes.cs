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
    //public GameObject IPAddressOverlayCanvas;
    //public TextMeshProUGUI IPAddressDisplayTextOverlay;
    public TextMeshProUGUI IPAddressDisplayTextWorld;

    public void Start()
    {
        //RELAY
        if (staticClass.RelayOn)
        { //if relay was chosen
            Debug.Log("Relay ONNN");

            joinCodeOverlayCanvas.SetActive(true);//if relay static variable is true
            joinCodeWorldCanvas.SetActive(true);//if relay static variable is true

            joinCodeDisplayTextOverlay.text = staticClass.staticJoinCodeVariable;
            joinCodeDisplayTextWorld.text = staticClass.staticJoinCodeVariable;

            textToCopy = joinCodeDisplayTextOverlay.text;//set the text to copy to the join code that we got from the host stating the scene
        }
        //LAN

        else if (staticClass.LANOn)//if LAN static variable is true
        {
            Debug.Log("LAN ONNN");
            joinCodeDisplayTextWorld.text = staticClass.staticIPAddressVariable;

            joinCodeDisplayTextOverlay.text = staticClass.staticIPAddressVariable;
            IPAddressDisplayTextWorld.text = staticClass.staticIPAddressVariable;

            joinCodeWorldCanvas.SetActive(true);

            //IPAddressOverlayCanvas.SetActive(true);
           

            textToCopy = joinCodeDisplayTextOverlay.text;//set the text to copy to the join code that we got from the host stating the scene

        }

        else if (staticClass.SingleON)
        {
            Debug.Log(" Single ON");

            //turn off the multiplayer displays for the join codes
            //IPAddressOverlayCanvas.SetActive(false);
            joinCodeOverlayCanvas.SetActive(false);
            joinCodeWorldCanvas.SetActive(false);
        }
    }

    public void copyJoinCode()
    {
        // Copy the text to the clipboard.

        GUIUtility.systemCopyBuffer = textToCopy;
        Debug.Log("copied code: " + textToCopy);

    }

}
