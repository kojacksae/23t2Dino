using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A class to handle the in game Ui of the game, in particular player selection, Instructional text, and game over.
/// </summary>
public class InGameUi : MonoBehaviour
{
  
    // player select screen variables
    [SerializeField] private GameObject playerSelectScreen;
    [SerializeField] private TextMeshProUGUI playerSelectText;
    [SerializeField] private List<GameObject> playerSelectCharacters = new List<GameObject>();
    // player instructional screen variables
    [SerializeField] private GameObject playerInstructionalTextScreen;
    [SerializeField] private TextMeshProUGUI playerInstructionalText;

    // game over screen variables
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI playerWonText;
    [SerializeField] private List<GameObject> playerGameOverCharacters = new List<GameObject>();

    private void OnEnable()
    {
        GameEvents.OnPlayerJoin += OnPlayerJoined;
        GameEvents.GameStarted += HidePlayerSelectScreen;
        GameEvents.PlayerSelectTextUpdated += SetPlayerSelectText;
        GameEvents.InstructionalTextUpdated += SetPlayerInstructionalText;
        GameEvents.ShowWinner += GameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerJoin -= OnPlayerJoined;
        GameEvents.GameStarted -= HidePlayerSelectScreen;
        GameEvents.PlayerSelectTextUpdated -= SetPlayerSelectText;
        GameEvents.InstructionalTextUpdated -= SetPlayerInstructionalText;
        GameEvents.ShowWinner -= GameOver;
    }

    void Awake()
    {
        // hide all the menus, except the player elect.
        playerSelectScreen.SetActive(true);
        playerInstructionalTextScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        HideAllCharacters(playerSelectCharacters);
    }

    /// <summary>
    /// Called to set the text of the player select screen.
    /// </summary>
    /// <param name="message"></param>
    void SetPlayerSelectText(string message)
    {
        playerSelectText.text = message;
    } 

    /// <summary>
    /// called to set the instructional text for the players, if you give it an empty string, it'll hide this message.
    /// </summary>
    /// <param name="message"></param>
    void SetPlayerInstructionalText(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            // if we get an empty message we can use this to hide the screen.
            playerInstructionalText.text = string.Empty;
            playerInstructionalTextScreen.SetActive(false);
        }
        else
        {
            if (!playerInstructionalTextScreen.activeInHierarchy)
            {
                playerInstructionalTextScreen.SetActive(true);
            }
            playerInstructionalText.text = message;
        }
    }

    /// <summary>
    /// Hides the player select screen.
    /// </summary>
    void HidePlayerSelectScreen()
    {
        playerSelectScreen.SetActive(false);
    }

    /// <summary>
    /// Loops over all the character sprites and hides them on the player select screen or game over screen.
    /// </summary>
    /// <param name="characters"></param>
    void HideAllCharacters(List<GameObject> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].SetActive(false);
        }
    }

    /// <summary>
    /// Shows the character for the player joined, or for the character who won on the game over screen.
    /// </summary>
    /// <param name="playerNum"></param>
    /// <param name="characters"></param>
    private void ShowCharacter(int playerNum, List<GameObject> characters)
    {
        // need to minus one here, due to lists/arrays starting at 0, so player 1 is equal to element 0
        characters[playerNum - 1].SetActive(true);
    }

    /// <summary>
    /// Called when a player joins the game.
    /// </summary>
    /// <param name="playerNumberToSet"></param>
    /// <param name="player"></param>
    private void OnPlayerJoined(PlayerNumbers playerNumberToSet, Transform player)
    {
        // so casting the enum, to the int value associated with it.
        ShowCharacter((int)playerNumberToSet, playerSelectCharacters);
    }

    /// <summary>
    /// called when the game is over, and shows who won on the game over screen
    /// </summary>
    /// <param name="player"></param>
    private void GameOver(PlayerNumbers player)
    {
        playerSelectScreen.SetActive(false);
        playerInstructionalTextScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        HideAllCharacters(playerGameOverCharacters);
        playerWonText.text = player.ToString();
        // again casting the enum to the value of an int, then using that to show the right sprite.
        ShowCharacter((int)player, playerGameOverCharacters);
    }

}
