using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class RigidPlayer : AbstractPlayer
{
	protected override void useAbility()
	{
		speed += 10;

		Invoke("endAbility", 3f);
	}

	protected void endAbility()
	{
		speed -= 10;
	}
}