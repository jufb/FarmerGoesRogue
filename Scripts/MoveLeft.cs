using System.Collections;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float minSpeed = 0;
    private float maxSpeed = 30f;
    private float offBounds = -15f;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver && !playerControllerScript.isStart)
        {
            if (!gameObject.CompareTag("Obstacle"))
            {
                StartCoroutine(StartSlow());
            }
            else
            {
                minSpeed = maxSpeed;
            }

            transform.Translate(Vector3.left * Time.deltaTime * minSpeed);
        }

        if ((transform.position.x < offBounds || transform.position.y < offBounds) 
            && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StartSlow()
    {
        float speed = 0.1f;

        while (minSpeed <= maxSpeed)
        {
            minSpeed += speed;
            yield return new WaitForSeconds(1);
        }
    }
}