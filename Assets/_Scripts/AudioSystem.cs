using UnityEngine;
using UnityEngine.UI;

public class AudioSystem : SingletonMB<AudioSystem>
{

    public AudioSource MusicSource;
    public AudioSource SoundsSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [Header("SFX Clips")]
    [SerializeField] private AudioClip _buttonPress;
    [SerializeField] private AudioClip _alarmSound;
    


    private void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        soundSlider.onValueChanged.AddListener(OnSoundVolumeChage);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            MusicSource.clip = clip;
        }
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

    public void PlaySound(AudioClip clip)
    {
        SoundsSource.transform.position = Camera.main.transform.position;
        SoundsSource.PlayOneShot(clip, 1);
    }

    public void PlayLoopingSound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        SoundsSource.transform.position = pos;
        SoundsSource.clip = clip;
        SoundsSource.volume = vol;
        SoundsSource.loop = true;
        SoundsSource.Play();
    }

    public void PlayButtonPressSound()
    {
        PlaySound(_buttonPress);
    }
    public void PlayAlarmSound()
    {
        PlayLoopingSound(_alarmSound, transform.position, 0.7f);
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
