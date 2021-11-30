using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class AvoiderPlayer : AbstractPlayer
{
	bool canUseAbility = true;
	GameObject DashFireParticle;
	float addedSpeed = 40;
	float bumpForceMultiplyer = 12;
	protected override void useAbility()
	{
		if (canUseAbility)
		{
			canUseAbility = false;
			//prev_vel = rb.GetPointVelocity(rb.centerOfMass);
			//speed += addedSpeed;
			maxSpeed += addedSpeed;
			bumpForce *= bumpForceMultiplyer; 
			Invoke("endAbility", 2f);
			DashFireParticle = gameManager.GetPM().Play_Effect(ParticlesManager.ParticleTypes.DashRocket, Vector3.forward*-1, gameObject.transform);
			gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Dash, player_index: playerIndex);
			//print("DASH");
		}
	}

	protected void endAbility()
	{
		//speed -= addedSpeed;
		maxSpeed -= addedSpeed;
		bumpForce /= bumpForceMultiplyer;
		Destroy(DashFireParticle);
		//rb.velocity = Vector3.zero;
		canUseAbility = true;

	}
}