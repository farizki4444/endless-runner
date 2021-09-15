using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
 public float jumpAccel;

    [Header("Ground Raycast")]
public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController Score;
    public float scoringRatio;
    private float lastPositionX;

    [Header("game over")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera ;

    private Rigidbody2D rig;
    

    private bool isJumping;
    private bool isOnGround;
    private Animator anim;
    private CharacterSoundController sound;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){

        if(isOnGround){

                isJumping = true;
                sound.PlayJump();
        }
        }

        anim.SetBool("isOnGround", isOnGround);

        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if(scoreIncrement > 0){

            Score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if(transform.position.y < fallPositionY){
            GameOver();
        }
    }


    private void GameOver(){

        Score.FinishScoring();

        gameCamera.enabled = false;

        gameOverScreen.SetActive(true);

        this.enabled = false;
    }
    private void FixedUpdate(){

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if(hit){
            if (!isOnGround && rig.velocity.y <= 0) {
                isOnGround = true;
            }

            else{
                isOnGround = false;
            }
        }

        Vector2 velocityVector  = rig.velocity;

        if(isJumping){
            velocityVector.y += jumpAccel;
            isJumping = false;
       
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel *Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void onDrawGizmoz(){
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
