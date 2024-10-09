using UnityEngine;
using UnityEngine.UI;

public class AudioSystem : SingletonMB<AudioSystem>
{

    public AudioSource MusicSource;
    public AudioSource SoundsSource;
    [SerializeField] private Slider audioSlider;

    private void Start()
    {
        audioSlider.onValueChanged.AddListener(OnAudioVolumeChange);
    }
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        SoundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1)
    {
        SoundsSource.PlayOneShot(clip, vol);
    }

    public void StopSound()
    {
        SoundsSource.Stop();
    }

    private void OnAudioVolumeChange(float volume)
    {
        MusicSource.volume = volume;
    }
}
