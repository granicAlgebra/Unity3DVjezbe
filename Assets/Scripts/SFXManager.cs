using System.Collections;
using UnityEngine;

public class SFXManager : ObjectPool<AudioSource>
{
    public static SFXManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySFX(AudioSource prefab, Vector3 position, AudioClip clip, float volume = 1)
    {
        AudioSource source = GetFromPool(prefab, position, Quaternion.identity);

        source.clip = clip;
        source.volume = volume;
        source.Play();

        StartCoroutine(ReturnAfterPlay(source, prefab));
    }

    private IEnumerator ReturnAfterPlay(AudioSource source, AudioSource prefab)
    {
        yield return new WaitForSeconds(source.clip.length);

        ReturnToPool(prefab, source);
    }
}
