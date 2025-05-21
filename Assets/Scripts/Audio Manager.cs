using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Author: Rehope Games.
    //Date: 25 February 2023.
    //Title: How to Add MUSIC and SOUND EFFECTS to a Game in Unity [Source Code].
    //Availability: https://www.youtube.com/watch?v=N8whM1GjH4w.

    [Header("---------- Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Source----------")]
    public AudioClip Background;
    public AudioClip MCWalk;
    public AudioClip MCWalk2;
    public AudioClip MCWalk3;
    public AudioClip GameOver;
    public AudioClip MainPage;
    public AudioClip MenuPopup;
    public AudioClip ChestOpen;
    public AudioClip ChestClose;
    public AudioClip Acquired;
    public AudioClip ObjectDrop;
    public AudioClip ButtonPress;
    public AudioClip MCAttack;
    public AudioClip MCHurt;
    public AudioClip Dialogue;
    public AudioClip MonsterDamage;
    public AudioClip MonsterWalk;
    public AudioClip MonsterLair;
    public AudioClip ButtonClick;

    private void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
