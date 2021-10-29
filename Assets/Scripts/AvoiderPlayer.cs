﻿using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class AvoiderPlayer : AbstractPlayer
{
	bool canUseAbility = true;
	protected override void useAbility()
	{
		if (canUseAbility)
		{
			canUseAbility = false;
			overSpeedAllowed = true;
			//prev_vel = rb.GetPointVelocity(rb.centerOfMass);
			speed += 200;
			Invoke("endAbility", 0.3f);
			gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Dash, player_index: playerIndex);
			//print("DASH");
		}
		//speed += 10;
		//Invoke("endAbility", 3f);
	}

	protected void endAbility()
	{
		speed -= 200;
		//rb.velocity = Vector3.zero;
		overSpeedAllowed = false;
		canUseAbility = true;

	}
}