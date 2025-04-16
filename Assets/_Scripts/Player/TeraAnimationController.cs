using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeraAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private NavMeshAgent _agent;

    private GameManager _gm => GameManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _gm.OnStartFishing += OnStartFishing;
        _gm.OnStopFishing += OnStopFishing;
        _gm.OnCatchFish += OnCatchFish;
        
        StartCoroutine(SwitchIdleAnimations());
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void OnStartFishing()
    {
        _anim.SetTrigger("Cast");
    }
    private void OnStopFishing()
    {
        _anim.SetTrigger("Pull");
    }
    private void OnCatchFish()
    {
        _anim.SetTrigger("Pull");
    }

    public void SetTrigger(string triggerName)
    {
        _anim.SetTrigger(triggerName);
    }

    private IEnumerator SwitchIdleAnimations()
    {
        while (true)
        {
            
            _anim.SetTrigger("Idle1");
            //Debug.Log("Idle1");
            yield return new WaitForSeconds(Random.Range(30f, 60f)); // Wait for a random time between 10 and 30 seconds
            if(_gm.IsFishing) continue;
            _anim.SetTrigger("Idle2");
            //Debug.Log("Idle2");
            yield return new WaitForSeconds(Random.Range(30f, 60f)); // Wait for a random time between 10 and 30 seconds
            if(_gm.IsFishing) continue;
        }
    }
}
