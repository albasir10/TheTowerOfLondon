using GamePlay.Info;
using Objects;
using Postes;
using Services;
using System.Collections;
using System.Collections.Generic;
using Toruses;
using UnityEngine;

namespace Generates
{
    public class TorusesGenerate : MonoBehaviour
    {
        private int _maxSwapPlaces;

        private int _maxTorus;

        [SerializeField] private PostNeed[] _postNeeds = new PostNeed[3];

        [SerializeField] private Torus[] _prefabsTorus;

        private int _maxTorusInPost;

        private PostInfo _postNeedInfo;

        private PostInfo _postCurrentInfo;

        private void Start()
        {
            _maxSwapPlaces = GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Settings.MaxSwapPlaces;

            _maxTorus = GameService.Singleton.GetService<IGameInfo>().GameInfoStruct.Settings.MaxTorus;

            _maxTorusInPost = (_maxTorus -  (_maxTorus % _postNeeds.Length)) / 2 ;

            _postNeedInfo.Toruses = new(_postNeeds.Length);

            _postCurrentInfo.Toruses = new(_postNeeds.Length);

            for (int i = 0; i < _postNeeds.Length; i++)
            {
                _postNeedInfo.Toruses.Add(i, new());
            }

            GeneratePostInfo();
        }

        private void GeneratePostInfo()
        {
            int currentTorus = 0;

            int torusInCurrentPost;

            for(int i = 0; i < _postNeeds.Length - 1; i++)
            {
                int minIndex = 1;

                while (currentTorus + minIndex + (_maxTorusInPost * (_postNeeds.Length - i - 1) ) < _maxTorus)
                {
                    minIndex++;
                }

                torusInCurrentPost = UnityEngine.Random.Range(minIndex, _maxTorusInPost + 1);

                currentTorus += torusInCurrentPost;

                GeneratePostNeedInfo(i, torusInCurrentPost);
            }

            GeneratePostNeedInfo(_postNeeds.Length - 1, _maxTorus - currentTorus);

            StartCoroutine(GeneratePostCurrentInfo());
        }

        private void GeneratePostNeedInfo(int index, int count)
        {
            int torusIndex;

            _postNeedInfo.Toruses[index] = new List<TorusType>(count);

            for (int i = 0; i < count; i++)
            {
                torusIndex = UnityEngine.Random.Range(0, _prefabsTorus.Length);

                if (i > 0)
                {
                    while (_prefabsTorus[torusIndex].TorusType == _postNeedInfo.Toruses[index][i-1])
                    {
                        torusIndex = UnityEngine.Random.Range(0, _prefabsTorus.Length);
                    }
                }


                _postNeedInfo.Toruses[index].Add(_prefabsTorus[torusIndex].TorusType);
            }
        }

        private IEnumerator GeneratePostCurrentInfo()
        {
            _postCurrentInfo.Toruses = new ();

            foreach (var kvp in _postNeedInfo.Toruses)
            {
                _postCurrentInfo.Toruses.Add(kvp.Key, new (kvp.Value));
            }

            int lastPostToMoved = -1;

            int currentPostMoving;

            int currentPostWillMove;

            for (int i = 0; i < _maxSwapPlaces; i++)
            {
                currentPostMoving = Random.Range(0, _postNeeds.Length);

                while (_postCurrentInfo.Toruses[currentPostMoving].Count == 0 || currentPostMoving == lastPostToMoved)
                {
                    currentPostMoving = Random.Range(0, _postNeeds.Length);

                    yield return null;
                }

                currentPostWillMove = Random.Range(0, _postNeeds.Length);

                while (currentPostMoving == currentPostWillMove)
                {
                    currentPostWillMove = Random.Range(0, _postNeeds.Length);

                    yield return null;
                }

                lastPostToMoved = currentPostWillMove;

                TorusType torusType = _postCurrentInfo.Toruses[currentPostMoving][^1];

                _postCurrentInfo.Toruses[currentPostMoving].RemoveAt(_postCurrentInfo.Toruses[currentPostMoving].Count - 1);

                _postCurrentInfo.Toruses[currentPostWillMove].Add(torusType);

                if (_postCurrentInfo.Toruses[currentPostWillMove].Count == _maxTorus)
                {
                    StartCoroutine(GeneratePostCurrentInfo()); // TODO: не будем дожидаться этой короутины, чтобы не создавать рекурсию, просто идем дальше и заканчиваем эту короутину.

                    yield break;
                }
            }

            SpawnToruses();
        }

        private void SpawnToruses()
        {
            for(int i = 0; i < _postNeedInfo.Toruses.Count; i++)
            {
                foreach (TorusType torusType in _postNeedInfo.Toruses[i])
                {
                    foreach(Torus torus in _prefabsTorus)
                    {
                        if (torus.TorusType == torusType)
                        {
                            _postNeeds[i].AddTorus(Instantiate(torus.gameObject).GetComponent<Torus>());
                        }
                    }
                }
            }

            for (int i = 0; i < _postCurrentInfo.Toruses.Count; i++)
            {

                _postNeeds[i].PostZone.SetNeedTorusesOrder(_postNeedInfo.Toruses[i]);

                foreach (TorusType torusType in _postCurrentInfo.Toruses[i])
                {
                    foreach (Torus torus in _prefabsTorus)
                    {
                        if (torus.TorusType == torusType)
                        {
                            _postNeeds[i].PostZone.AddTorus(Instantiate(torus.gameObject).GetComponent<Torus>());
                        }
                    }
                }
            }
        }
    }
}
