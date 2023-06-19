using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A more practical example, this demonstrates inheritance and polymorphism to override the base functionality to add a crouching mechanic
/// </summary>
public class DuckCharacter : BaseCharacterController
{
    [SerializeField] private BoxCollider2D collisionCollider;
    private Vector2 colliderScale;
    private float startingMoveSpeed;
    [SerializeField] private float moveSpeedCrouchModifier = 0.5f;
    [SerializeField] private float crouchingColliderScaleModifier = 0.7f;

    private bool isCrouching = false;
    private bool crouchCancelled = false;
    [SerializeField] private float crouchingHeight = 0.25f;
    [SerializeField] private LayerMask environmentLayers;

    protected override void OnEnable()
    {
        base.OnEnable();
        input.FindAndReturnButton(InputActions.ActionOne).onButtonPress += Crouch;
        input.FindAndReturnButton(InputActions.ActionOne).onButtonRelease += CancelCrouch;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        input.FindAndReturnButton(InputActions.ActionOne).onButtonPress -= Crouch;
        input.FindAndReturnButton(InputActions.ActionOne).onButtonRelease -= CancelCrouch;
    }

    protected override void Awake()
    {
        base.Awake();
        if (collisionCollider)
        {
            colliderScale = collisionCollider.size;
        }
        else
        {
            Debug.LogError("No Collison Collider assined on player");
        }

        // set the startin move speed for later
        startingMoveSpeed = moveSpeed;
    }
    
    // overriding the base functionality of the update to do something different.
    protected override void Update()
    {
        base.Update();
        DisableCrouch();
    }

    // overriding the start game function to be able to handle auto movement rather than based on input
    protected override void StartGame()
    {
        base.StartGame();
        // here I am setting the move vector that would normal be set from input to always be right i.e. new vector2(1,0);
        moveVector = Vector2.right;
    }


    protected override void GetMoveInput()
    {
        // so here I am overriding the base functionality and saying do nothing instead.
    }

    /// <summary>
    /// overriding the animation state function from the base, in order to add in a new animation state specific
    /// to this character.
    /// </summary>
    protected override void UpdateAnimationState()
    {
        // copied from the base class, but added update to include the new crouching state.
        if (hit)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Hit;
        }
        else if (isCrouching)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Crouch;
        }
        else if (moveVector.magnitude > 0 && !hit && crouchCancelled)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Running;
        }
        else if (!hit && crouchCancelled)
        {
            characterAnimator.CurrentState = CharacterAnimator.AnimationStates.Idle;
        }
    }

    /// <summary>
    /// used to handle the dyanmics of crouching i.e. slower move speed and smaller collider.
    /// </summary>
    private void Crouch()
    {
        Debug.Log("Crouching yay!");
        // need to reduce move speed
        moveSpeed = startingMoveSpeed * moveSpeedCrouchModifier;
        // need to half the box collider height
        collisionCollider.size = colliderScale * crouchingColliderScaleModifier;
        isCrouching = true;
        crouchCancelled = false;
    }

    // this one is used to detect when the crouch is no longer pressed.
    private void CancelCrouch()
    {
        isCrouching = false;
    }

    // this one is used to detect when the crouching should be cancelled, i.e. in case we are midway through a tunnel and the player lets go.
    private void DisableCrouch()
    {
        if (!isCrouching)
        {
            if (CheckIfCanCancelCrouch())
            {
                collisionCollider.size = colliderScale;
                moveSpeed = startingMoveSpeed;
                crouchCancelled = true;
            }
        }
    }

    /// <summary>
    /// Determines if we should auto stand up outside of a tunnel
    /// </summary>
    /// <returns></returns>
    private bool CheckIfCanCancelCrouch()
    {
        // draw a line and see if we are still hitting ground above us, and if so return true.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, crouchingHeight, environmentLayers);
        //Debug.DrawRay(transform.position, new Vector2(0,crouchingHeight), Color.red, 1);

        if(hit.transform == null)
        {
            return true;
        }
        else
        {
           // Debug.Log(hit.transform.name);
            return false;
        }
    }

    /// <summary>
    /// Here we are overiding the declare place, to be able to make us no move anymore.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="placed"></param>
    protected override void DeclarePlace(Transform player, int placed)
    {
        // need to orride this one, as we are forcing the move speed to change when not crouching and setting it back to the start speed.
        if (player != transform)
        {
            return;
        }
        // stop moving the character
        moveSpeed = 0;
        // disable input
        enableInput = false;
        // need to 0 out the move to make the animations play correctly.
        moveVector = Vector2.zero;
        // send info to the ui to show the number 1,2,3,4 etc.
        endOfGamePlacing = placed;
    }
}
