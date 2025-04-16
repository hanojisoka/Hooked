using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    [SerializeField] private BobberMovement bobMove;
    [SerializeField] private ParticleSystem splashVFX;
    [SerializeField] private ParticleSystem rippleVFX;
    [SerializeField] private GameObject _fish;
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
        splashVFX.Stop();
        rippleVFX.Stop();
        bobMove.StopMovement();
        _fish.SetActive(false);
        // stop splash sound?
        
    }

    private void GameManager_OnCountdownFinished()
    {
        splashVFX.Play();
        rippleVFX.Play();
        bobMove.StartMovement();
        _fish.SetActive(true);
        // play splash sound?
    }

    private IEnumerator SetPositionToSeaSurface()
    {
        Vector3 down = Vector3.down;

        if (Physics.Raycast(transform.position, down, out RaycastHit hitInfo, 2))
        {
            if(hitInfo.transform.tag == "Sea")
            {
                Vector3 hitPoint = hitInfo.point;
                transform.position = hitPoint;
            }
            
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetPositionToSeaSurface());
    }


}

