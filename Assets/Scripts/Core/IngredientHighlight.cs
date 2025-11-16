using System.Collections.Generic;
using System.Linq;
using Internal.Runtime.Dependencies.Core;
using UI.Elements;
using UnityEngine;

namespace Core
{
    public class IngredientHighlight : MonoBehaviour
    {
        [SerializeField] private Renderer[] affectedRenderers;
        [SerializeField] private IngredientsSettings settings;
        [SerializeField] private string text;
        [SerializeField] private Sprite icon;

        private IHintPresenter _presenter = DependencyInjector.Get<IHintPresenter>();
        private Dictionary<Renderer, Color> _defaultColorsByRenderer;
        private IngredientProcessor _processor;

        public string Text => text;

        private void Start()
        {
            _defaultColorsByRenderer = affectedRenderers.ToDictionary(r => r, r => r.material.color);
            _processor = gameObject.GetComponent<IngredientProcessor>();
        }

        private void OnValidate()
        {
            if (affectedRenderers.Length > 0)
            {
                return;
            }
            
            affectedRenderers = gameObject.GetComponentsInChildren<Renderer>();
        }

        public void ToggleHighlight(bool state)
        {
            foreach (Renderer affectedRenderer in affectedRenderers)
            {
                affectedRenderer.material.color = state ? settings.HighlightColor : _defaultColorsByRenderer[affectedRenderer];
            }

            if (state)
            {
                _presenter.Present(new AddHintData
                {
                    Code = icon,
                    Text = text
                });
            }
            else
            {
                _presenter.Present(new RemoveHintData()
                {
                    Text = text
                });
            }
        }
    }
}