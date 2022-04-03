using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance = null;

    private void Start()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("You are trying to create instance of GameSceneManager but you already have one");
        }
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene("Game");
        Fight.StartGameIfPossible();
    }

    public void OpenGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void OpenMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}