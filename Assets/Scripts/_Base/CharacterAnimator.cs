using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    
    public enum AnimationStates {Idle,Running,Crouch,Hit}

    [SerializeField] AnimationStates currentState;

    public Animator animator => GetComponent<Animator>();

    public AnimationStates CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            switch (currentState)
            {
                case AnimationStates.Idle:
                    {
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsCrouching", false);
                        animator.SetBool("IsHit", false);
                        break;
                    }
                case AnimationStates.Running:
                    {
                        animator.SetBool("IsRunning", true);
                        animator.SetBool("IsCrouching", false);
                        animator.SetBool("IsHit", false);
                        break;
                    }
                case AnimationStates.Crouch:
                    {
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsCrouching", true);
                        animator.SetBool("IsHit", false);
                        break;
                    }
                case AnimationStates.Hit:
                    {
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("IsCrouching", false);
                        animator.SetBool("IsHit", true);
                        break;
                    }
            }
        }
    }


}
