using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour
{
    public RectTransform meter; // The progress bar UI
    public RectTransform greenZone; // The green-colored zone
    public RectTransform indicator; // The moving indicator

    public float indicatorSpeed = 2f; // Speed of the indicator
    private bool movingRightIndicator = true;
    private bool movingRightGreen = true;
    private float greenZoneSpeed;

    private bool isFishing = false;

    private GameManager _gm;

    private void Start()
    {
        _gm = GameManager.Instance;
        _gm.OnCountdownFinished += _gm_OnCountdownFinished;
    }
    private void OnDisable()
    {
        _gm.OnCountdownFinished -= _gm_OnCountdownFinished;
    }

    private void _gm_OnCountdownFinished()
    {
        StartFishingMiniGame();
    }

    void Update()
    {
        if (!isFishing) return;

        MoveIndicator(indicatorSpeed * Time.deltaTime);
        MoveGreenZone(greenZoneSpeed * Time.deltaTime);
    }

    private void MoveIndicator(float step)
    {
        if (movingRightIndicator)
        {
            indicator.anchoredPosition += new Vector2(step * 100, 0);
            if (indicator.anchoredPosition.x >= meter.GetComponent<RectTransform>().sizeDelta.x / 2)
                movingRightIndicator = false;
        }
        else
        {
            indicator.anchoredPosition -= new Vector2(step * 100, 0);
            if (indicator.anchoredPosition.x <= -meter.GetComponent<RectTransform>().sizeDelta.x / 2)
                movingRightIndicator = true;
        }
    }

    private void MoveGreenZone(float step)
    {
        if (movingRightGreen)
        {
            greenZone.anchoredPosition += new Vector2(step * 100, 0);
            if (greenZone.anchoredPosition.x >= meter.GetComponent<RectTransform>().sizeDelta.x / 2)
                movingRightGreen = false;
        }
        else
        {
            greenZone.anchoredPosition -= new Vector2(step * 100, 0);
            if (greenZone.anchoredPosition.x <= -meter.GetComponent<RectTransform>().sizeDelta.x / 2)
                movingRightGreen = true;
        }
    }

    bool IsIndicatorInGreenZone()
    {
        float indicatorPos = indicator.anchoredPosition.x;
        float greenZoneStart = greenZone.anchoredPosition.x - greenZone.sizeDelta.x / 2;
        float greenZoneEnd = greenZone.anchoredPosition.x + greenZone.sizeDelta.x / 2;

        return indicatorPos >= greenZoneStart && indicatorPos <= greenZoneEnd;
    }

    public void StartFishingMiniGame()
    {
        isFishing = true;
        meter.gameObject.SetActive(true);
        LeanTween.alphaCanvas(meter.GetComponent<CanvasGroup>(), 1, 0.5f);
        greenZoneSpeed = Random.Range(0.1f, indicatorSpeed * 0.9f); // randomzied green zone speed
        // Randomize green zone position
        float meterWidth = meter.GetComponent<RectTransform>().sizeDelta.x;
        float randomX = Random.Range(-meterWidth / 2 + greenZone.sizeDelta.x, meterWidth / 2 - greenZone.sizeDelta.x);
        greenZone.anchoredPosition = new Vector2(randomX, greenZone.anchoredPosition.y);

        // Reset indicator
        indicator.anchoredPosition = new Vector2(-meterWidth / 2, indicator.anchoredPosition.y);
        movingRightIndicator = true;
    }

    public bool ReelIn()
    {
        if (IsIndicatorInGreenZone())
        {
            Debug.Log("Fish Caught!");
            isFishing = false; // Stop game
            LeanTween.alphaCanvas(meter.GetComponent<CanvasGroup>(), 0, 2f).setOnComplete(
                () => meter.gameObject.SetActive(false));
        }
        else
        {
            //LeanTween.moveLocalX(meter.gameObject, 10, 1).setEaseInOutBounce();
            ShakeMeter();
            Debug.Log("Missed! Reel in again.");

        }

        return IsIndicatorInGreenZone();
    }

    private void ShakeMeter()
    {
        float shakeAmount = 10f; // distance to shake left and right
        float duration = 0.1f;   // duration of each shake
        int shakeCount = 10;     // total number of shakes (5 cycles)

        for (int i = 0; i < shakeCount; i++)
        {
            float targetPosX = (i % 2 == 0) ? shakeAmount : -shakeAmount;
            LeanTween.moveLocalX(meter.gameObject, targetPosX, duration)
                .setEaseInOutSine()
                .setDelay(i * duration);
        }

        // Reset to center after shaking
        LeanTween.moveLocalX(meter.gameObject, 0, duration)
            .setEaseInOutSine()
            .setDelay(shakeCount * duration);
    }
}
