using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int MaxNumberOfShots = 3;
    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private GameObject _soundObject;
    [SerializeField] private AudioSource _gameSound;
    [SerializeField] private GameObject _nosoundObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;
    [SerializeField] private Image _nextLevelImage;
    [SerializeField] private GameObject _pauseScreenObject;
    private int _usedNumberOfShots;

    private IconHandler _iconHandler;

    private List<Baddie> _baddies = new List<Baddie>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_slingShotHandler == null)
        {
            _slingShotHandler = FindAnyObjectByType<SlingShotHandler>();

            if (_slingShotHandler == null)
            {
                Debug.LogError("GameManager: SlingShotHandler not found in the scene!");
            }
        }

        _iconHandler = FindAnyObjectByType<IconHandler>();
        if (_iconHandler == null)
        {
            Debug.LogError("GameManager: IconHandler not found in the scene!");
        }

        Baddie[] baddies = FindObjectsByType<Baddie>(FindObjectsSortMode.None);
        if (baddies != null)
        {
            for (int i = 0; i < baddies.Length; i++)
            {
                _baddies.Add(baddies[i]);
            }
        }

        if (_nextLevelImage != null)
        {
            _nextLevelImage.enabled = false;
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);

        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        return _usedNumberOfShots < MaxNumberOfShots;
    }

    public void CheckForLastShot()
    {
        if (_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if (_baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    private void CheckForAllDeadBaddies()
    {
        if (_baddies.Count == 0)
        {
            WinGame();
        }
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
    #region Win/Lose
    private void WinGame()
    {
        if (_restartScreenObject != null)
            _restartScreenObject.SetActive(true);
        else
            Debug.LogError("Restart Screen Object is NULL");

        if (_slingShotHandler != null)
            _slingShotHandler.enabled = false;
        else
            Debug.LogError("SlingShotHandler is NULL");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < maxLevels)
        {
            if (_nextLevelImage != null)
                _nextLevelImage.enabled = true;
            else
                Debug.LogError("Next Level Image is NULL");
        }
    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void pause()
    {
        _pauseScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;
    }
    public void SelectLevel(int selectLevel)
    {
        SceneManager.LoadScene(selectLevel);
    }
    public void resume()
    {
        _pauseScreenObject.SetActive(false);
        _slingShotHandler.enabled = true;
    }
    #endregion
}