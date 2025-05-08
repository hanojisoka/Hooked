using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FishPopUpUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> _fishSprites = new();
    [SerializeField] private TextMeshProUGUI _fishValueText;
    [SerializeField] private TextMeshProUGUI _fishSizeText;
    [SerializeField] private Image _fishImage; 
    public void ShowFishPopUp(int value, string fishName, string size)
    {
        _fishValueText.text = $"Value: {value}";
        _fishImage.sprite = GetFishSprite(fishName);
        _fishSizeText.text = $"Size: {size}";
    }

    private Sprite GetFishSprite(string fishName)
    {
        foreach (Sprite fish in _fishSprites)
        {
            if (fish.name == fishName)
            {
                return fish;
            }
        }
        Debug.LogError($"Fish sprite for {fishName} not found!");
        return null;
    }
}
