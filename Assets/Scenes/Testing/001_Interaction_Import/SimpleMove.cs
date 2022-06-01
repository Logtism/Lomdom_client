using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A simple free camera to be added to a Unity game object.
/// 
/// Keys:
///	wasd / arrows	- movement
///	q/e 			- up/down (local space)
///	r/f 			- up/down (world space)
///	pageup/pagedown	- up/down (world space)
///	hold shift		- enable fast movement mode
///	right mouse  	- enable free look
///	mouse			- free look / rotation
///     
/// </summary>
public class SimpleMove : MonoBehaviour
{
    // Add the variables
    public float speed = 100f; // Speed variable
    public Rigidbody rb; // Set the variable 'rb' as Rigibody
    public Vector3 movement; // Set the variable 'movement' as a Vector3 (x,y,z)

    [SerializeField] private float sensitvity = 3f;
    [SerializeField] private float ClampAngle = 85f;
    public GameObject cameraHolder;



    // 'Start' Method run once at start for initialisation purposes
    void Start()
    {
        // find the Rigidbody of this game object and add it to the variable 'rb'
        rb = this.GetComponent<Rigidbody>();
    }



    // 'Update' Method is called once per frame
    void Update()
    {
        // In Update we get the Input for left, right, up and down and put it in the variable 'movement'...
        // We only get the input of x and z, y is left at 0 as it's not required
        // 'Normalized' diagonals to prevent faster movement when two inputs are used together
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Look();
    }



    // 'FixedUpdate' Method is used for Physics movements
    void FixedUpdate()
    {
        moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement
    }



    // 'moveCharacter' Function for moving the game object
    void moveCharacter(Vector3 direction)
    {
        // We multiply the 'speed' variable to the Rigidbody's velocity...
        // and also multiply 'Time.fixedDeltaTime' to keep the movement consistant on all devices
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensitvity);

        ClampAngle += Input.GetAxisRaw("Mouse Y") * sensitvity;
        ClampAngle = Mathf.Clamp(ClampAngle, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * ClampAngle;
    }
}