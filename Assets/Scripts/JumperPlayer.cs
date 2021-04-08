using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]
 
public class JumperPlayer : AbstractPlayer
{
	private float jumpForce = 400;
	private AudioSource audioSource;
	protected override void init()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = AssetsManager.AM.jumpSound;
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
		audioSource.Play();
	}

}