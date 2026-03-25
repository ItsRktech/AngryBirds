using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip _pauseSound;
    [SerializeField] private AudioSource _gameSound;
    [SerializeField] private GameObject _soundObject;
    [SerializeField] private GameObject _nosoundObject;
    void awake()
    {
        _nosoundObject.SetActive(false);
    }
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
    public void Sound()
    {
        if (!_soundObject.activeInHierarchy)
        {
            _soundObject.SetActive(true);
            _nosoundObject.SetActive(false);
            _gameSound.Play();
        }
        else
        {
            _gameSound.Stop();
            _nosoundObject.SetActive(true);
            _soundObject.SetActive(false);
        }
    }
}