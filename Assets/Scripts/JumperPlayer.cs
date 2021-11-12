using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class JumperPlayer : AbstractPlayer
{
	private float jumpForce = 20;
	//private AudioClip jumpSound;
	public override void init()
	{
		base.init();
		//jumpSound = AssetsManager.AM.jumpSound;
	}
	protected override void useAbility()
	{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
			showAbilityEffect();
	}

	protected override void showAbilityEffect()
    {
		gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Jump, player_index: playerIndex);

		//audioSource.clip = jumpSound;
		//audioSource.Play();
	}

}