using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("LanderLvTemplate");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

