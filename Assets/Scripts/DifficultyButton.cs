using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DifficultyButton : MonoBehaviour
    {
        [SerializeField] private int _difficulty;

        private Button _button;
        private GameManager _gameManager;

        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _button= GetComponent<Button>();
            _button.onClick.AddListener(SetDifficulty);
        }

        public void SetDifficulty()
        {
            _gameManager.StartGame(_difficulty);
        }
    }
}