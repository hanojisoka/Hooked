using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCatchingManager : SingletonMB<FishCatchingManager>
{
    public enum FishType
    {
        Dilis,
        Tamban,
        Tilapia,
        Hito,
        Bangus,
        Tulingan
    }
    [System.Serializable]
    public class FishTypeData
    {
        public FishType Type;
        public float MinValue, MaxValue;
    }

    [SerializeField] private List<FishTypeData> fishTypeDatas = new();
    public FishingMiniGame MiniGame;

    public FishTypeData GetFishDataToCatch(float timeInMinutes)
    {
        float roll = Random.value; // Generates a value between 0.0 and 1.0

        if (timeInMinutes < 20)
        {
            // Less than 20 min: 50% Dilis, 50% Tamban
            
            return roll < 0.5f ? GetFishDataWithEnum(FishType.Dilis) : GetFishDataWithEnum(FishType.Tamban);
        }
        else
        {
            // 20 min or more: Main fish pool (Tilapia, Hito, Bangus)
            float tulinganChance = timeInMinutes >= 150 ? 0.1f : 0.0f; // 10% chance at 2.5 hours
            float dilisTambanChance = 0.05f; // 5% each

            roll = Random.value; // Reroll for the new probability distribution

            if (roll < tulinganChance) return GetFishDataWithEnum(FishType.Tulingan); // 10% chance after 2.5 hours
            if (roll < tulinganChance + dilisTambanChance) return GetFishDataWithEnum(FishType.Dilis); // 5% chance
            if (roll < tulinganChance + 2 * dilisTambanChance) return GetFishDataWithEnum(FishType.Tamban); // 5% chance

            // Remaining 80% or 90% chance divided among Tilapia, Hito, Bangus
            float remainingChance = 1f - (tulinganChance + 2 * dilisTambanChance);
            float section = remainingChance / 3;

            if (roll < tulinganChance + 2 * dilisTambanChance + section) return GetFishDataWithEnum(FishType.Tilapia);
            if (roll < tulinganChance + 2 * dilisTambanChance + 2 * section) return GetFishDataWithEnum(FishType.Hito);
            return GetFishDataWithEnum(FishType.Bangus); // Remaining probability
        }
    }

    public float GetFishValue(FishTypeData fishData, float timeInMinutes)
    {
        float randomInitValue = Random.Range(fishData.MinValue, fishData.MaxValue);
        float sizeMultiplier = GetSizeMultiplier(timeInMinutes);

        return randomInitValue * sizeMultiplier;
    }

    public string GetFishSizeString(float minutes)
    {
        if (minutes < 150) return "Small";
        if (minutes < 300) return "Medium";
        if (minutes < 600) return "Large";
        if (minutes < 900) return "Very Large";
        return "Giant";


    }

    float GetSizeMultiplier(float minutes)
    {
        if (minutes < 150) return 1f;
        if (minutes < 300) return 2f;

        return 3f + 0.5f * Mathf.Floor((minutes - 300) / 60);
    }

    FishTypeData GetFishDataWithEnum(FishType fish)
    {
        foreach(FishTypeData data in fishTypeDatas)
        {
            if (data.Type == fish)
                return data;
        }
        Debug.Log("Didnt find the FishType, setting fish to the first index of fishTypeData");
        return fishTypeDatas[0];
    }





}
