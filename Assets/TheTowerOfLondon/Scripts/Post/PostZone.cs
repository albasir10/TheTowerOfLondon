using GamePlay.Info;
using Services;
using System;
using System.Collections.Generic;
using Toruses;
using UnityEngine;

namespace Objects
{
    public class PostZone : MonoBehaviour
    {

        [SerializeField] private List<Torus> _toruses = new();

        private List<TorusType> _needTorusesOrder = new();

        public int CountToruses { get { return _toruses.Count; } }

        private bool _isSet = false;

        private MeshRenderer _mesh;

        private MaterialPropertyBlock _propertyBlock;

        private bool _isEquelsToruses = false;

        private void Awake()
        {
            _mesh = GetComponentInParent<MeshRenderer>();

            _propertyBlock = new();

            _mesh.GetPropertyBlock(_propertyBlock);
        }

        public void SetNeedTorusesOrder(List<TorusType> needTorusesOrder)
        {
            if (_isSet)
            {
                return;
            }

            _needTorusesOrder = new (needTorusesOrder);

            _isSet = true;
        }

        public void AddTorus(Torus torus)
        {
            if (_toruses.Count > 0)
            {
                _toruses[^1].SetupCanGrab(false);
            }

            _toruses.Add(torus);

            _toruses[^1].Init(this);

            _toruses[^1].SetupCanGrab(true);

            torus.transform.position = new Vector3(transform.position.x, _toruses.Count * torus.transform.localScale.y, transform.position.z);

            CheckIsTrue();
        }

        public void RemoveTorus()
        {
            _toruses.RemoveAt(_toruses.Count - 1);
            
            if (_toruses.Count > 0)
            {
                _toruses[^1].SetupCanGrab(true);
            }

            CheckIsTrue();
        }

        private void CheckIsTrue()
        {
            if (_needTorusesOrder.Count != _toruses.Count)
            {
                NotEquelsYet();

                return;
            }

            for(int i = 0; i < _needTorusesOrder.Count; i++)
            {
                if (_needTorusesOrder[i] != _toruses[i].TorusType)
                {
                    NotEquelsYet();

                    return;
                }
            }

            EquelsToruses();
        }

        private void NotEquelsYet()
        {
            if (_isEquelsToruses)
            {
                _isEquelsToruses = false;

                GameService.Singleton.GetService<IGameInfo>().RemoveSuccessPost();

                _mesh.GetPropertyBlock(_propertyBlock);

                _propertyBlock.SetColor("_BaseColor", Color.white);

                _mesh.SetPropertyBlock(_propertyBlock);
            }
        }

        private void EquelsToruses()
        {
            _isEquelsToruses = true;

            _mesh.GetPropertyBlock(_propertyBlock);

            _propertyBlock.SetColor("_BaseColor", Color.green);

            _mesh.SetPropertyBlock(_propertyBlock);

            GameService.Singleton.GetService<IGameInfo>().AddSuccessPost();
        }
    }

}