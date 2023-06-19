using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacterController
{
    protected override void OnEnable()
    {
        base.OnEnable();
        // here overriding to be able to add in a new button for this specific controller.
        input.FindAndReturnButton(InputActions.ActionOne).onButtonRelease += Jump;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        input.FindAndReturnButton(InputActions.ActionOne).onButtonRelease -= Jump;
    }

    /// <summary>
    /// makes the player jump, but it only works for this character controller.
    /// </summary>
    private void Jump()
    {
        Debug.Log("Jumping yay!");
        rigid.AddForce(Vector2.up * 1000);
    }
}
