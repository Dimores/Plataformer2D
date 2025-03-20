using System.Collections;
using System.Collections.Generic;
using Orby.Core.Singleton;
using UnityEngine;

namespace Orby.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        [Header("Axis References")]
        [SerializeField] private string _horizontalAxis;
        [SerializeField] private string _verticalAxis;

        void Update()
        {
            ProcessInputs();
        }

        private void ProcessInputs()
        {
            Horizontal = Input.GetAxisRaw(_horizontalAxis);
            Vertical = Input.GetAxisRaw(_verticalAxis);
        }
    }
}
