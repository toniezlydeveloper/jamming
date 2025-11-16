using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Recipes
{
    public static class RecipesHolder
    {
        public static readonly Dictionary<ProcessorType, Recipe> DefaultRecipesByProcessor = new()
        {
            {
                ProcessorType.Cauldron, new Recipe
                {
                    OutputType = IngredientType.Potion,
                    Output = PotionType.Goo
                }
            },
            {
                ProcessorType.CuttingBoard, new Recipe
                {
                    OutputType = IngredientType.Cut,
                    Output = CutType.Mush
                }
            },
            {
                ProcessorType.Mortar, new Recipe
                {
                    OutputType = IngredientType.Dust,
                    Output = DustType.Ash
                }
            }
        };
        public static readonly List<Recipe> Recipes = new()
        {
            new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Cauldron,
                RequiredIngredients = new List<Enum>
                {
                    DustType.Moon,
                    DistilledType.Alcohol,
                    OrganicType.Eye
                },
                OutputType = IngredientType.Potion,
                Output = PotionType.Speed,
            },
            new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Cauldron,
                RequiredIngredients = new List<Enum>
                {
                    DustType.Noble,
                    BaseType.Water,
                    CutType.OrganicMix
                },
                OutputType = IngredientType.Potion,
                Output = PotionType.Haste,
            },
            new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Cauldron,
                RequiredIngredients = new List<Enum>
                {
                    DustType.Noble,
                    BaseType.Alcohol,
                    CutType.UniversalMix
                },
                OutputType = IngredientType.Potion,
                Output = PotionType.Happiness,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Cauldron,
                RequiredIngredients = new List<Enum>
                {
                    DustType.Crest,
                    BaseType.Oil,
                    CutType.GemMix
                },
                OutputType = IngredientType.Potion,
                Output = PotionType.Love,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.CuttingBoard,
                RequiredIngredients = new List<Enum>(),
                IngredientsMatchingCallback = ingredients => ingredients.All(i => i.GetType() == typeof(NonOrganicType)),
                OutputType = IngredientType.Cut,
                Output = CutType.GemMix,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.CuttingBoard,
                RequiredIngredients = new List<Enum>(),
                IngredientsMatchingCallback = ingredients => ingredients.All(i => i.GetType() == typeof(OrganicType)),
                OutputType = IngredientType.Cut,
                Output = CutType.OrganicMix,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.CuttingBoard,
                RequiredIngredients = new List<Enum>(),
                IngredientsMatchingCallback = ingredients =>
                {
                    return ingredients.Any(i => i.GetType() == typeof(OrganicType)) &&
                           ingredients.Any(i => i.GetType() == typeof(NonOrganicType));
                },
                OutputType = IngredientType.Cut,
                Output = CutType.UniversalMix,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Mortar,
                RequiredIngredients = new List<Enum>()
                {
                    NonOrganicType.Diamond,
                    NonOrganicType.Ruby
                },
                OutputType = IngredientType.Dust,
                Output = DustType.Crest,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Mortar,
                RequiredIngredients = new List<Enum>()
                {
                    NonOrganicType.Diamond,
                    OrganicType.Bone
                },
                OutputType = IngredientType.Dust,
                Output = DustType.Moon,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Mortar,
                RequiredIngredients = new List<Enum>()
                {
                    NonOrganicType.Gold,
                    OrganicType.Root
                },
                OutputType = IngredientType.Dust,
                Output = DustType.Noble,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Distiller,
                RequiredIngredients = new List<Enum>()
                {
                    BaseType.Oil
                },
                OutputType = IngredientType.Distilled,
                Output = DistilledType.Oil,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Distiller,
                RequiredIngredients = new List<Enum>()
                {
                    BaseType.Water
                },
                OutputType = IngredientType.Distilled,
                Output = DistilledType.Water,
            },new Recipe
            {
                NormalizedChances = new List<float>
                {
                    0.25f,
                },
                ProcessorType = ProcessorType.Distiller,
                RequiredIngredients = new List<Enum>()
                {
                    BaseType.Alcohol
                },
                OutputType = IngredientType.Distilled,
                Output = DistilledType.Alcohol,
            },
        };
        
    }
}