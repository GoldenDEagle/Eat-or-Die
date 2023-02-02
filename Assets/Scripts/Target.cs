using Assets.Scripts;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int _pointValue;
    [SerializeField] private float _minSpeed = 14f;
    [SerializeField] private float _maxSpeed = 18f;
    [SerializeField] private float _maxTorque = 10f;
    [SerializeField] private float _xSpawnRange = 4f;
    [SerializeField] private float _ySpawnPosition = -6f;
    [SerializeField] private ParticleSystem _explosionParticles;

    private Rigidbody _targetRb;
    private GameManager _gameManager;

    private void Start()
    {
        _targetRb = GetComponent<Rigidbody>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        transform.position = RandomSpawnPosition();

        _targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        _targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

    }

    private void Update()
    {
        if (_gameManager.isGameActive && !_gameManager.isGamePaused)
        {
            if (Input.GetMouseButton(0))
            {
                // ray from the mouse cursor on screen in the direction of the camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // if raycast hits an object, destroy it and count score
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Destroy(hit.transform.gameObject);
                    _gameManager.UpdateScore(_pointValue);
                    Instantiate(_explosionParticles, hit.transform.position, _explosionParticles.transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad"))
        {
            _gameManager.LoseLife(1);
        }
    }

    private Vector3 RandomSpawnPosition() => new Vector3(Random.Range(-_xSpawnRange, _xSpawnRange), _ySpawnPosition);
    private Vector3 RandomForce() => Vector3.up * Random.Range(_minSpeed, _maxSpeed);
    private float RandomTorque() => Random.Range(-_maxTorque, _maxTorque);
}
