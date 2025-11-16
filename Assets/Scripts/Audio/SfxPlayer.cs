using System;
using System.Linq;
using Internal.Runtime.Utilities;
using UnityEngine;

namespace Audio
{
    public class SfxPlayer : MonoBehaviour
    {
        [Serializable]
        public class SfxData
        {
            [field: SerializeField] public AudioClip Clip { get; set; }
            [field: SerializeField] public SfxType Type { get; set; }
        }
        
        [SerializeField] private AudioSource source;
        [SerializeField] private SfxData[] datas;
        
        public void Play(SfxType type)
        {
            ExtendedDebug.Log($"{type}");
            source.PlayOneShot(datas.First(x => x.Type == type).Clip);
        }
    }
}