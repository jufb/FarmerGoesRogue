using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int countdown = 4;
    private int time = 0;
    public bool isGameStart;

    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI timeLapseText;
    public TextMeshProUGUI pauseText;

    public GameObject panelBackground;
    public GameObject titlePanel;
    public GameObject panelOptions;
    public GameObject panelButtons;
    public GameObject gameOverPanel;

    private PlayerController playerControllerScript;
    private ScoreManager scoreManagerScript;
    private SpawnManager spawnManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        scoreManagerScript = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isGameStart)
        {
            StartGame();
        }

        PauseGame();
        GameOver();
    }



    /// <summary>
    /// Pressing escape will pause or unpause the game.
    /// </summary>
    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameStart)
        {
            if (Time.timeScale.Equals(1))
            {
                panelBackground.SetActive(true);
                pauseText.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                panelBackground.SetActive(false);
                pauseText.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    /// <summary>
    /// Starts the game by calling the coroutine SpawnTarget.
    /// Added on OnClick event of the button BtnStart.
    /// </summary>
    public void StartGame()
    {
        CloseTitleScreen();
        isGameStart = false;
        playerControllerScript.isStart = isGameStart;
        playerControllerScript.FixRunningPosition();

        StartCoroutine(Countdown());
        spawnManagerScript.StartSpawning();
    }

    private void CloseTitleScreen()
    {
        titlePanel.SetActive(false);
        panelBackground.SetActive(false);
    }

    /// <summary>
    /// Loads the scene and the list score is kept in Dont Destroy On Load.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Displays a countdown of 3 seconds on screen before starting spawing the obstacles.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            countDownText.gameObject.SetActive(true);
            countdown--;
            countDownText.text = countdown.ToString();

            if (countdown.Equals(0))
            {
                countDownText.gameObject.SetActive(false);
                StartCoroutine(TimeLapse());
                
                yield return null;
            }

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Counts the time in seconds and displays on screen.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeLapse()
    {
        while (!playerControllerScript.gameOver)
        {
            timeLapseText.gameObject.SetActive(true);
            time++;

            timeLapseText.text = scoreManagerScript.FormatTime(time);

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Stops the coroutine TimeLapse and spawning obstacles, 
    /// shows best score, and displays the Game Over screen.
    /// </summary>
    private void GameOver()
    {
        if (playerControllerScript.gameOver && !gameOverPanel.activeSelf)
        {
            StopCoroutine(TimeLapse());
            spawnManagerScript.StopSpawning();
            ShowGameOverScreen();
        }
    }

    /// <summary>
    /// Displays the Game Over screen with the best score.
    /// </summary>
    public void ShowGameOverScreen()
    {
        panelBackground.SetActive(true);
        gameOverPanel.SetActive(true);
        scoreManagerScript.ShowBestScore(time);
    }


    public void ShowOptions()
    {
        panelOptions.gameObject.SetActive(true);
        panelButtons.gameObject.SetActive(false);
    }

    public void CloseOptions()
    {
        panelOptions.gameObject.SetActive(false);
        panelButtons.gameObject.SetActive(true);
    }
}