﻿using UnityEngine;
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
			overSpeedAllowed = true;
			speed += 10;
			Invoke("endAbility", 5f);
			print("DASH");
		}
	}

	protected void endAbility()
	{
		overSpeedAllowed = false;
		speed -= 10;
		canUseAbility = true;

	}
}