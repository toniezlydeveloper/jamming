using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flow.Setup
{
    [Serializable]
    public class GameInput
    {
        [field: SerializeField] public InputActionReference MousePosition { get; set; }
        [field: SerializeField] public InputActionReference MouseClick { get; set; }
        [field: SerializeField] public InputActionAsset All { get; set; }
    }
}