using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class KutingMovement : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private NavMeshAgent navMeshAgent;
    private bool isActive;
    private GameManager _gm => GameManager.Instance;
    private Transform _playerPosition;

    private void Start()
    {
        _playerPosition = _gm.Player.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(SwitchIdleAnimations());
    }

    void Update()
    {
        AnimationUpdate();
    }

    [Button]
    public void StartWalking()
    {
        Debug.Log("Start Walking");
        if (points.Length == 0) return;
        navMeshAgent.enabled = true;
        // Teleport to points[0]
        navMeshAgent.Warp(points[0].position);

        // Start walking coroutine
        StartCoroutine(WalkToRandomPoints());
    }

    private IEnumerator WalkToRandomPoints()
    {
        isActive = true;

        while (isActive)
        {
            // Choose a random point
            Transform targetPoint = points[Random.Range(0, points.Length)];

            // Set the NavMeshAgent destination
            navMeshAgent.SetDestination(targetPoint.position);

            // Wait until the agent reaches the destination 
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.1f)
            {

                yield return null;
            }

            // Rotate smoothly to a random direction
            float randomRotation = Random.Range(0f, 360f);
            Quaternion targetRotation = Quaternion.Euler(0, randomRotation, 0);
            float rotationDuration = 1f; // Adjust rotation duration as needed
            float elapsedTime = 0f;


            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation; // Ensure final rotation is exact

            // Wait for a random time between 1 to 3 seconds
            yield return new WaitForSeconds(Random.Range(10f, 25f));
        }
    }
    private void RotateTowardsPlayer()
    {
        // Get the player's position
        Vector3 playerPosition = _gm.Player.transform.position;

        // Calculate the direction to the player
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Rotate towards the player smoothly
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
    #region Animation
    [SerializeField] private Animator _anim;

    private void AnimationUpdate()
    {
        _anim.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private IEnumerator SwitchIdleAnimations()
    {
        while (true)
        {
            _anim.SetTrigger("Idle1");
            Debug.Log("Idle1");
            yield return new WaitForSeconds(Random.Range(30f, 60f)); // Wait for a random time between 10 and 30 seconds

            _anim.SetTrigger("Idle2");
            Debug.Log("Idle2");
            yield return new WaitForSeconds(Random.Range(30f, 60f)); // Wait for a random time between 10 and 30 seconds

        }
    }
    #endregion
}