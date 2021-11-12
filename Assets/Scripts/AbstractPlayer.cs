using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(BoxCollider))]

public class AbstractPlayer : MonoBehaviour
{
    private float GRAVITY_SCALE = 10;
    protected int playerIndex;
    protected GameManager gameManager;
    public float speed = 80.0f;
    protected Rigidbody rb;
    public float drag = 0;
    private float movementX = 0;
    private float movementY = 0;
    //protected bool grounded = true;
    private int size; //= startSize in start;
    [SerializeField] private int maxGrow = 8;
    [SerializeField] private int minGrow = 1;
    private float growRatio = 4f;  // must be less than startSize/(startSize - minGrow)
    public float massGrowRate = 1.05f; // must be less than startSize/(startSize - minGrow)
    private int startSize = 4;
    private GameObject playerCharacter;
    public Color MyColor;
    protected Renderer playerMeshRenderer;
    // Energy units are equivalent to seconds:
    public float energy = 4;
    public const float MAX_ENERGY = 10;
    public const float ENERGY_COST = 10;
    public IntEvent playerOut;
    protected bool isOut = false;
    bool isHumanPlayer = true;
    private GameObject AITargetGameObj;
    private AbstractPlayer AITargetScript;
    public float maxSpeed = 20;
    public bool overSpeedAllowed = false;
    public float bumpForce = 1100;
    protected bool isRigid = false;
    private bool canMove = false;
    private int invertFactor = 1;
    private int numSequentialInverts = 0;
    public AudioSource audioSource;
    public SimpleTouchController touchController;
    [SerializeField] private float holeFactor = 550;
    private float delayAfterBump = 0.15f;
    private float ArenaRadius = 40;
    private int outsideFactor = 50;
    private float HoleRadius = 15;
    private bool isPlayingMovementSound;



/*    void Awake()
    {
        playerCharacter = transform.Find("Body").gameObject.transform.Find("Character").gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        //playerMeshRenderer = playerCharacter.GetComponent<Renderer>();
        //MyColor = playerMeshRenderer.material.color;
        //rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        playerOut = new IntEvent();
        size = startSize;
        transform.localScale = new Vector3(startSize, startSize, startSize);
        if (!isHumanPlayer)
        {
            GetComponent<PlayerInput>().enabled = false;
            chooseTarget();
            Invoke("aiCalculateMove", 0.5f);
        }
        audioSource = GetComponentsInChildren<AudioSource>()[1];
        //bumpSound = AssetsManager.AM.bumpSound;
        //drownSound = AssetsManager.AM.drownSound;
        //shrinksound = AssetsManager.AM.growsound;
        //growsound = AssetsManager.AM.shrinksound;

        init();
    }*/


    public virtual void init()
    {

        playerCharacter = transform.Find("Body").gameObject.transform.Find("Character").gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        //playerMeshRenderer = playerCharacter.GetComponent<Renderer>();
        //MyColor = playerMeshRenderer.material.color;
        //rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        playerOut = new IntEvent();
        size = startSize;
        transform.localScale = new Vector3(startSize, startSize, startSize);
        if (!isHumanPlayer)
        {
            GetComponent<PlayerInput>().enabled = false;
            chooseTarget();
            Invoke("aiCalculateMove", 0.5f);
        }
        audioSource = GetComponentsInChildren<AudioSource>()[1];
        playerMeshRenderer = playerCharacter.GetComponent<Renderer>();
        MyColor = playerMeshRenderer.material.color;
        if (touchController && gameManager.isMobileGame)
        {
            touchController.TouchEvent += Controller_TouchEvent;
        }
        isInit = true;
    }

    void Controller_TouchEvent(Vector2 value)
    {
        movementX = value.normalized.x;
        movementY = value.normalized.y;
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>().normalized;//*invertFactor;
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    public void OnFire()
    {
        if (!isOut && energy >= ENERGY_COST)
        {
            energy -= ENERGY_COST;
            useAbility();
        }
    }

    protected virtual void useAbility()
    {

    }

    protected virtual void showAbilityEffect()
    {

    }

    private void chooseTarget()
    {
        if (isOut)
        {
            AITargetGameObj = gameObject;
            AITargetScript = this;
            return;
        }
        int res = playerIndex;
        /*todo:
         * 
         * make waves mode;
         * 
         */
        //if (gameManager.mobilePlayer && !gameManager.mobilePlayer.isOut) // if one against all
        //{
        //    res = gameManager.mobilePlayer.playerIndex;
       // }
        //else
       // {
       if(gameManager.GetNumPlayersAlive() > 1)
        {
            int count = 0;
            while ( res == playerIndex || !gameManager.liveOrDead[res])
            {
                res = UnityEngine.Random.Range(0, gameManager.players.Length);
                count++;
                if (count > 100)
                {
                    print("ERROR in chooseTarget in " + gameObject);
                    break;
                }
            }
        }
       // }
        AITargetGameObj = gameManager.players[res];
        AITargetScript = gameManager.playersScripts[res];
        Invoke("chooseTarget", 5);


    }
    private void aiCalculateMove()
    {
        if (AITargetScript.isOut)
        {
            chooseTarget();
        }
        Vector3 attackLine = Vector3.zero;
        if (ArenaRadius - (transform.position - transform.position.y*Vector3.up).magnitude > 1) //if we are far enough from the edge
        {
            if((transform.position - transform.position.y * Vector3.up).magnitude > HoleRadius) //if we are far enough from the hole
            {
                attackLine = (AITargetGameObj.transform.position - transform.position);
                attackLine = new Vector3(attackLine.x, 0, attackLine.z);//(transform.position-Vector3.up * transform.position.z)*holeFactor;
                if (!isTargetBetweenMeAndMiddle())
                {
                    attackLine += holeFactor * transform.position / transform.position.sqrMagnitude;
                }
                if (!isTargetBetweenMeAndOutside())
                {
                    attackLine += outsideFactor * -1 * transform.position.normalized / (ArenaRadius - transform.position.magnitude);
                }
            }
            else
            {
                attackLine = transform.position;
            }
        }
        else
        {
            attackLine = -transform.position;
        }
        if (attackLine.magnitude <= 0)
        {
            movementX = 0;
            movementY = 0;
        }
        else
        {
            movementX = attackLine.x / attackLine.magnitude;
            movementY = attackLine.z / attackLine.magnitude;
        }
        if (!isOut)
        {
            Invoke("aiCalculateMove", 0.1f);
        }
    }

    private bool isTargetBetweenMeAndOutside()
    {
        return AITargetGameObj.transform.position.magnitude > transform.position.magnitude &&
           Vector3.Angle(AITargetGameObj.transform.position, transform.position) < 90;
    }

    private bool isTargetBetweenMeAndMiddle()
    {
        return AITargetGameObj.transform.position.magnitude < transform.position.magnitude &&
           Vector3.Angle(AITargetGameObj.transform.position- transform.position, -transform.position) < 90;
    }

    void FixedUpdate()
    {
        if (isInit)
        {
            rb.AddForce(Vector3.down * GRAVITY_SCALE);
            if (canMove)
            {
                Vector3 movement = new Vector3(movementX, 0, movementY);
                rb.AddForce(movement * speed * rb.mass * invertFactor);
                if (rb.velocity.magnitude > 0.5)
                {
                    Vector3 heading = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    transform.LookAt(transform.position + heading);


                    //Quaternion targetRotation = Quaternion.LookRotation(heading); can work also
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * 100);
                }
                if (!overSpeedAllowed)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                }
                if (energy < MAX_ENERGY)
                {
                    energy += Time.deltaTime;
                }
                //code for playing movement by pitch
                if (rb.velocity.magnitude > 2)
                {
                    if (isPlayingMovementSound)
                    {
                        float normal = Mathf.InverseLerp(0, maxSpeed, rb.velocity.magnitude);
                        float pitch = Mathf.Lerp(MinMovementSoundPitch, MaxMovementSoundPitch, normal);
                        audioSource.pitch = pitch;
                    }
                    else
                    {
                        gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Movement, true, player_index: playerIndex);
                        isPlayingMovementSound = true;
                    }
                }
                else
                {
                    audioSource.Stop(); // to do: stop using audiomanager
                    isPlayingMovementSound = false;
                    audioSource.pitch = 1;

                }
            }
        }
    }

    public float MinMovementSoundPitch = 0.9f;
    public float MaxMovementSoundPitch = 1.1f;
    private bool isInit = false;

    private void OnCollisionEnter(Collision otherPlayer)
    {
        AbstractPlayer other = otherPlayer.gameObject.GetComponent<AbstractPlayer>();
        if (other)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
            if (!isRigid)
            {
                //gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Bump);
                //audioSource.clip = bumpSound;
                //audioSource.Play();
                rb.AddExplosionForce(bumpForce * other.getMass(), (other.transform.position + transform.position) / 2, 100, 0);//, ForceMode.Acceleration); or other.size??maybe cancel mass at all??
            }
            else
            {
                Vector3 collisionForce = otherPlayer.impulse;
                rb.AddForce(collisionForce, ForceMode.Impulse);
            }
            Time.timeScale = 0.6f;
            Invoke("stopSlowDown", 0.6f);
            gameManager.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Boom, transform.position);
            if (!other.isRigid && !isRigid)
            {
                    if (other.getColor() == MyColor)
                    {
                        grow();
                    }
                    else
                    {
                        shrink();
                    }
            }
            canMove = false;
            StartCoroutine(setCanMove(true, delayAfterBump));
        }
        else
        {
            if(otherPlayer.gameObject.tag == "Arena")
            {
                //grounded = true;
            }
            else if (otherPlayer.gameObject.tag == "Out" && !isOut)
            {
                die();
            }
        }
    }

    private void stopSlowDown()
    {
        Time.timeScale = 1;
    }

    private void die()
    {
        //print(gameObject + "out, player index = " + playerIndex);
        playerOut.Invoke(playerIndex);
        isOut = true;
        canMove = false;
        gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Player_Death, player_index: playerIndex);
        gameManager.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Splash, transform.position+Vector3.up*2);
        //audioSource.clip = drownSound;
        //audioSource.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().AddForce(Vector3.down*10);

    }

    private void OnCollisionExit(Collision otherPlayer)
    {
        AbstractPlayer other = otherPlayer.gameObject.GetComponent<AbstractPlayer>();
        if (other)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }

    }
        public void grow(int times = 1)
    {
        if (size < maxGrow)
        {
            size = Math.Min(maxGrow,size + times);
            setNewSize();
            gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Grow);
//            audioSource.clip = growsound;
//            audioSource.Play();
        }
        else
        {
            gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Bump);
        }

    }

    public void shrink(int times = 1)
    {
        if (size > minGrow)
        {
            size = Math.Max(minGrow, size - times);
            setNewSize();
            gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Shrink);
            //audioSource.clip = shrinksound;
            //audioSource.Play();
        }
        else
        {
            gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Bump);
        }
    }

    private void setNewSize()
    {
        //float newSize = size * growRatio * sizeNormalizer;
        float multiplyCoefficient = (float)size / startSize - 1;
        float newSize = startSize + growRatio * multiplyCoefficient;
       // transform.localScale = new Vector3(newSize, newSize, newSize);
		LeanTween.scale (gameObject,  new Vector3(newSize, newSize, newSize), 0.2f).setEase(LeanTweenType.easeSpring);
        rb.mass = 1 + massGrowRate * multiplyCoefficient;
        //print("mass " + rb.mass);

    }

    public void invertControls(float time)
    {
    
        if (invertFactor == -1)
        {
            numSequentialInverts++;
        }
        invertFactor *= -1;
        Invoke("unInvertControls", time);
    }

    public void unInvertControls()
    {
        if (numSequentialInverts == 0)
        {
            invertFactor = 1;
        }
        numSequentialInverts--;
    }

    public virtual void setColor(Color color)
    {
        MyColor = color;
        if (!isRigid)
        {
            playerMeshRenderer.material.color = color;
        }
    }

    public Color getColor()
    {
        return MyColor;
    }

    public int getIndex()
    {
        return playerIndex;
    }

    public void setPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public void setGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    public bool getIsOut()
    {
        return isOut;
    }

    public bool isInFrame(float mergin)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return (screenPoint.z > mergin && screenPoint.x > mergin && screenPoint.x < 1- mergin && screenPoint.y > mergin && screenPoint.y < 1- mergin);
    }

    public void setIsHuman(bool isHuman)
    {
        isHumanPlayer = isHuman;

    }

    public bool getIsHuman()
    {
        return isHumanPlayer;
    }

    public float getMass()
    {
        if (rb)
        {
            return rb.mass;
        }
        return 0;
    }

    public IEnumerator setCanMove(bool Move, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canMove = Move;
    }

    public void setEnergy(float energy)
    {
        this.energy = energy;
    }

}
