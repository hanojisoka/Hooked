using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeraAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private NavMeshAgent _agent;
    private BobberMovement _bobberMovement;
    private GameManager _gm => GameManager.Instance;
    private bool _lookAtTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        _bobberMovement = FindObjectOfType<BobberMovement>();
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _gm.OnStartFishing += OnStartFishing;
        _gm.OnStopFishing += OnStopFishing;
        _gm.OnCatchFish += OnCatchFish;
        _gm.OnReelIn += OnReelIn;
        _gm.OnCountdownFinished += OnCountdownFinished;
        
        StartCoroutine(SwitchIdleAnimations());
    }

    


    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Speed", _agent.velocity.magnitude);

        if(_lookAtTarget)
            LookAtTarget(_bobberMovement.gameObject);
    }
    private void OnCountdownFinished()
    {
        _lookAtTarget = true;
    }
    private void OnStartFishing()
    {
        Debug.Log(_anim);
        _anim.SetTrigger("Cast");
    }
    private void OnStopFishing()
    {
        _anim.SetTrigger("Pull");
        ResetRotation();
        _lookAtTarget = false;
    }
    private void OnCatchFish()
    {
        
    }
    private void OnReelIn()
    {
        _anim.SetTrigger("Pull");
        ResetRotation();
        _lookAtTarget = false;
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
    public void LookAtTarget(GameObject target)
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - _anim.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _anim.transform.rotation = Quaternion.Slerp(_anim.transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
    public void ResetRotation()
    {
        //_anim.transform.rotation = Quaternion.Zero;
        _anim.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
