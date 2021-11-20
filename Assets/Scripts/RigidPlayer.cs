using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
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
			gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Rigid , player_index: playerIndex);
            GetPlayerMeshRenderer().material.color = Color.black;
			Invoke("endAbility", 4f);
			print("RIGID");
		}
	}

	protected void endAbility()
	{
		//overSpeedAllowed = false;
		//speed -= 20;
		isRigid = false;
        GetPlayerMeshRenderer().material.color = MyColor;
		canUseAbility = true;
	}
}