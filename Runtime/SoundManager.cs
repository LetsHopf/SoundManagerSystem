using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    AudioSource currentMusic;
    List<AudioSource> currentSFXs = new List<AudioSource>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    [Header("-----------------Audio Clips-----------------")]

    public AudioClip exampleSFX;

    [Header("----------------- Music / Ambiance Clips-----------------")]

    public AudioClip exampleMusic;


    public GameObject PlaySFX(AudioClip sfx, bool loop, Vector3 position, float hearDistance, int spatialBlend, Transform parent, float volume = 0.7f, float pitch = 1) {
        GameObject soundObj = new GameObject("SFX");
        soundObj.transform.position = position;
        soundObj.transform.parent = parent;

        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.spatialBlend = spatialBlend;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.maxDistance = hearDistance;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.clip = sfx;
        audioSource.pitch = pitch;

        currentSFXs.Add(audioSource);

        audioSource.Play();

        if (!audioSource.loop) {
            Destroy(soundObj, sfx.length);
            currentSFXs.Remove(audioSource);
        }

        return soundObj;
    }

    public GameObject PlayMusic(AudioClip music, Vector3 position, float hearDistance, Transform parent, float volume = 0.7f) {
        GameObject soundObj = new GameObject("SFX");
        soundObj.transform.position = position;
        soundObj.transform.parent = parent;

        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        audioSource.maxDistance = hearDistance;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.clip = music;

        SetCurrentMusic(audioSource);

        audioSource.Play();

        if (!audioSource.loop) {
            Destroy(soundObj, music.length);
        }

        return soundObj;
    }

    public void SetCurrentMusic(AudioSource music) {
        currentMusic = music;
    }

    public void PauseMusic() {
        if (currentMusic != null && currentMusic.isPlaying) {
            currentMusic.Pause();
        }
    }

    public void ResumeMusic() {
        if (currentMusic != null) {
            currentMusic.UnPause();
        }
    }

    public void PauseSFX() {
        for (int i = currentSFXs.Count - 1; i >= 0; i--) {
            AudioSource sfx = currentSFXs[i];
            if (sfx == null) {
                currentSFXs.RemoveAt(i);
            } else {
                if (sfx.isPlaying) {
                    sfx.Pause();
                }
            }
        }
    }

    public void ResumeSFX() {
        for (int i = currentSFXs.Count - 1; i >= 0; i--) {
            AudioSource sfx = currentSFXs[i];
            if (sfx == null) {
                currentSFXs.RemoveAt(i); // Remove null references
            } else {
                sfx.UnPause();
            }
        }
    }
}
