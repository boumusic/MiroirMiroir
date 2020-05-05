using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipSettings
{
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
    [Range(-1, 1)] public float pan;
    public bool loop = false;
    [Range(0, 1)] public float randomPitch = 0;
    public bool playOnAwake = false;

    public AudioClip[] randomVariations;

    public AudioClip FinalClip
    {
        get
        {
            if (randomVariations.Length == 0)
            {
                return clip;
            }

            else
            {
                List<AudioClip> totalClips = new List<AudioClip>();
                totalClips.Add(clip);
                for (int i = 0; i < randomVariations.Length; i++)
                {
                    totalClips.Add(randomVariations[i]);
                }

                return totalClips[Random.Range(0, totalClips.Count)];
            }
        }                
    }

    public AudioClipSettings()
    {
        volume = 1f;
    }

    //public AudioClipSettings(float volume, float pan, bool loop, bool awake, float randomPitch)
    //{
    //    this.volume = volume;
    //    this.pan = pan;
    //    playOnAwake = awake;
    //    this.loop = loop;
    //    this.randomPitch = randomPitch;
    //}
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { if (instance == null) instance = FindObjectOfType<AudioManager>(); return instance; } }

    public bool clipsFoldout = false;
    public List<AudioClipSettings> clips = new List<AudioClipSettings>();
    public List<AudioSource> sourcePool = new List<AudioSource>();

    public bool on = true;

    [Range(1, 150)] public int sourceAmount = 30;

    private void Awake()
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if(clips[i].playOnAwake)
            {
                PlaySound(clips[i].clip.name);
            }
        }
    }

    private void Update()
    {
        DeactivateSourcesNotPlayingSound();
    }

    private void DeactivateSourcesNotPlayingSound()
    {
        for (int i = 0; i < sourcePool.Count; i++)
        {
            if (sourcePool[i].gameObject.activeInHierarchy && !sourcePool[i].isPlaying)
            {
                sourcePool[i].gameObject.SetActive(false);
            }
        }
    }

    #region Public Methods

    public void PlaySFX(string name)
    {
        name = "SFX_" + name;
        PlaySound(name);
    }

    public void PlaySound(string name)
    {
        if (on)
        {
            AudioSource newSource = GetSource();
            newSource.gameObject.SetActive(true);
            AudioClipSettings newClip = GetClip(name);
            SetSourceSettings(newSource, newClip);
            newSource.Play();
        }
    }

    public void StopSound(string name)
    {
        for (int i = 0; i < sourcePool.Count; i++)
        {
            if (sourcePool[i].gameObject.activeInHierarchy)
            {
                if (sourcePool[i].clip != null)
                {
                    if (sourcePool[i].clip.name == name)
                    {
                        sourcePool[i].Stop();
                    }
                }
            }
        }
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < sourcePool.Count; i++)
        {
            sourcePool[i].Stop();
            sourcePool[i].gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        DestroySources();

        for (int i = 0; i < sourceAmount; i++)
        {
            GameObject go = new GameObject("AudioSource_" + i);
            go.transform.localPosition = Vector3.zero;
            go.transform.parent = transform;
            AudioSource newSource = go.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            sourcePool.Add(newSource);
            newSource.gameObject.SetActive(false);
        }
    }

    public void DestroySources()
    {
        if (sourcePool.Count > 0)
        {
            for (int i = 0; i < sourcePool.Count; i++)
            {
                DestroyImmediate(sourcePool[i].gameObject);
            }

            sourcePool.Clear();
        }
    }

    #endregion

    private void SetSourceSettings(AudioSource source, AudioClipSettings clipSettings)
    {
        if (source && clipSettings != null)
        {
            
            source.clip = clipSettings.FinalClip;
            source.volume = clipSettings.volume;
            source.panStereo = clipSettings.pan;
            source.loop = clipSettings.loop;
            source.playOnAwake = clipSettings.playOnAwake;
            source.pitch = 1 + Random.Range(-clipSettings.randomPitch, clipSettings.randomPitch);
        }
    }

    #region Returns

    private AudioSource GetSource()
    {
        for (int i = 0; i < sourcePool.Count; i++)
        {
            if (!sourcePool[i].gameObject.activeInHierarchy)
            {
                return sourcePool[i];
            }
        }

        return null;
    }

    private AudioClipSettings GetClip(string name)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            if (clips[i].clip.name == name)
            {
                return clips[i];
            }
        }

        return null;
    }

    #endregion
}
