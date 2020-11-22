using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Tooltip("The audio source component that plays the sound effects.")]
    [SerializeField]
    private AudioSource soundEffectAS;
    [Tooltip("The sound clip that plays when a player completes a lap.")]
    [SerializeField]
    private AudioClip lapSound;
    [Tooltip("The sound clip that plays when the race starts.")]
    [SerializeField]
    private AudioClip startSound;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    public void PlayLapSound()
    {
        soundEffectAS.PlayOneShot(lapSound);
    }

    public void PlayStartSound()
    {
        soundEffectAS.PlayOneShot(startSound);
    }
}
