using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFollowLine : MonoBehaviour
{
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
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
}
