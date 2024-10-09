using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMB<GameManager>
{
    public bool IsTimerRunning => countdownTimer.IsTimerRunning();
    public bool IsFishing { get; private set;}

    public event Action<int> OnFishCountChange;
    public event Action OnCountdownFinished;
    
    private UIManager UIManager => UIManager.Instance;
    private AudioSystem AudioSystem => AudioSystem.Instance;
    private CountdownTimer countdownTimer;
    private Task currentTask;
    private int currentFishCount;
    

    private MenuSettings currentSettings;

    private class MenuSettings
    {
        public float MusicVolume = 1f;
        public float AlarmVolume = 1f;
    }


    public class Task
    {
        public int Minutes;
        public bool IsSoundEnabled;
    }

    void Start()
    {
        countdownTimer = GetComponent<CountdownTimer>();
        //StartCoroutine(GetTimeEverySecond());
        if(UIManager)
            UIManager.ToggleNewTaskPanel();
    }
    /*IEnumerator GetTimeEverySecond()
    {
        currentTime = DateTime.Now;
        UIManager.SetUITime(currentTime.ToString(isMilitaryTime ? "H:mm" : "h:mm tt"));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GetTimeEverySecond());
    }

    public void ToggleMilitaryTime()
    {
        isMilitaryTime = !isMilitaryTime;
        UIManager.SetUITime(currentTime.ToString(isMilitaryTime ? "H:mm" : "h:mm tt"));
    }*/

    public void StartNewTask()
    {
        currentTask = UIManager.GetNewTask();
        if (currentTask.Minutes <= 0) return;

        countdownTimer.StartCountdown(currentTask.Minutes);
        UIManager.TimerStarted(true);
        IsFishing = true;
        // close the panel
        UIManager.ToggleNewTaskPanel();
    }

    public void CountdownFinished()
    {
        OnCountdownFinished?.Invoke();
        if(currentTask.IsSoundEnabled)
            AudioSystem.SoundsSource.Play();
    }

    public void ReelInPressed()
    {
        if (IsTimerRunning)
            //restart timer warning
            UIManager.ShowReelWarning();
        else
            CatchFish();
    }

    public void StopFishing()
    {
        countdownTimer.StopTimer();
        UIManager.TimerStarted(false);
        IsFishing = false;
    }

    private void CatchFish()
    {
        int fishToAdd = (currentTask.Minutes - 1) / 20 + 1;
        UIManager.PlusFishAnimation(fishToAdd);
        FishCountHandler(fishToAdd);
        AudioSystem.SoundsSource.Stop();
        UIManager.TimerStarted(false);
        IsFishing = false;
    }

    public void FishCountHandler(int fish)
    {
        currentFishCount += fish; // can be - or + fish
        OnFishCountChange?.Invoke(currentFishCount);
        UIManager.SetFishCountUI(currentFishCount);
    }
    public void PlayGame() => SceneManager.LoadScene("FishingGame");
    public void ExitGame() => Application.Quit();
}
