using UnityEngine;

namespace Core
{
    public class IngredientContainer : MonoBehaviour
    {
        [field:SerializeField] public IngredientType Type { get; set; }

        public Vector3 Position => transform.position;
    }
}