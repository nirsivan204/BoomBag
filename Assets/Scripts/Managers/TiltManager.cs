using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TiltManager : MonoBehaviour
{
    private GameObject ground;
    private float timer, tiltAngle, tiltTime;
    private bool isTilting;
    private Vector3 tiltVector;
    private const float MAX_TILT = 2f;

    void Start()
    {
        ground = GameObject.Find("baseGround"); // Probably should use a tag.
        timer = 0;
    }

#if DEBUG
    // For debugging tilt on special inputs:
    private void OnTilt(InputValue lookValue)
	{
        Vector2 v = lookValue.Get<Vector2>();
        startTilt(new Vector3(v.x, 0, v.y), 0.1f * v.magnitude, 0.1f);
    }
#endif

    // Tilt angle around axis (unless it's more then MAX_TILT).
    void tilt(Vector3 axis, float angle)
	{
        Transform transform = ground.transform;
        transform.Rotate(axis, angle);
        if (transform.rotation.x <= MAX_TILT && transform.rotation.z <= MAX_TILT)
        {
            ground.transform.Rotate(axis, angle);
        }
    }

    // Set the parameters to start tilting. The actual tilt happens in FixedUpdate.
    public void startTilt(Vector3 axis, float angle, float time)
	{
        tiltVector = axis;
        tiltAngle = angle;
        tiltTime = time;
        isTilting = true;
    }

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
