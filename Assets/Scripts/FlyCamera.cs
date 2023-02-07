using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlyCamera : MonoBehaviour
{
    public AudioSource audioDead;
    public float acceleration = 50; // how fast you accelerate
    public float accSprintMultiplier = 8; // how much faster you go when "sprinting"
	public float lookSensitivity = 1; // mouse look sensitivity
	public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
	public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable
    public float life = 10.0f;
    float speedPlayerRatio;

    public bool dead = false;

    public FlyPlayer playerBobo;
    public FlyPlayer playerTucusito; 
    private Rigidbody body;

    private Vector3 lastTouchPosition = Vector3.zero;
    private bool touchLeft = false;
    private bool touchRight = false;
    private bool touchUp = false;
    private bool touchDown = false;
    private bool isFirstTouch = true;

Vector3 velocity; // current velocity
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speedPlayerRatio = PlayerPrefs.GetFloat("speedPlayerRatio");
        string playerName = PlayerPrefs.GetString("playerName");
        if (playerName == null || playerName == "")
        {
            playerName = "Tucusito";
        }
        if (playerName == "Tucusito")
        {
            playerTucusito.gameObject.SetActive(true);
            playerBobo.gameObject.SetActive(false);
        } else
        {
            playerTucusito.gameObject.SetActive(false);
            playerBobo.gameObject.SetActive(true);
        }
    }
    static bool Focused {
		get => Cursor.lockState == CursorLockMode.Locked;
		set {
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = value == false;
		}
	}

	void OnEnable() {
		if( focusOnEnable ) Focused = true;
	}

	void OnDisable() => Focused = false;

    private IEnumerator BackToMain()
    {
        for (float t = 0; t < 2.0f; t += Time.deltaTime)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenuScene");
    }
    void Update()
    {
        // Input

        //Debug.Log("Current Box Rotation:" + gameObject.transform.rotation.ToString());
        if ( Focused )
			UpdateInput();
		else if( Input.GetMouseButtonDown( 0 ) )
			Focused = true;

        // Physics
        velocity = Vector3.Lerp( velocity, Vector3.zero, dampingCoefficient * Time.deltaTime );
        //transform.position += velocity * Time.deltaTime;
        if (!dead)
            body.velocity = velocity * Time.deltaTime;

        if (life < 0.01f && !dead)
        {
            Animator animator1 = playerBobo.GetComponent<Animator>();
            Animator animator2 = playerTucusito.GetComponent<Animator>();
            playerBobo.isDead = true;
            playerTucusito.isDead = true;
            dead = true;
            body.velocity = Vector3.zero;
            body.AddForce(new Vector3(0.0f, -15.0f, 0.0f), ForceMode.Impulse);
            audioDead.PlayDelayed(0.3f);
            StartCoroutine(BackToMain());
            animator1.SetBool("isDead", true);
            animator2.SetBool("isDead", true);
        }
        //Debug.Log("Position: " + body.position);
        if (body.position.x < -90.0f)
        {
            body.velocity = new Vector3(0.0f, body.velocity.y, body.velocity.z);
            gameObject.transform.position = new Vector3(-90.0f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (body.position.x > 37.5f)
        {
            body.velocity = new Vector3(0.0f, body.velocity.y, body.velocity.z);
            gameObject.transform.position = new Vector3(37.5f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (body.position.y > 13.5f)
        {
            body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 13.5f, gameObject.transform.position.z);
        }
        if (body.position.y < 2.0f)
        {
            body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 2.0f, gameObject.transform.position.z);
        }

    }

    public void addDanger(float level)
    {
        life -= level;
    }
    void UpdateInput() {
		// Position
		velocity += GetAccelerationVector() * Time.deltaTime;

		// Rotation
		/*Vector2 mouseDelta = lookSensitivity * new Vector2( Input.GetAxis( "Mouse X" ), -Input.GetAxis( "Mouse Y" ) );
		Quaternion rotation = transform.rotation;
		Quaternion horiz = Quaternion.AngleAxis( mouseDelta.x, Vector3.up );
		Quaternion vert = Quaternion.AngleAxis( mouseDelta.y, Vector3.right );
		transform.rotation = horiz * rotation * vert;*/

		// Leave cursor lock
		if( Input.GetKeyDown( KeyCode.Escape ) )
			Focused = false;
	}

	Vector3 GetAccelerationVector() {
		Vector3 moveInput = default;
        bool fromTouch = false;

		void AddMovement( KeyCode key, Vector3 dir ) {
			if( Input.GetKey( key ) )
				moveInput += dir;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved || !isFirstTouch && touch.phase == TouchPhase.Stationary)
            {
                if (isFirstTouch)
                {
                    lastTouchPosition = touch.position;
                    isFirstTouch = false;
                }
                else
                {
                    fromTouch = true;
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        if (touchRight)
                        {
                            moveInput += Vector3.right;
                        }
                        else if (touchLeft)
                        {
                            moveInput += Vector3.left;
                        }
                        /*if (touchUp)
                        {
                            moveInput += Vector3.up;
                        }
                        else if (touchDown)
                        {
                            moveInput += Vector3.down;
                        }*/
                    } else
                    { 
                        if (lastTouchPosition.x < touch.position.x)
                        {
                            moveInput += Vector3.right;
                            touchLeft = false;
                            touchRight = true;
                        }
                        else if (lastTouchPosition.x > touch.position.x)
                        {
                            moveInput += Vector3.left;
                            touchLeft = true;
                            touchRight = false;
                        } else
                        {
                            touchLeft = false;
                            touchRight = false;
                        }
                        if (lastTouchPosition.y < touch.position.y)
                        {
                            moveInput += Vector3.up;
                            touchUp = true;
                            touchDown = false;
                        }
                        else if (lastTouchPosition.y > touch.position.y)
                        {
                            moveInput += Vector3.down;
                            touchUp = false;
                            touchDown = true;
                        }
                        else
                        {
                            touchUp = false;
                            touchDown = false;
                        }
                    }
                    lastTouchPosition = touch.position;
                }
            }
            else
            {
                isFirstTouch = true;
                touchLeft = false;
                touchRight = false;
                touchUp = false;
                touchDown = false;
            }
            

        }

        //AddMovement( KeyCode.W, Vector3.forward );
        //AddMovement( KeyCode.S, Vector3.back );
        AddMovement( KeyCode.W, Vector3.up );
        AddMovement(KeyCode.UpArrow, Vector3.up);
        AddMovement( KeyCode.S, Vector3.down );
        AddMovement(KeyCode.DownArrow, Vector3.down);
        AddMovement(KeyCode.D, Vector3.right );
        AddMovement(KeyCode.RightArrow, Vector3.right);
        AddMovement(KeyCode.A, Vector3.left);
        AddMovement(KeyCode.LeftArrow, Vector3.left);
        //AddMovement( KeyCode.Space, Vector3.up );
        //AddMovement( KeyCode.LeftControl, Vector3.down );
        Vector3 direction = moveInput.normalized; //transform.TransformVector( moveInput.normalized );

        if (fromTouch)
        {
            direction = new Vector3(direction.x, direction.y * 0.5f, direction.z);
        }

        return direction * ( acceleration * accSprintMultiplier * speedPlayerRatio); // "sprinting"
    }
}