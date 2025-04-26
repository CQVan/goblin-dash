using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    [SerializeField] float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locks your mouse to the center of your screen
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //get mouse movement on the x-axis with the mouse sensitivity, moving the speed despite the frame rate which is why time.deltatime
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY; //Decrease our xRotation on the mouse movement on the y-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Sets limits that the player can look; keeps them from looking over 90 degress and under -90 degrees/

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 

        playerBody.Rotate(Vector3.up * mouseX); //Rotate around the y-axis of our player based on our mouse input on the x-axis. 
        
    }
}
