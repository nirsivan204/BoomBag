using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class JumperPlayer : AbstractPlayer
{
	public float jumpForce;
	
	protected override void useAbility()
	{
        if (grounded)
        {
			grounded = false;
			//rb jumpForce
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
			print("jumpiong");
		}

	}
}