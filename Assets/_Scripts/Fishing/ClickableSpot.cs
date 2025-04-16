using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSpot : MonoBehaviour
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
        Transform spot = transform;
        Debug.Log(spot.name);
        playerMove.MoveToPosition(new Vector3(spot.position.x, transform.position.y, spot.position.z), spot.gameObject.tag); // Target position (even if outside NavMesh)
    }
}
