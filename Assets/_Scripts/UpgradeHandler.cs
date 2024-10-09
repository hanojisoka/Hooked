using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeHandler : SingletonMB<UpgradeHandler>
{
    public int CurrentLevel;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI fishRequiredText;
    [SerializeField] private List<UpgradeProgression> upgradeProgressions = new();

    private GameManager GameManager => GameManager.Instance;

    [System.Serializable]
    public class UpgradeProgression
    {
        public GameObject UpgradePart;
        public int FishCost;
    }

    private void Start()
    {
        GameManager.OnFishCountChange += GameManager_OnFishCountChange;
        upgradeButton.onClick.AddListener(UpgradeIsland);
        fishRequiredText.text = $"Get {upgradeProgressions[CurrentLevel].FishCost} to upgrade";
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        GameManager.OnFishCountChange -= GameManager_OnFishCountChange;
    }
    private void GameManager_OnFishCountChange(int currentFishCount)
    {
        if(currentFishCount >= upgradeProgressions[CurrentLevel].FishCost)
            upgradeButton.gameObject.SetActive(true);
        else
            upgradeButton.gameObject.SetActive(false);
    }

    public void UpgradeIsland()
    {
        if (upgradeProgressions.Count > CurrentLevel)
        {
            GameManager.FishCountHandler(-upgradeProgressions[CurrentLevel].FishCost); // take fish required
            AnimationUpgrade();
            CurrentLevel++;

            if(CurrentLevel >= upgradeProgressions.Count)
                fishRequiredText.text = $"Max upgrade reached";
            else
                fishRequiredText.text = $"Get {upgradeProgressions[CurrentLevel].FishCost} fish to upgrade";
        }
            
    }

    private void AnimationUpgrade()
    {
        //TODO: make animation upgrade
        upgradeProgressions[CurrentLevel].UpgradePart.gameObject.SetActive(true);
    }
}
