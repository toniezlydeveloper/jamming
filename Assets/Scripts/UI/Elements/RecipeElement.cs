using System;
using System.Linq;
using Core;
using Recipes;
using UnityEngine;

namespace UI.Elements
{
    public class RecipeElement : MonoBehaviour
    {
        [SerializeField] private IngredientElement[] ingredients;
        [SerializeField] private IngredientElement output;
        [SerializeField] private IngredientsSettings settings;

        public void Setup(RecipesSaver.RecipeOutputData data)
        {
            for (int i = 0; i < data.Ingredients.Count; i++)
            {
                IIngredientData iD = GetIngredientData(data.Ingredients[i], data.IngredientTypes[i]);
                ingredients[i].TextContainer.text = iD.Name;
                ingredients[i].IconContainer.sprite = iD.Icon;
                ingredients[i].Holder.SetActive(true);
            }
            for (int i = data.Ingredients.Count; i < ingredients.Length; i++)
            {
                ingredients[i].Holder.SetActive(false);
            }
            IIngredientData oI = GetIngredientData(data.Output, data.OutputType);
            output.TextContainer.text = oI.Name;
            output.IconContainer.sprite = oI.Icon;
            output.Holder.SetActive(true);
            
        }

        private IIngredientData GetIngredientData(Enum ingredient, IngredientType ingredientType) => ingredientType switch
        {
            IngredientType.None => null,
            IngredientType.Organic => settings.OrganicsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Base => settings.BasesData.First(d => d.Type.Equals(ingredient)),
            IngredientType.NonOrganic => settings.NonOrganicsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Potion => settings.PotionsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Dust => settings.DustsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Cut => settings.CutsData.First(d => d.Type.Equals(ingredient)),
            IngredientType.Distilled => settings.DistilledData.First(d => d.Type.Equals(ingredient)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}