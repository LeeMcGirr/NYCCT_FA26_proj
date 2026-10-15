using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class wasd : MonoBehaviour
{

    public GameObject dustParticles;
    //public Camera myCam;
    public GameObject gm;

    //variables for jumping
    public float accel = 10f;
    public float accelTime = .3f;
    public bool grounded;

    public float health = 5f;
    public float score = 0f;
    public float speed = 10f;
    public Vector2 direction;

    public SpriteRenderer mySprite;
    public Collider2D myCol;
    public Rigidbody2D myRB;
    AudioSource myAudio;

    public Vector3 startScale;
    //we could declare the keys as variables here but we're using the new input system


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;
        gm = GameManager.gameManager.gameObject; //you can find a static variable without searching for it

        mySprite = GetComponent<SpriteRenderer>();
        myCol = GetComponent<Collider2D>();
        myRB = GetComponent<Rigidbody2D>();
        myAudio = GetComponent<AudioSource>();

        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //clean our direction vector before figuring out what the WASD is this frame
        direction = Vector2.zero;

        //one fourth of a WASD
        if(Keyboard.current.wKey.isPressed) //checking input with the NEW InputSystem Keyboard.current struct
        {
            //direction += Vector2.up * speed; //we can use Vector2/3 shorthands for up/down/left/right/forwards
        }
        if(Keyboard.current.sKey.isPressed)
        {
            //direction += Vector2.down * speed;
        }
        if(Keyboard.current.aKey.isPressed)
        {
            direction += Vector2.left * speed;
        }
        if(Keyboard.current.dKey.isPressed)
        {
            direction += Vector2.right * speed;
        }
        myRB.AddForce(direction);


        //what if we want to flip the sprite to face how we move?
        //first we check to see if player input a new direction this turn or is idle
        if (direction != Vector2.zero)
        {
            //if the player is NOT idle, flip the sprite when moving left
            if (direction.x < 0f) 
            {
                mySprite.flipX = true;
            }
            else //make sure to flip it back when moving right!
            {
                mySprite.flipX = false;
            }
        }


        //squish/squeeze
        //find the DIRECTION the player is traveling
        Vector3 playerVel = myRB.linearVelocity.normalized; //.normalized sets length of the vector to 1
        //our limit for scaling should be a fraction of starting scale
        //in this case, I only want the player to get about 33% larger or smaller at most!
        Vector3 newScale = startScale/3f;
        //modifying the new scale based off direction of player! so when we move left/right we stretch,
        //when we move up/down we squeeze
        newScale.y *= Mathf.Abs(playerVel.y);
        newScale.x *= Mathf.Abs(playerVel.x);

        //if the player is moving SLOWER, let's divide that normalized vector 
        //to make it a smaller shift in scale
        if (myRB.linearVelocity.magnitude < 10f)
        {
            newScale *= (myRB.linearVelocity.magnitude/10);
        }
        //add the new scale modifier and our start scale up to find our target scale
        Vector3 targetScale = startScale + newScale;

        //lerp to smoothly transition the scale 10% every frame!
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, .1f);
        
        
        //transform.localScale = startScale + newScale;
    }

    void FixedUpdate()
    {
        if(Keyboard.current.spaceKey.isPressed && grounded)
        {
            myRB.AddForce(Vector2.up * accel);
        }
    }

    //this function runs when the gameObject hits a SOLID collider object
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            Instantiate(dustParticles, collision.contacts[0].point, Quaternion.identity);
        }


        Debug.Log("hit an object");
        if (collision.gameObject.tag == "hazard")
        { 
            health--;
            gm.SendMessage("ChangePlayerHealth", health);
        }
        //this runs the frame we first contact an object


        Vector3 currentPlayerVel = myRB.linearVelocity;
        //Vector3 otherColliderVel = collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity;

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //this runs any frame we stay in contact with an object
        if (collision.gameObject.tag == "ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //this runs the frame we leave contact with an object
        if(collision.gameObject.tag == "ground")
        {
            grounded = false;
        }
    }

    //this runs when the gameObject enters a trigger VOLUME
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "collectible")
        {
            score++;
            myAudio.Play();
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }

}
