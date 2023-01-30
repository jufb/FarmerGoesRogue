using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject scoresPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI listScores;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI timeLapseText;
    private DontDestroy dontDestroyScript;
    private bool isScoreShown = false;

    private void Start()
    {
        dontDestroyScript = GameObject.Find("Config").GetComponent<DontDestroy>();
    }

    /// <summary>
    /// Adds the current time into a list and displays the best time on Game Over screen.
    /// </summary>
    public void ShowBestScore(int time)
    {
        if (!isScoreShown)
        {
            isScoreShown = true;

            dontDestroyScript.scores.Add(time);
            dontDestroyScript.scores.Sort((a, b) => a - b);
            dontDestroyScript.scores.Reverse();

            foreach (int score in dontDestroyScript.scores)
            {
                if (time >= score)
                {
                    bestTimeText.text = FormatTime(time);
                    break;
                }
                else
                {
                    bestTimeText.text = FormatTime(score);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Displays a list of player's scores.
    /// Added on OnClick event of the button BtnScores.
    /// </summary>
    public void ShowScoresScreen()
    {
        gameOverPanel.SetActive(false);
        timeLapseText.gameObject.SetActive(false);
        scoresPanel.SetActive(true);
        ListScores();
    }

    /// <summary>
    /// Closes the list of player's scores.
    /// Added on OnClick event of the button BtnClose.
    /// </summary>
    public void CloseScoresScreen()
    {
        scoresPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        timeLapseText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Lists the time performed by the player.
    /// </summary>
    public void ListScores()
    {
        listScores.text = string.Empty;

        foreach (int item in dontDestroyScript.scores)
        {
            listScores.text += FormatTime(item);
        }
    }

    /// <summary>
    /// Converts seconds into a readable time format.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public string FormatTime(int item)
    {
        int min = item / 60;
        int sec = item % 60;

        string time;
        if (min > 0)
        {
            time = string.Format("{0}m {1}s \r\n", min, sec);
        }
        else
        {
            time = string.Format("{0}s \r\n", sec);
        }
        return time;
    }
}