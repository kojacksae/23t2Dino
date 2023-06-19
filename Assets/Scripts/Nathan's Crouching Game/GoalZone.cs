using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple example of a goal or way to win the game, this could easily be replaced with other logic i.e. when a character collects 10 coins, you can use the same event, 
/// just tell the game when they achieved that goal as it's only passing in the transform of the character, and their placing i.e. 1st, 2nd 3rd..
/// </summary>
public class GoalZone : MonoBehaviour
{
    private List<Transform> playersEntered = new List<Transform>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // because our character has two colliders, this would trigger this on trigger enter twice, so just checking we aint already in the list.
            if (!playersEntered.Contains(collision.transform))
            {
                playersEntered.Add(collision.transform);
                // pass in the transform of the player then the count of the list, i.e. first player enters count = 1 therefore they placed first.
                GameEvents.GoalAchieved?.Invoke(collision.transform, playersEntered.Count);
            }
        }
    }
}
