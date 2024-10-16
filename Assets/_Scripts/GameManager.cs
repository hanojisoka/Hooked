using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : SingletonMB<GameManager>
{
    public bool IsTimerRunning => CountdownTimer.IsTimerRunning();
    public bool IsFishing { get; private set;}

    public event Action<int> OnFishCountChange;
    public event Action OnCountdownFinished;
    
    private UIManager UIManager => UIManager.Instance;
    private AudioSystem AudioSystem => AudioSystem.Instance;
    private UpgradeHandler UpgradeHandler => UpgradeHandler.Instance;
    private CountdownTimer CountdownTimer => CountdownTimer.Instance;
    private Task currentTask;
    private int currentFishCount;
    private int currentLevel;
    private List<UpgradeHandler.UpgradeProgression> Progression => UpgradeHandler.UpgradeProgressions;

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
        if(UIManager)
            UIManager.ToggleNewTaskPanel();
    }

    public void StartNewTask()
    {
        currentTask = UIManager.GetNewTask();
        if (currentTask.Minutes <= 0) return;

        CountdownTimer.StartCountdown(currentTask.Minutes);
        UIManager.TimerStarted(true);
        IsFishing = true;
        // close the panel
        UIManager.ToggleNewTaskPanel();
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            UpgradeHandler.OnUpgradeIsland += UpgradeHandler_OnUpgradeIsland;
            Debug.Log($"{level} was loaded");
            StartCoroutine(InitDelay());
            
        }
    }
    private IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        UIManager.SetFishCountUI(currentFishCount);

        for (int i = 0; i < currentLevel; i++)
        {
            Progression[i].UpgradePart.SetActive(true);
        }
        UpgradeHandler.SetCurrentIslandLevel(currentLevel);
        UpgradeHandler.UpdateFishRequiredText();
        UpgradeHandler.SetUpgradeButtonActive(currentLevel < Progression.Count && currentFishCount >= Progression[currentLevel].FishCost);
    }
    private void OnDestroy()
    {
        if(UpgradeHandler)
            UpgradeHandler.OnUpgradeIsland -= UpgradeHandler_OnUpgradeIsland;
        base.OnDestroy();
    }
    private void UpgradeHandler_OnUpgradeIsland()
    {
        currentLevel = UpgradeHandler.CurrentLevel;
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
        CountdownTimer.StopTimer();
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
}
