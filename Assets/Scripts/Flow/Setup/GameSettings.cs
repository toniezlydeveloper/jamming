using System;
using UnityEngine;

namespace Flow.Setup
{
    [Serializable]
    public class GameSettings
    {
        [field: SerializeField] public string LevelName { get; set; }
    }
}