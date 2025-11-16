using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [Serializable]
        public class SoundData
        {
            [field: SerializeField] public AudioClip Clip { get; set; }
            [field: SerializeField] public SoundType Type { get; set; }
        }
        
        [SerializeField] private GameObject sourcesParent;
        [SerializeField] private SoundData[] datas;

        private Dictionary<SoundType, AudioSource> _sourcesByType = new();
        
        private void Start()
        {
            foreach (SoundType type in Enum.GetValues(typeof(SoundType)))
            {
                AudioSource source = sourcesParent.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.Pause();
                _sourcesByType.Add(type, source);
            }
        }

        public void Play(SoundType type)
        {
            _sourcesByType[type].clip = datas.First(d => d.Type == type).Clip;
            _sourcesByType[type].loop = true;
            _sourcesByType[type].volume = 0.1f;
            _sourcesByType[type].Play();
        }

        public void Stop(SoundType type)
        {
            _sourcesByType[type].Stop();
        }

        public void StopAll()
        {
            foreach (AudioSource audioSource in _sourcesByType.Values)
            {
                audioSource.Stop();
            }
        }
    }
}