using UnityEngine;

public class Brushweapon : MonoBehaviour
{
    public float damage = 1;// damage amount

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Before trying to get component, you can check the tag to make sure it's an enemy! (and add enemy tag to enemy object)
        Enemy enemy = collision.GetComponent<Enemy>();// get the enemy component from the object that was hit
        if(enemy != null)// if the object has an enemy component
        {
            enemy.TakeDamage(damage);// call the TakeDamage function from the enemy script and pass in the damage amount
        }
    }

}
