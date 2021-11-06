using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class JumperPlayer : AbstractPlayer
{
	private float jumpForce = 2000;
	//private AudioClip jumpSound;
	public override void init()
	{
		base.init();
		//jumpSound = AssetsManager.AM.jumpSound;
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
		gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Jump, player_index: playerIndex);

		//audioSource.clip = jumpSound;
		//audioSource.Play();
	}

}