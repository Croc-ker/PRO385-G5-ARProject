using System.Collections;
using UnityEngine;

public enum SoundType
{
    ASGORE_PASSIVE,
    ASGORE_DRUNK,
    DRINK,
    PICKUP,
    HIT,
    HIT2,
    CAR
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }

    // Plays a sound in a loop until stopped
    public static void PlayLoopingSound(SoundType sound, float volume)
    {
        instance.audioSource.clip = instance.soundList[(int)sound];
        instance.audioSource.volume = volume;
        instance.audioSource.loop = true;
        instance.audioSource.Play();
    }

    // Stops the currently looping sound
    public static void StopLoopingSound()
    {
        instance.audioSource.Stop();
        instance.audioSource.loop = false;
        instance.audioSource.clip = null;
    }

   public static IEnumerator playAfterDelay(SoundType sound, float volume, float delay = 0)
   {
      yield return new WaitForSeconds(delay);
      PlayLoopingSound(sound, volume);
   }
}