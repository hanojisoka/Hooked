using UnityEngine;
using TMPro;
using System.Collections;
using NaughtyAttributes;

public class CountdownTimer : SingletonMB<CountdownTimer>
{
    private GameManager GameManager => GameManager.Instance;
    //[SerializeField] private GameObject timerPausedText;
    [SerializeField] private TextMeshProUGUI timerText;

    public float timeRemaining;      // Time remaining in seconds
    private bool isTimerRunning = false; // Is the timer running?
    private bool isPaused = false;      // Is the timer paused?

    private Coroutine coroutineTimer;


    private void Start()
    {

    }
    void Update()
    {
        // If the timer is running and not paused
        if (isTimerRunning && !isPaused)
        {
            if (timeRemaining > 0)
            {
                // Subtract the elapsed time from the remaining time
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                // If time is up, stop the timer
                timeRemaining = 0;
                isTimerRunning = false;
                OnTimerFinished();
            }
        }
    }

    // Start the countdown with the specified minutes
    public void StartCountdown(int minutes)
    {
        timeRemaining = minutes * 60; // Convert minutes to seconds
        isTimerRunning = true;         // Start the timer
        isPaused = false;              // Ensure it's not paused
        timerText.gameObject.SetActive(true);

        coroutineTimer = StartCoroutine(DisplayTime(timeRemaining - 0.1f));
    }

    // Display the time in minutes and seconds format (MM:SS)
    IEnumerator DisplayTime(float timeToDisplay)
    {

        yield return new WaitForSeconds(0.1f);
        //timeToDisplay += 1;  // Add 1 second to round the countdown display properly

        int hours = Mathf.FloorToInt(timeToDisplay / 3600);   // Calculate the hours
        int minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60);  // Calculate the remaining minutes
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);   // Calculate the remaining seconds

        // Display the time in HH:MM:SS format
        if (hours > 0)
        {
            // If hours exist, show hours, minutes, and seconds
            timerText.text = string.Format("{0}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            // If only minutes exist, show minutes and seconds
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
        else
        {
            // If only seconds exist, show seconds only
            timerText.text = seconds.ToString();
        }
        yield return new WaitForSeconds(0.9f);
        coroutineTimer = StartCoroutine(DisplayTime(timeRemaining));
    }

    // Pause the timer
    public void TogglePauseTimer()
    {
        isPaused = !isPaused;
        //timerPausedText.SetActive(isPaused);
        StopCoroutine(coroutineTimer);
    }

    public void StopTimer()
    {
        if(coroutineTimer != null)
            StopCoroutine(coroutineTimer);
        isTimerRunning = false;
        timerText.text = "";
        timerText.gameObject.SetActive(false);
    }

    // Check if the timer is running
    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }

    // Check the remaining time
    public float GetRemainingTime()
    {
        return timeRemaining;
    }


    // This method is called when the timer finishes
    private void OnTimerFinished()
    {
        GameManager.CountdownFinished();
        timerText.gameObject.SetActive(false);
    }


    // for debugging
    [Button]
    public void DebugFinishTimer()
    {
        timeRemaining = -1;
    }


}
