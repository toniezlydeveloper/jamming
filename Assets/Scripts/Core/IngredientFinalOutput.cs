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
        [SerializeField] private GameObject[] models;

        private List<Enum> _potions = new();

        public override bool CanAdd => _potions.Count < maxCount - 1;

        public override void Add(Enum ingredient, IngredientType ingredientType)
        {
            if (ingredient == null)
            {
                return;
            }
            
            ResultsCounter.BrewedPotions.Add((PotionType)ingredient);
            ExtendedDebug.Log($"Adding {ingredient}");
            _potions.Add(ingredient);

            for (int i = 0; i < _potions.Count; i++)
            {
                models[i].SetActive(true);
            }
            for (int i = _potions.Count; i < models.Length; i++)
            {
                models[i].SetActive(false);
            }
        }
    }
}