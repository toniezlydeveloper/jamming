using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class IngredientProcessorWithModel : IngredientProcessor
    {
        [SerializeField] private Transform[] points;
        
        private List<GameObject> _models = new();

        protected override void Add(Enum ingredient, IngredientType ingredientType)
        {
            Transform point = points[_ingredients.Count];
            GameObject prefab = null;
            
            switch (ingredientType)
            {
                case IngredientType.None:
                    break;
                case IngredientType.Organic:
                    prefab = settings.OrganicsData.First(d => d.Type.Equals(ingredient)).Model;
                    break;
                case IngredientType.Base:
                    prefab = settings.BasesData.First(d => d.Type.Equals(ingredient)).Model;
                    break;
                case IngredientType.NonOrganic:
                    prefab = settings.NonOrganicsData.First(d => d.Type.Equals(ingredient)).Model;
                    break;
                case IngredientType.Potion:
                    prefab = settings.PotionsData.First(d => d.Type.Equals(ingredient)).Model;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _models.Add(Instantiate(prefab, point.position, point.rotation));
        }
        
        protected override void Remove(int index)
        {
            Destroy(_models[index]);
            _models.RemoveAt(index);
        }
    }
}