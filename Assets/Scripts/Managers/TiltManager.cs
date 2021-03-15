using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TiltManager : MonoBehaviour
{
    private GameObject[] grounds;
    private float timer, tiltAngle, tiltTime;
    private bool isTilting;
    private Vector3 tiltVector;
    // private const float MAX_TILT = 0.1f;

    void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Arena");
        timer = 0;
    }

#if DEBUG
    // For debugging tilt on special inputs:
    private void OnTilt(InputValue lookValue)
	{
        Vector2 v = lookValue.Get<Vector2>();
        startTilt(new Vector3(v.y, 0, -v.x), 0.1f * v.magnitude, 0.1f);  // 2d y is placed in 3d z.
    }
#endif

    // Tilt angle around axis (unless it's more then MAX_TILT).
    void tilt(Vector3 axis, float angle)
	{
        foreach (GameObject ground in grounds)
        {
            ground.transform.Rotate(axis, angle, Space.World);
        }
        // TODO: Add max tilt.
    }

    // Set the parameters to start tilting. The actual tilt happens in FixedUpdate.
    public void startTilt(Vector3 axis, float angle, float time)
	{
        tiltVector = axis;
        tiltAngle = angle;
        tiltTime = time;
        isTilting = true;
    }

    // Currently tilting is done by changing the transform. It is more correct to do it by applying force on a rigid body. But the ground being a rigid body has its problems...
    void FixedUpdate()
    {
        // Tilt slowly over some frames:
        if (isTilting)
		{
            tilt(tiltVector, tiltAngle);
            timer += Time.deltaTime;
            if (timer > tiltTime)
			{
                timer = 0;
                isTilting = false;
            }
        }
    }
}
