using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static VoidEventNoPram GameStarted;
    public static VoidEventNoPram GameOver;

    /// <summary>
    /// so this one can take in the player transform, and then if they came 1st,second, third etc. 
    /// </summary>
    public static VoidEventTwoParam<Transform, int> GoalAchieved;

    /// <summary>
    /// Called when the player joins, and sends a message that contains the tranform of that player and player number to set it to.
    /// </summary>
    public static VoidEventTwoParam<PlayerNumbers, Transform> OnPlayerJoin;

    /// <summary>
    ///      invoke this to load a scene.
    /// </summary>
    /// <param name="load"></param>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static VoidEventOneParam<int> LoadScene;

    /// <summary>
    /// Called to update the player select text.
    /// </summary>
    public static VoidEventOneParam<string> PlayerSelectTextUpdated;

    /// <summary>
    /// Called to update the player instructional text
    /// </summary>
    public static VoidEventOneParam<string> InstructionalTextUpdated;

    /// <summary>
    /// Called to update the game over screentwith the winner.
    /// </summary>
    public static VoidEventOneParam<PlayerNumbers> ShowWinner;
    
}
