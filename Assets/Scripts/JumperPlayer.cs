using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class JumperPlayer : AbstractPlayer
{
	private float jumpForce = 2000;
	private AudioClip jumpSound;
	protected override void init()
	{
		jumpSound = AssetsManager.AM.jumpSound;
	}
	protected override void useAbility()
	{
        if (grounded)
        {
			grounded = false;
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
			showAbilityEffect();
		}

	}

	protected override void showAbilityEffect()
    {
		audioSource.clip = jumpSound;
		audioSource.Play();
	}

}