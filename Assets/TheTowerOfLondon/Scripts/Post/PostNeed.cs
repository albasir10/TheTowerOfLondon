using System;
using System.Collections.Generic;
using Toruses;
using UnityEngine;

namespace Objects
{
    public class PostNeed : MonoBehaviour
    {
        [SerializeField] private List<Torus> _toruses = new();

        public PostZone PostZone;


        public void AddTorus(Torus torus)
        {
            _toruses.Add(torus);
                
            torus.transform.position = new Vector3(transform.position.x, _toruses.Count * torus.transform.localScale.y, transform.position.z);  
        }
    }
}
