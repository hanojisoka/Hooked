using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class KutingtingConversation : SingletonMB<KutingtingConversation>
{
    private ConversationManager _cm => ConversationManager.Instance;
    private NPCConversation _conversation;
    private AudioSystem _audioSystem => AudioSystem.Instance;
    [SerializeField] List<AudioClip> _audioClips = new List<AudioClip>();

    void Start()
    {
        _conversation = GetComponent<NPCConversation>();
    }
    
    public void StartConversation()
    {
        _cm.StartConversation(_conversation);
        _cm.SetInt("Rand", Random.Range(0, 10)); 
        if (_audioClips.Count > 0)
        {
            _audioSystem.PlaySound(_audioClips[Random.Range(0, _audioClips.Count)], transform.position, 0.2f);
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }
}
