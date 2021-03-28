using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]
 
public class RigidPlayer : AbstractPlayer
{
	bool canUseAbility = true;
	protected override void useAbility()
	{
		if (canUseAbility)
		{
			canUseAbility = false;
			//overSpeedAllowed = true;
			//speed += 20;
			isRigid = true;
			Invoke("endAbility", 5f);
			print("RIGID");
		}
	}

	protected void endAbility()
	{
		//overSpeedAllowed = false;
		//speed -= 20;
		isRigid = false;

		canUseAbility = true;
	}
}