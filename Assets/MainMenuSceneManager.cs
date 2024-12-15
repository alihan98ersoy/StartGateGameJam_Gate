using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ÇýkýþButonu()
    {
        Application.Quit();

    }

    public void oynaButon()
    {
        SceneManager.LoadScene(1);
    }
}
