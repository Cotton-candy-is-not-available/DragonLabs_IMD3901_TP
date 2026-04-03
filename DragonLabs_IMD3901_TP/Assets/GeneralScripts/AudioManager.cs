using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : NetworkBehaviour
{
    public string chosenGame;

    [Header("---- Audio Source ----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---- Audio Clip ----")]
    public AudioClip pinata_background;
     //add other background music for the other minigames
    public AudioClip beerPong_background;
    public AudioClip lobby_background;

    [Header("---- Pinata pop Audio Clip ----")]
    //pinata pop sound effects
    public AudioClip pinataPopSound;
    public AudioClip pickupORdrop;
    public AudioClip partyBlower;
    public AudioClip batHitSound;


    [Header("---- BeerPong Audio Clip ----")]
    //pinata pop sound effects
    public AudioClip ballSpawnSFX;
    public AudioClip ballFloorSFX;
    public AudioClip ballTableSFX;
    public AudioClip ballBeerSFX;
    public AudioClip drinkingSFX;




    public override void OnNetworkSpawn()
    {
        //check which minigame we are in, and play the corresponding background music
        PlayBackgroundMusicServerRpc(chosenGame);
    }


    //[ServerRpc(RequireOwnership = false)]

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        Debug.Log("played SFX for host");
    }

    //takes in a string of which game is selected (aka which minigame's scene the audio manager prfab is in)
    [ServerRpc(RequireOwnership = false)]
    public void PlayBackgroundMusicServerRpc(string chosenGame) 
    {
        Debug.Log("chosen game is " + chosenGame);

        switch (chosenGame)
        {
            case "PinataPop":
                musicSource.clip = pinata_background;
                musicSource.Play();
                break;

            case "beerPong":
                musicSource.clip = beerPong_background;
                musicSource.Play();
                break;

            case "lobby":
                musicSource.clip = lobby_background;
                musicSource.Play();
                break;


        }
    }





}
