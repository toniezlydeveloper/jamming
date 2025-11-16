using Internal.Runtime.Utilities;
using UnityEngine;

namespace Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        public void Play(MusicType type)
        {
            ExtendedDebug.Log($"Playing {type}");
        }
    }
}