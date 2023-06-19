using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int buildIndex)
    {
        GameEvents.LoadScene?.Invoke(buildIndex);
    }

    public void Options()
    {
        Debug.Log("no options implemented yet");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
