using Unity.Netcode;
using Unity.VisualScripting;
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
    //beer pong sound effects
    public AudioClip ballSpawnSFX;
    public AudioClip ballFloorSFX;
    public AudioClip ballTableSFX;
    public AudioClip ballBeerSFX;
    public AudioClip drinkingSFX;

    //list of all audio clips to play
    public enum SFXList
    {
        PinataPop,
        PickupDrop,
        PartyBlower,
        BatHit,
        BallSpawn,
        BallFloor,
        BallTable,
        BallBeer,
        Drinking
    }

    public override void OnNetworkSpawn()
    {
        //check which minigame we are in, and play the corresponding background music
        PlayBackgroundMusicServerRpc(chosenGame);
    }

    private AudioClip GetClipFromType(SFXList sound) //selects the correct audio clip for clientRpc
    {
        switch(sound)
        {
            //pinata pop
            case SFXList.PinataPop: 
                return pinataPopSound;
            case SFXList.PickupDrop: 
                return pickupORdrop;
            case SFXList.PartyBlower: 
                return partyBlower;
            case SFXList.BatHit: 
                return batHitSound;

            //beer pong
            case SFXList.BallSpawn: 
                return ballSpawnSFX;
            case SFXList.BallFloor: 
                return ballFloorSFX;
            case SFXList.BallTable: 
                return ballTableSFX;
            case SFXList.BallBeer: 
                return ballBeerSFX;
            case SFXList.Drinking: 
                return drinkingSFX;
        }
        return null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlaySFXServerRpc(SFXList chosenSFX)
    {
        AudioClip clip = GetClipFromType(chosenSFX);
        SFXSource.PlayOneShot(clip);
        Debug.Log("played SFX for host");
        PlaySFXClientRpc(chosenSFX);
    }

    [ClientRpc]
    private void PlaySFXClientRpc(SFXList chosenSFX)
    {
        AudioClip clip = GetClipFromType(chosenSFX);
        SFXSource.PlayOneShot(clip);
        Debug.Log("played SFX for client");
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
