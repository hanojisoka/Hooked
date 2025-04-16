using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndButtonsManager : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene("FishingGame");
    public void ExitGame() => Application.Quit();
}
