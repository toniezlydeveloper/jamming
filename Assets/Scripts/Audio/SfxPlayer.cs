using Internal.Runtime.Utilities;
using UnityEngine;

namespace Audio
{
    public class SfxPlayer : MonoBehaviour
    {
        public void Play(SfxType type)
        {
            ExtendedDebug.Log($"{type}");
        }
    }
}