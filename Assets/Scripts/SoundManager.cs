using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickUpSound;
    public AudioSource grassWalkSound;
    public AudioSource treeFallingSound;

    //Music
    public AudioSource startingZoneBGMusic;
    public AudioSource natureSounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }
    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
