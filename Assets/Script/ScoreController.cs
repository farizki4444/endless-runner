using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public CharacterSoundController sound;

private int curretScore = 0;
    private int lastScoreHighlight = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        //restart
        curretScore = 0;
        lastScoreHighlight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetCurrentScore (){

        return curretScore;
    }

    public void IncreaseCurrentScore(int increment){

        curretScore += increment;
        if(curretScore - lastScoreHighlight > scoreHighlightRange){
            sound.PlayScoreHighlight();
            lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring(){

    if(curretScore > ScoreData.highScore){

            ScoreData.highScore = curretScore;
    }
    }
}
