using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) s.source = gameObject.AddComponent<AudioSource>();
        CheckSounds();
    }

    private void OnValidate() => CheckSounds();

    private void CheckSounds()
    {
        foreach (Sound s in sounds)
        {
            if (s.source == null) return;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        if (s.useRandomPitch) s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found !");
            return;
        }
        s.source.Stop();
    }
}