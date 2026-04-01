using System;
using UnityEngine;

public class AudioManagerSinglePlayer : MonoBehaviour
{
    public static AudioManagerSinglePlayer instance;

    public string chosenGame;

    [Header("---- Audio Source ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---- Audio Clip ----")]

    // Pinata Pop
    public AudioClip pinata_background;
    public AudioClip pinataPopSound;
    public AudioClip pickupORdrop;
    public AudioClip partyBlower;
    public AudioClip batHitSound;

    // Other game background music
    public AudioClip beerPong_background;
    public AudioClip ticTacToe_background;

    [Header("---- Parkour Audio Clip ----")]
    public AudioClip parkour_background;
    public AudioClip parkour_jumpSound;

    [Header("---- TicTacToe Audio ----")]
    public AudioClip ttt_winSound;
    public AudioClip ttt_loseSound;
    public AudioClip ttt_drawSound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("started");
        PlayBackgroundMusic(chosenGame);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || SFXSource == null) return;
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(string chosenGame)
    {
        if (musicSource == null) return;

        Debug.Log("chosen game is " + chosenGame);

        switch (chosenGame)
        {
            case "PinataPop":
                musicSource.clip = pinata_background;
                musicSource.Play();
                break;

            case "BeerPong":
                musicSource.clip = beerPong_background;
                musicSource.Play();
                break;

            case "TicTacToe":
                musicSource.clip = ticTacToe_background;
                musicSource.Play();
                break;

            case "ParkourRace":
                musicSource.clip = parkour_background;
                musicSource.Play();
                break;
        }
    }
}