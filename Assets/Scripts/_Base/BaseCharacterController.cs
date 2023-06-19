using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a base class that allows for expansion, this contains all the core functionality that is requried for a character in our game.
/// if you inherit from this class you can then expand upon it by adding more functionality or remove functionality from the based by overriding.
/// </summary>
public class BaseCharacterController : MonoBehaviour
{
    // core game variables
    [SerializeField] protected PlayerNumbers playerNumber;
    protected int endOfGamePlacing;
    [SerializeField] protected bool enableInput = false;
    // references to other components
    protected InputRouter input => GetComponent<InputRouter>();
    protected Rigidbody2D rigid => GetComponent<Rigidbody2D>();
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected CharacterAnimator characterAnimator;

    // Color tints to sprites.
    [SerializeField] protected List<Color> playerSpriteTints = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green };

    // movement related variables
    protected Vector2 moveVector;
    [SerializeField] protected float moveSpeed = 2f;

    // damage related variables
    protected bool hit;
    [SerializeField] protected List<string> enemyTags = new List<string>();
    protected Vector2 knockbackDirection;
    protected Vector2 startPosition;
    [SerializeField] protected float knockbackDuration = 0.5f;
    [SerializeField] protected float knockbackDistance = 0.25f;


    protected virtual void OnEnable()
    {
        GameEvents.GoalAchieved += DeclarePlace;
        GameEvents.OnPlayerJoin += OnPlayerJoined;
        GameEvents.GameStarted += StartGame;
        GameEvents.GameOver += GameOver;
    }

    protected virtual void OnDisable()
    {
        GameEvents.GoalAchieved -= DeclarePlace;
        GameEvents.OnPlayerJoin -= OnPlayerJoined;
        GameEvents.GameStarted -= StartGame;
        GameEvents.GameOver -= GameOver;
    }

    protected virtual void Awake()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!enableInput)
        {
            return;
        }
        GetMoveInput();
    }

    protected virtual void FixedUpdate()
    {
        if (!enableInput)
        {
            return;
        }
        Move();
    }

    protected virtual void LateUpdate()
    {
        UpdateVisuals();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyTags.Contains(collision.transform.tag))
        {
            ApplyKnockback(-collision.contacts[0].normal);
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {

    }

    /// <summary>
    /// When called allows movement of the character
    /// </summary>
    protected virtual void StartGame()
    {
        enableInput = true;
    }


    /// <summary>
    /// Gets input from the new input system
    /// </summary>
    protected virtual void GetMoveInput()
    {
        // check if we hit
        if (hit)
        {
            return;
        }
        moveVector = input.FindAndReturnButton(InputActions.Move).AxisVectorValue;
    }

    /// <summary>
    /// actually moves our charcter in 2D space
    /// </summary>
    protected virtual void Move()
    {
        // check if we hit
        if (hit)
        {
            return;
        }
        rigid.MovePosition(rigid.position + (moveVector * (moveSpeed * Time.deltaTime)));
    }

    /// <summary>
    /// Updates the visual sprites and handles animation updating too
    /// </summary>
    protected virtual void UpdateVisuals()
    {
        // Flips the sprite
        if (moveVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        UpdateAnimationState();
    }

    /// <summary>
    /// Updates the animation state of the sprite to show the current state
    /// </summary>
    protected virtual void UpdateAnimationState()
    {
        // update which state should be shown
        if (hit)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Hit;
        }
        else if (moveVector.magnitude > 0 && !hit)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Running;
        }
        else if (!hit)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Idle;
        }
    }

    /// <summary>
    /// A simple function to apply a little knock back to our character if hit.
    /// </summary>
    /// <param name="direction"></param>
    protected virtual void ApplyKnockback(Vector2 direction)
    {
        knockbackDirection = direction;
        startPosition = transform.position;
        StartCoroutine(MovePosition());
    }

    /// <summary>
    /// Using a coroutine here to push the character back in 2D space.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator MovePosition()
    {
        float elapsedTime = 0;
        hit = true;
        while (elapsedTime < knockbackDuration)
        {
            // move from the point of impact, backwards x number of units over y number of seconds
            transform.position = Vector2.Lerp(startPosition, startPosition - knockbackDirection * knockbackDistance, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hit = false;
    }

    /// <summary>
    /// Used to determine what the placing of the mini game is i.e. first, second, third etc.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="placed"></param>
    protected virtual void DeclarePlace(Transform player, int placed)
    {
        if(player != transform)
        {
            return;
        }
        // stop moving the character
        moveSpeed = 0;

        enableInput = false;
        // send info to the ui to show the number 1,2,3,4 etc.
        endOfGamePlacing = placed;
    }

    /// <summary>
    /// Called when the player is first spawned in this case we can set the player number and colour of our sprite.
    /// </summary>
    /// <param name="playerNumberToSet"></param>
    /// <param name="player"></param>
    protected virtual void OnPlayerJoined(PlayerNumbers playerNumberToSet, Transform player)
    {
        if (player != transform)
        {
            return;
        }

        playerNumber = playerNumberToSet;
        
        // if we have a sprite renderer
        if(spriteRenderer)
        {
            // so here using the enum value for the player, casting it as an int, then removing one, to be able to access the list.
            // i.e. player 1 = 1 - 1 = element 0, which by default is red.
            // player 2 = 2 - 1 = element 1, which by default is blue.
            spriteRenderer.color = playerSpriteTints[(int)playerNumber - 1];
        }

        enableInput = false;
    }

    /// <summary>
    /// Handles what should happen when the game is over, in this case we are shouting out we won!
    /// </summary>
    protected virtual void GameOver()
    {
        if(endOfGamePlacing == 1)
        {
            // shout out that this player is the winner of this game
            GameEvents.ShowWinner?.Invoke(playerNumber);
        }
    }
}
