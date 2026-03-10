using TMPro;
using UnityEngine;

public class StartTimer : MonoBehaviour
{
    private float startTimer = 10.0f;
    public GameObject startWall;

    public TMP_Text timerText;
    private bool countdownStarted = false;

    void Start()
    {
        timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(startTimer % 60f), Mathf.FloorToInt((startTimer * 1000f) % 1000f));
    }

    void Update()
    {
        if (countdownStarted)
        {
            startTimer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(startTimer % 60f);
            int milliseconds = Mathf.FloorToInt((startTimer * 1000f) % 1000f);
            timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);

            if (startTimer <= 0)
            {
                startTimer = 0;
                startWall.SetActive(false);
                timerText.enabled = false;
                Debug.Log("Timer finished");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (!countdownStarted)
             countdownStarted = true;
    }
}