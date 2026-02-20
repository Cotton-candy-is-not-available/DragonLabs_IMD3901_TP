using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Button : MonoBehaviour
{
    public float interactRange = 2f;
    public Camera playerCam;

    public float range = 0.5f;
    private float speed = 0.5f;
    private Vector3 startPos;

    private bool isPressed = false;
    private bool isAnimating = false;

    private float timer = 0f;
    private bool isTimerRunning = true;

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
