using UnityEngine;
using UnityEngine.UI;

public class AudioSystem : SingletonMB<AudioSystem>
{

    public AudioSource MusicSource;
    public AudioSource SoundsSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;


    private void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        soundSlider.onValueChanged.AddListener(OnSoundVolumeChage);
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

    private void OnMusicVolumeChange(float volume)
    {
        MusicSource.volume = volume;
    }

    private void OnSoundVolumeChage(float volume)
    {
        SoundsSource.volume = volume;
    }
    
    public void SetAudioSettings(float musicVolume, float soundVolume)
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }
}
