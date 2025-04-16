using UnityEngine.AI;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using System;

public class CharacterMoveToPosition : MonoBehaviour
{
    public event Action<string> OnArrivedToSpot;

    public NavMeshAgent agent;
    public float maxSearchRadius = 5.0f; // Max distance to find a valid NavMesh position
    public float rotationSpeed = 5f; // Speed of rotation to face target

    [SerializeField] private Transform fishingSpot;
    public UnityEvent onArrival; // Event triggered on arrival
    //public Animator animator; // Reference to the Animator component

    private bool hasArrived = true; // To prevent multiple triggers
    private Vector3 lastTargetPosition; // Store the target position
    private GameManager _gm;
    private string currentSpotTag;

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.Player = this.gameObject;
    }

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        
        _gm.ListenToEventsFromPlayer();
        /*if (animator == null)
            animator = GetComponent<Animator>(); // Auto-assign Animator*/
    }

    public void MoveToPosition(Vector3 targetPosition, string spotTag)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, maxSearchRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            lastTargetPosition = targetPosition;
            hasArrived = false; // Reset arrival status
            currentSpotTag = spotTag;
        }
        else
        {
            Debug.Log("No valid NavMesh position found near the target!");
        }
    }

    void Update()
    {
        // Check if the agent has arrived
        if (!hasArrived && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                hasArrived = true; // Ensure event triggers only once
                onArrival?.Invoke(); // Trigger arrival event
                //animator.SetBool("isWalking", false); // Stop walking animation
                Debug.Log("Arrived");
                OnArrivedToSpot?.Invoke(currentSpotTag);
                StartCoroutine(RotateTowardsTarget(lastTargetPosition));
            }
        }
    }

    System.Collections.IEnumerator RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 lookDirection = (targetPosition - transform.position).normalized;
        lookDirection.y = 0; // Keep rotation on the horizontal plane
        Debug.Log("Start Rotating");
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            Debug.Log("Adjusting Rotation");
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure precise alignment
    }
}
