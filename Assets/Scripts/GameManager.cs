using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
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
        [SerializeField] private List<Target> _targets;
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _livesText;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _scoreCounter;
        [SerializeField] private GameObject _musicSettings;
        [SerializeField] private GameObject _pauseScreen;
        [Header("Audio")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private AudioClip _gameST;
        [SerializeField] private AudioClip _menuST;


        public bool IsGameActive;
        public bool IsGamePaused;

        private int _score;
        private float _defaultTimeScale;
        private int _index;
        private InputReader _inputReader;

        private ObjectPool<Target> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<Target>(SpawnTarget, OnPoolGet, OnPoolRelease, OnPoolDestroy);
            _inputReader = GetComponent<InputReader>();
        }

        private void Start()
        {
            _inputReader.SetPool(_pool);
            _defaultTimeScale = Time.timeScale;
            _index = 0;
            _volumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        public void StartGame(int difficulty = 1)
        {
            IsGameActive = true;

            _spawnRate /= difficulty; // spawn rate depending on difficulty

            ShowGameUI();   

            HideMenu();

            StartCoroutine(SpawnTargetsRoutine());

            SetMusicClip(_gameST);
        }

        private void ShowGameUI()
        {
            _pauseButton.gameObject.SetActive(true);

            _livesText.gameObject.SetActive(true);
            LoseLife(0);

            _scoreCounter.SetActive(true);
            _score = 0;
            UpdateScore(_score);
        }

        private void HideMenu()
        {
            _titleScreen.SetActive(false);
            _musicSettings.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
                PauseGame();
        }

        private IEnumerator SpawnTargetsRoutine()
        {
            while (IsGameActive)
            {
                yield return new WaitForSeconds(_spawnRate);
                _pool.Get();
            }
        }

        private Target SpawnTarget()
        {
            Target target = Instantiate(_targets[_index]);
            _index++;
            _index = _index % _targets.Count;
            target.SetPool(_pool);
            return target;
        }

        // pooling
        private void OnPoolGet(Target target)
        {
            target.gameObject.SetActive(true);
            target.Launch();
        }

        private void OnPoolRelease(Target target)
        {
            target.gameObject.SetActive(false);
        }

        private void OnPoolDestroy(Target target)
        {
            Destroy(target.gameObject);
        }

        public void PauseGame()
        {
            if (!IsGameActive) return;
            if (_pauseScreen.activeInHierarchy == false)
            {
                _pauseScreen.SetActive(true);
                _musicSettings.SetActive(true);
                Time.timeScale = 0;
                IsGamePaused = true;
            }
            else
            {
                _pauseScreen.SetActive(false);
                _musicSettings.SetActive(false);
                Time.timeScale = _defaultTimeScale;
                IsGamePaused = false;
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

        private void SetMusicClip(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void GameOver()
        {
            IsGameActive = false;

            _gameOverText.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);

            SetMusicClip(_menuST);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}