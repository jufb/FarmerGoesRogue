using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player variables
    public Rigidbody playerRB;
    private Animator playerAnimator;
    private AudioSource playerAudio;
    public bool isStart;

    //Control variables
    private float horizontal;
    private float vertical;
    private float speed = 10f;
    public ParticleSystem dirtParticle;

    //Player boundaries
    private float zBound = 6;
    private float xBound = 27;

    //Jump variables
    private float gravityModifier = 10;
    private float jumpForce = 1650;
    private bool doubleJump = true;
    private int countJump = 0;
    public AudioClip jumpSound;

    //Death variables
    public bool gameOver = false;
    public ParticleSystem explosionParticle;
    public AudioClip crashSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.gravity *= gravityModifier;
        AnimationStart();
    }

    private void Update()
    {
        RunHorizontalVertical();
        SetPlayerBoundaries();
        Jump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver && !isStart)
        {
            countJump = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Death();
        }
    }

    /// <summary>
    /// Animation that is played during the title screen.
    /// Hands on hips, Animation_int = 2.
    /// Wipe mouth, Animation_int = 7.
    /// </summary>
    private void AnimationStart()
    {
        playerAnimator.SetFloat("Speed_f", 0);
        playerAnimator.SetInteger("Animation_int", 1); //Crossed arms
    }

    /// <summary>
    /// Goes back to the running position.
    /// </summary>
    public void FixRunningPosition()
    {
        playerAnimator.SetInteger("Animation_int", 0); //Idle
        playerAnimator.SetFloat("Speed_f", 1.0f);
    }

    /// <summary>
    /// Allows player to jump or double jump using a public boolean doubleJump.
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && !isStart)
        {
            countJump++;
            if ((doubleJump && countJump <= 2) || (!doubleJump && countJump == 1))
            {
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                playerAudio.PlayOneShot(jumpSound, 1.0f);
                playerAnimator.Play("Running_Jump", 3, 0f);
                dirtParticle.Stop();
            }
        }
    }

    /// <summary>
    /// Moves the player based on arrow key inputs.
    /// </summary>
    private void RunHorizontalVertical()
    {
        if (!gameOver && !isStart)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector3 position = playerRB.transform.position;
            position.x += Time.deltaTime * horizontal * speed;
            position.z += Time.deltaTime * vertical * speed;

            playerRB.transform.position = position;
        }
    }

    /// <summary>
    /// Defines player boundaries on screen to avoid falling off the ground.
    /// </summary>
    private void SetPlayerBoundaries()
    {
        if (playerRB.transform.position.x > xBound)
        {
            playerRB.transform.position = new Vector3(xBound, playerRB.transform.position.y, playerRB.transform.position.z);
        }
        if (playerRB.transform.position.x < -zBound)
        {
            playerRB.transform.position = new Vector3(-zBound, playerRB.transform.position.y, playerRB.transform.position.z);
        }
        if (playerRB.transform.position.z < -zBound)
        {
            playerRB.transform.position = new Vector3(playerRB.transform.position.x, playerRB.transform.position.y, -zBound);
        }
    }

    /// <summary>
    /// Death event if player collides into an Obstacle.
    /// </summary>
    private void Death()
    {
        gameOver = true;

        explosionParticle.Play();
        playerAudio.PlayOneShot(crashSound, 1.0f);
        dirtParticle.Stop();
        playerAnimator.SetBool("Death_b", true);
        playerAnimator.SetInteger("DeathType_int", 1);
        GameObject.Find("CamManager").GetComponent<AudioSource>().Stop();
    }
}