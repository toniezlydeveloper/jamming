using System;
using System.Linq;
using UnityEngine;

namespace Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [Serializable]
        public class MusicData
        {
            [field: SerializeField] public AudioClip Clip { get; set; }
            [field: SerializeField] public MusicType Type { get; set; }
        }
        
        [SerializeField] private AudioSource source;
        [SerializeField] private MusicData[] datas;
        
        public void Play(MusicType type)
        {
            source.clip = datas.First(x => x.Type == type).Clip;
            source.Play();
        }
    }
}