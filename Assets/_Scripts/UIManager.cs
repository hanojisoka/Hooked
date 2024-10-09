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
        //if(!isActive)
            //LeanTween.moveLocalY(taskNamePanel, -20, 1f);
        //else
            //LeanTween.moveLocalY(taskNamePanel, 100, 1f);
    }

    public void TimerStarted(bool isTimeRunning)
    {
        if (isTimeRunning)
        {
            startFishButtonText.text = "Stop Fishing";
            startFishButtonText.transform.parent.GetComponent<Image>().color = stopColor;
        }
        else
        {
            startFishButtonText.text = "Start Fishing";
            startFishButtonText.transform.parent.GetComponent<Image>().color = startColor;
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

    public void PlusFishAnimation(int addFish)
    {
        CanvasGroup canvasGroup = plusFishUI.GetComponent<CanvasGroup>();
        plusFishUI.GetComponentInChildren<TextMeshProUGUI>().text = $"+{addFish}";
        canvasGroup.alpha = 1f;
        plusFishUI.transform.localPosition = Vector3.zero;
        LeanTween.moveLocalY(plusFishUI, 15f, 5f);
        LeanTween.alphaCanvas(canvasGroup, 0f, 2f);

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
            // If it's negative, reset to an empty string or to zero
            if (value < 5)
                minutesInput.text = "5";
        }
    }

    public void MainMenu() => SceneManager.LoadScene("MainMenu");

}
