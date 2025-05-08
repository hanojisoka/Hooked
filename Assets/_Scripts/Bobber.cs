using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    private AudioSystem AudioSystem => AudioSystem.Instance;
    [SerializeField] private BobberMovement bobMove;
    [SerializeField] private ParticleSystem splashVFX;
    [SerializeField] private ParticleSystem rippleVFX;
    [SerializeField] private GameObject _fish;
    [SerializeField] private AudioClip _fishHookedSound;
    void Start()
    {
        StartCoroutine(SetPositionToSeaSurface());
        GameManager.OnCountdownFinished += GameManager_OnCountdownFinished;
        GameManager.OnFishCountChange += GameManager_OnFishCountChange;
        GameManager.OnReelIn += ReelIn;
    }

    private void ReelIn()
    {
        bobMove.StopMovement();
    }

    private void OnDisable()
    {
        if(GameManager == null) return;
        GameManager.OnCountdownFinished -= GameManager_OnCountdownFinished;
        GameManager.OnFishCountChange -= GameManager_OnFishCountChange;
    }

    private void GameManager_OnFishCountChange(int fish)
    {
        splashVFX.Stop();
        rippleVFX.Stop();
        
        _fish.SetActive(false);
        // stop splash sound
        AudioSystem.StopSound();
        
    }

    private void GameManager_OnCountdownFinished()
    {
        splashVFX.Play();
        rippleVFX.Play();
        bobMove.StartMovement();
        _fish.SetActive(true);
        // play splash sound
        AudioSystem.PlayLoopingSound(_fishHookedSound, transform.position, 0.7f);
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

