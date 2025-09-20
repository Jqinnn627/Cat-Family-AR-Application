using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ViewCatFamily()
    {
        SceneManager.LoadSceneAsync("Cat_Object"); // AR Scene
    }

    public void PlayQuiz()
    {
        SceneManager.LoadSceneAsync("QuizModule"); // Quiz Scene, not yet
    }

    public void Quit()
    {
        Application.Quit();
    }

}
