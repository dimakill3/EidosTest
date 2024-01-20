using System;
using UnityEngine;

namespace Services
{
    public class MonoService : MonoBehaviour
    {
        public event Action UpdateTick;

        private void Update() => 
            UpdateTick?.Invoke();
    }
}