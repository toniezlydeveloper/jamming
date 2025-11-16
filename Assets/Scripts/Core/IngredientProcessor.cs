using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class IngredientProcessor : MonoBehaviour
    {
        [SerializeField] protected IngredientsSettings settings;

        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;
        [SerializeField] private IngredientType[] handledIngredientTypes;
        [SerializeField] private ProcessorType type;
        [SerializeField] private IngredientsOutput output;
        [SerializeField] private bool canDuplicateTypes;
        
        protected List<Enum> _ingredients = new();
        
        private List<IngredientType> _ingredientTypes = new();

        public IngredientsOutput Output => output;
        public List<IngredientType> IngredientTypes => _ingredientTypes;
        public bool HasAnyIngredient => _ingredients.Count > 0;
        public bool CanProcess => _ingredients.Count >= minCount && output.CanAdd;
        public int MaxCount => maxCount;
        public List<Enum> Ingredients => _ingredients;
        public ProcessorType Type => type;

        public bool TryAdd(Enum ingredient, IngredientType ingredientType)
        {
            if (!handledIngredientTypes.Contains(ingredientType))
            {
                return false;
            }

            if (!canDuplicateTypes && _ingredientTypes.Contains(ingredientType))
            {
                return false;
            }

            if (_ingredients.Count >= maxCount)
            {
                return false;
            }

            Add(ingredient, ingredientType);
            _ingredientTypes.Add(ingredientType);
            _ingredients.Add(ingredient);
            return true;
        }

        public void Remove(int index, out Enum ingredient, out IngredientType ingredientType)
        {
            ingredientType = _ingredientTypes[index];
            ingredient = _ingredients[index];
            _ingredientTypes.RemoveAt(index);
            _ingredients.RemoveAt(index);
        }

        public void Clear()
        {
            ClearInternal();
            _ingredientTypes.Clear();
            _ingredients.Clear();
        }

        protected virtual void Add(Enum ingredient, IngredientType ingredientType)
        {
        }

        protected virtual void Remove(int index)
        {
        }

        protected virtual void ClearInternal()
        {
        }
    }
}