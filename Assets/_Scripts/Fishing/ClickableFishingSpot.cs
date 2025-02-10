using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableFishingSpot : MonoBehaviour
{
    private GameManager _gm;
    private CharacterMoveToPosition playerMove;

    private void Start()
    {
        _gm = GameManager.Instance;
        playerMove = _gm.Player.GetComponent<CharacterMoveToPosition>();
    }
    void OnMouseUp()
    {
        Transform fishingSpot = transform.parent;
        playerMove.MoveToPosition(new Vector3(fishingSpot.position.x, transform.position.y, fishingSpot.position.z)); // Target position (even if outside NavMesh)
    }
}
