using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class RigidPlayer : AbstractPlayer {
 
	/*public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private Rigidbody rb;
	private float movementX;
	private float movementY;
	private bool grounded = false;

	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
	    rb.freezeRotation = true;
	}

	private void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();
		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	void FixedUpdate ()
	{
		Vector3 movement = new Vector3(movementX, 0, movementY);
		rb.AddForce(movement * speed);
	}
 
	void OnCollisionStay () {
	    grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}*/
}