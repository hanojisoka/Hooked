using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    [SerializeField] private ParticleSystem splashVFX;
    [SerializeField] private ParticleSystem rippleVFX;
    void Start()
    {
        StartCoroutine(SetPositionToSeaSurface());
        GameManager.OnCountdownFinished += GameManager_OnCountdownFinished;
        GameManager.OnFishCountChange += GameManager_OnFishCountChange;
    }

    private void OnDisable()
    {
        GameManager.OnCountdownFinished -= GameManager_OnCountdownFinished;
        GameManager.OnFishCountChange -= GameManager_OnFishCountChange;
    }

    private void GameManager_OnFishCountChange(int fish)
    {
        if(fish > 0) // if the fish change means a fish will be added
        {
            splashVFX.Stop();
            rippleVFX.Stop();
            // stop splash sound?
        }
    }

    private void GameManager_OnCountdownFinished()
    {
        splashVFX.Play();
        rippleVFX.Play();
        // play splash sound?
    }

    private IEnumerator SetPositionToSeaSurface()
    {
        Vector3 down = Vector3.down;

        if (Physics.Raycast(transform.position, down, out RaycastHit hitInfo, 2))
        {
            Vector3 hitPoint = hitInfo.point;
            transform.position = hitPoint;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetPositionToSeaSurface());
    }


}
