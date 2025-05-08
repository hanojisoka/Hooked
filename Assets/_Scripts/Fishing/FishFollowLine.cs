using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;

public class FishFollowLine : MonoBehaviour
{
    [SerializeField] private GameObject _swimmingFish;
    [SerializeField] private GameObject _floppingFish;
    [SerializeField] private AudioClip _fishFloppingSound;
    private Vector3 lastPosition;
    private GameManager _gm => GameManager.Instance;
    private GameObject _player => GameManager.Instance.Player;
    private AudioSystem _audio => AudioSystem.Instance;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;        
        _gm.OnReelIn += OnReelIn;
    }


    // Update is called once per frame
    void Update()
    {
        // Calculate the movement direction
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.sqrMagnitude > 0.001f) // Check if there is significant movement
        {
            // Rotate towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
        }

        // Update the last position
        lastPosition = transform.position;        
    }
    private void OnReelIn()
    {
        LaunchTowardsPlayer();
    }
    [Button("Launch Fish Towards Player")]
    public void LaunchTowardsPlayer()
    {
        if (_player == null) return;

        StartCoroutine(LaunchTowardsPlayerCoroutine());
    }

    private IEnumerator LaunchTowardsPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.75f); // Wait for a moment before launching
        _swimmingFish.SetActive(true);
        // Calculate the target position (player's position)
        Vector3 targetPosition = _player.transform.position;

        // Calculate the peak height for the arc
        float peakHeight = 5f; // Adjust peak height as needed
        float moveDuration = 1f; // Adjust duration as needed
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / moveDuration;

            // Interpolate position horizontally
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Add vertical arc using a parabola formula
            float height = Mathf.Sin(t * Mathf.PI) * peakHeight;
            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap to the final position to ensure accuracy
        transform.position = targetPosition;

        _floppingFish.SetActive(true);
        _swimmingFish.SetActive(false);
        _floppingFish.transform.parent = null; // Unparent the flopping fish to avoid unwanted transformations
        _audio.PlaySound(_fishFloppingSound, _floppingFish.transform.position, 0.5f); // Play sound at the flopping fish position
        //_swimmingFish.transform.position = Vector3.zero; // Reset swimming fish position
        transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(2f); // Wait for a moment before adding fish
        _gm.CatchFish();
        _audio.StopSound(); // Stop the sound after the fish is caught
        _floppingFish.SetActive(false);
        _floppingFish.transform.parent = _player.transform; // Reparent the flopping fish to the player
    }
}
