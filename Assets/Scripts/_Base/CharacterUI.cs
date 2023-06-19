using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Handles the character related ui in this case, the placing at the end of the game i.e. 1st, 2nd, etc.
/// </summary>
public class CharacterUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI placingText;

    private void OnEnable()
    {
        GameEvents.GoalAchieved += Placed;
    }

    private void OnDisable()
    {
        GameEvents.GoalAchieved -= Placed;
    }

    // Start is called before the first frame update
    void Start()
    {
        placingText.text = string.Empty;
    }

    /// <summary>
    /// Called when the goal is achieved and shows a piece of text above our character and their placing.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="place"></param>
    private void Placed(Transform player, int place)
    {

        if(player == null || player != transform)
        {
            return;
        }

        switch (place)
        {
            case  1:
            {
                placingText.text = place.ToString() +"ST";
                break;
            }
            case 2:
            {
                placingText.text = place.ToString() + "ND";
                break;
            }
            case 3:
            {
                placingText.text = place.ToString() + "RD";
                break;
            }
            case 4:
            {
                placingText.text = place.ToString() + "TH";
                break;
            }
        }
    }
}
