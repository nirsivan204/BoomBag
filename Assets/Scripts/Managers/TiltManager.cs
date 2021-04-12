using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TiltManager : MonoBehaviour
{
    private GameObject[] grounds;
    private float timer, tiltAngle, tiltTime;
    private bool isTilting;
    private Vector3 tiltVector;
    public GameManager gm;
     private const float MAX_TILT = 20f;
    public GameObject middle1;
    public GameObject middle2;

    void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Arena");
        timer = 0;
        calculateTilt();
    }

#if DEBUG
    // For debugging tilt on special inputs:
    private void OnTilt(InputValue lookValue)
	{
        Vector2 v = lookValue.Get<Vector2>();
        startTilt(new Vector3(v.y, 0, -v.x), 0.1f * v.magnitude, 0.1f);  // 2d y is placed in 3d z.
    }
#endif

    private void massTilt(Vector2 vector, float mass)
    {
        
        startTilt(new Vector3(vector.x, vector.y, 0), 0.1f * mass, 0.1f);  // 2d y is placed in 3d z.
    }


    Quaternion lastRotation;
    // Tilt angle around axis (unless it's more then MAX_TILT).
    void tilt(Vector3 axis, float angle)
	{
        foreach (GameObject ground in grounds)
        {
            Vector3 resultingUpVector = middle2.transform.position - middle1.transform.position;
            print(Vector3.Angle(Vector3.up, resultingUpVector));
            if(Vector3.Angle(Vector3.up, resultingUpVector) < MAX_TILT)
            {
                lastRotation = ground.transform.rotation;
                ground.transform.Rotate(axis, angle, Space.World);
            }
            else
            {
                ground.transform.rotation = lastRotation;
            }
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


    private void calculateTilt()
    {
        Vector3 torque = Vector3.ProjectOnPlane(gm.calculateTorque(grounds[0].transform.position),Vector3.up);
        startTilt(new Vector3(-torque.z, 0, torque.x), -0.005f * torque.magnitude, 0.05f);  // 2d y is placed in 3d z.
        Invoke("calculateTilt", 0.1f) ;
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
