using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _spawnRate;
        [SerializeField] private int _lives = 3;
        [Header("Objects")]
        [SerializeField] private List<GameObject> _targets;
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _livesText;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _scoreCounter;
        [SerializeField] private GameObject _musicSettings;
        [SerializeField] private GameObject _pauseScreen;
        [Header("Audio")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private AudioClip _gameST;
        [SerializeField] private AudioClip _menuST;

        public bool isGameActive;
        public bool isGamePaused;

        private int _score;
        private float _defaultTimeScale;

        private void Start()
        {
            _defaultTimeScale = Time.timeScale;
            _volumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        public void StartGame(int difficulty = 1)
        {
            isGameActive = true;
            _spawnRate /= difficulty;

            _livesText.gameObject.SetActive(true);
            LoseLife(0);

            _scoreCounter.SetActive(true);
            _score = 0;
            UpdateScore(0);

            _titleScreen.SetActive(false);
            _musicSettings.SetActive(false);

            StartCoroutine(SpawnTargets());

            _musicSource.clip = _gameST;
            _musicSource.Play();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
                PauseGame();
        }

        private IEnumerator SpawnTargets()
        {
            while (isGameActive)
            {
                yield return new WaitForSeconds(_spawnRate);
                int index = Random.Range(0, _targets.Count);
                Instantiate(_targets[index]);
            }
        }
        
        private void PauseGame()
        {
            if (!isGameActive) return;
            if (_pauseScreen.activeInHierarchy == false)
            {
                _pauseScreen.SetActive(true);
                Time.timeScale = 0;
                isGamePaused = true;
            }
            else
            {
                _pauseScreen.SetActive(false);
                Time.timeScale = _defaultTimeScale;
                isGamePaused = false;
            }
        }

        private void UpdateMusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void UpdateScore(int scoreToAdd)
        {
            _score += scoreToAdd;
            _scoreText.text = $"Score: {_score}";
        }

        public void LoseLife(int lives)
        {
            _lives -= lives;
            _lives = Mathf.Max(0, _lives);
            _livesText.text = $"Lives: {_lives}";

            if (_lives < 1)
                GameOver();
        }

        public void GameOver()
        {
            isGameActive = false;

            _gameOverText.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}