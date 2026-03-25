using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class getIpAdress : MonoBehaviour
{
    //display Ip address so the client can connect
    public TextMeshProUGUI IPAdressText;
    void Start()
    {
        staticClass.staticIPAddressVariable = GetLocalIPv4();//get the ip address of the host to display on the screen
    }

    public string GetLocalIPv4()
    {
        //gets default or first entry on ip adress
        string IPAdress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        staticClass.staticIPAddressVariable = IPAdress;

        Debug.Log("IPAdress:"+IPAdress);
        //online ip adress is different from this one
        //this ip is local and can be found when you do -> ipconfig in command prompt under IPV4 on windows

        return IPAdress;
    }
}
