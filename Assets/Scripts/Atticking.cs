using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Atticking : MonoBehaviour
{
    public GameObject Melee;// the melee weapon prefab
    bool isAttacking = false;// is the player attacking or not
    float atkDuration = 0.3f;//how long is the attack duration

    float atkTimer = 0f;// the timer for the attack duration

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();// check if the attack duration is over

        if(Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButton(0))// Check if the player is pressing the attack button
        {
            //Attack
            OnAttack();// call the attack function
        }
    }

    void OnAttack()// the attack function
    {
        if(!isAttacking)// if the player is not attacking
        {
            Melee.SetActive(true);// activate the melee weapon prefab
            isAttacking = true;// set the player to attacking
            //call your animator play your melee attack
        }
    }

    void CheckMeleeTimer()// check if the attack duration is over
    {
        if(isAttacking)// if the player is attacking
        {
            atkTimer += Time.deltaTime;// add the time since the last frame to the timer
            if(atkTimer  >= atkDuration)// if the timer is greater than or equal to the attack duration
            {
                atkTimer = 0;
                isAttacking = false;// set the player to not attacking
                Melee.SetActive(false); // deactivate the melee weapon prefab
            }
        }
    }
}
