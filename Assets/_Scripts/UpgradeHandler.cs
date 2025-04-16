using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class UpgradeHandler : SingletonMB<UpgradeHandler>
{
    public int CurrentLevel { get; private set; }
    public event Action OnUpgradeIsland;
    public static List<UpgradeProgression> UpgradeProgressions { get => Instance.upgradeProgressions; }

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI fishRequiredText;
    [SerializeField] private List<UpgradeProgression> upgradeProgressions = new();
    [SerializeField] private GameObject upgradeVFX;

    private GameManager GameManager => GameManager.Instance;

    [System.Serializable]
    public class UpgradeProgression
    {
        public GameObject UpgradePart;
        public int FishCost;
        public UnityEvent OnUpgrade;
    }

    private void Start()
    {
        GameManager.OnFishCountChange += GameManager_OnFishCountChange;
        upgradeButton.onClick.AddListener(UpgradeIsland);
        UpdateFishRequiredText();
    }
/*    private void OnDestroy()
    {
        //GameManager.OnFishCountChange -= GameManager_OnFishCountChange;
        base.OnDestroy();
    }*/
    private void GameManager_OnFishCountChange(int currentFishCount)
    {
/*        Debug.Log($"{CurrentLevel} {upgradeProgressions.Count}");
        if (CurrentLevel >= upgradeProgressions.Count)
        {
            SetUpgradeButtonActive(false);
            return;
        }*/
        SetUpgradeButtonActive(CurrentLevel < upgradeProgressions.Count && currentFishCount >= upgradeProgressions[CurrentLevel].FishCost);
    }

    public void SetUpgradeButtonActive(bool active) => upgradeButton.gameObject.SetActive(active);

    public void UpgradeIsland()
    {
        if (upgradeProgressions.Count > CurrentLevel)
        {
            upgradeProgressions[CurrentLevel].OnUpgrade.Invoke();
            Debug.Log($"Upgrade Island to level {CurrentLevel + 1}");
            CurrentLevel++;
            GameManager.GameData.Level = CurrentLevel;
            GameManager.FishCountHandler(-upgradeProgressions[CurrentLevel - 1].FishCost); // take fish required
            Debug.Log($"{-upgradeProgressions[CurrentLevel - 1].FishCost} Fish");
            AnimationUpgrade();
            OnUpgradeIsland?.Invoke();
            UpdateFishRequiredText();
        } 
    }

    public void UpdateFishRequiredText()
    {
        if (CurrentLevel >= upgradeProgressions.Count)
            fishRequiredText.text = $"Max upgrade reached";
        else
            fishRequiredText.text = $"Get {upgradeProgressions[CurrentLevel].FishCost} fish to upgrade";
    }
    public void SetCurrentIslandLevel(int level)
    {
        CurrentLevel = level;
    }

    private void AnimationUpgrade()
    {
        GameObject newUpgrade = upgradeProgressions[CurrentLevel - 1].UpgradePart.gameObject;

        // Find the MeshFilter in the children of newUpgrade
        MeshFilter meshFilter = newUpgrade.GetComponentInChildren<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError($"No MeshFilter found in {newUpgrade.name} or its children.");
            return;
        }

        foreach (ParticleSystem vfx in upgradeVFX.GetComponentsInChildren<ParticleSystem>())
        {
            var shape = vfx.shape;
            shape.shapeType = ParticleSystemShapeType.Mesh;
            shape.mesh = meshFilter.sharedMesh;

            shape.scale = newUpgrade.transform.localScale;
            vfx.Play();
        }
        upgradeVFX.transform.position = newUpgrade.transform.position;
        newUpgrade.SetActive(true);
    }

}
