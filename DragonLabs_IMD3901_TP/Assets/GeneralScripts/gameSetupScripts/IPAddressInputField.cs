using UnityEngine;

public class IPAddressInputField : MonoBehaviour
{

    public string inputText;

    //Accessign scripts
    public startGame VRBool;
    public startGame PCBool;

    public NetworkManagerHud PCNetManagerHud;
    public NetworkManagerHud VRNetManagerHud;



    public void GetIPAddressInput(string input)
    {

        if (VRBool.VRMode)//if vr mode was chosen
        {
            inputText = input;
            Debug.Log(input + "input");
            VRNetManagerHud.m_ConnectAddress = input;//change the VR netHud connected address
        }
        else if (PCBool.PCMode)//if PC mode was chosen
        {
            inputText = input;
            Debug.Log(input + "input");
            PCNetManagerHud.m_ConnectAddress = input;//change the PC netHud connected address
        }

        if(input == null)//use default IP address "127.0.0.1"
        {
            input = PCNetManagerHud.m_ConnectAddress;
            Debug.Log(input + "input");

        }


    }
}
