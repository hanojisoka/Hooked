using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleHandler : MonoBehaviour
{

    [SerializeField] private ParticleSystem magicCircleParticleSystem;
    private GameManager _gm => GameManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        _gm.OnStartFishing += StartFishing;
    }

    private void StartFishing()
    {
        if (magicCircleParticleSystem != null)
        {
            magicCircleParticleSystem.Play();
            magicCircleParticleSystem.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            LeanTween.scale(magicCircleParticleSystem.gameObject, new Vector3(3f, 3f, 3f), 0.5f).setEaseInOutSine();
        }
    }
}
