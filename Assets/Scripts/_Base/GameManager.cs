using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager manager;

    [SerializeField] private int playerSelectWaitTime = 5;
    [SerializeField] private int playerInstructionalMessageTime = 2;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private int maxNumberOfPlayers;
    private Coroutine startJoinRoutine;
    private Coroutine gameOverRoutine;
    private Coroutine waitingForJoinRoutine;

    private List<Transform> allPlayers = new List<Transform>();

    [SerializeField] private string playerInstruction = "Press A To Crouch";

    private Transform winningPlayer;

    private void OnEnable()
    {
        manager.onPlayerJoined += Manager_onPlayerJoined;
        GameEvents.GoalAchieved += GoalAchieved;
    }

    private void OnDisable()
    {
        manager.onPlayerJoined -= Manager_onPlayerJoined;
        GameEvents.GoalAchieved -= GoalAchieved;
    }

    private void Awake()
    {
        maxNumberOfPlayers = spawnPoints.Count;
    }

    private void Start()
    {
        startJoinRoutine = StartCoroutine(StartJoinProcess());
    }

    private IEnumerator StartJoinProcess()
    {
        // show on the screen here Press Start to join.

        // wait for 5 seconds.
        int seconds = 0;
        while(seconds < playerSelectWaitTime)
        {
          
            // show the number on the screen.
            seconds++;
            GameEvents.PlayerSelectTextUpdated?.Invoke("Time Till Start: " + (playerSelectWaitTime - seconds));
            yield return new WaitForSeconds(1);
        }
        GameEvents.PlayerSelectTextUpdated?.Invoke("Game Starting!");
        // give one more second grace period.
        yield return new WaitForSeconds(1);
        // then show start and allow player movement and enable the player movement.  
        manager.DisableJoining();
        // invoke event here to send out a message to enable input.
        GameEvents.GameStarted?.Invoke();

        // here if our spawn points haven't been used, then no players joined.
        if(manager.playerCount <=0)
        {
            // jump back to the main menu
            GameEvents.LoadScene?.Invoke(0);
        }

        GameEvents.InstructionalTextUpdated?.Invoke(playerInstruction);

        // show our messagge, then hide it.
        yield return new WaitForSeconds(playerInstructionalMessageTime);
        GameEvents.InstructionalTextUpdated?.Invoke(string.Empty);

        startJoinRoutine = null;
    }

    private void GameOver()
    {
        // get the placings vs getting the number in the list of players.
        gameOverRoutine = StartCoroutine(GameOverProcess());
    }

    private IEnumerator GameOverProcess()
    {
        // show the winner playr
        yield return new WaitForSeconds(1);

        // this shouts out to all the players to tell the Ui who won.
        GameEvents.GameOver?.Invoke();

        // wait 5 seconds to go back to the main
        yield return new WaitForSeconds(5);
        // go back to main menu
        GameEvents.LoadScene?.Invoke(0);
        gameOverRoutine = null;
    }

    private void Manager_onPlayerJoined(PlayerInput obj)
    {
        // spawn the character, remove the spawn point.
        obj.transform.position = spawnPoints[0].position;

        // keep track of how many players we have here, we'll need to know later to handle the game over logic i.e to show winning screen.
        allPlayers.Add(obj.transform);

        // best practice when starting a coroutine is to store it somewhere to access or keep track of, remember each time you start a coroutine, its a new instance of those instructions.
        // Here I need to this to be able to wait for one frame, this fixes a bug where the player doesn't get their number due to an execution order.
        // given it's one frame it happens so fast you'd never see it.
        waitingForJoinRoutine = StartCoroutine(WaitForJoin(obj.transform));
    }

    private IEnumerator WaitForJoin(Transform player)
    {
        yield return new WaitForEndOfFrame();

        // so here, we grab max num i.e. 4, take the current count i.e. 4, then add 1,
        // so player one would be (4-4) + 1 = 1.
        // player two would be, (4-3) + 1 = 2. this is cause we removed the spawn point from player one.
        int getPlayerNumber = (maxNumberOfPlayers - spawnPoints.Count) + 1;

        // so here because the player number enum is set up to equal a number i.e. playerOne = 1;
        // I can cast our int, as the enum type, i.e. convert it to an enumerated value.
        PlayerNumbers currentPlayer = (PlayerNumbers)getPlayerNumber;

        //Debug.Log(currentPlayer.ToString());

        // invoke the event, to let the player know their number
        GameEvents.OnPlayerJoin?.Invoke(currentPlayer, player);

        // remove the spawn point from the possible choices.
        spawnPoints.RemoveAt(0);

        // setting the routine to null cause it's done.
        waitingForJoinRoutine = null;
    }

    private void GoalAchieved(Transform player, int placed)
    {
        if(placed == maxNumberOfPlayers - spawnPoints.Count)
        {
            // so if we placed forth, and 4 -0 spawn points are left, we all reached the goal.
            // if we have 3 players, and placed 3rd, then 4 max - 1 spawn point left, = 3 which is number players in game.
            // the game is over now, cause all players that are in the game have reached the goal, start the game over process
            GameOver();
        }
    }
}
