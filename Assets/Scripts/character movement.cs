using UnityEngine;

public class character : MonoBehaviour
{

    public float movespeed = 5f;//speed of player

    private Vector2 input;//input from player
    private Vector2 lastMoveDirection;//last move direction

    public Transform Aim;//aiming direction
    bool isWalking = false;//is player walking

    private Animator animator;

    private bool playingFootsteps = false;//is player playing footsteps
    public float footstepSpeed = 0.5f;//speed of footsteps 

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (PauseController.IsGamePaused)//if the game is paused
        {
            movespeed = 0f;// stop movement 
            StopFootsteps();//stop footsteps
        }
        else
        {
            movespeed = 1f;// set movement speed
        }
        if (movespeed == 1f && !playingFootsteps)//if the player is walking and not playing footsteps
        {
            StartFootsteps();//start footsteps
        }
        else if (movespeed == 0)//if the player is not walking and playing footsteps
        {
            StopFootsteps();//stop footsteps
        }

        ProcessInputs();//process inputs
        Vector3 moveDirection = Vector3.zero;
        isWalking = false;
        animator.SetBool("isWalking", false);

        // Removed ambiguous Context.canceled check and related animator updates.

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y += 1;//move up
            animator.SetBool("isWalking", true);//set animator walking state

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y -= 1;//move down
            animator.SetBool("isWalking", true);//set animator walking state
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1;//move left
            animator.SetBool("isWalking", true);//set animator walking state
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;//move right
            animator.SetBool("isWalking", true);//set animator walking state
        }

        transform.position += moveDirection.normalized * movespeed * Time.deltaTime;//move player   
        animator.SetFloat("InputY", moveDirection.y);//set animator input y
        animator.SetFloat("InputX", moveDirection.x);//set animator input x

        // After movement and animator updates
        Vector3 aimDirection = new Vector3(input.x, input.y, 0f);
        if (aimDirection.sqrMagnitude > 0.01f)
        {
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -aimDirection);
            lastMoveDirection = input;
        }
        else if (lastMoveDirection.sqrMagnitude > 0.01f)
        {
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -lastMoveDirection);
        }
    }

    public void FixedUpdate()
    {
        if (isWalking)//if the player is walking
        {
            Vector3 vector3 = new Vector3(input.x, input.y, 0f);//calculate the direction
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -vector3);//set the rotation of the aim
        }
    }

    void ProcessInputs()//A different way to move the player, but i'm using both of them
    {
        //Store last move direction
        float moveX = Input.GetAxisRaw("Horizontal");//horizontal input
        float moveY = Input.GetAxisRaw("Vertical");//vertical input

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))//if the player is not moving set isWalking to false
        {
            isWalking = false;//set isWalking to false
            lastMoveDirection = input;//set last move direction to input
            Vector3 vector3 = new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0f);//calculate the direction
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -vector3);//set the rotation of the aim
        }
        else if (moveX != 0 || moveY != 0)//if the player is moving set isWalking to true
        {
            isWalking = true;//set isWalking to true
        }
        input.x = Input.GetAxisRaw("Horizontal");//horizontal input
        input.y = Input.GetAxisRaw("Vertical");//vertical input

        input.Normalize();//normalize the input
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            print("Collided with ground");
        }
    }
    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootsteps), 0f, footstepSpeed);
    }
    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootsteps));
    }
    void PlayFootsteps()
    {
        audioManager.PlaySFX(audioManager.MCWalk);
    }
}

