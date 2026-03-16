using TMPro;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    //initialize text on the scoreboard
    public TextMeshProUGUI P1Hits_txt;
    public TextMeshProUGUI P1Candy_txt;

    private int P1HitPoints = 0;
    private int P1CandyPoints = 0;

    private void Update()
    {
        //constantly update the texts on the scoreboard
        P1Hits_txt.GetComponent<TextMeshProUGUI>().text = "Player 1 # of Hits: " + P1HitPoints;
        P1Candy_txt.GetComponent<TextMeshProUGUI>().text = "Player 1 # of Candies: " + P1CandyPoints;
    }

    public void increaseP1Hits()
    {
        P1HitPoints += 1;
    }

}
