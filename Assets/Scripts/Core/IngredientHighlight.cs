using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class IngredientHighlight : MonoBehaviour
    {
        [SerializeField] private Renderer[] affectedRenderers;
        [SerializeField] private IngredientsSettings settings;

        private Dictionary<Renderer, Color> _defaultColorsByRenderer;
        private IngredientProcessor _processor;

        private void Start()
        {
            _defaultColorsByRenderer = affectedRenderers.ToDictionary(r => r, r => r.material.color);
            _processor = gameObject.GetComponent<IngredientProcessor>();
        }

        private void OnValidate()
        {
            affectedRenderers = gameObject.GetComponentsInChildren<Renderer>();
        }

        public void ToggleHighlight(bool state)
        {
            foreach (Renderer affectedRenderer in affectedRenderers)
            {
                affectedRenderer.material.color = state ? settings.HighlightColor : _defaultColorsByRenderer[affectedRenderer];
            }
        }
    }
}