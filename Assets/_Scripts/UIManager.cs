using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMB<UIManager>
{

    private GameManager GameManager => GameManager.Instance;
    [Header("HUD")]
    [SerializeField] private GameObject reelWarningPanel;
    [SerializeField] private TextMeshProUGUI startFishButtonText;
    [SerializeField] private Color startColor, stopColor;
    [SerializeField] private TextMeshProUGUI fishCatchedUI;
    [Header("Task Panel")]
    [SerializeField] private GameObject newTaskPanel;
    [SerializeField] private TMP_InputField minutesInput;
    [SerializeField] private Toggle soundToggle;
    [Header("Catch Fish")]
    [SerializeField] private GameObject plusFishUI;

    // Start is called before the first frame update
    void Start()
    {
        minutesInput.onEndEdit.AddListener(MinutesInputValidator);
        GameManager.OnCountdownFinished += GameManager_OnCountdownFinished;
        //taskNamePanel.GetComponent<RectTransform>().position = new Vector3(0, 100, 0);
    }

/*    private void OnDestroy()
    {
        GameManager.OnCountdownFinished -= GameManager_OnCountdownFinished;
        base.OnDestroy();
    }*/

    private void GameManager_OnCountdownFinished()
    {
        startFishButtonText.text = "Reel in";
        startFishButtonText.transform.parent.GetComponent<Image>().color = startColor;
    }

    public void ShowReelWarning()
    {
        reelWarningPanel.SetActive(true);
    }

    public void StartFishReelInButton()
    {
        if (GameManager.IsFishing)
            GameManager.ReelInPressed();  
        else
            ToggleNewTaskPanel();
    }

    public void ToggleNewTaskPanel()
    {
        bool isActive = newTaskPanel.activeSelf;
        newTaskPanel.SetActive(!isActive);
    }

    public void TimerStarted(bool isTimeRunning)
    {
        if (isTimeRunning)
        {
            startFishButtonText.text = "Stop Fishing";
            startFishButtonText.transform.parent.GetComponent<Image>().color = stopColor;
            startFishButtonText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            startFishButtonText.text = "Start Fishing";
            startFishButtonText.transform.parent.gameObject.SetActive(false);
        }
    }


    public void MinutesInputButtons(int value)
    {
        string text = minutesInput.text;
        if (int.TryParse(text, out int currentMin))
            minutesInput.text = (currentMin + value).ToString();
        else
            minutesInput.text = (0 + value).ToString();
        MinutesInputValidator(minutesInput.text);
    }
    public GameManager.Task GetNewTask()
    {
        GameManager.Task newTask = new();
        if (int.TryParse(minutesInput.text, out int value))
            newTask.Minutes = value;
        else
            newTask.Minutes = 0;
        newTask.IsSoundEnabled = soundToggle.isOn;
        return newTask;
    }

    public void PlusFishAnimation(int addFish, string fishType, string size)
    {
        CanvasGroup canvasGroup = plusFishUI.GetComponent<CanvasGroup>();
        plusFishUI.GetComponentInChildren<TextMeshProUGUI>().text = $"{size} {fishType}\nValue: {addFish}";
        canvasGroup.alpha = 1f;
        plusFishUI.transform.localPosition = Vector3.zero;
        LeanTween.moveLocalY(plusFishUI, 15f, 5f);
        LeanTween.alphaCanvas(canvasGroup, 0f, 3f);

    }

    public void SetFishCountUI(int value)
    {
        fishCatchedUI.text = $"= {value}";
    }

    private void MinutesInputValidator(string input)
    {
        // Check if the input is a valid number
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int value))
        {
            // If value is less than 5 remove input
            if (value < 5)
                minutesInput.text = "";


            
        }
    }
    public void StopFishingButton() => GameManager.StopFishing();
    public void StartNewTaskButton() => GameManager.StartNewTask();
    public void MainMenu() 
    {
        GameManager.StopFishing();
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

}
