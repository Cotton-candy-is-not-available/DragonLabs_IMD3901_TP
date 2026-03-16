using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : NetworkBehaviour
{
    [Header("---- Audio Source ----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---- Audio Clip ----")]
    public AudioClip pinata_background;
    /* add other background music for the other minigames
    public AudioClip beerPong_background;
    public AudioClip ticTacToe_background;
    public AudioClip parkour_background; */

    public string chosenGame;


    public override void OnNetworkSpawn()
    {
        //check which minigame we are in, and play the corresponding background music
        PlayBackgroundMusicServerRpc(chosenGame);
    }


    //[ServerRpc(RequireOwnership = false)]
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
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
