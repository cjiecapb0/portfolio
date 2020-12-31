using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    private bool open_another = true;
    public void OpenScene(int numberScene)
    {
        if (!open_another)
            numberScene += 1;
        else
        {
            SceneManager.LoadScene(numberScene);
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void PauseGame(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
    }
}
