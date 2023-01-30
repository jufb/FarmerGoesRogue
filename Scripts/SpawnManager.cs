using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> obstacles;

    private readonly float startDelay = 3; //three seconds delay
    private PlayerController playerControllerScript;
    private DontDestroy dontDestroyScript;

    private void Start()
    {
        dontDestroyScript = GameObject.Find("Config").GetComponent<DontDestroy>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// Starts invoking random obstacles after 3 sec delay and repeating every sec / difficulty.
    /// </summary>
    public void StartSpawning()
    {
        InvokeRepeating("SpawnRandomObstacle", startDelay, dontDestroyScript.difficultyStart);
    }

    /// <summary>
    /// Stops invoking random obstacles.
    /// </summary>
    public void StopSpawning()
    {
        CancelInvoke("SpawnRandomObstacle");
    }

    /// <summary>
    /// Sets the random position of the obstacles and instantiate them.
    /// </summary>
    void SpawnRandomObstacle()
    {
        if (!playerControllerScript.isStart && !playerControllerScript.gameOver)
        {
            int randomObstacle = Random.Range(0, obstacles.Count);
            Vector3 randomPosition = obstacles[randomObstacle].transform.position;
            randomPosition.z = Random.Range(-7, 13);
            Instantiate(obstacles[randomObstacle], randomPosition, obstacles[randomObstacle].transform.rotation);
        }
    }
}