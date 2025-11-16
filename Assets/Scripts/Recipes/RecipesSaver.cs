using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Recipes
{
    public static class RecipesSaver
    {
        public class RecipeData
        {
            public List<IngredientType> IngredientTypes { get; set; }
            public List<IngredientValue> Ingredients { get; set; }
            public IngredientType OutputType { get; set; }
            public IngredientValue Output { get; set; }
            public ProcessorType ProcessorType { get; set; }
        }

        public class RecipeOutputData
        {
            public ProcessorType ProcessorType { get; set; }
            public List<IngredientType> IngredientTypes { get; set; }
            public List<Enum> Ingredients { get; set; }
            public IngredientType OutputType { get; set; }
            public Enum Output { get; set; }
        }

        public class IngredientValue
        {
            public string EnumType { get; set; }
            public string Value { get; set; }

            public IngredientValue() {}

            public IngredientValue(Enum e)
            {
                EnumType = e.GetType().ToString();
                Value = e.ToString();
            }

            public Enum ToEnum()
            {
                Type type = Type.GetType(EnumType);
                return (Enum)Enum.Parse(type, Value);
            }
        }

        private const string RecipeKey = "RECIPE {0}";
        private const string CountKey = "COUNT";
        
        public static void Save(List<Enum> ingredients, List<IngredientType> ingredientTypes, Enum output, IngredientType outputType, ProcessorType type)
        {
            int count = PlayerPrefs.GetInt(CountKey, 0);

            RecipeData data = new RecipeData()
            {
                Output = new IngredientValue(output),
                OutputType = outputType,
                IngredientTypes = ingredientTypes,
                Ingredients = ingredients.Select(i => new IngredientValue(i)).ToList(),
                ProcessorType = type
            };
            
            PlayerPrefs.SetString(string.Format(RecipeKey, count), JsonConvert.SerializeObject(data));
            PlayerPrefs.SetInt(CountKey, count + 1);
        }

        public static List<RecipeOutputData> Read()
        {
            int count = PlayerPrefs.GetInt(CountKey, 0);
            List<RecipeOutputData> recipesData = new();

            for (int i = 0; i < count; i++)
            {
                string json = PlayerPrefs.GetString(string.Format(RecipeKey, i));

                RecipeData data = JsonConvert.DeserializeObject<RecipeData>(json);

                RecipeOutputData outputData = new RecipeOutputData
                {
                    Ingredients = data.Ingredients.Select(x => x.ToEnum()).ToList(),
                    IngredientTypes = data.IngredientTypes,
                    OutputType = data.OutputType,
                    Output = data.Output.ToEnum(),
                    ProcessorType = data.ProcessorType
                };
                
                recipesData.Add(outputData);
            }

            return recipesData;
        }

        public static void Clear()
        {
            int count = PlayerPrefs.GetInt(CountKey, 0);

            for (int i = 0; i < count; i++)
            {
                PlayerPrefs.DeleteKey(string.Format(RecipeKey, i));
            }
            
            PlayerPrefs.SetInt(CountKey, 0);
        }
    }
}