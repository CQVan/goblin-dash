using JetBrains.Annotations;
using UnityEngine;

public class move : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private bool controlCheck;

    [SerializeField] private float currentSpeed = 0f;

    private Rigidbody rb;
    private sneak playerSneak;
    private float SneakSpeed;

    private float speedMultiplier = .5f;

    void Start()
    {
        currentSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        SneakSpeed = GetComponent<sneak>().sneakSpeed;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        SneakSpeed = moveSpeed * multiplier;
    }

    public void playerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        rb.linearVelocity = new Vector3(moveX * moveSpeed, rb.linearVelocity.y, moveZ * moveSpeed); //Moves the object in the direction inputed with through the linear velocity of the rigidbody

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetSpeedMultiplier(speedMultiplier);
            controlCheck = false;
        }
        else
        {
            moveSpeed = 5.0f;
            controlCheck = true;
        }
    }
    
}
