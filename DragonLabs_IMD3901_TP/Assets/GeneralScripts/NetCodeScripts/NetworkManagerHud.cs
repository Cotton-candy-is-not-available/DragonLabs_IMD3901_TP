using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
[DisallowMultipleComponent]
public class NetworkManagerHud : MonoBehaviour
{
    NetworkManager m_NetworkManager;

    UnityTransport m_Transport;

    GUIStyle m_LabelTextStyle;

    // This is needed to make the port field more convenient. GUILayout.TextField is very limited and we want to be able to clear the field entirely so we can't cache this as ushort.
    public string m_PortString = "7777";
    public string m_ConnectAddress = "127.0.0.1";

    public Vector2 DrawOffset = new Vector2(10, 10);

    public Color LabelColor = Color.black;

    //Network Canvas
    public Canvas IPAdressCanvas;



    void Awake()
    {
        // Only cache networking manager but not transport here because transport could change anytime.
        m_NetworkManager = GetComponent<NetworkManager>();
        m_LabelTextStyle = new GUIStyle(GUIStyle.none);
    }

    void OnGUI()
    {
        m_LabelTextStyle.normal.textColor = LabelColor;

        m_Transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;

        GUILayout.BeginArea(new Rect(DrawOffset, new Vector2(200, 200)));

        if (IsRunning(m_NetworkManager))
        {
            DrawStatusGUI();
        }
        //else
        //{
        //    DrawConnectGUI();
        //}

        GUILayout.EndArea();
    }



    //void DrawConnectGUI()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Space(10);
    //    GUILayout.Label("Address", m_LabelTextStyle);
    //    GUILayout.Label("Port", m_LabelTextStyle);

    //    GUILayout.EndHorizontal();

    //    GUILayout.BeginHorizontal();

    //    m_ConnectAddress = GUILayout.TextField(m_ConnectAddress);//IP address GUI text field
    //    m_PortString = GUILayout.TextField(m_PortString);//port GUI text field
    //    if (ushort.TryParse(m_PortString, out ushort port))
    //    {
    //        m_Transport.SetConnectionData(m_ConnectAddress, port);
    //    }
    //    else
    //    {
    //        m_Transport.SetConnectionData(m_ConnectAddress, 7777);
    //    }

    //    GUILayout.EndHorizontal();
    //}

    void DrawStatusGUI()
    {
        //if (m_NetworkManager.IsServer)
        //{
        //    var mode = m_NetworkManager.IsHost ? "Host" : "Server";
        //    GUILayout.Label($"{mode} active on port: {m_Transport.ConnectionData.Port.ToString()}", m_LabelTextStyle);
        //}
        //else
        //{
        //    if (m_NetworkManager.IsConnectedClient)
        //    {
        //        GUILayout.Label($"Client connected {m_Transport.ConnectionData.Address}:{m_Transport.ConnectionData.Port.ToString()}", m_LabelTextStyle);
        //    }
        //}

        //Exit server button
        if (GUILayout.Button("Shutdown"))
        {
            m_NetworkManager.Shutdown();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IsRunning(NetworkManager networkManager) => networkManager.IsServer || networkManager.IsClient;
}