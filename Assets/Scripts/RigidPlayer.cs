using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class RigidPlayer : AbstractPlayer
{
	bool canUseAbility = true;
	protected override void useAbility()
	{
		if (canUseAbility)
		{
			canUseAbility = false;
			speed += 10;
			Invoke("endAbility", 5f);
			print("DASH");
		}
		speed += 10;
		Invoke("endAbility", 3f);
	}

	protected void endAbility()
	{
		speed -= 10;
		canUseAbility = true;
	}
}