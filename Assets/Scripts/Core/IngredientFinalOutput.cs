using System;
using System.Collections.Generic;
using Highscore;
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
            ResultsCounter.BrewedPotions.Add((PotionType)ingredient);
            ExtendedDebug.Log($"Adding {ingredient}");
            _potions.Add(ingredient);
        }
    }
}