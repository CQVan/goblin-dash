using UnityEngine;

public class dash : MonoBehaviour
{
    [Header("references")]
    private Rigidbody rb;
    private move playerMove;
    private movement playerMovement;
    public Transform playerBody;

    [Header("Dashing variables")]
    public float dashForce, dashUpwardForce, dashDuration;

    

    public float dashCooldown;
    public float dashCooldownTimer;

    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<movement>();

    }

    private Vector3 delayedForceToApply;
    public void Dash()
    {
        //If the timer is greater than 0 and you're still dashing, return the fuction. Else is that the timer is now the cooldown for the dash
        if (dashCooldownTimer > 0) return;
        else dashCooldownTimer = dashCooldown;


            playerMovement.isDashing = true;

        //force that will be applied to the dash
        Vector3 forceToApply = playerBody.forward * dashForce + playerBody.up * dashUpwardForce;

        //delays for about 0.025 seconds before the actual dash force is applied to the movement
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        //Adds the force to the player
        rb.AddForce(forceToApply, ForceMode.Impulse);

        //Stops the dash after a certain duration
        Invoke(nameof(ResetDash), dashDuration);
    }

    public void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }


    //Sets dash to false
    private void ResetDash()
    {
        playerMovement.isDashing = false;
    }
}
