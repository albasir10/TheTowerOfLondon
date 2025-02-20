using GamePlay.Info;
using Objects;
using Services;
using System;
using System.Collections;
using UnityEngine;

namespace Toruses
{
    public class Torus : MonoBehaviour
    {
        private Rigidbody _rb;

        [SerializeField] private float _speed = 2;

        [SerializeField] private TorusType _torusType;

        public TorusType TorusType { get { return _torusType; } }

        [SerializeField] private Color _color = Color.white;

        private MeshRenderer _meshRenderer;

        private bool _isCanGrab = false;

        public bool IsCanGrab { get { return _isCanGrab; } }

        private PostZone _activeZone;

        private PostZone _startZone;

        private bool _isInit = false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            InitColor();
        }

        private void Start()
        {
            StartCoroutine(WaitToThrowInFloor());
        }

        public void Init(PostZone startZone)
        {
            if (_isInit)
            {
                return;
            }

            _isInit = true;

            _startZone = startZone;
        }

        public void SetupCanGrab(bool isCanGrab)
        {
            _isCanGrab = isCanGrab;
        }

        public void StartGrab()
        {
            StopAllCoroutines();

            _rb.isKinematic = false;
        }

        public void StopGrab()
        {
            if (_activeZone == null)
            {
                _activeZone = _startZone;
            }
            else if (_activeZone != _startZone)
            {
                _startZone.RemoveTorus();

                _startZone = _activeZone;

                _startZone.AddTorus(this);

                GameService.Singleton.GetService<IGameInfo>().GrabbedToNewPost();
            }

            SetPosInPost(_activeZone);
        }

        private void SetPosInPost(PostZone postZone)
        {
            StartCoroutine(SetupingPositionInPost(postZone));
        }

        private IEnumerator SetupingPositionInPost(PostZone postZone)
        {
            Vector3 pos = postZone.transform.position;

            pos.y = postZone.CountToruses * transform.localScale.y;

            _rb.isKinematic = true;

            while (Vector3.Distance(transform.position, pos) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 20);

                yield return null;
            }

            transform.rotation = Quaternion.identity;

            _rb.isKinematic = false;

            StartCoroutine(WaitToThrowInFloor());
        }

        private IEnumerator WaitToThrowInFloor()
        {
            yield return new WaitForFixedUpdate();

            while (_rb.linearVelocity.magnitude > 0.01f)
            {
                yield return new WaitForFixedUpdate();
            }

            _rb.isKinematic = true;
        }

        public void Move(Vector3 pos)
        {
            _rb.linearVelocity = (pos - transform.position) * _speed;
        }

        private void InitColor()
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
            }

            MaterialPropertyBlock block = new ();

            _meshRenderer.GetPropertyBlock(block);

            block.SetColor("_BaseColor", _color);

            _meshRenderer.SetPropertyBlock(block);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PostZone postZone))
            {
                EnterZone(postZone);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PostZone postZone))
            {
                ExitZone(postZone);
            }
        }

        private void EnterZone(PostZone postZone)
        {
            _activeZone = postZone;
        }

        private void ExitZone(PostZone postZone)
        {
            _activeZone = null;
        }
    }
}