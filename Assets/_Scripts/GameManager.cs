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
    public event Action OnStartFishing;
    public event Action OnStopFishing;
    public event Action OnCatchFish;
    public event Action OnReelIn;
    
    private UIManager UIManager => UIManager.Instance;
    private AudioSystem AudioSystem => AudioSystem.Instance;
    private UpgradeHandler UpgradeHandler => UpgradeHandler.Instance;
    private CountdownTimer CountdownTimer => CountdownTimer.Instance;
    private FishingSpotManager FishingSpotManager => FishingSpotManager.Instance;
    private FishCatchingManager FishCatchingManager => FishCatchingManager.Instance;

    private Task currentTask;
    private List<UpgradeHandler.UpgradeProgression> Progression => UpgradeHandler.UpgradeProgressions;

    private MenuSettings currentSettings;


    public GameObject Player;
    public GameData GameData;

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

    public void ListenToEventsFromPlayer()
    {
        Player.GetComponent<CharacterMoveToPosition>().OnArrivedToSpot
        += GameManager_OnArrivedToFishingSpot;
    }

    private void GameManager_OnArrivedToFishingSpot(string tag)
    {
        if (tag == "FishingSpot")
        {
            UIManager.StartFishReelInButton(); //change this in the future
        }
        if(tag == "Kutingting")
        {
            KutingtingConversation.Instance.StartConversation();
        }
    }

    public void StartNewTask()
    {
        currentTask = UIManager.GetNewTask();
        if (currentTask.Minutes <= 0) return;

        CountdownTimer.StartCountdown(currentTask.Minutes);
        UIManager.TimerStarted(true);
        IsFishing = true;
        OnStartFishing?.Invoke();
        // close the panel
        UIManager.ToggleNewTaskPanel();
    }
    private IEnumerator InitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        UIManager.SetFishCountUI(GameData.FishCount);

        for (int i = 0; i < GameData.Level; i++)
        {
            Progression[i].UpgradePart.SetActive(true);
            Progression[i].OnUpgrade.Invoke();
        }
        UpgradeHandler.SetCurrentIslandLevel(GameData.Level);
        UpgradeHandler.UpdateFishRequiredText();
        UpgradeHandler.SetUpgradeButtonActive(GameData.Level < Progression.Count && GameData.FishCount >= Progression[GameData.Level].FishCost);
    }

    public void CountdownFinished()
    {
        OnCountdownFinished?.Invoke();
        if(currentTask.IsSoundEnabled) // Play aralm sound
            AudioSystem.PlayAlarmSound();
    }

    public void ReelInPressed()
    {
        if (IsTimerRunning)
            //restart timer warning
            UIManager.ShowReelWarning();
        else
        {
            if (FishCatchingManager.MiniGame.ReelIn()){
                OnReelIn?.Invoke();
            }
        }
    }

    public void StopFishing()
    {
        CountdownTimer.StopTimer();
        UIManager.TimerStarted(false);
        IsFishing = false;
        OnStopFishing?.Invoke();
    }

    public void CatchFish()
    {
        FishCatchingManager.FishTypeData fishData = FishCatchingManager.GetFishDataToCatch(currentTask.Minutes); // gets a random fish depending on minutes passed
        string fishSize = FishCatchingManager.GetFishSizeString(currentTask.Minutes); // get size from minutes
        int fishToAdd = (int)FishCatchingManager.GetFishValue(fishData, currentTask.Minutes); // gets fish calculated value      
        UIManager.PlusFishAnimation(fishToAdd, fishData.Type.ToString(), fishSize);
        FishCountHandler(fishToAdd);
        AudioSystem.SoundsSource.Stop();
        UIManager.TimerStarted(false);
        IsFishing = false;
        OnCatchFish?.Invoke();
        Debug.Log($"{fishData.Type.ToString()} is caught with a value of: {fishToAdd}");
    }

    public void FishCountHandler(int fishCount)
    {
        GameData.FishCount += fishCount; // can be - or + fish 
        OnFishCountChange?.Invoke(GameData.FishCount);
        UIManager.SetFishCountUI(GameData.FishCount);
    }
    /// <summary>
    /// Save data
    /// </summary>

    public void LoadData(GameData data)
    {
        GameData = data;
        StartCoroutine(InitDelay());
    }

    public void SaveData(ref GameData data)
    {
        data = GameData;
    }
}
