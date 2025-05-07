using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class character : MonoBehaviour
{

   public float movespeed = 5f;//speed of player

   private Vector2 input;//input from player
   private Vector2 lastMoveDirection;//last move direction

    public Transform Aim;//aiming direction
    bool isWalking = false;//is player walking
    
    void Update()
    {
        if(PauseController.IsGamePaused)//if the game is paused
        {
            movespeed = 0f;// stop movement  
        }
        else
        {
            movespeed = 1.25f;// set movement speed
        }

        ProcessInputs();//process inputs
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y += 1;//move up
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y -= 1;//move down
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1;//move left
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;//move right
        }

        transform.position += moveDirection.normalized*movespeed*Time.deltaTime;//move player   

    }

    public void FixedUpdate()
    {
        if(isWalking)//if the player is walking
        {
            Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;//calculate the direction
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);//set the rotation of the aim
        }
    }

    void ProcessInputs()//A different way to move the player, but i'm using both of them
    {
        //Store last move direction
         float moveX = Input.GetAxisRaw("Horizontal");//horizontal input
        float moveY = Input.GetAxisRaw("Vertical");//vertical input

        if((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))//if the player is not moving set isWalking to false
        {
            isWalking = false;//set isWalking to false
            lastMoveDirection = input;//set last move direction to input
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;//calculate the direction
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);//set the rotation of the aim
        }
        else if(moveX != 0 || moveY != 0)//if the player is moving set isWalking to true
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
}

