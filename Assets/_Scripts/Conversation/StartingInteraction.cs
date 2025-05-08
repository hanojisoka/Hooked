using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartingInteraction : MonoBehaviour
{
    private GameManager _gm => GameManager.Instance;
    private ConversationManager _cm => ConversationManager.Instance;
    private AudioSystem _audio => AudioSystem.Instance;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _videoScreen;
    [SerializeField] private Slider _holdToSkipSlider;
    [SerializeField] private float _holdDuration = 3f;
    [SerializeField] private AudioClip _musicClip;

    private NPCConversation _conversation;
    private float _spaceHoldTime = 0f;

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _conversation = GetComponent<NPCConversation>();

        if (_gm.GameData.Level == 0 && _gm.GameData.FishCount == 0)
        {
            _videoPlayer.loopPointReached += OnVideoFinished;
            StartVideo();
        }
        else
        {
            _videoScreen.SetActive(false);
            _holdToSkipSlider.gameObject.SetActive(false);
            _audio.PlayMusic(_musicClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_videoPlayer.isPlaying)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _spaceHoldTime += Time.deltaTime;
                _holdToSkipSlider.gameObject.SetActive(true);
                _holdToSkipSlider.value = _spaceHoldTime / _holdDuration;

                if (_spaceHoldTime >= _holdDuration)
                {
                    SkipVideo();
                }
            }
            else
            {
                _spaceHoldTime = 0f;
                _holdToSkipSlider.gameObject.SetActive(false);
            }
        }
        else
        {
            _holdToSkipSlider.gameObject.SetActive(false);
        }
    }

    // Called when the video finishes playing
    private void OnVideoFinished(VideoPlayer source)
    {
        _videoPlayer.loopPointReached -= OnVideoFinished;
        StartConversation();
        _audio.PlayMusic(_musicClip);
    }

    private void StartVideo()
    {
        _videoScreen.SetActive(true);
        _videoPlayer.time = 0; 
        _videoPlayer.Play();
    }

    private void SkipVideo()
    {
        _videoPlayer.Stop();
        _videoScreen.SetActive(false);
        _holdToSkipSlider.gameObject.SetActive(false);
        StartConversation();
        _audio.PlayMusic(null);
    }

    private void StartConversation()
    {
        _videoScreen.SetActive(false);
        _cm.StartConversation(_conversation);
    }

}
