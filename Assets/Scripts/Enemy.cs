using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.2f;// Speed of the enemy
    Rigidbody2D rb;// Rigidbody2D component for physics interactions
    Transform target;// Target to follow (the character)
    Vector2 moveDirection;// Direction to move towards

    public float detectionRange = 1.2f; // Range within which the enemy detects the character

    float health, maxHealth = 3f;// Maximum health of the enemy
    private void Awake()// Called when the script instance is being loaded
    {
        rb = GetComponent<Rigidbody2D>();// Get the Rigidbody2D component attached to this GameObject
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("Character").transform;// Find the character GameObject in the scene and get its Transform component
        health = maxHealth ;// Initialize health to maximum health
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseController.IsGamePaused)//check if the game is paused
        {
            moveSpeed = 0f;//stop movement  
        }
        else
        {
            moveSpeed = 0.5f;// Resume movement speed
        }

        if(target)// check if the target (Character) is not null
        {
           

            //float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg;// Calculate the angle to rotate towards the target
           // rb.rotation = angle;//rotate the enemy toward the target
           float distanceToTarget = Vector3.Distance(transform.position, target.position);// Calculate the distance to the target

            if (distanceToTarget <= detectionRange) // Check if the character is within range
            {
                Vector3 direction = (target.position - transform.position).normalized;// Calculate the direction to the target
                moveDirection = direction;// Set the move direction towards the target
            }
            else
            {
                moveDirection = Vector2.zero; // Stop moving if the character is out of range
            }
        }
    }

    private void FixedUpdate()
    {
        if(target && moveDirection != Vector2.zero)
        {
            // Move the enemy towards the target
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else if (moveDirection != Vector2.zero) // If not moving towards the target, just move in the last direction
        {
            rb.linearVelocity = new UnityEngine.Vector2(moveDirection.x, moveDirection.y) * moveSpeed;// Set the linear velocity of the Rigidbody2D to move in the last direction
        }
    }

    public void TakeDamage(float damage)// Method to apply damage to the enemy
    {
        health -= damage;// Reduce health by the damage amount
        if(health <= 0)// Check if health is less than or equal to zero
        {
            // Destroy the enemy GameObject
            Destroy(gameObject);
        }
    }
}
