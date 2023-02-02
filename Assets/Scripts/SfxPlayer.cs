using UnityEngine;

namespace Assets.Scripts
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _lifeLostSound;

        private AudioSource _source;
        private GameManager _gameManager;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void OnEnable()
        {
            Target.OnLifeLost += PlayLifeLostSound;
            InputReader.OnHit += PlayHitSound;
        }

        private void PlayHitSound()
        {
            if (!_gameManager.IsGameActive)
                return;
            _source.PlayOneShot(_hitSound);
        }

        private void PlayLifeLostSound()
        {
            if (!_gameManager.IsGameActive)
                return;
            _source.PlayOneShot(_lifeLostSound);
        }

        private void OnDisable()
        {
            Target.OnLifeLost += PlayLifeLostSound;
            InputReader.OnHit += PlayHitSound;
        }
    }
}