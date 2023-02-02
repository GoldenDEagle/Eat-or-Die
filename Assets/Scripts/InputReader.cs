using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts
{
    public class InputReader : MonoBehaviour
    {
        private GameManager _gameManager;
        private IObjectPool<Target> _pool;

        public static event Action OnHit;

        private void Start()
        {
            _gameManager = GetComponent<GameManager>();
        }

        public void SetPool(IObjectPool<Target> pool) => _pool = pool;

        private void Update()
        {
            if (_gameManager.IsGameActive && !_gameManager.IsGamePaused)
            {
                if (Input.GetMouseButton(0))
                {
                    // ray from the mouse cursor on screen in the direction of the camera
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    // if raycast hits an object, destroy it and count score
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.transform.gameObject.TryGetComponent(out Target target))
                        {
                            Instantiate(target.ExplosionParticles, hit.transform.position, target.ExplosionParticles.transform.rotation);

                            if (_pool != null)
                                _pool.Release(target);

                            _gameManager.UpdateScore(target.PointValue);
                            OnHit.Invoke();
                        }
                    }
                }
            }
        }
    }
}