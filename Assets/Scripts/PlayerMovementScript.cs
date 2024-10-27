using DG.Tweening;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public CharacterController controller;

    public MovementState state;
    public MouseLook playerCamera;
    public Camera mainCamera;
    private float baseFOV;
    public PlayerParticleEffects playerParticleEffects;
    public enum MovementState {
        standing,
        running, 
        crouching, 
        air, 
        sliding,
        onWall,
        dashing,
        slamming,
        dash,

    }
    [SerializeField]
    private Transform playerBody;

    [Header("Normal State")]
    public float regularGravity = -9.81f;
    public float standingSpeed = 12;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public float jumpSideStrength = 10f;
    public float regularJumpHeight = 3;
    public float boostedJumpHeight = 8;
    public float standingHeightScale = 1f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    bool canMove = true;

    [Header("On wall State")]
    bool touchingWall;
    private Vector3 disabledWallNormal;
    private Vector3 currentWallNormal;
    public float wallCheckDistance = 1.0f;

    private bool wallFwd;
    private bool wallRight;
    private bool wallLeft;
    private bool wallBack;
    public bool exitingWall;
    public float exitingWallTimer = 0.2f;


    public float onWallMovementSpeed = 12f;
    public float wallJumpSideStrength = 10;
    public float wallJumpUpwardsStrength = 1;
    public float wallJumpForwardStrength = 1;
    public float onWallGravity = -9.81f;

    [Header("In air state")]
    public float decayingAirSpeedFactor = 10f;
    public float decayAirSpeedTime = 0.6f;
    public float airMovementSpeedFactor = 12;

    public float maxAirVelocity = 5;
    public float airMomentumDecayRate = 0.1f;
    public float knockBackDecayRate = 0.1f;

    private Vector3 addedAirVelocity;
    private Vector3 knockBackVelocity;

    [Header("Crouching State")]

    public float crouchingSpeed = 6f;
    public float crouchHeightScale = 0.5f;
    public float crouchDownWardForceMultiplier = 10;
    public float crouchSlideSpeed = 20f;
    public float minimumSlamSpeed = 20f;
    private bool canSlam =false;
    public LayerMask groundMask;
    [Header("Ground Slam settings")]

    public float groundSlamRadiusFactor = 0.1f;
    Vector3 slideVector = Vector3.zero;
    Vector3 velocity;

    bool isGrounded;
    
    bool canJump;
    bool canWallJump;
    [Header ("Dashing state")]
    public float dashingDistance = 100f;
    public float dashingDuration = 0.1f;
    public float dashingInvincibilityDuration = 0.2f;
    private bool currentlyDashing;
    bool canDash = true;

    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;
        baseFOV = mainCamera.fieldOfView;
    }
    void Update()
    {   
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;


        DashControls(move);
        CrouchingControls(move);
        JumpControls(move);
        WallJumpControls(move);
        WallDetection();
        StateHandler(move);
        SlamControls();
        AddedVelocityDecay();







        if (canMove == true && isGrounded)
        {
      
            controller.Move(speed * Time.deltaTime * move);
        } else if (canMove == true && !isGrounded)
        {

            controller.Move(speed * Time.deltaTime * move);
            controller.Move(Time.deltaTime * addedAirVelocity);


        }

        //Debug.Log(knockBackVelocity.magnitude);
        if (knockBackVelocity.magnitude > 10) controller.Move(Time.deltaTime * knockBackVelocity);



        if (isGrounded && velocity.y < 0)
        {

            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Dash(Vector3 direction)
    {
        gm.removeStamina(333);
        // "I-Frames" when dashing set to dashing duration/2 ish
        gm.enableGodModeDuration(dashingInvincibilityDuration);
        canDash = false;
        DOTween.To(() => velocity, x => velocity = x, direction*dashingDistance, dashingDuration).OnComplete(()=> {
            DOTween.To(() => velocity, x => velocity = x, new Vector3(0, 0, 0), dashingDuration);
            Debug.Log("Finshed Dashing!");


            currentlyDashing = false;
            canDash = true;
        });       
    }
    void CrouchingControls(Vector3 move)
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && move != Vector3.zero && isGrounded)
        {
            //Set glide vector
            
            slideVector = move.normalized;
            Quaternion currentRot = Quaternion.LookRotation(new Vector3(-slideVector.x, slideVector.y,-slideVector.z), Vector3.up);
            playerParticleEffects.PlaySlideParticleAnimation(currentRot);
            playerCamera.DoFov(95, 0.15f);

        }
     
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Glide
            if (Input.GetButtonDown("Jump"))
                {
                    slideVector = Vector3.zero;
                    playerParticleEffects.StopSlideParticleAnimation();
                    playerCamera.DoFov(90, 0.15f);


            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            playerParticleEffects.StopSlideParticleAnimation();

            //transform.localScale = new Vector3(transform.localScale.x, standingHeightScale, transform.localScale.z);
            controller.height = 3.8f;
            controller.center = Vector3.zero;
            playerBody.localScale = new Vector3(1, 1.8f, 1);
            slideVector = Vector3.zero;
            playerCamera.DoFov(90, 0.15f);

        }
    }

    void DashControls(Vector3 move)
    {
        if (move != Vector3.zero && gm.getStamina() > 333)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
        if (canDash == true && Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerParticleEffects.PlaySpeedLineAnimation();

            currentlyDashing = true;
            if (Input.GetKey("w"))
            {
                gm.playerHeal(20);

                //playerCamera.DoDashFov(90,95,0.25f, 0.15f);
                playerCamera.DoDashFov(baseFOV, baseFOV + 5, 0.25f, 0.15f);

            }
            if (Input.GetKey("s"))
            {
                //playerCamera.DoDashFov(85, 90, 0.15f, 0.25f);
                playerCamera.DoDashFov(baseFOV - 5, baseFOV, 0.15f, 0.25f);


            }
            if (Input.GetKey("d"))
            {
                playerCamera.DoDashTilt(-3, 0.25f, 0.15f);
            }
            if (Input.GetKey("a"))
            {
                playerCamera.DoDashTilt(3, 0.25f, 0.15f);
            }



            Dash(move);
        }
    }
  
    void JumpControls(Vector3 move) {
        //Check if player can states where player can jump


         if (Input.GetButtonDown("Jump") && canJump)
            {


            //AddDecayingAirSpeed(move.normalized, decayAirSpeedTime, decayingAirSpeedFactor);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            addedAirVelocity = move * jumpSideStrength;

            }

    }
    void AddedVelocityDecay() {
        //Decays added velocity, in air from knockbacks
  
        if (addedAirVelocity.x > 0)
        {
            addedAirVelocity.x -= airMomentumDecayRate*Time.deltaTime;

        }
        else
        {
            addedAirVelocity.x += airMomentumDecayRate * Time.deltaTime;

        }
        if (addedAirVelocity.z > 0)
        {
            addedAirVelocity.z -= airMomentumDecayRate * Time.deltaTime;

        }
        else
        {
            addedAirVelocity.z += airMomentumDecayRate * Time.deltaTime;

        }

        if (knockBackVelocity.x > 0)
        {
            knockBackVelocity.x -= knockBackDecayRate * Time.deltaTime;

        }
        else
        {
            knockBackVelocity.x += knockBackDecayRate * Time.deltaTime;

        }
        if (knockBackVelocity.y > 0)
        {
            knockBackVelocity.y -= knockBackDecayRate * Time.deltaTime;

        }
        else
        {
            knockBackVelocity.y += knockBackDecayRate * Time.deltaTime;

        }
        if (knockBackVelocity.z > 0)
        {
            knockBackVelocity.z -= knockBackDecayRate * Time.deltaTime;
        }
        else
        {
            knockBackVelocity.z += knockBackDecayRate * Time.deltaTime;
        }
    }

    void WallJumpControls(Vector3 move)
    {
        //Check if player can states where player can jump

        if (Input.GetButtonDown("Jump") && canWallJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight* wallJumpUpwardsStrength * -2f * gravity);
            addedAirVelocity = currentWallNormal*wallJumpSideStrength + move*wallJumpForwardStrength;
        }

    }
    private void StateHandler(Vector3 move) {

        //Default settings
        canJump = true;
        canMove = true;
        gravity = regularGravity;
        jumpHeight = regularJumpHeight;
        speed = standingSpeed;
        if (currentlyDashing == true)
        {
            //Dashing state
            state = MovementState.dashing;

        }

        else if (isGrounded && move == Vector3.zero)
        {
            //Standining still state
            state = MovementState.standing;
            canWallJump = false;

        }
        else if(isGrounded && move != Vector3.zero) 
        {
            //Moving on ground state

            state = MovementState.running;
            canWallJump = false;


        }
        else if(!isGrounded && !touchingWall) 
        {
            //In air state
            state = MovementState.air;
            speed = airMovementSpeedFactor;
            canJump = false;
            canWallJump = false;


        } else if (!isGrounded && exitingWall)
        {
            //In air state
            state = MovementState.air;
            speed = airMovementSpeedFactor;
            canJump = false;
            canWallJump = false;
        }
        else if(touchingWall == true && !exitingWall) {
            //Wall gliding state
             state = MovementState.onWall;
             speed = onWallMovementSpeed;
             gravity = onWallGravity;
             canJump = false;


        }
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //All crouching states
            canDash = false;
            //transform.localScale = new Vector3(transform.localScale.x, crouchHeightScale, transform.localScale.z);
            //Do new collider scale when crouching, reposition ground check
            playerBody.localScale = new Vector3(1, 0.9f, 1);
            controller.height = 2.3f;
            controller.center = new Vector3(0, 0.41f, 0);

            
            if(slideVector!= Vector3.zero) {
            //Sliding state
                state = MovementState.sliding;
                canMove = false;
                jumpHeight = boostedJumpHeight;
                controller.Move(slideVector * crouchSlideSpeed * Time.deltaTime);
                

            } else if(velocity.y < 0 && state == MovementState.air){
            //Slamming in air state
                state = MovementState.slamming;
                canJump = false;
                velocity.y += crouchDownWardForceMultiplier * gravity * Time.deltaTime;
                
            } 
            else {
            //Crouch state

                state = MovementState.crouching;
                speed = crouchingSpeed;

            }
        } 
        if (state == MovementState.dash) {


        }
      
    }
    void WallDetection()
    {

            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 left = transform.TransformDirection(Vector3.left);
            Vector3 back = transform.TransformDirection(Vector3.back);

            RaycastHit fwdWallHit;
            RaycastHit rightWallHit;
            RaycastHit leftWallHit;
            RaycastHit backWallHit;


        if (!currentlyDashing)
            {

            wallFwd = Physics.Raycast(transform.position, fwd, out fwdWallHit, wallCheckDistance, groundMask);
            wallRight = Physics.Raycast(transform.position, right, out rightWallHit, wallCheckDistance, groundMask);
            wallLeft = Physics.Raycast(transform.position, left, out leftWallHit, wallCheckDistance, groundMask);
            wallBack = Physics.Raycast(transform.position, back, out backWallHit, wallCheckDistance, groundMask);

            touchingWall = wallFwd || wallRight || wallLeft|| wallBack;


            //Storing wall normals for disabling multiple jumps on same wall
            if (wallFwd)
            {
               currentWallNormal = fwdWallHit.normal;
                
            }
             if (wallRight)
            {
                currentWallNormal = rightWallHit.normal;
            }
             if (wallLeft)
            {
                currentWallNormal = leftWallHit.normal;
            }
             if (wallBack)
            {
                currentWallNormal = backWallHit.normal;
            }
                if(isGrounded)
            {
                disabledWallNormal = currentWallNormal = Vector3.zero;
            }

            //Wall Jump
            if (state == MovementState.onWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    disabledWallNormal = currentWallNormal;
                    float wallTimerval = 1.0f;
                    exitingWall = true;

                    DOTween.To(() => wallTimerval, x => wallTimerval = x, 2.0f,exitingWallTimer).OnComplete(() =>
                    {
                        exitingWall = false;
                    });
                  }

                if (Vector3.Distance(currentWallNormal, disabledWallNormal) < 0.1)
                {   
                    
                    canWallJump = false;    
                } else
                {
                    canWallJump = true;
                }
            
            }
         


        }
            


            
    }
    void SlamControls()
    {
        bool anim = true;
        if (state == MovementState.slamming && velocity.y < -minimumSlamSpeed)
        {

            if (anim)
            {
                playerParticleEffects.PlayFallLineAnimation();
             anim = false;
            }
            Debug.Log("Can slam!");
            canSlam = true;
        }

        if (isGrounded && canSlam)
        {

            playerParticleEffects.StopFallLineAnimation();
            playerParticleEffects.CreateGroundSlamAnimatiion();
            Debug.Log("Slammed! with radius:"+ -velocity.y);
            gm.drawCircle(transform.position, -velocity.y/(1/groundSlamRadiusFactor));

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, -velocity.y / (1 / groundSlamRadiusFactor));
            foreach (var hitCollider in hitColliders)
            {

                Rigidbody rb = hitCollider.attachedRigidbody;
                if (hitCollider.tag == "Enemy"&& rb != null) {

                     Debug.Log("enemy hit");
                    //Debug.Log("Collider hit "+ rb.name+"Damage: " + -velocity.y / (1 / groundSlamRadiusFactor));
                    //rb.AddExplosionForce(150000.0F, transform.position, -velocity.y / (1 / groundSlamRadiusFactor), 50.0F);
                }

            }
            canSlam = false;

        }
    }

    public void ApplyKnockBack(Vector3 knockBackDir, float knockBackForce)
    {
        Debug.Log("Applied knockback");
        knockBackVelocity = knockBackDir * knockBackForce;


    }
    
    

}
    
