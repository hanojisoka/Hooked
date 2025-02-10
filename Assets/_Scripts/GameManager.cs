using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : SingletonMB<GameManager>, IDataPersistence
{
    public bool IsTimerRunning => CountdownTimer.IsTimerRunning();
    public bool IsFishing { get; private set;}

    public event Action<int> OnFishCountChange;
    public event Action OnCountdownFinished;
    
    private UIManager UIManager => UIManager.Instance;
    private AudioSystem AudioSystem => AudioSystem.Instance;
    private UpgradeHandler UpgradeHandler => UpgradeHandler.Instance;
    private CountdownTimer CountdownTimer => CountdownTimer.Instance;
    private FishingSpotManager FishingSpotManager => FishingSpotManager.Instance;
    private FishCatchingManager FishCatchingManager => FishCatchingManager.Instance;

    private Task currentTask;
    private int currentFishCount;
    private int currentLevel => UpgradeHandler.CurrentLevel;
    private List<UpgradeHandler.UpgradeProgression> Progression => UpgradeHandler.UpgradeProgressions;

    private MenuSettings currentSettings;

    public GameObject Player;

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
        /*if(UIManager)
            UIManager.ToggleNewTaskPanel();*/
        Player.GetComponent<CharacterMoveToPosition>().OnArrivedToFishingSpot 
            += GameManager_OnArrivedToFishingSpot;
    }

    private void GameManager_OnArrivedToFishingSpot()
    {
        UIManager.StartFishReelInButton();
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
        {
            if (FishCatchingManager.MiniGame.ReelIn())
                CatchFish();
        }
    }

    public void StopFishing()
    {
        CountdownTimer.StopTimer();
        UIManager.TimerStarted(false);
        IsFishing = false;
    }

    private void CatchFish()
    {
        FishCatchingManager.FishTypeData fishData = FishCatchingManager.GetFishDataToCatch(currentTask.Minutes); // gets a random fish depending on minutes passed
        string fishSize = FishCatchingManager.GetFishSizeString(currentTask.Minutes); // get size from minutes
        int fishToAdd = (int)FishCatchingManager.GetFishValue(fishData, currentTask.Minutes); // gets fish calculated value      
        UIManager.PlusFishAnimation(fishToAdd, fishData.Type.ToString(), fishSize);
        FishCountHandler(fishToAdd);
        AudioSystem.SoundsSource.Stop();
        UIManager.TimerStarted(false);
        IsFishing = false;
        FishingSpotManager.MakeNewFishingSpot();

        Debug.Log($"{fishData.Type.ToString()} is caught with a value of: {fishToAdd}");
    }

    public void FishCountHandler(int fishCount)
    {
        currentFishCount += fishCount; // can be - or + fish 
        OnFishCountChange?.Invoke(currentFishCount);
        UIManager.SetFishCountUI(currentFishCount);
    }
    /// <summary>
    /// Save data
    /// </summary>
    public void LoadData(GameData data)
    {
        this.currentFishCount = data.FishCount;
        StartCoroutine(InitDelay());
    }

    public void SaveData(ref GameData data)
    {
        data.FishCount = this.currentFishCount;
    }
}
