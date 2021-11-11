using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class AvoiderPlayer : AbstractPlayer
{
	bool canUseAbility = true;
	GameObject DashFireParticle;
	float addedSpeed = 20;
	protected override void useAbility()
	{
		if (canUseAbility)
		{
			canUseAbility = false;
			overSpeedAllowed = true;
			//prev_vel = rb.GetPointVelocity(rb.centerOfMass);
			speed += addedSpeed;
			Invoke("endAbility", 1f);
			DashFireParticle = gameManager.GetPM().Play_Effect(ParticlesManager.ParticleTypes.DashRocket, Vector3.zero, gameObject.transform);
			gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Dash, player_index: playerIndex);
			//print("DASH");
		}
	}

	protected void endAbility()
	{
		speed -= addedSpeed;
		Destroy(DashFireParticle);
		//rb.velocity = Vector3.zero;
		overSpeedAllowed = false;
		canUseAbility = true;

	}
}