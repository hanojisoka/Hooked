using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableSpot : MonoBehaviour
{
    private GameManager _gm;
    private CharacterMoveToPosition playerMove;
    [SerializeField] private float _stoppingDistance = 1.5f; // Adjust this value as needed

    public UnityEvent OnSpotClicked; // Event triggered on spot click

    private void Start()
    {
        _gm = GameManager.Instance;
        playerMove = _gm.Player.GetComponent<CharacterMoveToPosition>();
    }
    void OnMouseUp()
    {
        Transform spot = transform;
        Debug.Log(spot.name);

        Vector3 direction = (spot.position - playerMove.transform.position).normalized; // Correct direction calculation
        Vector3 targetPosition = spot.position - direction * _stoppingDistance;

        playerMove.MoveToPosition(new Vector3(targetPosition.x, spot.position.y, targetPosition.z), spot.gameObject.tag); // Adjusted target position
        playerMove.RotateTowards(spot.position); // Trigger rotation towards the spot
        OnSpotClicked?.Invoke(); // Invoke the event
    }
}
