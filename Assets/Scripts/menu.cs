using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip _pauseSound;
    public void SelectLevel(int selectLevel)
    {
        AudioSource.PlayClipAtPoint(_pauseSound, Camera.main.transform.position);
        SceneManager.LoadScene(selectLevel);
    }
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}