using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class Button : NetworkBehaviour
{
    public float interactRange = 2f;
    public Camera playerCam;

    public float range = 0.5f;
    private float speed = 0.5f;
    private Vector3 startPos;

    private bool isPressed = false;
    private bool isAnimating = false;

    private float timer = 0f;
    public bool isTimerRunning = false;

    public TMP_Text timerText;

    public NetworkVariable<int> winnerPlayer = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<bool> pressSignal = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    void Start()
    {
        startPos = transform.localPosition;
    }

    public override void OnNetworkSpawn()
    {
        pressSignal.OnValueChanged += OnPressSignalChanged;
        winnerPlayer.OnValueChanged += OnWinnerChanged;
    }

    public override void OnNetworkDespawn()
    {
        pressSignal.OnValueChanged -= OnPressSignalChanged;
        winnerPlayer.OnValueChanged -= OnWinnerChanged;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            int mins = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", mins, seconds, milliseconds);
        }

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (Keyboard.current.iKey.wasPressedThisFrame)
                {
                    Press();
                }
            }
        }

        Vector3 target = isPressed ? startPos - Vector3.up * range : startPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        {
            if (isPressed)
                isPressed = false;
            else
                isAnimating = false;
        }
    }

    public void Press()
    {
        isPressed = true;
        isAnimating = true;
        isTimerRunning = false;
        Debug.Log("Pressed");
    }

    private void OnPressSignalChanged(bool oldValue, bool newValue)
    {
        PlayPressAnimation();
        if (timer != 0)
            isTimerRunning = false;
    }

    private void PlayPressAnimation()
    {
        isPressed = true;
        isAnimating = true;
    }

    private void OnWinnerChanged(int oldValue, int newValue)
    {
        if (newValue == 1)
            Debug.Log("player 1 wins");
        else if (newValue == 2)
            Debug.Log("player 2 wins");
    }

    [ServerRpc(RequireOwnership = false)]
    public void PressButtonServerRpc(ServerRpcParams rpcParams = default)
    {
        pressSignal.Value = !pressSignal.Value;

        if (winnerPlayer.Value == 0)
        {
            ulong senderId = rpcParams.Receive.SenderClientId;
            winnerPlayer.Value = senderId == 0 ? 1 : 2;
        }
    }
}
