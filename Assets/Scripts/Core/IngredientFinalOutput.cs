using System;
using System.Collections.Generic;
using Internal.Runtime.Utilities;
using UnityEngine;

namespace Core
{
    public class IngredientFinalOutput : IngredientsOutput
    {
        [SerializeField] private int maxCount;

        private List<Enum> _potions = new();

        public override bool CanAdd => _potions.Count < maxCount - 1;

        public override void Add(Enum ingredient, IngredientType ingredientType)
        {
            ExtendedDebug.Log($"Adding {ingredient}");
            _potions.Add(ingredient);
        }
    }
}