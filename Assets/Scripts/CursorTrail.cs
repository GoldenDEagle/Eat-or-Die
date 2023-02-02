using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CursorTrail : MonoBehaviour
    {
        [SerializeField] LineRenderer _trailPrefab = null;
        [SerializeField] Camera _trailCamera;
        [SerializeField] float _clearSpeed = 1;
        [SerializeField] float _distanceFromCamera = 1;

        private LineRenderer _currentTrail;
        private List<Vector3> _points = new List<Vector3>();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DestroyCurrentTrail();
                CreateCurrentTrail();
                AddPoint();
            }

            if (Input.GetMouseButton(0))
            {
                AddPoint();
            }

            UpdateTrailPoints();

            ClearTrailPoints();
        }

        private void DestroyCurrentTrail()
        {
            if (_currentTrail != null)
            {
                Destroy(_currentTrail.gameObject);
                _currentTrail = null;
                _points.Clear();
            }
        }

        private void CreateCurrentTrail()
        {
            _currentTrail = Instantiate(_trailPrefab);
            _currentTrail.transform.SetParent(transform, true);
        }

        private void AddPoint()
        {
            Vector3 mousePosition = Input.mousePosition;
            _points.Add(_trailCamera.ViewportToWorldPoint(new Vector3(mousePosition.x / Screen.width, mousePosition.y / Screen.height, _distanceFromCamera)));
        }

        private void UpdateTrailPoints()
        {
            if (_currentTrail != null && _points.Count > 1)
            {
                _currentTrail.positionCount = _points.Count;
                _currentTrail.SetPositions(_points.ToArray());
            }
            else
            {
                DestroyCurrentTrail();
            }
        }

        private void ClearTrailPoints()
        {
            float clearDistance = Time.deltaTime * _clearSpeed;
            while (_points.Count > 1 && clearDistance > 0)
            {
                float distance = (_points[1] - _points[0]).magnitude;
                if (clearDistance > distance)
                {
                    _points.RemoveAt(0);
                }
                else
                {
                    _points[0] = Vector3.Lerp(_points[0], _points[1], clearDistance / distance);
                }
                clearDistance -= distance;
            }
        }

        void OnDisable()
        {
            DestroyCurrentTrail();
        }
    }
}