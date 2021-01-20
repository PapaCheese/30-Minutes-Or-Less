using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 123;
    public SinglePlayerModeMain brain;

    private bool playedRunningOutOfTimeSound = false;
    private bool playedRanOutOfTimeSound = false;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = (int)(timeRemaining / 60);
            int seconds = (int)(timeRemaining % 60);

            string strSeconds = seconds.ToString();
            if (seconds < 10)
            {
                strSeconds = "0" + seconds.ToString();
            }
            GetComponent<UnityEngine.UI.Text>().text = minutes.ToString() + ":" + "<color=#464646>" + strSeconds + "</color>";

            if (timeRemaining < 5 && !playedRunningOutOfTimeSound)
            {
                playedRunningOutOfTimeSound = true;
                brain.audioController.PlaySound("RunningOutOfTime");
            }

        }
        else if (!playedRanOutOfTimeSound)
        {
            playedRanOutOfTimeSound = true;
            brain.RanOutOfTime();
        }
    }
}
