using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private int _pointValue;
        [Header("Launch Parameters")]
        [SerializeField] private float _minSpeed = 14f;
        [SerializeField] private float _maxSpeed = 18f;
        [SerializeField] private float _maxTorque = 10f;
        [Header("Position")]
        [SerializeField] private float _xSpawnRange = 4f;
        [SerializeField] private float _ySpawnPosition = -6f;
        [Header("Particles")]
        [SerializeField] private ParticleSystem _explosionParticles;

        public int PointValue => _pointValue;
        public ParticleSystem ExplosionParticles => _explosionParticles;

        public static event Action OnLifeLost;

        private Rigidbody _targetRb;
        private GameManager _gameManager;
        private IObjectPool<Target> _pool;

        private void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            transform.position = RandomSpawnPosition();
        }

        public void Launch()
        {
            _targetRb = GetComponent<Rigidbody>();

            _targetRb.AddForce(RandomForce(), ForceMode.Impulse);
            _targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_pool != null)
                _pool.Release(this);
            
            if (!gameObject.CompareTag("Bad"))
            {
                _gameManager.LoseLife(1);
                OnLifeLost.Invoke();
            }
        }

        public void SetPool(IObjectPool<Target> pool) => _pool = pool;

        public Vector3 RandomSpawnPosition() => new Vector3(Random.Range(-_xSpawnRange, _xSpawnRange), _ySpawnPosition);
        private Vector3 RandomForce() => Vector3.up * Random.Range(_minSpeed, _maxSpeed);
        private float RandomTorque() => Random.Range(-_maxTorque, _maxTorque);


        private void OnDisable()
        {
            transform.position = RandomSpawnPosition();
            _targetRb.velocity = Vector3.zero;
            _targetRb.angularVelocity = Vector3.zero;
        }
    }
}
