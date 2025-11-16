using UnityEngine;

namespace Recipes
{
    public class RecipeBoomPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] systems;
        
        public void Play()
        {
            foreach (ParticleSystem system in systems)
            {
                system.Play();
            }
        }
    }
}