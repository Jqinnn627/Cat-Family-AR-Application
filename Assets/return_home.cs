using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void BackToHome()
    {
        // Load scene by name
        SceneManager.LoadScene("MainMenu"); // <-- Replace with your home scene name

        // Or load by index (if Home is 0)
        // SceneManager.LoadScene(0);
    }
}
