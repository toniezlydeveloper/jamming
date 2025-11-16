using System;
using System.Collections.Generic;
using Flow.States;
using Internal.Runtime.Utilities;
using UnityEngine;

namespace Core
{
    public class IngredientsOutput : MonoBehaviour
    {
        [SerializeField] private List<IngredientType> handledTypes;
        
        public Enum Ingredient { get; private set; }
        public IngredientType IngredientType { get; private set; }

        public virtual bool CanAdd => IngredientType == IngredientType.None;

        private void Start()
        {
            GameplayState.OnAddRequired += Handle;
        }

        private void OnDestroy()
        {
            GameplayState.OnAddRequired -= Handle;
        }

        public virtual void Add(Enum ingredient, IngredientType ingredientType)
        {
            ExtendedDebug.Log($"Adding {ingredient}");
            IngredientType = ingredientType;
            Ingredient = ingredient;
        }

        private void Handle(Enum arg1, IngredientType arg2)
        {
            if (handledTypes.Contains(arg2))
            {
                IngredientType = arg2;
                Ingredient = arg1;
            }
        }
    }
}