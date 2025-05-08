using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class FishingSpotManager : SingletonMB<FishingSpotManager>
{
    public event Action<FishingSpot> OnNewFishingSpot;
    private List<FishingSpot> fishingSpots = new();
    private GameManager _gm => GameManager.Instance;

    private void Start()
    {
        MakeNewFishingSpot();
        _gm.OnReelIn += OnReelIn;
    }

    private void OnReelIn()
    {
        MakeNewFishingSpot();
    }

    [Button]
    public void MakeNewFishingSpot()
    {
        if(fishingSpots.Count <= 0) // makes sure that fishing spots are not empty
        {
            fishingSpots = GetComponentsInChildren<FishingSpot>(true).ToList();
        }

        foreach (FishingSpot spot in fishingSpots)
        {
            spot.gameObject.SetActive(false);
        }
        int randomSpotIndex = Random.Range(0, fishingSpots.Count);
        FishingSpot currentSpot = fishingSpots[randomSpotIndex];
        currentSpot.RandomizeSpawnPosition();
        currentSpot.gameObject.SetActive(true);
        OnNewFishingSpot?.Invoke(currentSpot);
        
    }
}
