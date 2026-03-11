using TMPro;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    //initialize text on the scoreboard
    public TextMeshProUGUI P1Hits_txt;
    public TextMeshProUGUI P1Candy_txt;
    public TextMeshProUGUI P2Hits_txt;
    public TextMeshProUGUI P2Candy_txt;

    private int P1HitPoints = 0;
    private int P1CandyPoints = 0;
    private int P2HitPoints = 0;
    private int P2CandyPoints = 0;

    private void Update()
    {
        //constantly update the texts on the scoreboard
        P1Hits_txt.GetComponent<TextMeshProUGUI>().text = "Player 1 # of Hits: " + P1HitPoints;
        P1Candy_txt.GetComponent<TextMeshProUGUI>().text = "Player 1 # of Candies: " + P1CandyPoints;

        P2Hits_txt.GetComponent<TextMeshProUGUI>().text = "Player 2 # of Hits: " + P2HitPoints;
        P2Candy_txt.GetComponent<TextMeshProUGUI>().text = "Player 2 # of Candies: " + P2CandyPoints;




    }

    public void increaseP1Hits()
    {
        P1HitPoints += 1;
    }
    public void increaseP2Hits()
    {
        P2HitPoints += 1;
    }










}
