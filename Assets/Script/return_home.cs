using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void BackToHome()
    {
        // Load scene by name
        SceneManager.LoadScene("MainMenu"); 
    }
}
