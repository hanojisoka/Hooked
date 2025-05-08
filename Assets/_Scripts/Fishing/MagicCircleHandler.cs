using UnityEngine;

public class MagicCircleHandler : MonoBehaviour
{

    [SerializeField] private ParticleSystem magicCircleParticleSystem;
    private GameManager _gm => GameManager.Instance;
    private Transform _originalParent;
    private Vector3 _originalScale;
    private BobberMovement _bobberMovement;
    private AudioSystem _audioSystem => AudioSystem.Instance;
    [SerializeField] private AudioClip _magicCircleStart;
    [SerializeField] private AudioClip _magicCircleLoop;
    // Start is called before the first frame update
    void Start()
    {
        _originalParent = magicCircleParticleSystem.transform.parent;
        _originalScale = magicCircleParticleSystem.transform.localScale;
        _bobberMovement = FindObjectOfType<BobberMovement>();
        _gm.OnStartFishing += StartFishing;
        _gm.OnCountdownFinished += CountdownFinished;
        _gm.OnStopFishing += StopFishing;
        _gm.OnReelIn += ReelIn;
    }

    private void ReelIn()
    {

        magicCircleParticleSystem.transform.parent = null;
        magicCircleParticleSystem.transform.localScale = _originalScale;
        LeanTween.scale(magicCircleParticleSystem.gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                magicCircleParticleSystem.Stop();
                magicCircleParticleSystem.Clear();
                magicCircleParticleSystem.transform.parent = _originalParent;
                magicCircleParticleSystem.transform.localPosition = Vector3.zero;
                magicCircleParticleSystem.transform.parent.localPosition = Vector3.zero;
            });
        
    }

    private void StopFishing()
    {
        magicCircleParticleSystem.transform.parent = null;
        magicCircleParticleSystem.transform.localScale = _originalScale;
        LeanTween.scale(magicCircleParticleSystem.gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
            .setEaseInOutSine()
            .setOnComplete(() =>
            {
                magicCircleParticleSystem.Stop();
                magicCircleParticleSystem.Clear();
                magicCircleParticleSystem.transform.parent = _originalParent;
                magicCircleParticleSystem.transform.localPosition = Vector3.zero;
                magicCircleParticleSystem.transform.parent.localPosition = Vector3.zero;
            });
        
    }

    private void CountdownFinished()
    {
        if(!transform.parent.gameObject.activeSelf) return;
        LeanTween.scale(magicCircleParticleSystem.gameObject, new Vector3(2f, 2f, 2f), 2f).setEaseInOutSine();
        magicCircleParticleSystem.transform.parent = _bobberMovement.transform;
    }

    private void StartFishing()
    {
        if(!transform.parent.gameObject.activeSelf) return;
        if (magicCircleParticleSystem != null)
        {
            magicCircleParticleSystem.Play();
            magicCircleParticleSystem.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            LeanTween.scale(magicCircleParticleSystem.gameObject, new Vector3(3f, 3f, 3f), 4f).setEaseInOutSine();
            _audioSystem.PlaySound(_magicCircleStart, transform.position, 0.5f);
            _audioSystem.PlayLoopingSound(_magicCircleLoop, transform.position, 0.7f);
        }
    }
}
