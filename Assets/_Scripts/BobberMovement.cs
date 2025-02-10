using UnityEngine;
using System.Collections;

public class BobberMovement : MonoBehaviour
{
    //public float squareSize = 10f; // Size of the square boundary
    public float minWaitTime = 1f; // Minimum wait time before moving
    public float maxWaitTime = 5f; // Maximum wait time before moving
    public float moveSpeed = 2f; // Speed of the movement
    //public Vector3 squareOffset = Vector3.zero; // Offset for the square's position
    [SerializeField] private FishingSpot fishingSpot;

    private Vector3 targetPosition;
    private Coroutine movementCoroutine;
    private FishingSpotManager _fm;
    private void Start()
    {
        _fm = FishingSpotManager.Instance;
        _fm.OnNewFishingSpot += _fm_OnNewFishingSpot;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        _fm.OnNewFishingSpot -= _fm_OnNewFishingSpot;
    }

    private void _fm_OnNewFishingSpot(FishingSpot spot)
    {
        fishingSpot = spot;
    }

    IEnumerator MoveRandomly()
    {
        transform.position = fishingSpot.transform.GetChild(0).position;
        while (true)
        {
            // Wait for a random time before moving
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            targetPosition = GetTargetPosition();

            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null; // Wait until the next frame
            }
        }
    }

    // Function to stop the movement
    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
            transform.localPosition = Vector3.zero;
        }
    }

    // Function to start the movement
    public void StartMovement()
    {
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(MoveRandomly());
        }
    }

    private Vector3 GetTargetPosition()
    {
        float squareSize = fishingSpot.SquareSize;
        // Generate a new random target position inside the square boundary with offset
        float randomX = Random.Range(-squareSize / 2f, squareSize / 2f) + fishingSpot.transform.GetChild(0).transform.position.x;
        float randomZ = Random.Range(-squareSize / 2f, squareSize / 2f) + fishingSpot.transform.GetChild(0).transform.position.z;
        return new Vector3(randomX, transform.position.y, randomZ);
    }


}
