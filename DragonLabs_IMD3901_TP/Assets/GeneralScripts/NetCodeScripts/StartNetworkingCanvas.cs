using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;


public class StartNetworkingCanvas : MonoBehaviour
{
    //[SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    public Canvas networkCanvas;

    private void Awake()
    {
        /*
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Debug.Log("started server");
        });
        */

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("started host");
            networkCanvas.gameObject.SetActive(false);
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("started client");
            networkCanvas.gameObject.SetActive(false);
        });
    }
}
