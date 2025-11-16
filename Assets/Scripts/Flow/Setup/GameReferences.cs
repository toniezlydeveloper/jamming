using System;
using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Flow.Setup
{
    [Serializable]
    public class GameReferences
    {
        [field: SerializeField] public EventSystem EventSystem { get; set; }
        [field: SerializeField] public Camera MainCamera { get; set; }
        [field: SerializeField] public SfxPlayer SfxPlayer { get; set; }
        [field: SerializeField] public MusicPlayer MusicPlayer { get; set; }
    }
}