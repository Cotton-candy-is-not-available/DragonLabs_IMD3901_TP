using UnityEngine;

public class IPAddressInputField : MonoBehaviour
{
    //Attached to ip address input field
    public string inputText;

    public startGame VRBool;
    public startGame PCBool;

    public NetworkManagerHud NetManagerHud;



    public void GetIPAddressInput(string input)
    {

        if (VRBool.VRMode)//if vr mode was chosen
        {
            inputText = input;
            Debug.Log(input + "input");
            NetManagerHud.m_ConnectAddress = inputText;//connect to network manager hud
        }
        else if (PCBool.PCMode)//if PC mode was chosen
        {
            inputText = input;
            Debug.Log(input + "input");
            NetManagerHud.m_ConnectAddress = inputText;//connect to network manager hud
        }

        if (input == null)//use default IP address "127.0.0.1"
        {
            input = NetManagerHud.m_ConnectAddress;
            Debug.Log(input + "input");

        }


    }
}
