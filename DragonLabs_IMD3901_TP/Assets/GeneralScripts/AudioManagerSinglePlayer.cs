using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class AudioManagerSinglePlayer : MonoBehaviour
{
    Scene currentScene;
    public String chosenGame;

    [Header("---- Audio Source ----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---- Audio Clip ----")]

    //pinata pop sound effects
    public AudioClip pinata_background;
    public AudioClip pinataPopSound;
    public AudioClip pickupORdrop;
    public AudioClip partyBlower;
    public AudioClip batHitSound;

    //other game sound effects (type below) >>>
    /*add other background music for the other minigames
      public AudioClip beerPong_background;
      public AudioClip ticTacToe_background;
      public AudioClip parkour_background; */




    private void Start()
    {
        Debug.Log("started");
        PlayBackgroundMusic(chosenGame);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    
    //takes in a string of which game is selected (aka which minigame's scene the audio manager prfab is in)
    public void PlayBackgroundMusic(string chosenGame)
    {
        Debug.Log("chosen game is " + chosenGame);

        switch (chosenGame)
        {
            case "PinataPop":
                musicSource.clip = pinata_background;
                musicSource.Play();
                break;

            /*
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
            */
        }
    }

}
