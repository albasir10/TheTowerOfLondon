using GamePlay.Info;
using Services;
using System.Collections;
using Toruses;
using UnityEngine;

namespace Players
{
    public class PlayerController : MonoBehaviour
    {
        private bool _isGrab = false;

        private Camera _camera;

        private Torus _grabTorus;

        [SerializeField] private LayerMask _layerTorus;

        private bool _isPlay = true;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            StartCoroutine(Playing());
        }

        private void OnEnable()
        {
            GameService.Singleton.GetService<IGameInfo>().OnGameEnd += EndGame;
        }

        private void OnDisable()
        {
            GameService.Singleton.GetService<IGameInfo>().OnGameEnd -= EndGame;
        }

        private void EndGame(bool isWin, int score)
        {
            _isPlay = false;
        }

        private IEnumerator Playing()
        {
            while (_isPlay)
            {
                if (Input.GetMouseButtonUp(0) && _isGrab)
                {
                    Throw();
                }
                else if (_isGrab)
                {
                    Grabbing();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Grab();
                }
                yield return null;
            }
        }

        private void Grab()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerTorus) && hit.rigidbody.TryGetComponent(out Torus torus) && torus.IsCanGrab)
            {
                _grabTorus = torus;

                _isGrab = true;

                _grabTorus.StartGrab();
            }
        }

        private void Throw()
        {
            _grabTorus.StopGrab();

            _grabTorus = null;

            _isGrab = false;
        }

        private void Grabbing()
        {
            _grabTorus.Move(GetMouseWorldPos());
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;

            mousePoint.z = Mathf.Abs(_camera.transform.position.z - _grabTorus.transform.position.z) + 1 ;

            return _camera.ScreenToWorldPoint(mousePoint);
        }
    }
}
