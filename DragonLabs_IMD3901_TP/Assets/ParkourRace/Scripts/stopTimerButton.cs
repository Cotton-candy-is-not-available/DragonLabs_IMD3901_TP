using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class stopTimerButton : MonoBehaviour
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

    void Start()
    {
        startPos = transform.localPosition;
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
}

/*public class stopTimerButton : NetworkBehaviour
{
    public float interactRange = 2f;
    public Camera playerCam;

    public float range = 0.5f;
    private float speed = 0.5f;
    private Vector3 startPos;

    private bool isAnimating = false;

    public TMP_Text timerText;

    private NetworkVariable<bool> isPressed = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<bool> isTimerRunning = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<float> timer = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        HandleInteraction();

        if (IsServer)
        {
            UpdateTimerOnServer();
        }

        UpdateTimerText();
        AnimateButton();
    }

    private void HandleInteraction()
    {
        if (!IsOwner && !IsClient)
            return;

        if (playerCam == null)
            return;

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (Keyboard.current.iKey.wasPressedThisFrame)
                {
                    PressServerRpc();
                }
            }
        }
    }

    private void UpdateTimerOnServer()
    {
        if (isTimerRunning.Value)
        {
            timer.Value += Time.deltaTime;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        float currentTime = timer.Value;
        int mins = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", mins, seconds, milliseconds);
    }

    private void AnimateButton()
    {
        Vector3 target = isPressed.Value ? startPos - Vector3.up * range : startPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) < 0.001f)
        {
            if (isPressed.Value && !isAnimating)
            {
                return;
            }

            if (isPressed.Value)
            {
                if (IsServer)
                {
                    isPressed.Value = false;
                }
            }
            else
            {
                isAnimating = false;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PressServerRpc(ServerRpcParams rpcParams = default)
    {
        if (isAnimating)
            return;

        isPressed.Value = true;
        isAnimating = true;
        isTimerRunning.Value = false;

        Debug.Log("Pressed");
    }

    public void StartTimer()
    {
        if (IsServer)
        {
            isTimerRunning.Value = true;
        }
    }

    public void ResetTimer()
    {
        if (IsServer)
        {
            timer.Value = 0f;
            isTimerRunning.Value = false;
        }
    }
}*/