using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public enum SoundType
    {
        sfx,
        music
    }

    private struct LoopingSource
    {
        public GameObject player;
        public SoundType type;
    }

    public static SoundManager instance;
    private Dictionary<string, LoopingSource> loopingAudio = new();

    private float masterVolume = 0;
    private float musicVolume = 0;
    private float sfxVolume = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogWarning("Found Multiple Sound Managers. Deleting duplicates");
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);    
    }

    private void Start()
    {
        ApplyVolumePrefs();
    }

    public void UpdateVolumePrefs(float masterVol, float musicVol, float sfxVol)
    {
        PlayerPrefs.SetFloat("masterVolume", masterVol);
        PlayerPrefs.SetFloat("musicVolume", musicVol);
        PlayerPrefs.SetFloat("sfxVolume", sfxVol);
    }

    public void ApplyVolumePrefs()
    {
        if (!PlayerPrefs.HasKey("masterVolume"))
            PlayerPrefs.SetFloat("masterVolume", 1);
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 0.8f);
        if (!PlayerPrefs.HasKey("sfxVolume"))
            PlayerPrefs.SetFloat("sfxVolume", 0.8f);

        masterVolume = PlayerPrefs.GetFloat("masterVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        foreach(KeyValuePair<string, LoopingSource> source in loopingAudio)
        {
            float newVolume = masterVolume * (source.Value.type == SoundType.music ? musicVolume : sfxVolume);
            source.Value.player.GetComponent<AudioSource>().volume = newVolume;
        }
    }

    public void AddLoopingAudio(string tag, AudioClip clip, SoundType type)
    {
        GameObject soundPlayer = Instantiate(new GameObject(), transform);
        
        AudioSource source = soundPlayer.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = masterVolume;

        source.volume *= type == SoundType.music ? musicVolume : sfxVolume;

        source.Play();

        LoopingSource finalSouce = new()
        {
            player = soundPlayer,
            type = type,
        };

        loopingAudio.Add(tag, finalSouce);
    }

    public void RemoveLoopingAudio(string tag)
    {
        LoopingSource soundPlayer = loopingAudio[tag];

        Destroy(soundPlayer.player);

        loopingAudio.Remove(tag);
    }
    public void SetLoopingSoundVolume(string tag, float volume)
    {
        LoopingSource soundPlayer = loopingAudio[tag];
        AudioSource source = soundPlayer.player.GetComponent<AudioSource>();

        source.volume = volume;
    }

    public void PlayOneshotAudio(AudioClip clip, Vector3 location, SoundType type, float volume = 1)
    {
        float finalVolume = volume * masterVolume;

        AudioSource.PlayClipAtPoint(clip, location, finalVolume * (type == SoundType.music ? musicVolume : sfxVolume));
    }
}
