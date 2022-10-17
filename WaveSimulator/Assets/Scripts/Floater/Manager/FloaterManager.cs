using System;
using UnityEngine;

namespace Floater
{
    public class FloaterManager : MonoBehaviour
    {
        private int _FloaterAmount = 0;
        [SerializeField] private Floater[] _Floaters;

        private void OnEnable()
        {
            _FloaterAmount = _Floaters.Length;
        }

        private void FixedUpdate()
        {
            foreach (var floater in _Floaters)
                FloaterHandler.FloaterUpdate(floater, _FloaterAmount);
        }
    }
}
