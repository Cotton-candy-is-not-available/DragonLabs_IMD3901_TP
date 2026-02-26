using System.Linq;
using System.Net;
using UnityEngine;

public class getIpAdress : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetLocalIPv4();
    }

    public string GetLocalIPv4()
    {
      
        string IPAdress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

        Debug.Log("IPAdress:"+IPAdress);
        //online ip adress is different from this one
        //this ip is local and can be found when you do -> ipconfig in command prompt inder IPV4

        return IPAdress;
    }
}
