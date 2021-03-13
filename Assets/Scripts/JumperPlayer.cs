using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class JumperPlayer : AbstractPlayer
{
	private float jumpForce = 400;
	
	protected override void useAbility()
	{
        if (grounded)
        {
			grounded = false;
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
		}

	}
}