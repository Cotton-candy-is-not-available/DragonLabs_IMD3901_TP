using TMPro;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private float timer = 0f;
    public bool isTimerRunning = false;

    public TMP_Text timerText;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isTimerRunning = false;
        }
    }
}
